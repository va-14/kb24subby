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

        private int Bullits;
        [DataMember]
        public int _bullits
        {
            get { return Bullits; }
            set { Bullits = value; }
        }
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D TorpedoTexture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
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
                return ((float)Math.PI) * _angle / 180.0f;
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
        [DataMember]
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
            TextureName = "subby";
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
            PivotPoint = new Vector2(Texture.Width / 2, Texture.Height / 2);
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
