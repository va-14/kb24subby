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

namespace Subby
{
    [KnownType(typeof(Player))]
    [KnownType(typeof(Missile))]
    [KnownType(typeof(TankStation))]
    [KnownType(typeof(Wrak))]
    [DataContract]
    public class Level
    {
        [DataMember]
        public List<ISprite> SpriteList;
        [DataMember]
        public int[] Highscores;
        [DataMember]
        public Background Background;
        [DataMember]
        public Vector4 LevelBoundaries;
        [DataMember]
        public int ScrollingPosition;

        public void Initialize()
        {
            SpriteList = new List<ISprite>();
            Highscores = new int[10];
            Background = new Background();
            Background.Initialize();
        }

        public void Load(ContentManager manager, GraphicsDevice graphicsDevice)
        {
            foreach (ISprite sprite in SpriteList)
            {
                Texture2D texture = manager.Load<Texture2D>(sprite.TextureName);
                sprite.Texture = texture;
            }

            Texture2D waterTexture = manager.Load<Texture2D>(Background.WaterTextureName);
            Texture2D wavesTexture = manager.Load<Texture2D>(Background.WavesTextureName);
            Texture2D skyTexture = manager.Load<Texture2D>(Background.SkyTextureName);
            Background.Load(graphicsDevice, waterTexture, wavesTexture, skyTexture);

        }

        public void Update(GameTime gameTime)
        {
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Update(gameTime);

                if (sprite is Player )
                {
                    UpdateScrollingPosition((Player)sprite);
                    Background.UpdatePosition(ScrollingPosition, gameTime);
                }
            }

        }

        public void Draw(SpriteBatch batch)
        {
            Background.Draw(batch);
            foreach (ISprite sprite in SpriteList)
            {
                if (sprite is Player)
                {
                    batch.Draw(sprite.Texture, sprite.Position, null, sprite.Color, sprite.Rotation, sprite.PivotPoint, 1f, SpriteEffects.None, 1);
                }
                else
                {
                    batch.Draw(sprite.Texture, new Vector2(sprite.Position.X - (float)ScrollingPosition, sprite.Position.Y), null, sprite.Color, sprite.Rotation, sprite.PivotPoint, 1f, SpriteEffects.None, 1);
                }
            }
        }

        private void UpdateScrollingPosition(Player subby)
        {
            if (subby.Position.Y >= LevelBoundaries.Z || subby.Position.Y <= LevelBoundaries.Y || subby.Position.X <= LevelBoundaries.W)
            {
                subby.Speed = 0;
            }
            if (subby.Position.X >= LevelBoundaries.X)
            {
                ScrollingPosition += (int)subby.Position.X - (int)LevelBoundaries.X;
                subby.Position = new Vector2(LevelBoundaries.X, subby.Position.Y);
            }
        }

    }
}
