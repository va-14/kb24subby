using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby.Sprites
{
    class Wrak : ISprite
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        
        private int _schade;
        public int Schade
        {
            get { return _schade; }
            set { _schade = value; }
        }


        public int Height
        {
            get { return Texture.Height; }
        }
        public int Width
        {
            get { return Texture.Width; }
        }

        public void Update(GameTime gameTime) { }

        public void CollisionWith(ISprite s)
        {

        }

        public void Initialize()
        {
            Color = Color.White;
            //Position = new Vector2(960, 270);
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }


        public void Draw(SpriteBatch batch, Vector2 positionDeflection)
        {
            batch.Draw(Texture, new Vector2(Position.X - positionDeflection.X, Position.Y), Color);
        }

    }
}
