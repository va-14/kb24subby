using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby
{
    class Player : ISprite
    {
        
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public int Fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
        }
        public float Angle
        {
            get
            {
                return ((float)Math.PI) * _angle / 180.0f;
            }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        

        private float _speed;
        private float _angle; // in degrees
        private int _fuel;

        public Player()
        {
            Fuel = 1000;
        }
        public void Update(GameTime gameTime)
        {

            if (_fuel > 0)
            {
                Position += new Vector2(_speed * (float)Math.Cos(Angle), (_speed * (float)Math.Sin(Angle)));
            }
            return;
        }

        public void GoUp()
        {
            if (UseFuel(1))
            _angle -= 1;
        }

        public void GoDown()
        {
            if (UseFuel(1))
            _angle += 1;
        }

        public void GoFaster()
        {
            if (UseFuel(2))
            _speed += .05f;
        }
        
        public void GoSlower()
        {
            if (UseFuel(2))
            _speed -= .05f;
        }

        public bool UseFuel(int fuel)
        {
            if (_fuel >= fuel)
            {
                _fuel -= fuel;
                return true;
            }
            return false;
        }

        public void CollisionWith(ISprite s)
        {
            
        }

        public void Start()
        {
            // geen start implemented
        }
        
    }
}
