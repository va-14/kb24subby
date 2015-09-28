using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby
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
            if (s.GetType().Name.Equals("Player"))
            {
                Player a = (Player)s;
                a.fillTank(_tank);
            }
            _tank = 0;
        }
    }
}
