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
    class Wrak : ISprite
    {
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        
        private int _schade;
        [DataMember]
        public int Schade
        {
            get { return _schade; }
            set { _schade = value; }
        }

        private int _health;

        [DataMember]
        public int Health
        {
            get { return _health; }
            set { _health = value; }
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

    }
}
