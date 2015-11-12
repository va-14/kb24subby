using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby.Sprites
{
    [DataContract]
    public class TankStation : ISprite
    {
        //ISprite properties
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        public int Width
        {
            get { return Texture.Width; }
        }
        public int Height
        {
            get { return Texture.Height; }
        }


        //FillingStation properties
        [DataMember]
        public int Tank { get; set; }

        //ISprite functions
        public void CollisionWith(ISprite s)
        {

        }
        public void Update(GameTime gameTime) {  }


        //FillingStation functions
        public int GetFuel()
        {
            int tmpTank = Tank;
            EmptyTank();
            return tmpTank;
        }
        private void EmptyTank()
        {
            Tank = 0;
            Color = Color.Black ;
        }

    }
}
