﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Subby.Strategies;

namespace Subby
{
    [KnownType(typeof(Player))]
    [KnownType(typeof(Missile))]
    [KnownType(typeof(TankStation))]
    [KnownType(typeof(Wrak))]
    [KnownType(typeof(Mine))]
    [KnownType(typeof(Chopper))]
    [KnownType(typeof(HostileSub))]
    [DataContract]
    public class Level
    {
        [DataMember]
        public Background Background;
        [DataMember]
        public LevelBoundaries LevelBoundaries;
        [DataMember]
        public List<Missile> MissileList;
        [DataMember]
        public int ScrollingPosition;
        [DataMember]
        public Player Subby;
        [DataMember]
        public List<ISprite> SpriteList;
        [DataMember]
        public float TotalRoundTime;
        [DataMember]
        public int Score;
        [DataMember]
        public int EndPosition;
        [DataMember]
        public int TargetTime;


        private Texture2D _chopperTexture;
        private Texture2D _missileTexture;
        [DataMember]
        private int _spawnChopperSecond;

        private int _screenWidth;

        public void Initialize()
        {
            _spawnChopperSecond = 0;
        }

        public void Load(ContentManager manager, GraphicsDevice graphicsDevice)
        {
            _screenWidth = graphicsDevice.Viewport.Width;
            LevelBoundaries.SetByGraphicDevice(graphicsDevice);
            Subby.Texture = manager.Load<Texture2D>(Subby.TextureName);
            _chopperTexture = manager.Load<Texture2D>("chopper");
            _missileTexture = manager.Load<Texture2D>("missile");

            foreach (ISprite sprite in SpriteList)
            {
                sprite.Texture = manager.Load<Texture2D>(sprite.TextureName);
                if (sprite is HostileSub)
                {
                    ((HostileSub)sprite).Strategy = new AimedShots();
                    ((HostileSub)sprite).Load(Subby, LevelBoundaries);
                }
            }

            if (MissileList != null)
            {
                foreach (ISprite sprite in MissileList)
                {
                    sprite.Texture = manager.Load<Texture2D>(sprite.TextureName);
                }
            }
            Texture2D waterTexture = manager.Load<Texture2D>(Background.WaterTextureName);
            Texture2D wavesTexture = manager.Load<Texture2D>(Background.WavesTextureName);
            Texture2D skyTexture = manager.Load<Texture2D>(Background.SkyTextureName);
            Background.Load(graphicsDevice, waterTexture, wavesTexture, skyTexture);
        }


        public Boolean IsSubbyAlive()
        {
            if (Subby.Health <= 0 || (Subby.Speed > -0.01f && Subby.Speed < 0.01f && Subby.Fuel <= 0))
                return false;
            else
                return true;

        }
        public void Update(GameTime gameTime)
        {
            TotalRoundTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CleanUpSpriteList();
            Subby.Update(gameTime);
            foreach (ISprite sprite in SpriteList)
            {
                if (sprite is HostileSub)
                {
                    ((HostileSub)sprite).ScrollingPosition = ScrollingPosition;
                    if (!((HostileSub)sprite).Active)
                    {
                        if (((HostileSub)sprite).Position.X < ScrollingPosition + _screenWidth)
                        {
                            ((HostileSub)sprite).Active = true;
                        }
                    }
                }
                sprite.Update(gameTime);
            }

            CheckCollisions();
            SubbyOnLevelBounaries(Subby);
            Background.UpdatePosition(ScrollingPosition, gameTime);
            ChopperGenerator();
            HostileSubShoot(gameTime);
        }

        private void HostileSubShoot(GameTime gameTime)
        {
            foreach (ISprite sprite in SpriteList.ToList())
	        {
		        if (sprite is HostileSub)
	            {
                    if (((HostileSub)sprite).Shoot(TotalRoundTime) & ((HostileSub)sprite).Active)
	                {
                        Point position = PointOnCircle((int)(sprite.Texture.Width / 2 + 30), (int)(((HostileSub)sprite).AngleDeg + 180), new Point((int)sprite.Position.X, (int)sprite.Position.Y));
                        position = new Point(position.X - ScrollingPosition, position.Y);
                        Missile missile = new Missile();
                        missile.Rotation = ((HostileSub)sprite).AngleDeg;
                        missile.Speed = -7.0f;
                        CreateMissile(missile, position, 300);
	                }		            
	            }
	        }
            
        }

