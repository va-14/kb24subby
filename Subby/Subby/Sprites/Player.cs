﻿using System;
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
    public class Player : IDamageableSprite
    {
        //ISprite properties
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        private int _health;
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public float Rotation
        {
            get
            {
                return ((float)Math.PI) * _angle / 180f; ;
            }
            set
            {
                _angle = value * 180.0f / ((float)Math.PI);
            }
        }
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

        //Player properties
        private float _angle; // in degrees
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
        [DataMember]
        private int _bullits;
        public int Bullits
        {
            get { return _bullits; }
            set { _bullits = value; }
        }
        [DataMember]
        private int _fuel;
        public int Fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
        }
        [DataMember]
        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        //ISprite functions
        public void CollisionWith(ISprite sprite)
        {
            if (sprite is TankStation)
            {
                FillTank(((TankStation)sprite).GetFuel());
            }
            if (sprite is Wrak)
            {
                TakeDamage(((Wrak)sprite).Damage);
            }
            if (sprite is Missile)
            {
                TakeDamage(((Missile)sprite).Damage);
            }
        }
        public void Update(GameTime gameTime)
        {
            UpdatePosition();
        }


        //Player functions
        private void Accelerate(float acceleration)
        {
            if ((_speed < 4 && _speed >= 0) || (_speed > -4 && _speed <= 0))
            {
                _speed += acceleration;
            }
        }
        public void FillTank(int fuel)
        {
            _fuel += fuel;
        }
        private Missile GetBullit()
        {
            if (Bullits > 0)
            {
                Bullits--;
                return new Missile { Speed = 7f, Rotation = this._angle, Color = Color.White };
            }
            return null;
        }
        public void GoDown()
        {
            if (UseFuel(1))
                Rotate(1);
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
        public void GoUp()
        {
            if (UseFuel(1))
            Rotate(-1);
        }
        public Vector2 GetDamagedPositionBehavour()
        {
            float snelheid = 0;
            if (_health < 800)
                snelheid = 0.25f;

            if (_health < 600)
                snelheid = 0.4f;   

            if (_health < 400)
                snelheid = 0.6f;   

            if (_health < 200)
                snelheid = 0.9f;   
            
            return new Vector2(0, snelheid);
        }
        private void Rotate(int degrees)
        {
            _angle += degrees;
            while (this._angle < 0) this._angle += 360;
            while (this._angle > 359) this._angle -= 360;
        }
        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
        public Missile Shoot()
        {
            return GetBullit();
        }
        public void UpdatePosition()
        {
            if (_speed > 0.01)
                _speed -= 0.01f;
            if (_speed < -0.01)
                _speed += 0.01f;

            Position += GetDamagedPositionBehavour();

            Position += new Vector2(_speed * (float)Math.Cos(Rotation), (_speed * (float)Math.Sin(Rotation)));
        }
        public bool UseFuel(int fuel)
        {
            if (_fuel > 0)
            {
                _fuel -= fuel;
                return true;
            }
            return false;
        }
    }
}
