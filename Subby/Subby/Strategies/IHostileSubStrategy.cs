using Microsoft.Xna.Framework;
using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby.Strategies
{
    public interface IHostileSubStrategy
    {
        void Move(HostileSub sub, int scrollingPosition);
        bool Shoot(HostileSub sub, float totalTime);
    }
}
