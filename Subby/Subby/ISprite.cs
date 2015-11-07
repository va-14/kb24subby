using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Subby.Sprites;

namespace Subby
{
    public interface ISprite
    {
        Color Color { get; set; }
        int Health { get; set; }
        Vector2 PivotPoint { get; set; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Texture2D Texture { get; set; }
        string TextureName { get; set; }
        int Width { get; }
        int Height { get; }

        void CollisionWith(ISprite s);
        void Update(GameTime gameTime);
    }
}
