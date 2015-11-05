using Microsoft.Xna.Framework;
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
        public List<ISprite> SpriteList;
        [DataMember]
        public List<Missile> MissileList;
        [DataMember]
        public int[] Highscores;
        [DataMember]
        public Background Background;
        [DataMember]
        public GameBoundaries LevelBoundaries;
        [DataMember]
        public int ScrollingPosition;
        [DataMember]
        public Player Subby;


        private Texture2D _chopperTexture;
        private Texture2D _missileTexture;

        public DateTime startRoundTime;
        public TimeSpan totalRoundTime;

        [DataMember]
        private int _spawnChopperSecond;

        public void Initialize()
        {
            startRoundTime = DateTime.Now;
            _spawnChopperSecond = 0;
        }

        public void Load(ContentManager manager, GraphicsDevice graphicsDevice)
        {
            LevelBoundaries.Top = graphicsDevice.Viewport.Height / 2.5f + 20;
            LevelBoundaries.Bottom = graphicsDevice.Viewport.Height - 80;
            LevelBoundaries.Left = 20;
            LevelBoundaries.Right = graphicsDevice.Viewport.Width / 2;
            Subby.Texture = manager.Load<Texture2D>(Subby.TextureName);
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
            _chopperTexture = manager.Load<Texture2D>("chopper");
            _missileTexture = manager.Load<Texture2D>("missile");
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
            totalRoundTime = DateTime.Now - startRoundTime;
            CleanUpSpriteList();
            Subby.Update(gameTime);
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Update(gameTime);
                if (sprite is HostileSub)
                {
                    ((HostileSub)sprite).ScrollingPosition = ScrollingPosition;
                }
            }

            checkCollisions();
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
                    if (((HostileSub)sprite).Shoot(gameTime))
	                {
                        int x = (int)((sprite.Texture.Width / 2 + 30) * Math.Cos((((HostileSub)sprite).AngleDeg + 180) * Math.PI / 180F)) + (int)sprite.Position.X - ScrollingPosition;
                        int y = (int)((sprite.Texture.Width / 2 + 30) * Math.Sin((((HostileSub)sprite).AngleDeg + 180) * Math.PI / 180F)) + (int)sprite.Position.Y;
                        Point position = new Point(x, y);
                        Missile missile = new Missile();
                        missile.Rotation = ((HostileSub)sprite).AngleDeg;
                        missile.Speed = -7.0f;
                        createMissile(missile, position, 300);
	                }		            
	            }
	        }
            
        }

        private void ChopperGenerator()
        {
            int second = (int)totalRoundTime.TotalSeconds;
            if (second >= _spawnChopperSecond)
            {
                Random random = new Random();
                _spawnChopperSecond = random.Next(_spawnChopperSecond + 3, _spawnChopperSecond + 7);
                Chopper chopper = new Chopper() {
                    Texture = _chopperTexture, 
                    Color = Color.White, 
                    Damage = 300, 
                    Position = new Vector2(ScrollingPosition-70, 60), 
                    Speed = 6f,
                    TextureName = "chopper",
                    DropSecond = random.Next(0, 5),
                    Health = 200
                };
                chopper.Missiles = new List<Missile>(){
                    createMissile(new Missile(), new Point(-40,0), 300),
                    createMissile(new Missile(), new Point(-40,0), 300)
                    };

                SpriteList.Add(chopper);
            }
        }
        private int NewRandom(int lastRandomSecond){
            Random random = new Random();
            int minSeconds = lastRandomSecond + 2;

            return random.Next(minSeconds, minSeconds + 3);
        }

        private void CleanUpSpriteList()
        {
            Boolean remove;
            foreach (ISprite sprite in SpriteList.Reverse<ISprite>())
            {
                remove = false;
                //alle sprites die geen health meer hebben
                if (sprite.Health < 0)
                {
                    remove = true;
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
                        Missile missile = (Missile)sprite;
                        if (missile.Active)
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
       
        public Missile createMissile(Missile missile, Point position, int Damage)
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
                    Mine m = (Mine)sprite;
                    batch.Draw(sprite.Texture, new Vector2(sprite.Position.X - (float)ScrollingPosition + (m.Range / 2), sprite.Position.Y + (m.Range / 2)), sprite.Color);
                }
                else
                {
                    batch.Draw(sprite.Texture, new Vector2(sprite.Position.X - (float)ScrollingPosition, sprite.Position.Y), null, sprite.Color, sprite.Rotation, sprite.PivotPoint, 1f, SpriteEffects.None, 1);
                }
            }
        }
        private List<Rectangle> calculateSubbyRect()
        {
            int pixels = 10; // deze kan maximaal op 10 voor een goede collision
            Point size = new Point(pixels, pixels);
            int widthRadius = Subby.Texture.Width / 2;
            int heightRadius = Subby.Texture.Height / 2;
            List<Rectangle> values = new List<Rectangle>();

            for (int x = 0; x < widthRadius; x = x + pixels)
            {
                for (int y = 0; y < heightRadius; y = y + pixels)
                {
                    Point radiusPoint = PointOnCircle(x, (int)Subby.AngleDegrees, new Point((int)Subby.Position.X, (int)Subby.Position.Y));
                    Point point = PointOnCircle(y, (int)Subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(x, (int)Subby.AngleDegrees, new Point((int)Subby.Position.X, (int)Subby.Position.Y));
                    point = PointOnCircle(y, (int)Subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-x, (int)Subby.AngleDegrees, new Point((int)Subby.Position.X, (int)Subby.Position.Y));
                    point = PointOnCircle(y, (int)Subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-x, (int)Subby.AngleDegrees, new Point((int)Subby.Position.X, (int)Subby.Position.Y));
                    point = PointOnCircle(y, (int)Subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
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
        private void checkCollisions()
        {

            int boundingLength;
            if (Subby.Width > Subby.Height)
            {
                boundingLength = Subby.Width;
            }
            else
            {
                boundingLength = Subby.Height;
            }
            Rectangle rectSubby = new Rectangle((int)Subby.Position.X - (boundingLength / 2), (int)Subby.Position.Y - (boundingLength / 2), boundingLength, boundingLength);
            Boolean subbyCollision;
            foreach (ISprite sprite in SpriteList)
            {
                subbyCollision = false;
                //collision voor subby is apart, want deze wordt opgedeeld in meerdere vierkantjes
                Rectangle rectSprite = new Rectangle((int)sprite.Position.X - ScrollingPosition, (int)sprite.Position.Y, sprite.Width, sprite.Height);
                Rectangle overlap = Rectangle.Intersect(rectSubby, rectSprite);
                if (!overlap.IsEmpty)
                {
                    List<Rectangle> partRectsSubby = calculateSubbyRect();

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
                        foreach (ISprite missile in MissileList)
                        {
                            // checkt alle collison met missiles
                            if (!sprite.GetType().Name.Equals("Missile"))
                                checkCollisionRectangleAction(missile, sprite);
                        }
                    }
                }
            }


        }
        private void checkCollisionRectangleAction(ISprite s1, ISprite s2)
        {
            Rectangle r1 = new Rectangle((int)s1.Position.X - ScrollingPosition, (int)s1.Position.Y, s1.Width, s1.Height);
            Rectangle r2 = new Rectangle((int)s2.Position.X - ScrollingPosition, (int)s2.Position.Y, s2.Width, s2.Height);

            Rectangle collisionCheck = Rectangle.Intersect(r1, r2);
            if (!collisionCheck.IsEmpty)
            {
                s2.CollisionWith(s1);
                s1.CollisionWith(s2);
            }
        }
        private void SubbyOnLevelBounaries(Player subby)
        {
            if (subby.Position.Y >= LevelBoundaries.Bottom)
            {
                subby.Position += new Vector2(0, -1);
                subby.Speed = -subby.Speed / 4;
            }
            if (subby.Position.Y <= LevelBoundaries.Top || subby.Position.X <= LevelBoundaries.Left)
            {
                subby.Speed = -subby.Speed / 4;
            }
            if (subby.Position.X >= LevelBoundaries.Right)
            {
                ScrollingPosition += (int)subby.Position.X - (int)LevelBoundaries.Right;
                subby.Position = new Vector2(LevelBoundaries.Right, subby.Position.Y);
            }
        }

    }
}