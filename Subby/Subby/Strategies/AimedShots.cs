using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby.Strategies
{
    public class AimedShots : HostileSubStrategy
    {
        public override void Move(HostileSub sub)
        {
            if (sub.Position.X < sub.Subby.Position.X + 100)
            {
                sub.MoveRight();
            }
            if (sub.Position.X > sub.Subby.Position.X + 500)
            {
                sub.MoveLeft();
            }
            if (sub.Position.Y < sub.Boundaries.Top)
            {
                sub.MoveDown();
            }
            if (sub.Position.Y > sub.Boundaries.Bottom)
            {
                sub.MoveUp();
            }

            sub.Position += sub.Velocity;

            float dX = sub.Position.X - sub.Subby.Position.X;
            float dY = sub.Position.Y - sub.Subby.Position.Y;

            float angle = (float)Math.Atan2(dY, dX);
            sub.Rotation = (float)(Math.PI * angle / 180.0);
        }
    }
}
