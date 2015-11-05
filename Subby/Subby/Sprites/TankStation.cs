using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby.Sprites
{
    [DataContract]
    class TankStation : ISprite
    {
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }
        private int _health;

        [DataMember]
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
    
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        private int _tank;
        [DataMember]
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

    }
}
