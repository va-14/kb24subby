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
    public class Chopper : IDamageableSprite
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


        //Chopper properties
        [DataMember]
        private int _counter;
        [DataMember]
        private int _damage;

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        [DataMember]
        public int DropSecond { get; set; }
        [DataMember]
        private int _lastSecond;
        [DataMember]
        public List<Missile> Missiles { get; set; }
        public int Score {
            get
            {
                return 100;
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
        public void CollisionWith(ISprite sprite)
        {
            if (sprite is Missile)
            {
                TakeDamage(((Missile)sprite).Damage);
            }
        }
        public void Update(GameTime gameTime)
        {
            if ((int)gameTime.TotalGameTime.TotalSeconds != _lastSecond)
            {
                _lastSecond = (int)gameTime.TotalGameTime.TotalSeconds;
                _counter++;
            }
            if (Health > 0)
            {
                if (_counter > DropSecond)
                {
                    DropMissile();
                }

                Position += new Vector2(_speed * (float)Math.Cos(Rotation), (_speed * (float)Math.Sin(Rotation)));
            }
        }


        //Chopper functions
        private void DropMissile()
        {
            if (Missiles != null)
            {
                if (Missiles.Count > 0) 
                {
                    Random random = new Random();
                    Missile missile = Missiles.FirstOrDefault();
                    Missiles.Remove(missile);
                    missile.Position = new Vector2(this.Position.X, this.Position.Y + 40);
                    missile.Speed = 5f;
                    missile.Rotation = random.Next(25,75);
                    DropSecond += random.Next(1,3);
                }
            }
        }

        

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }
}
