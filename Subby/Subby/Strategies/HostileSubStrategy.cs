using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby.Strategies
{
    public abstract class HostileSubStrategy
    {
        public abstract void Move(HostileSub sub);
    }
}
