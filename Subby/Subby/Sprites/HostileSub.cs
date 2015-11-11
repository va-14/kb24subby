using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Subby.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Subby.Sprites
{
    [DataContract]
    public class HostileSub : IDamageableSprite
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
                return ((float)Math.PI) * AngleDeg / 180f; ;
            }
            set
            {
                AngleDeg = value * 180.0f / ((float)Math.PI);
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

        //HostileSub properties
        public float AngleDeg { get; set; }
        public LevelBoundaries Boundaries;
        [DataMember]
        public Boolean Active { get; set; }
        public int Score
        {
            get
            {
                return 1000;
            }
        }
        public int ScrollingPosition { get; set; }
        [DataMember]
        public float ShootTimer { get; set; }
        public IHostileSubStrategy Strategy { get; set; }
        public Player Subby;
        [DataMember]
        public Vector2 Velocity { get; set; }


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
            if (Active)
            Strategy.Move(this, ScrollingPosition);
        }


        //HostileSub functions
        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
        public void Load(Player subby, LevelBoundaries boundaries)
        {
            Subby = subby;
            Boundaries = boundaries;
        }
        public void MoveDown()
        {
            Velocity = new Vector2(Velocity.X, 4);
        }
        public void MoveLeft()
        {
            Velocity = new Vector2(-4, Velocity.Y);
        }
        public void MoveRight()
        {
            Velocity = new Vector2(4, Velocity.Y);
        }
        public void MoveUp()
        {
            Velocity = new Vector2(Velocity.X, -4);
        }
        public bool Shoot(float totalTime)
        {
            return Strategy.Shoot(this, totalTime);
        }
        public void StopMoveSideways()
        {
            Velocity = new Vector2(0, Velocity.Y);
        }
    }
}
