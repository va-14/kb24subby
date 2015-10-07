using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Subby.Sprites
{
    [Serializable]
    public class Player : ISprite
    {

        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        [XmlIgnore]
        public Texture2D Texture { get; set; }
        private float _positionDeflection;

        public float PositionDeflection
        {
            get { return _positionDeflection; }
            set { _positionDeflection = value; }
        }
        
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

        private GameBoundaries _boundaries;

        public void SetBoundaries(GameBoundaries boundaries)
        {
            _boundaries = boundaries;
        }

        public void Initialize()
        {
            Fuel = 10000;
            Health = 1000;
            Color = Color.White;
            Position = new Vector2(960, 590);
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }
        private void UpdatePosition()
        {
            if (_speed > 0.01)
            {
                _speed -= 0.01f;
            }
            if (_speed < -0.01)
            {
                _speed += 0.01f;
            }

            IsDamaged();

            Position += new Vector2(_speed * (float)Math.Cos(Angle), (_speed * (float)Math.Sin(Angle)));

            if (Position.Y >= _boundaries.Bottom || Position.Y <= _boundaries.Top || Position.X <= _boundaries.Left)
            {
                _speed = 0;
            }
            if (Position.X >= _boundaries.Right)
            {
                _positionDeflection += Position.X -_boundaries.Right;
                Position = new Vector2(_boundaries.Right, Position.Y);
            }
            System.Diagnostics.Debug.WriteLine(_positionDeflection);
        }
        public void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(_fuel);

            UpdatePosition();
            return;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Position, null, Color, Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 1);
        }
        public void IsDamaged()
        {

            System.Diagnostics.Debug.WriteLine(_health);
            if (_health < 800)
            {
                Position += new Vector2(0, 0.1f);
            }
            if (_health < 600)
            {
                Position += new Vector2(0, 0.15f);
            }
            if (_health < 400)
            {
                Position += new Vector2(0, 0.2f);
            }
            if (_health < 200)
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
            if (s.GetType().Name.Equals("TankStation"))
            {
                TankStation tankstation = (TankStation)s;
                fillTank(tankstation.getFuel());
            }
            if (s.GetType().Name.Equals("Wrak"))
            {
                Wrak wrak = (Wrak)s;
                SchadeAanBoot(wrak.Schade);
            }
        }
        public void SchadeAanBoot(int schade)
        {
            _health -= schade;
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
