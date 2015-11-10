using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby
{
    class ClickableButton
    {
        public Vector2 Position;
        Texture2D Texture;
        Rectangle Rectangle;
        Color Color;
        bool Down;
        public bool IsClicked;
        public Vector2 Size;

        public void Initialize()
        {
            Color = new Color(255, 255, 255, 255);
        }

        public void Load(Texture2D texture, GraphicsDevice graphics, Vector2 position)
        {
            Texture = texture;
            Size = new Vector2(graphics.Viewport.Width / 9.5f, graphics.Viewport.Height / 27);
            Position = position;
        }

        public void Update(MouseState mouse)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                if (Color.A == 255)
                {
                    Down = false;
                }
                if (Color.A == 0)
                {
                    Down = true;
                }
                if (Down)
                {
                    Color.A += 3;
                }
                else
                {
                    Color.A -= 3;
                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    IsClicked = true;
                }
            }
            else if(Color.A < 255)
            {
                Color.A += 3;
                IsClicked = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Rectangle, Color);
        }

    }
}
