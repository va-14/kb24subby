using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Subby.Sprites;

namespace Subby
{
    public interface IExplodableSprite :ISprite
    {
        Boolean Exploded { get; set; }
    }
}
