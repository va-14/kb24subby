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

namespace Subby
{
    [KnownType(typeof(Player))]
    [KnownType(typeof(Missile))]
    [KnownType(typeof(TankStation))]
    [KnownType(typeof(Wrak))]
    [KnownType(typeof(Mine))]
    [KnownType(typeof(Chopper))]
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
        private int _spawnChopperSecond;

        public void Initialize()
        {
            SpriteList = new List<ISprite>();
            MissileList = new List<Missile>();
            Highscores = new int[10];
            Background = new Background();
            _spawnChopperSecond = 0;
            Background.Initialize();
        }

        public void Load(ContentManager manager, GraphicsDevice graphicsDevice)
        {
            Subby.Texture = manager.Load<Texture2D>(Subby.TextureName);
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Texture = manager.Load<Texture2D>(sprite.TextureName);
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

        public void Update(GameTime gameTime)
        {
            Subby.Update(gameTime);
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Update(gameTime);
            }
            UpdateScrollingPosition(Subby);
            Background.UpdatePosition(ScrollingPosition, gameTime);
            ChopperGenerator(gameTime);
        }
        private void ChopperGenerator(GameTime gameTime)
        {
            int second = (int)gameTime.TotalGameTime.TotalSeconds;
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
                    DropSecond = random.Next(second + 2, second + 5),
                };
                chopper.Missiles = new List<Missile>(){
                    createMissile(new Missile(), new Point(), 300),
                    createMissile(new Missile(), new Point(), 300)
                    };

                SpriteList.Add(chopper);
            }
        }
        private int NewRandom(int lastRandomSecond){
            Random random = new Random();
            int minSeconds = lastRandomSecond + 2;

            return random.Next(minSeconds, minSeconds + 3);
        }


        public Missile createMissile(Missile missile, Point position, int Damage)
        {

            if (missile != null)
            {
                missile.Texture = _missileTexture;
                missile.TextureName = "missile";
                missile.Color = Color.White;
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
        private void UpdateScrollingPosition(Player subby)
        {
            if (subby.Position.Y >= LevelBoundaries.Bottom || subby.Position.Y <= LevelBoundaries.Top || subby.Position.X <= LevelBoundaries.Left)
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