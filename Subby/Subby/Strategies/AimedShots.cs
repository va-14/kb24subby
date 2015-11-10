using Microsoft.Xna.Framework;
using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby.Strategies
{
    public class AimedShots : IHostileSubStrategy
    {
        public void Move(HostileSub sub, int scrollingPosition)
        {
            if (sub.Position.X < (scrollingPosition + sub.Subby.Position.X + 200))
            {
                sub.MoveRight();
            }
            if (sub.Position.X > (scrollingPosition + sub.Subby.Position.X + 800))
            {
                sub.MoveLeft();
            }
            if (sub.Position.Y < sub.Boundaries.Top + 100)
            {
                sub.MoveDown();
            }
            if (sub.Position.Y > sub.Boundaries.Bottom - 100)
            {
                sub.MoveUp();
            }

            sub.Position += sub.Velocity;

            float dX = sub.Position.X - (scrollingPosition + sub.Subby.Position.X);
            float dY = sub.Position.Y - sub.Subby.Position.Y;

            sub.Rotation = (float)Math.Atan2(dY, dX);
        }

        public bool Shoot(HostileSub sub, float totalTime)
        {
            if (totalTime > sub.ShootTimer)
            {
                sub.ShootTimer += 3;
                return true;
            }
            return false;
        }
    }
}
