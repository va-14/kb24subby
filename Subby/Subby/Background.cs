using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Subby
{
    [DataContract]
    public class Background
    {
        [DataMember]
        public Vector2 WaterPosition, WaterOrigin, WavesPosition;
        public Texture2D WaterTexture, WavesTexture, SkyTexture;
        [DataMember]
        public string WaterTextureName, WavesTextureName, SkyTextureName;
        [DataMember]
        public int ScreenWidth, ScreenHeight;

        public void Initialize()
        {
            WavesPosition = new Vector2(960, 270);
            WaterTextureName = "ocean3";
            WavesTextureName = "waves";
            SkyTextureName = "sky";
        }

        public void Load(GraphicsDevice device, Texture2D waterTexture, Texture2D wavesTexture, Texture2D skyTexture)
        {
            WaterTexture = waterTexture;
            WavesTexture = wavesTexture;
            SkyTexture = skyTexture;
            ScreenHeight = device.Viewport.Height;
            ScreenWidth = device.Viewport.Width;
            WavesPosition.Y = ScreenHeight / 2.5f;
            WaterPosition.Y = ScreenHeight / 3.5f;
        }

        public void UpdatePosition(int scrollingPosition, GameTime gameTime)
        {
            WaterPosition = new Vector2((float)-scrollingPosition % WaterTexture.Width, WaterPosition.Y);

            float deltaX = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            WavesPosition = new Vector2(WavesPosition.X + deltaX, WavesPosition.Y);
            WavesPosition = new Vector2(WavesPosition.X % WavesTexture.Width, WavesPosition.Y);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(WaterTexture, WaterPosition, null, Color.White, 0, WaterOrigin, 1.0f, SpriteEffects.None, 0f);

            batch.Draw(WaterTexture, new Vector2(WaterPosition.X + WaterTexture.Width, WaterPosition.Y), null, Color.White, 0, WaterOrigin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(WaterTexture, new Vector2(WaterPosition.X + 2 * WaterTexture.Width, WaterPosition.Y), null, Color.White, 0, WaterOrigin, 1.0f, SpriteEffects.None, 0f);

            batch.Draw(SkyTexture, new Vector2(0, 0), Color.White);

            if (WavesPosition.X < ScreenWidth)
            {
                batch.Draw(WavesTexture, WavesPosition, Color.White);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(WavesTexture, WavesPosition - new Vector2(WavesTexture.Width, 0), Color.White);
        }
    }
}
