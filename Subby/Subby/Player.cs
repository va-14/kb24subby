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
        private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        private int _fuel;

        public int Fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
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

        public Player()
        {
            Fuel = 10000;
            Health = 100;
        }
        public void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(_fuel);
            if (_fuel > 0)
            {
                Position += new Vector2(_speed * (float)Math.Cos(Angle), (_speed * (float)Math.Sin(Angle)));
            }
            if (_speed > 0)
            {
                _speed -= 0.01f;
            }
            IsDamaged();
            return;
        }
        public void IsDamaged()
        {
            if (_health < 80)
            {
                Position += new Vector2(0, 0.1f);
            }
            if (_health < 60)
            {
                Position += new Vector2(0, 0.15f);
            }
            if (_health < 40)
            {
                Position += new Vector2(0, 0.2f);
            }
            if (_health < 20)
            {
                Position += new Vector2(0, 0.25f);
            }
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
        public void fillTank(int fuel)
        {
            _fuel += fuel;
        }
    }
}
