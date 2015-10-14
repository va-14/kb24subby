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
    class Wrak : ISprite
    {
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        [DataMember]
        public string TextureName { get; set; }
        [DataMember]
        public float Rotation { get; set; }
        [DataMember]
        public Vector2 PivotPoint { get; set; }
        
        private int _schade;
        [DataMember]
        public int Schade
        {
            get { return _schade; }
            set { _schade = value; }
        }
        

        public void Update(GameTime gameTime) { }

        public void CollisionWith(ISprite s)
        {

        }

        public void Initialize()
        {
            Color = Color.White;
            //Position = new Vector2(960, 270);
        }

        public void Load(Texture2D _texture)
        {
            Texture = _texture;
        }


        public void Draw(SpriteBatch batch, Vector2 positionDeflection)
        {
            batch.Draw(Texture, new Vector2(Position.X - positionDeflection.X, Position.Y), Color);
        }

    }
}
