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
    public class HostileSub : ISprite
    {
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Vector2 Velocity { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
        public int Health { get; set; }
        public Player Subby;
        public GameBoundaries Boundaries;
        public HostileSubStrategy strategy { get; set; }
        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        public void Initialize()
        {

        }

        public void Load(Player subby, GameBoundaries boundaries)
        {
            Subby = subby;
            Boundaries = boundaries;
        }

        public void Update(GameTime gameTime)
        {
            strategy.Move(this);
        }

        public void CollisionWith(ISprite s)
        {

        }

        public void Move()
        {
            if (Position.X < Subby.Position.X + 100)
            {
                MoveRight();
            }
            if (Position.X > Subby.Position.X + 500)
            {
                MoveLeft();
            }
            if (Position.Y < Boundaries.Top)
            {
                MoveDown();
            }
            if (Position.Y > Boundaries.Bottom)
            {
                MoveUp();
            }

            Position += Velocity;
        }

        public void MoveLeft()
        {
            Velocity = new Vector2(-1, Velocity.Y);
        }

        public void MoveRight()
        {
            Velocity = new Vector2(1, Velocity.Y);
        }

        public void MoveUp()
        {
            Velocity = new Vector2(Velocity.X, -1);
        }

        public void MoveDown()
        {
            Velocity = new Vector2(Velocity.X, 1);
        }
    }
}
