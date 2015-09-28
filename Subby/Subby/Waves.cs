using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby
{
    class Waves : ISprite
    {
        public Vector2 position { get; set; }
        public Color color { get; set; }
        public Texture2D texture { get; set; }

        public void Initialize()
        {
            color = Color.White;
            position = new Vector2(960, 270);
        }

        public void Load(Texture2D _texture)
        {
            texture = _texture;
        }

        public void Update(GameTime gameTime)
        {
            float deltaX = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            position = new Vector2(position.X + deltaX, position.Y);
            position = new Vector2(position.X % texture.Width, position.Y);
        }

        public void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (position.X < 1920)
            {
                batch.Draw(texture, position, color);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(texture, position - new Vector2(texture.Width, 0), color);
        }

        public void CollisionWith(ISprite s)
        {

        }
    }
}