        private void ChopperGenerator()
        {
            if (TotalRoundTime >= _spawnChopperSecond)
            {
                Random random = new Random();
                _spawnChopperSecond = random.Next(_spawnChopperSecond + 3, _spawnChopperSecond + 7);
                Chopper chopper = new Chopper() {
                    Texture = _chopperTexture, 
                    Color = Color.White, 
                    Position = new Vector2(ScrollingPosition-70, 60), 
                    Speed = 6f,
                    TextureName = "chopper",
                    DropSecond = random.Next(0, 5),
                    Health = 200
                };
                chopper.Missiles = new List<Missile>(){
                    CreateMissile(new Missile(), new Point(-40,0), 300),
                    CreateMissile(new Missile(), new Point(-40,0), 300)
                    };

                SpriteList.Add(chopper);
            }
        }
        public int CalculateLevelScore()
        {
            return this.Score + (this.Subby.Fuel / 2) + this.Subby.Health + ((this.TargetTime - (int)this.TotalRoundTime) * 25);
        }
        private void CleanUpSpriteList()
        {
            Boolean remove;
            foreach (ISprite sprite in SpriteList.Reverse<ISprite>())
            {
                remove = false;
                //alle sprites die geen health meer hebben
                if (sprite is IDamageableSprite)
                {
                    if (((IDamageableSprite)sprite).Health <= 0)
                    {
                        remove = true;
                    }
                }
                //sprites die geexplodeerd zijn
                if (sprite is IExplodableSprite)
                {
                    remove = ((IExplodableSprite)sprite).Exploded;
                }
                //check alleen chopper en missile wanneer ze aan de rechter kant uit het scherm vliegen
                if (sprite is Chopper || sprite is Missile)
                {
                    if (sprite.Position.X - ScrollingPosition > Background.ScreenWidth)
                    {
                        remove = true; 
                    }
                }
                //check alles aan de boven/onder/linker kant, behalve de missiles, want die starten aan de linker kant van de nieuwe choppers
                if (sprite.Position.Y > Background.ScreenHeight || sprite.Position.X - ScrollingPosition < - sprite.Width - 200 || sprite.Position.Y < - sprite.Height)
                {
                    if (sprite is Missile)
                    {
                        if (((Missile)sprite).Active)
                        {
                            remove = true;
                        }
                    }
                    else
                    {
                        remove = true;
                    }
                }
                if (remove)
                {
                    SpriteList.Remove(sprite);
                    if (sprite is Missile)
                    {
                        MissileList.Remove((Missile)sprite);
                    }
                }
            }
        }
       
