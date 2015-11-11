using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Subby.Sprites;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Globalization;
using Subby.Strategies;

namespace Subby
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Level level;
        KeyboardState oldState;
        bool paused;
        bool pauseKeyDown;
        bool levelEnd;
        Texture2D pausedTexture;
        ClickableButton resumeButton;
        ClickableButton quitButton;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            paused = false;
            pauseKeyDown = false;
            levelEnd = false;
            resumeButton = new ClickableButton();
            resumeButton.Initialize();
            quitButton = new ClickableButton();
            quitButton.Initialize();
            Deserialize("level1.xml");
            base.Initialize();
            level.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pausedTexture = Content.Load<Texture2D>("paused");
            Texture2D resumeButtonTexture = Content.Load<Texture2D>("resume");
            Texture2D quitButtonTexture = Content.Load<Texture2D>("quit");
            resumeButton.Load(resumeButtonTexture, GraphicsDevice, 
                new Vector2(GraphicsDevice.Viewport.Width / 2 - resumeButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - 40));
            quitButton.Load(quitButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - resumeButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + 40));
            font = Content.Load<SpriteFont>("SpriteFontTemPlate");
            level.Load(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            CheckPauseKey(Keyboard.GetState());

            if (!paused)
            {
                CheckKeys();
                level.Update(gameTime);
                if (!level.IsSubbyAlive())
                {
                    ResetLevel();
                }
                base.Update(gameTime);
            }
            else
            {
                if (resumeButton.IsClicked)
                {
                    EndPause();
                }
                if (quitButton.IsClicked)
                {
                    Exit();
                }
                resumeButton.Update(mouse);
                quitButton.Update(mouse);
            }
            
        }
        
        private void ResetLevel()
        {
            Initialize();
        }

        private void DrawText()
        {
            spriteBatch.DrawString(font, "Health: " + level.Subby.Health, new Vector2(20, 45), Color.White);
            spriteBatch.DrawString(font, "Fuel: " + level.Subby.Fuel, new Vector2(20, 70), Color.White);
            spriteBatch.DrawString(font, "Bullits: " + level.Subby.Bullits, new Vector2(20, 95), Color.White);
            spriteBatch.DrawString(font, "Seconds: " + level.TotalRoundTime.ToString("0", CultureInfo.CurrentCulture), new Vector2(20, 120), Color.White);
            spriteBatch.DrawString(font, "Score: " + level.Score, new Vector2(20, 155), Color.White);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(); 
            
            level.Draw(spriteBatch);
            DrawText();

            if (paused)
            {
                spriteBatch.Draw(pausedTexture, 
                    new Rectangle((GraphicsDevice.Viewport.Width / 2) - (pausedTexture.Width /2),
                        (GraphicsDevice.Viewport.Height / 2) - (pausedTexture.Height / 2), 
                        pausedTexture.Width, pausedTexture.Height), Color.White);
                resumeButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private Boolean CheckKeys()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up))
            {
                level.Subby.GoUp();
            }
            if (state.IsKeyDown(Keys.Down))
            {
                level.Subby.GoDown();
            }
            if (state.IsKeyDown(Keys.Right))
            {
                level.Subby.GoFaster();
            }
            if (state.IsKeyDown(Keys.Left))
            {
                level.Subby.GoSlower();
            }
            if (state.IsKeyDown(Keys.D1))
            {
                Deserialize("level1.xml");
                base.Initialize();
                level.Initialize();
            }
            if (state.IsKeyDown(Keys.D2))
            {
                Deserialize("level2.xml");
                base.Initialize();
                level.Initialize();
            }
            if (state.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    Missile missile = level.Subby.Shoot();
                    Point position = level.PointOnCircle(level.Subby.Texture.Width / 2 + 30, (int)level.Subby.AngleDegrees, new Point((int)level.Subby.Position.X, (int)level.Subby.Position.Y));
                    level.CreateMissile(missile, position, 300);
                }
            }
            if (state.IsKeyDown(Keys.P))
            {
                Serialize("level3.xml");
            }
            if (state.IsKeyDown(Keys.K))
            {
                foreach(ISprite sprite in level.SpriteList)
                {
                    if (sprite is HostileSub)
                    {
                        ((HostileSub)sprite).Strategy = new AimedShots();
                    }
                }
            }
            if (state.IsKeyDown(Keys.L))
            {
                foreach (ISprite sprite in level.SpriteList)
                {
                    if (sprite is HostileSub)
                    {
                        ((HostileSub)sprite).Strategy = new WallOfShots();
                    }
                }
            }

            oldState = state;

            return false;
        }
      

        private void Serialize(string filePath)
        {
            using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                ser.WriteObject(writer, level);
            }
        }

        private void Deserialize(string filePath)
        {
            using (FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                level = (Level)ser.ReadObject(reader);
            }
        }

        private void BeginPause()
        {
            paused = true;
            resumeButton.IsClicked = false;
        }

        private void EndPause()
        {
            paused = false;
        }

        private void CheckPauseKey(KeyboardState keyboardState)
        {
            bool pauseKeyDownThisFrame = keyboardState.IsKeyDown(Keys.Escape);

            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused)
                    BeginPause();
                else
                    EndPause();
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }

        private void EndLevel()
        {
            if (level.Subby.Position.X == level.End)
            {
                levelEnd = true;
            }
        }

        private void StartLevel(string level)
        {
            levelEnd = false;
            Deserialize(level);
        }
    }
}
