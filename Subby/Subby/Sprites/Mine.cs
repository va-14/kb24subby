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
    public class Mine : IExplodableSprite
    {
        //ISprite properties
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        public Texture2D Texture { get; set; }
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

        //Mine properties
        [DataMember]
        private Boolean _activated;
        private int _damage;
        [DataMember]
        public int Damage
        {
            get
            {
                int tmpDamage = _damage;
                _damage = 0;
                return tmpDamage;
            }
            set { _damage = value; }
        }
        private float _delay;
        [DataMember]
        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }
        [DataMember]
        public Boolean Exploded { get; set; }
        private int _range;
        [DataMember]
        public int Range
        {
            get
            {
                if (_range.Equals(null))
                    _range = 0;
                return _range;
            }
            set { _range = value; }
        }
        private float _timeSinceActivated;


        //ISprite functions
        public void CollisionWith(ISprite s)
        {

            _activated = true;
            if (Exploded && _timeSinceActivated < Delay +0.5 && _timeSinceActivated > Delay)
            {
                if (s.GetType().Name.Equals("Player"))
                {
                    Player p = (Player)s;
                    p.TakeDamage(Damage);
                }
            }

        }
        public void Update(GameTime gameTime)
        {

            if (_activated)
            {
                _timeSinceActivated += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeSinceActivated > Delay)
                {
                    //mine is exploded
                    Exploded = true;
                }
                else
                {
                    //mine counting for explosion
                    this.Color = Color.Red;
                }
            }
        }
    }
}
