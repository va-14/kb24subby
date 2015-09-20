using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby
{
    interface ISprite
    {
        Vector2 Position { get; set; }
        Color Color { get; set; }
        Texture2D Texture { get; set; }


        void Update(GameTime gameTime);

        void CollisionWith(ISprite s);
    }
}
