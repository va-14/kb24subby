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
        public Vector2 position { get; set; }
        public Color color { get; set; }
        public Texture2D texture { get; set; }
        private int _tank;

        public int Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }

        public void Initialize()
        {

        }

        public void Load(Texture2D _texture)
        {

        }
        
        public void Update(GameTime gameTime) {  }

        public void Draw(SpriteBatch batch)
        {

        }
        
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
