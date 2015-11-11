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
        string currentLevel;
        KeyboardState oldState;
        bool paused;
        bool pauseKeyDown;
        bool levelEnd;
        bool showHighscores;
        Texture2D pausedTexture;
        Texture2D endOfLevelTexture;
        ClickableButton pausedResumeButton;
        ClickableButton pausedQuitButton;
        ClickableButton endNextLevelButton;
        ClickableButton endQuitButton;
        ClickableButton endReplayLevelButton;
        ClickableButton endHighscoresButton;
        ClickableButton highBackButton;

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
            currentLevel = "level1.xml";
            this.IsMouseVisible = true;
            paused = false;
            pauseKeyDown = false;
            levelEnd = false;
            showHighscores = false;
            pausedResumeButton = new ClickableButton();
            pausedResumeButton.Initialize();
            pausedQuitButton = new ClickableButton();
            pausedQuitButton.Initialize();
            endNextLevelButton = new ClickableButton();
            endNextLevelButton.Initialize();
            endQuitButton = new ClickableButton();
            endQuitButton.Initialize();
            endReplayLevelButton = new ClickableButton();
            endReplayLevelButton.Initialize();
            endHighscoresButton = new ClickableButton();
            endHighscoresButton.Initialize();
            highBackButton = new ClickableButton();
            highBackButton.Initialize();
            DeserializeLevel(currentLevel);
            base.Initialize();
            level.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pausedTexture = Content.Load<Texture2D>("paused");
            endOfLevelTexture = Content.Load<Texture2D>("endoflevel");
            Texture2D resumeButtonTexture = Content.Load<Texture2D>("resume");
            Texture2D quitButtonTexture = Content.Load<Texture2D>("quit");
            Texture2D nextLevelButtonTexture = Content.Load<Texture2D>("nextlevel");
            Texture2D replayLevelButtonTexture = Content.Load<Texture2D>("replaylevel");
            Texture2D highscoresButtonTexture = Content.Load<Texture2D>("highscores");
            Texture2D backButtonTexture = Content.Load<Texture2D>("back");
            pausedResumeButton.Load(resumeButtonTexture, GraphicsDevice, 
                new Vector2(GraphicsDevice.Viewport.Width / 2 - resumeButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - 40));
            pausedQuitButton.Load(quitButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - quitButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + 40));
            endNextLevelButton.Load(nextLevelButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - nextLevelButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80));
            endQuitButton.Load(quitButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - quitButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + 160));
            endReplayLevelButton.Load(replayLevelButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - replayLevelButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2));
            endHighscoresButton.Load(highscoresButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - highscoresButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + 80));
            highBackButton.Load(backButtonTexture, GraphicsDevice,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - backButtonTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 + 200));
            font = Content.Load<SpriteFont>("SpriteFontTemPlate");
            level.Load(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            CheckLevelEnd();

            if (levelEnd)
            {
                if (!showHighscores)
                {
                    endNextLevelButton.Update(mouse);
                    endQuitButton.Update(mouse);
                    endReplayLevelButton.Update(mouse);
                    endHighscoresButton.Update(mouse);

                    if (endNextLevelButton.IsClicked)
                    {
                        endNextLevelButton.IsClicked = false;
                        if (currentLevel == "level1.xml")
                        {
                            currentLevel = "level2.xml";
                        }
                        else if (currentLevel == "level2.xml")
                        {
                            currentLevel = "level1.xml";
                        }
                        StartLevel();
                    }
                    if (endQuitButton.IsClicked)
                    {
                        Exit();
                    }
                    if (endReplayLevelButton.IsClicked)
                    {
                        endReplayLevelButton.IsClicked = false;
                        StartLevel();
                    }
                    if (endHighscoresButton.IsClicked)
                    {
                        endHighscoresButton.IsClicked = false;
                        showHighscores = true;
                    }  
                }
                else
                {
                    highBackButton.Update(mouse);
                    if (highBackButton.IsClicked)
                    {
                        highBackButton.IsClicked = false;
                        showHighscores = false;
                    } 
                }
            }
            else
            {
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
                    if (pausedResumeButton.IsClicked)
                    {
                        EndPause();
                    }
                    if (pausedQuitButton.IsClicked)
                    {
                        Exit();
                    }
                    pausedResumeButton.Update(mouse);
                    pausedQuitButton.Update(mouse);
                }
            }   
        }
        
        private void ResetLevel()
        {
            DeserializeLevel(currentLevel);
            base.Initialize();
            level.Initialize();
        }

        private void DrawText()
        {
            spriteBatch.DrawString(font, "Health: " + level.Subby.Health, new Vector2(20, 45), Color.White);
            spriteBatch.DrawString(font, "Fuel: " + level.Subby.Fuel, new Vector2(20, 70), Color.White);
            spriteBatch.DrawString(font, "Bullits: " + level.Subby.Bullits, new Vector2(20, 95), Color.White);
            spriteBatch.DrawString(font, "Seconds: " + level.TotalRoundTime.ToString("0", CultureInfo.CurrentCulture), new Vector2(20, 120), Color.White);
            spriteBatch.DrawString(font, "Score: " + level.Score, new Vector2(20, 145), Color.White);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(); 
            
            level.Draw(spriteBatch);
            DrawText();

            if (levelEnd)
            {
                spriteBatch.Draw(endOfLevelTexture,
                    new Rectangle((GraphicsDevice.Viewport.Width / 2) - (endOfLevelTexture.Width / 2),
                        (GraphicsDevice.Viewport.Height / 2) - (endOfLevelTexture.Height / 2),
                        endOfLevelTexture.Width, endOfLevelTexture.Height), Color.White);

                if (!showHighscores)
                {                    
                    endNextLevelButton.Draw(spriteBatch);
                    endQuitButton.Draw(spriteBatch);
                    endReplayLevelButton.Draw(spriteBatch);
                    endHighscoresButton.Draw(spriteBatch);
                }
                else
                {
                    highBackButton.Draw(spriteBatch);
                    DrawHighscoreList(new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 130));
                }
                string scoreString = "Total Score: " + level.CalculateLevelScore();
                spriteBatch.DrawString(font, scoreString, 
                    new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 150), 
                    Color.White);
            }
            else
            {
                if (paused)
                {
                    spriteBatch.Draw(pausedTexture,
                        new Rectangle((GraphicsDevice.Viewport.Width / 2) - (pausedTexture.Width / 2),
                            (GraphicsDevice.Viewport.Height / 2) - (pausedTexture.Height / 2),
                            pausedTexture.Width, pausedTexture.Height), Color.White);
                    pausedResumeButton.Draw(spriteBatch);
                    pausedQuitButton.Draw(spriteBatch);
                }
            }
           
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHighscoreList(Vector2 position)
        {
            HighscoreList highscores = new HighscoreList();
            if (currentLevel == "level1.xml")
            {
                highscores = DeserializeHighscores("level1highscores.xml");
            }
            else if (currentLevel == "level2.xml")
            {
                highscores = DeserializeHighscores("level2highscores.xml");
            }

            spriteBatch.DrawString(font, "Highscores", position, Color.White);

            for (int i = 1; i < highscores.Highscores.Length + 1; i++)
            {
                spriteBatch.DrawString(font, i + ": " + highscores.Highscores[i - 1], new Vector2(position.X, position.Y + (i * 30)), Color.White);
            }
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
            if (state.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    Missile missile = level.Subby.Shoot();
                    Point position = level.PointOnCircle(level.Subby.Texture.Width / 2 + 30, (int)level.Subby.AngleDegrees, new Point((int)level.Subby.Position.X, (int)level.Subby.Position.Y));
                    level.CreateMissile(missile, position, 300);
                }
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
      

        private void SerializeLevel(string filePath)
        {
            using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                ser.WriteObject(writer, level);
            }
        }

        private void SerializeHighscores(string filePath, HighscoreList highscores)
        {
            using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(HighscoreList));
                ser.WriteObject(writer, highscores);
            }
        }

        private void SerializeScore(string filePath)
        {
            HighscoreList highscores = DeserializeHighscores(filePath);
            highscores.CompareScoreWithHighscores(level.CalculateLevelScore());
            SerializeHighscores(filePath, highscores);
        }

        private void DeserializeLevel(string filePath)
        {
            using (FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                level = (Level)ser.ReadObject(reader);
            }
        }

        private HighscoreList DeserializeHighscores(string filePath)
        {
            using (FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(HighscoreList));
                return (HighscoreList)ser.ReadObject(reader);
            }
        }

        private void BeginPause()
        {
            paused = true;
            pausedResumeButton.IsClicked = false;
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

        private void CheckLevelEnd()
        {
            if (level.Subby.Position.X + level.ScrollingPosition > level.EndPosition && levelEnd == false)
            {
                EndLevel();
            }
        }

        private void EndLevel()
        {
            levelEnd = true;
            if (currentLevel == "level1.xml")
            {
                SerializeScore("level1highscores.xml");
            }
            else if (currentLevel == "level2.xml")
            {
                SerializeScore("level2highscores.xml");
            }
        }

        private void StartLevel()
        {
            levelEnd = false;
            ResetLevel();
        }
    }
}
