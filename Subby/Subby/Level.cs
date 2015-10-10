using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Subby.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Subby
{
    [Serializable]
    public class Level
    {
        public Player Subby;
        [XmlIgnore]
        public List<ISprite> SpriteList;
        [XmlIgnore]
        public Background Background;
        public int[] Highscores;
        public string SubbyTextureName;

        public void Initialize()
        {
            SpriteList = new List<ISprite>();
            Highscores = new int[10];
            Subby = new Player();
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            Subby.Update(gameTime);
            foreach (ISprite sprite in SpriteList)
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
<<<<<<< HEAD
            //Subby.Draw(batch);
            foreach (ISprite sprite in SpriteList)
            {
              //  sprite.Draw(batch);
            }
        }

    }
}
=======
            
        }

    }
}
>>>>>>> origin/master
