using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby
{
    class ScrollingBackground
    {
        public Vector2 position, origin;
        public Texture2D texture;
        public int screenWidth, screenHeight;

        public void Load(GraphicsDevice device, Texture2D surfaceTexture)
        {
            texture = surfaceTexture;
            screenHeight = device.Viewport.Height;
            screenWidth = device.Viewport.Width;
        }

        public void UpdatePosition(int scrollingPosition)
        {
            position = new Vector2((float)-scrollingPosition % texture.Width, 312);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0f);

            batch.Draw(texture, new Vector2(position.X + texture.Width, position.Y), null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(texture, new Vector2(position.X + 2 * texture.Width, position.Y), null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
