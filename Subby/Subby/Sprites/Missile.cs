using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Subby.Sprites
{
    [DataContract]
    public class Missile : ISprite
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

        private int _damage;
        [DataMember]
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        

        private float _angle; // in degrees
        [DataMember]
        public float Angle
        {
            set
            {
                _angle = value;
            }
            get
            {
                return ((float)Math.PI) * _angle / 180.0f;
            }
        }

        private float _speed;
        [DataMember]
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        public void Initialize()
        {
        }

        public void Load(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {

            Position += new Vector2(_speed * (float)Math.Cos(Angle), (_speed * (float)Math.Sin(Angle)));
        }


        public void CollisionWith(ISprite s)
        {
        }
    }
}
