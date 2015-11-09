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
    public class Missile : IExplodableSprite
    {
        //ISprite properties
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        public int Width
        {
            get { return Texture.Width; }
        }
        public int Height
        {
            get { return Texture.Height; }
        }

        //Missile properties
        [DataMember]
        public Boolean Active { get; set; }
        [DataMember]
        private int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        public Boolean Exploded { get; set; }
        [DataMember]
        private float _rotation; // in degrees
        public float Rotation
        {
            set
            {
                _rotation = value;
            }
            get
            {
                return ((float)Math.PI) * _rotation / 180.0f;
            }
        }
        [DataMember]
        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }



        //ISprite functions
        public void Update(GameTime gameTime)
        {

            Position += new Vector2(_speed * (float)Math.Cos(Rotation), (_speed * (float)Math.Sin(Rotation)));
        }
        public void CollisionWith(ISprite s)
        {
            Exploded = true;
        }
    }
}
