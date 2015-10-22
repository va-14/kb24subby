using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Subby.Sprites
{
    [DataContract]
    public class Player : ISprite
    {

        private Color[] _textureData;

	    public Color[] TextureData
	    {
		    get { return _textureData;}
	    }
        private String _state;

        public String State
        {
            get { return _state; }
            set { _state = value; }
        }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }
        private int Bullits;
        [DataMember]
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

        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }
        
        private Texture2D _texture;
        public Texture2D TorpedoTexture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
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
        private float _positionDeflection;
        [DataMember]
        public float PositionDeflection
        {
            get { return _positionDeflection; }
            set { _positionDeflection = value; }
        }
        
        private int _health;
        [DataMember]
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        private int _fuel;
        [DataMember]
        public int Fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
        }

        private float _angle; // in degrees
        [DataMember]
        public float Rotation
        {
            get 
            {
                return ((float)Math.PI) * _angle / 180f;;  
            }
            set
            {
                _angle = value * 180.0f / ((float)Math.PI);
            }
        }
        public float AngleDegrees
        {
            get
            {
                return _angle;
            }
            set 
            {
                _angle = value * 180.0f / ((float)Math.PI);
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

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
            PivotPoint = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public Missile Shoot()
        {
            _bullits--;
            return new Missile { Speed = 5f, Damage = 50, Angle = this._angle, Color = Color.White };
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

            Position += new Vector2(_speed * (float)Math.Cos(Rotation), (_speed * (float)Math.Sin(Rotation)));

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

        private void Accelerate(float acceleration)
        {
            if ((_speed < 4 && _speed >= 0) || (_speed > -4 && _speed <= 0))
            {
                _speed += acceleration;
            }
        }
        public void GoFaster()
        {
            if (UseFuel(2))
            Accelerate(.05f);
        }
        
        public void GoSlower()
        {
            if (UseFuel(2))
             Accelerate(-.05f);
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
            if (s.GetType().Name.Equals("Waves"))
            {
                BoatBackInWater(_speed,_angle);
            }
        }
        private void BoatBackInWater(float speed, float angle)
        {

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
