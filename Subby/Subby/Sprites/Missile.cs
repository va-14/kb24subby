using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Subby.Sprites
{
    public class Missile : ISprite 
    {
        public Vector2 Position { get; set; }

        public Color Color { get; set; }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        
        public Texture2D Texture { get; set; }

        private int _damage;

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        

        private float _angle; // in degrees
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
