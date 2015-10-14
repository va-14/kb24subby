using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Subby.Sprites;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace Subby
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player subby;
        int scrollingPosition;
        GameBoundaries levelBoundaries;
        Background background;

        List<ISprite> allSprites;
        List<ISprite> allSpriteObstakels;

        Level level1;

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
            //level1 = new Level();
            //level1.Initialize();
            //subby = new Player();
            //level1.SpriteList.Add(subby);
            //subby.Initialize();
            //levelBoundaries = new GameBoundaries { Left = 20, Right = (float)graphics.PreferredBackBufferWidth - 200, Top = 200, Bottom = (float)graphics.PreferredBackBufferHeight };
            //level1.LevelBoundaries = new Vector4((float)graphics.PreferredBackBufferWidth - 200, 200, (float)graphics.PreferredBackBufferHeight, 20);
            //background = new Background();
            //background.Initialize();
            //level1.Background = background;

            using (FileStream reader = new FileStream("level1.xml", FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                level1 = (Level)ser.ReadObject(reader);
            }
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Texture2D subbyTexture = Content.Load<Texture2D>("subby");
            //Texture2D wavesTexture = Content.Load<Texture2D>("waves");
            //Texture2D scrollingBackgroundTexture = Content.Load<Texture2D>("ocean3");
            //Texture2D sky = Content.Load<Texture2D>("sky");
            //subby.Load(subbyTexture);
            //background.Load(GraphicsDevice, scrollingBackgroundTexture, wavesTexture, sky);
            //InitializeLevel1();
            level1.Load(Content, GraphicsDevice);
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

            //checkCollisions();
            //subby.Update(gameTime);
            //UpdateScrollingPosition();
            //foreach (ISprite s in allSprites)
            //{
            //    s.Update(gameTime);
            //}

            //background.UpdatePosition(scrollingPosition, gameTime);

            level1.Update(gameTime);

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
            //DrawBackground(spriteBatch);
            ////background.Draw(spriteBatch);
            //foreach (ISprite s in allSprites)
            //{
            //    spriteBatch.Draw(s.Texture, new Vector2(s.Position.X - (float)scrollingPosition, s.Position.Y), s.Color);
            //}

            //spriteBatch.Draw(subby.Texture, subby.Position, null, subby.Color, subby.Rotation, new Vector2(subby.Texture.Width / 2, subby.Texture.Height / 2), 1f, SpriteEffects.None, 1);
            level1.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBackground(SpriteBatch batch)
        {
            batch.Draw(background.WaterTexture, background.WaterPosition, null, Color.White, 0, background.WaterOrigin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(background.WaterTexture, new Vector2(background.WaterPosition.X + background.WaterTexture.Width, background.WaterPosition.Y), null, Color.White, 0, background.WaterOrigin, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(background.WaterTexture, new Vector2(background.WaterPosition.X + 2 * background.WaterTexture.Width, background.WaterPosition.Y), null, Color.White, 0, background.WaterOrigin, 1.0f, SpriteEffects.None, 0f);
            if (background.WavesPosition.X < GraphicsDevice.Viewport.Width)
            {
                batch.Draw(background.WavesTexture, background.WavesPosition, Color.White);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(background.WavesTexture, background.WavesPosition - new Vector2(background.WavesTexture.Width, 0), Color.White);
            batch.Draw(background.SkyTexture, new Vector2(0, 0), Color.White);
        
        }
        private Boolean checkKeys()
        {
            KeyboardState state = Keyboard.GetState();

            Player subby = null;

            foreach (ISprite sprite in level1.SpriteList)
            {
                if (sprite is Player)
                {
                    subby = (Player)sprite;
                }
            }

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
            if (state.IsKeyDown(Keys.Space))
            {
                Missile s = subby.Shoot();
                s.Texture = Content.Load<Texture2D>("missile");
                s.Position = new Vector2(subby.Position.X + scrollingPosition, subby.Position.Y);
                level1.SpriteList.Add(s);
                allSprites.Add(s);
            }
            if (state.IsKeyDown(Keys.P))
            {
                using (FileStream writer = new FileStream("level1.xml", FileMode.Create, FileAccess.Write))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                    ser.WriteObject(writer, level1);
                }
            }

            return false;
        }


        private void checkCollisions()
        {
            Rectangle rectball1 = new Rectangle((int)subby.Position.X, (int)subby.Position.Y, subby.Texture.Width, subby.Texture.Height); //to refactor real size of ISprite (30, 29)

            foreach (ISprite s in allSprites)
            {
                Rectangle rectSprite = new Rectangle((int)s.Position.X - scrollingPosition, (int)s.Position.Y, s.Texture.Width, s.Texture.Height); //to refactor get property of ISprite

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
            allSprites.Add(new TankStation { Color = Color.White, Position = new Vector2(600, 450), Tank = 300, Texture = Content.Load<Texture2D>("tankstation") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(400, 350), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(3000, 550), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(5000, 550), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });

            level1.SpriteList.Add(new TankStation { Color = Color.White, Position = new Vector2(600, 450), PivotPoint = new Vector2(0, 0), Tank = 300, TextureName = "tankstation", Texture = Content.Load<Texture2D>("tankstation") });
            level1.SpriteList.Add(new Wrak { Color = Color.White, Position = new Vector2(400, 350), PivotPoint = new Vector2(0, 0), Schade = 1, TextureName = "wrak", Texture = Content.Load<Texture2D>("wrak") });
            level1.SpriteList.Add(new Wrak { Color = Color.White, Position = new Vector2(3000, 550), PivotPoint = new Vector2(0, 0), Schade = 1, TextureName = "wrak", Texture = Content.Load<Texture2D>("wrak") });
            level1.SpriteList.Add(new Wrak { Color = Color.White, Position = new Vector2(5000, 550), PivotPoint = new Vector2(0, 0), Schade = 1, TextureName = "wrak", Texture = Content.Load<Texture2D>("wrak") });
        }

    }
}
