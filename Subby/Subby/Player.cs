using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Subby
{
    class Player : ISprite
    {
        
        public Vector2 position { get; set; }
        public Color color { get; set; }
        public Texture2D texture { get; set; }
        public int fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
        }
        public float angle
        {
            get
            {
                return ((float)Math.PI) * _angle / 180.0f;
            }
        }
        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        

        private float _speed;
        private float _angle; // in degrees
        private int _fuel;

        public void Initialize()
        {
            fuel = 1000;
            color = Color.White;
            position = new Vector2(960, 590);
        }

        public void Load(Texture2D _texture)
        {
            texture = _texture;
        }

        public void Update(GameTime gameTime)
        {

            if (_fuel > 0)
            {
                position += new Vector2(_speed * (float)Math.Cos(angle), (_speed * (float)Math.Sin(angle)));
            }
            return;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, color, angle, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 1);
        }

        public void GoUp()
        {
            if (UseFuel(1))
            _angle -= 1;
        }

        public void GoDown()
        {
            if (UseFuel(1))
            _angle += 1;
        }

        public void GoFaster()
        {
            if (UseFuel(2))
            _speed += .05f;
        }
        
        public void GoSlower()
        {
            if (UseFuel(2))
            _speed -= .05f;
        }

        public bool UseFuel(int fuel)
        {
            if (_fuel >= fuel)
            {
                _fuel -= fuel;
                return true;
            }
            return false;
        }

        public void CollisionWith(ISprite s)
        {
            
        }

        public void Start()
        {
            // geen start implemented
        }
        
    }
}
