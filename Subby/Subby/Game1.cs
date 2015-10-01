using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Subby.Sprites;

namespace Subby
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player subby;
        Waves waves;
        ScrollingBackground scrollingBackground;

        Texture2D sky;

        List<ISprite> allSprites;
        List<ISprite> allSpriteObstakels;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            allSprites = new List<ISprite>();
            allSpriteObstakels = new List<ISprite>();

            subby = new Player();
            subby.Initialize();
            subby.SetBoundaries(new GameBoundaries { Bottom = graphics.PreferredBackBufferHeight, Top = 200, Left = 20, Right = graphics.PreferredBackBufferWidth - 200 });
            

            waves = new Waves();
            waves.Initialize();

            allSprites.Add(waves);
            allSprites.Add(subby);

            scrollingBackground = new ScrollingBackground();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D subbyTexture = Content.Load<Texture2D>("subby");
            subby.Load(subbyTexture);

            Texture2D wavesTexture = Content.Load<Texture2D>("waves");
            waves.Load(wavesTexture);

            Texture2D scrollingBackgroundTexture = Content.Load<Texture2D>("ocean3");
            scrollingBackground.Load(GraphicsDevice, scrollingBackgroundTexture);

            sky = Content.Load<Texture2D>("sky");

            InitializeLevel1();
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

            scrollingBackground.Update(GetDeflectedPlayerPosition());

            base.Update(gameTime);
        }
        private Vector2 GetDeflectedPlayerPosition()
        {
            return new Vector2(subby.Position.X + subby.PositionDeflection, subby.Position.Y);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(sky, new Vector2(0, 0), Color.White);
            scrollingBackground.Draw(spriteBatch);
            
            foreach (ISprite s in allSprites)
            {
                //spriteBatch.Draw(s.Texture, s.Position, s.Color);
                s.Draw(spriteBatch);
            }
            
            //spriteBatch.Draw(subby.Texture,subby.Position, null,subby.Color,subby.Angle,new Vector2(subby.Texture.Width/2, subby.Texture.Height/2),1f,SpriteEffects.None,1);
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
            Rectangle rectball1 = new Rectangle((int)subby.Position.X, (int)subby.Position.Y, subby.Texture.Width, subby.Texture.Height); //to refactor real size of ISprite (30, 29)

            foreach (ISprite s in allSprites)
            {
                Rectangle rectSprite = new Rectangle((int)s.Position.X, (int)s.Position.Y, s.Texture.Width, s.Texture.Height); //to refactor get property of ISprite

                Rectangle overlap = Rectangle.Intersect(rectball1, rectSprite);
                if (!overlap.IsEmpty)
                {
                    //collision
                    s.CollisionWith(subby);
                    subby.CollisionWith(s);
                }
            }
        }

        private void InitializeLevel1()
        {
            TankStation t1 = new TankStation();
            t1.Color = Color.White;
            t1.Texture = Content.Load<Texture2D>("tankstation");
            t1.Tank = 300;
            t1.Position = new Vector2(300, 300);
            allSprites.Add(t1);

            Wrak w1 = new Wrak();
            w1.Color = Color.White;
            w1.Texture = Content.Load<Texture2D>("wrak");
            w1.Schade = 1;
            w1.Position = new Vector2(400, 350);
            allSprites.Add(w1);
        }
    }
}
