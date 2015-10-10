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

        private Color[] _textureData;

	    public Color[] TextureData
	    {
		    get { return _textureData;}
	    }


        private int Bullits;

        public int _bullits
        {
            get { return Bullits; }
            set { Bullits = value; }
        }
        private Vector2 _origin;

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
<<<<<<< HEAD
        private Texture2D _texture;
        public Texture2D Texture {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
                _textureData = new Color[value.Width * value.Height];
                _origin = new Vector2(value.Width / 2, value.Height / 2);
            }
        }
=======
        [XmlIgnore]
        public Texture2D Texture { get; set; }
>>>>>>> origin/master
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
                return ((float)Math.PI) * _angle / 180f;;  
            }
        }
        public float AngleDegrees
        {
            get
            {
                return _angle;
            }
        }

        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Vector4 _boundaries;

        public void SetBoundaries(Vector4 boundaries)
        {
            _boundaries = boundaries;
        }

        public void Initialize()
        {
            Fuel = 10000;
            Health = 1000;
            Color = Color.White;
            Position = new Vector2(120, 590);
            _bullits = 100;
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }

        public Missile Shoot()
        {
            _bullits--;
            return new Missile { Speed = 5f, Damage = 50, Angle = this._angle, Color = Color.White};
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

            System.Diagnostics.Debug.WriteLine(_positionDeflection);
        }
        public void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(_fuel);

            UpdatePosition();
            return;
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
            Rotate(-1);
        }

        public void GoDown()
        {
            if (UseFuel(1))
            Rotate(1);
        }

        private void Rotate(int degrees)
        {
            _angle += degrees;
            while (this._angle < 0) this._angle += 360;
            while (this._angle > 359) this._angle -= 360;
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
