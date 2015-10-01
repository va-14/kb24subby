using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Subby.Sprites;

namespace Subby
{
    interface ISprite
    {
        Vector2 Position { get; set; }
        Color Color { get; set; }
        Texture2D Texture { get; set; }

        void Initialize();
        void Load(Texture2D texture);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch, Vector2 positionDeflection);

        void CollisionWith(Player s);
    }
}
