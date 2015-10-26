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
    //[KnownType(typeof(Player))]
    [KnownType(typeof(Missile))]
    [KnownType(typeof(TankStation))]
    [KnownType(typeof(Wrak))]
    [KnownType(typeof(Mine))]
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


        public void Initialize()
        {
            SpriteList = new List<ISprite>();
            MissileList = new List<Missile>();
            Highscores = new int[10];
            Background = new Background();
            Background.Initialize();
        }

        public void Load(ContentManager manager, GraphicsDevice graphicsDevice)
        {
            Subby.Texture = manager.Load<Texture2D>(Subby.TextureName);
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Texture = manager.Load<Texture2D>(sprite.TextureName);
            }
            
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
        }

        public void Draw(SpriteBatch batch)
        {
            Background.Draw(batch);
            batch.Draw(Subby.Texture, Subby.Position, null, Subby.Color, Subby.Rotation, Subby.PivotPoint, 1f, SpriteEffects.None, 1);
            foreach (ISprite sprite in SpriteList)
            {
                if (sprite is Player)
                {
                   
                }
                else
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