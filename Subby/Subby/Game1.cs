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
        int scrollingPosition;
        GameBoundaries levelBoundaries;
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
            levelBoundaries = new GameBoundaries { Left = 20, Right = (float)graphics.PreferredBackBufferWidth - 200, Top = 200, Bottom = (float)graphics.PreferredBackBufferHeight };
            

            waves = new Waves();
            waves.Initialize();

            allSprites.Add(waves);
            //allSprites.Add(subby);

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
            subby.Update(gameTime);
            UpdateScrollingPosition();
            foreach (ISprite s in allSprites)
            {
                s.Update(gameTime);
            }

            scrollingBackground.UpdatePosition(scrollingPosition);

            base.Update(gameTime);
        }
        private void UpdateScrollingPosition()
        {
            Vector2 positionSubby = subby.Position;
            if (subby.Position.Y >= levelBoundaries.Bottom || subby.Position.Y <= levelBoundaries.Top || subby.Position.X <= levelBoundaries.Left)
            {
                subby.Speed = 0;
            }
            if (subby.Position.X >= levelBoundaries.Right)
            {
                scrollingPosition += (int)subby.Position.X - (int)levelBoundaries.Right;
                subby.Position = new Vector2(levelBoundaries.Right, subby.Position.Y);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(sky, new Vector2(0, 0), Color.White);
            DrawBackground(spriteBatch);
            subby.Draw(spriteBatch);
            foreach (ISprite s in allSprites)
            {
                spriteBatch.Draw(s.Texture, new Vector2(s.Position.X - (float)scrollingPosition, s.Position.Y), s.Color);
                //s.Draw(spriteBatch, GetDeflectedPlayerPosition());
            }
            
            spriteBatch.Draw(subby.Texture,subby.Position, null,subby.Color,subby.Angle,new Vector2(subby.Texture.Width/2, subby.Texture.Height/2),1f,SpriteEffects.None,1);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBackground(SpriteBatch batch)
        {
            batch.Draw(scrollingBackground.texture, scrollingBackground.position, null, Color.White, 0, scrollingBackground.origin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(scrollingBackground.texture, new Vector2(scrollingBackground.position.X + scrollingBackground.texture.Width, scrollingBackground.position.Y), null, Color.White, 0, scrollingBackground.origin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(scrollingBackground.texture, new Vector2(scrollingBackground.position.X + 2 * scrollingBackground.texture.Width, scrollingBackground.position.Y), null, Color.White, 0, scrollingBackground.origin, 1.0f, SpriteEffects.None, 0f);
            if (waves.Position.X < 1920)
            {
                batch.Draw(waves.Texture, waves.Position, waves.Color);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(waves.Texture, waves.Position - new Vector2(waves.Texture.Width, 0), waves.Color);
        
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
