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
    class Mine : ISprite
    {
        private Vector2 _position;

        [DataMember]
        public Vector2 Position
        {
            get; set;
        }

        private float _delay;

        [DataMember]
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


        [DataMember]
        public Boolean Exploded { get; set; }

        private float _timeSinceActivated;


        [DataMember]
        private Boolean _activated;
        [DataMember]
        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        private int _health;

        [DataMember]
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

        [DataMember]
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
            get
            {
                if (Texture != null) 
                    return Texture.Width + Range; 
                else
                    return 0; 
            }
        }

        public int Height
        {
            get
            {
                if (Texture != null)
                    return Texture.Height + Range;
                else
                    return 0;
            }
        }

        public Mine()
        {
           /* activated = false;
            Range = 200;
            _delay = 2f;
            exploded = false;
            _damage = 2000;*/
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
            
            if (_activated)
            {
                _timeSinceActivated += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeSinceActivated > Delay)
                {
                    //mine is exploated
                    Exploded = true;
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

            _activated = true;
            if (Exploded && _timeSinceActivated < Delay +0.5 && _timeSinceActivated > Delay)
            {
                if (s.GetType().Name.Equals("Player"))
                {
                    Player p = (Player)s;
                    p.SchadeAanBoot(Damage);
                }
            }

        }
    }
}
