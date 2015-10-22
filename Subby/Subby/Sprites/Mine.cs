using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Subby;

namespace Subby.Sprites
{
     [DataContract]
    public class Mine : ISprite
    {
        private Vector2 _position;
        public Vector2 Position {
            get {
                return new Vector2(_position.X, _position.Y);
            }
            set
            {
                _position = value;
            } 
        }

        private float _delay;

        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }

        public Boolean exploded { get; set; }

        private float _timeSinceActivated;

        private Boolean activated;

        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        

        private int _damage;

        [DataMember]
        public int Damage
        {
            get {
                int tmpDamage = _damage;
                _damage = 0;
                return tmpDamage; }
            set { _damage = value; }
        }

        private int _range;

        public int Range
        {
            get {
                if (_range.Equals(null))
                    _range = 0;
                return _range; }
            set { _range = value; }
        }
        

        private float _angle; // in degrees
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
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int Width
        {
            get { return Texture.Width + Range; }
        }

        public int Height
        {
            get { return Texture.Height + Range; }
        }

        public Mine()
        {
            activated = false;
            Range = 200;
            _delay = 2f;
            exploded = false;
            _damage = 2000;
        }
        public void Initialize()
        {
        }

        public void Load(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            
            if (activated)
            {
                _timeSinceActivated += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeSinceActivated > Delay)
                {
                    //mine is exploated
                    exploded = true;
                    this.Color = Color.Black;
                }
                else
                {
                    //mine counting for explosion
                    this.Color = Color.Red;
                }
            }

        }


        public void CollisionWith(ISprite s)
        {
            activated = true;
            if (activated && exploded)
            {
                int damage = Damage;
                if (s.GetType().Name.Equals("Player"))
                {
                    Player p = (Player)s;
                    p.SchadeAanBoot(damage);
                }
            }

        }
    }
}
