using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Subby
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player subby;

        List<ISprite> allSprites;
        List<ISprite> allSpriteObstakels;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            allSprites = new List<ISprite>();
            allSpriteObstakels = new List<ISprite>();

            subby = new Player();
            subby.Color = Color.Black;
            //allSprites.Add(subby);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            subby.Texture = Content.Load<Texture2D>("subby");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            checkKeys();

            checkCollisions();

            foreach (ISprite s in allSprites)
            {
                s.Update(gameTime);
            }
            subby.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (ISprite s in allSprites)
            {
                spriteBatch.Draw(s.Texture, s.Position, s.Color);
            }
            spriteBatch.Draw(subby.Texture,subby.Position, null,subby.Color,subby.Angle,new Vector2(subby.Texture.Width/2, subby.Texture.Height/2),1f,SpriteEffects.None,1);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Boolean checkKeys()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up))
            {
                subby.GoUp();
            }
            if (state.IsKeyDown(Keys.Down))
            {
                subby.GoDown();
            }
            if (state.IsKeyDown(Keys.Right))
            {
                subby.GoFaster();
            }
            if (state.IsKeyDown(Keys.Left))
            {
                subby.GoSlower();
            }
            if (state.IsKeyDown(Keys.S))
            {
                subby.Start();
            }

            return false;
        }


        private void checkCollisions()
        {
            Rectangle rectball1 = new Rectangle((int)subby.Position.X, (int)subby.Position.Y, 30, 29); //to refactor real size of ISprite (30, 29)

            foreach (ISprite s in allSpriteObstakels)
            {
                Rectangle rectSprite = new Rectangle((int)s.Position.X, (int)s.Position.Y, 82, 46); //to refactor get property of ISprite

                Rectangle overlap = Rectangle.Intersect(rectball1, rectSprite);
                if (!overlap.IsEmpty)
                {
                    //collision
                    s.CollisionWith(subby);
                    subby.CollisionWith(s);


                }

            }
        }
    }
}
