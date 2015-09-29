using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby.Sprites
{
    class TankStation : ISprite
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        private int _tank;

        public int Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }
        
        public void Update(GameTime gameTime) {  }
        
        public void CollisionWith(ISprite s) 
        {
            
        }
        public int getFuel()
        {
            int tmpTank = _tank;
            emptyTank();
            return tmpTank;
        }
        private void emptyTank()
        {
            _tank = 0;
            Color = Color.Black ;
        }

        public void Initialize()
        {
            Color = Color.White;
            Position = new Vector2(960, 270);
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }


        public void Draw(SpriteBatch batch)
        {
            
            batch.Draw(Texture, Position, Color);
        }

    }
}
