using Microsoft.Xna.Framework;
using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby.Strategies
{
    public class WallOfShots : IHostileSubStrategy
    {
        public void Move(HostileSub sub, int scrollingPosition)
        {
            sub.Rotation = 0;
            if (sub.Position.X < (scrollingPosition + sub.Subby.Position.X + 400))
            {
                sub.MoveRight();
            }
            if (sub.Position.X > (scrollingPosition + sub.Subby.Position.X + 430))
            {
                sub.StopMoveSideways();
                if (sub.Velocity.Y == 0)
                {
                    sub.MoveDown();
                }
            }
            if (sub.Position.Y < sub.Boundaries.Top + 30)
            {
                sub.MoveDown();
            }
            if (sub.Position.Y > sub.Boundaries.Bottom - 30)
            {
                sub.MoveUp();
            }
            sub.Position += sub.Velocity;
        }


        public bool Shoot(HostileSub sub, float totalTime)
        {
            if (totalTime > sub.ShootTimer)
            {
                sub.ShootTimer += 0.5f;
                return true;
            }
            return false;
        }
    }
}
