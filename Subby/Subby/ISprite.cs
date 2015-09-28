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
        Vector2 position { get; set; }
        Color color { get; set; }
        Texture2D texture { get; set; }

        void Initialize();
        void Load(Texture2D texture);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch);

        void CollisionWith(ISprite s);
    }
}
