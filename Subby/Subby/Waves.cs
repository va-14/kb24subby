using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subby.Sprites;

namespace Subby
{
    class Waves : ISprite
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public string TextureName { get; set; }
        public float Rotation { get; set; }
        public Vector2 PivotPoint { get; set; }

        public void Initialize()
        {
            Color = Color.White;
            Position = new Vector2(960, 270);
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }

        public void Update(GameTime gameTime)
        {
            float deltaX = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            Position = new Vector2(Position.X + deltaX, Position.Y);
            Position = new Vector2(Position.X % Texture.Width, Position.Y);
        }

        public void Draw(SpriteBatch batch, Vector2 positionDeflection)
        {
            // Draw the texture, if it is still onscreen.
            if (Position.X < 1920)
            {
                batch.Draw(Texture, Position, Color);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(Texture, Position - new Vector2(Texture.Width, 0), Color);
        }

        public void CollisionWith(ISprite s)
        {

        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            