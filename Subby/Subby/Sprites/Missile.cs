using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Subby.Sprites
{
    class Missile : ISprite
    {
        public Vector2 Position { get; set; }

        public Color Color { get; set; }

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
            _damage = 50;
            //Texture = .Load<Texture2D>("wrak");
        }

        public void Load(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch batch, Vector2 positionDeflection)
        {
        }

        public void CollisionWith(ISprite s)
        {
        }
    }
}
