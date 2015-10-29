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
    public class Chopper : ISprite
    {
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }

        public List<Missile> Missiles { get; set; }

        public int DropSecond { get; set; }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }
        private int _health;

        [DataMember]
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }

        private int _damage;
        [DataMember]
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }


        private float _angle; // in degrees
        [DataMember]
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
        [DataMember]
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        public void Initialize()
        {
        }

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
                    missile.Angle = random.Next(25,75);
                    DropSecond += random.Next(1,3);
                }
            }
        }
        public void Load(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if (Health > 0)
            {
                if (gameTime.TotalGameTime.TotalSeconds > DropSecond)
                {
                    DropMissile();
                }

                Position += new Vector2(_speed * (float)Math.Cos(Angle), (_speed * (float)Math.Sin(Angle)));
            }
        }


        public void Schade(int schade)
        {
            _health -= schade;
        }
        public void CollisionWith(ISprite s)
        {
            if (s.GetType().Name.Equals("Missile"))
            {
                Missile missile = (Missile)s;
                Schade(missile.Damage);
            }
        }
    }
}