        public Missile CreateMissile(Missile missile, Point position, int Damage)
        {
            if (missile != null)
            {
                missile.Texture = _missileTexture;
                missile.TextureName = "missile";
                missile.Color = Color.White;
                missile.Active = false;
                if (position != null)
                    missile.Position = new Vector2(position.X + ScrollingPosition, position.Y);

                if (Damage > 0)
                    missile.Damage = Damage;

                SpriteList.Add(missile);

                if (MissileList == null)
                    MissileList = new List<Missile>();

                MissileList.Add(missile);
            }
            return missile;
        }
        public void Draw(SpriteBatch batch)
        {
            Background.Draw(batch);
            batch.Draw(Subby.Texture, Subby.Position, null, Subby.Color, Subby.Rotation, Subby.PivotPoint, 1f, SpriteEffects.None, 1);
            foreach (ISprite sprite in SpriteList)
            {
                if (sprite.GetType().Name.Equals("Mine"))
                {
                    batch.Draw(sprite.Texture, new Vector2(sprite.Position.X - (float)ScrollingPosition + (((Mine)sprite).Range / 2), sprite.Position.Y + (((Mine)sprite).Range / 2)), sprite.Color);
                }
                else
                {
                    batch.Draw(sprite.Texture, new Vector2(sprite.Position.X - (float)ScrollingPosition, sprite.Position.Y), null, sprite.Color, sprite.Rotation, sprite.PivotPoint, 1f, SpriteEffects.None, 1);
                }
            }
        }
        public List<Rectangle> CalculateSubbyRect(int subbyWidth, int subbyHeight, Point position, int angle)
        {
            int pixels = 10; 
            Point size = new Point(pixels, pixels);
            int widthRadius = subbyWidth / 2;
            int heightRadius = subbyHeight / 2;
            List<Rectangle> values = new List<Rectangle>();

            for (int tmpWidthRadius = 0; tmpWidthRadius < widthRadius; tmpWidthRadius = tmpWidthRadius + pixels)
            {
                for (int tmpHeightRadius = 0; tmpHeightRadius < heightRadius; tmpHeightRadius = tmpHeightRadius + pixels)
                {
                    Point radiusPoint = PointOnCircle(tmpWidthRadius, angle, new Point((int)position.X, (int)position.Y));
                    Point point = PointOnCircle(tmpHeightRadius, angle - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(tmpWidthRadius, angle, new Point((int)position.X, (int)position.Y));
                    point = PointOnCircle(tmpHeightRadius, angle + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-tmpWidthRadius, angle, new Point((int)position.X, (int)position.Y));
                    point = PointOnCircle(tmpHeightRadius, angle - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-tmpWidthRadius, angle, new Point((int)position.X, (int)position.Y));
                    point = PointOnCircle(tmpHeightRadius, angle + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                }
            }
            return values;
        }

        public Point PointOnCircle(int radius, int angleInDegrees, Point origin)
        {  
            int x = (int)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            int y = (int)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;
            return new Point(x, y);
        }
        private void CheckCollisions()
        {

            int boundingLength;
            if (Subby.Width > Subby.Height)
                boundingLength = Subby.Width;
            else
                boundingLength = Subby.Height;
           
            Rectangle rectSubby = new Rectangle((int)Subby.Position.X - (boundingLength / 2), (int)Subby.Position.Y - (boundingLength / 2), boundingLength, boundingLength);
            Boolean subbyCollision;
            foreach (ISprite sprite in SpriteList)
            {
                subbyCollision = false;
                Rectangle rectSprite = new Rectangle((int)sprite.Position.X - ScrollingPosition, (int)sprite.Position.Y, sprite.Width, sprite.Height);
                Rectangle overlap = Rectangle.Intersect(rectSubby, rectSprite);
                if (!overlap.IsEmpty)
                {
                    List<Rectangle> partRectsSubby = CalculateSubbyRect(Subby.Width,Subby.Height,new Point((int)Subby.Position.X,(int)Subby.Position.Y),(int)Subby.AngleDegrees);

                    foreach (Rectangle partRectSubby in partRectsSubby)
                    {
                        Rectangle collisionCheck = Rectangle.Intersect(partRectSubby, rectSprite);
                        if (!collisionCheck.IsEmpty && !subbyCollision)
                        {
                            subbyCollision = true;
                            // checkt alle collision met Subby
                            Subby.CollisionWith(sprite);
                            sprite.CollisionWith(Subby);
                        }
                    }
                }
                if (!(sprite is Missile))
                {
                    if (MissileList != null)
                    {
                        foreach (Missile missile in MissileList)
                        {
                            // checkt alle collison met missiles
                            if (!sprite.GetType().Name.Equals("Missile"))
                                CheckMissileCollision(missile, sprite);
                            
                        }
                    }
                }
            }
        }
        private void CheckMissileCollision(Missile s1, ISprite s2)
        {
            Rectangle r1 = new Rectangle((int)s1.Position.X - ScrollingPosition, (int)s1.Position.Y, s1.Width, s1.Height);
            Rectangle r2 = new Rectangle((int)s2.Position.X - ScrollingPosition, (int)s2.Position.Y, s2.Width, s2.Height);

            Rectangle collisionCheck = Rectangle.Intersect(r1, r2);
            if (!collisionCheck.IsEmpty)
            {
                s2.CollisionWith(s1);
                s1.CollisionWith(s2);
                if (s2 is Chopper )
                {
                    UpdateScore(((Chopper)s2).Score);
                }
                if (s2 is HostileSub)
                {
                    UpdateScore(((HostileSub)s2).Score);
                }
            }
        }
        private void UpdateScore(int score)
        {
            Score += score;
        }
        public void SubbyOnLevelBounaries(Player subby)
        {
            if (subby.Position.Y > LevelBoundaries.Bottom - 3)
            {
                subby.Position -= subby.GetDamagedPositionBehavour();
            }
            if (subby.Position.Y <= LevelBoundaries.Top || 
                subby.Position.X <= LevelBoundaries.Left || 
                subby.Position.Y >= LevelBoundaries.Bottom)
            {
                subby.Speed = -subby.Speed;

            }
            if (subby.Position.X >= LevelBoundaries.Right)
            {
                UpdateScrollingPosition(subby, LevelBoundaries);
                subby.Position = new Vector2(LevelBoundaries.Right, subby.Position.Y);
            }
        }
        private void UpdateScrollingPosition(Player subby, LevelBoundaries levelBoundaries)
        {
            ScrollingPosition += (int)subby.Position.X - (int)LevelBoundaries.Right;
        }
    }
}