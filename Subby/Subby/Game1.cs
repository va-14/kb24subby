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
        //List<Rectangle> subbyRects;
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
            foreach (ISprite s in allSprites)
            {
                spriteBatch.Draw(s.Texture, new Vector2(s.Position.X - (float)scrollingPosition, s.Position.Y), s.Color);
            }
            
            spriteBatch.Draw(subby.Texture, subby.Position, null, subby.Color, subby.Angle, new Vector2(subby.Texture.Width / 2, subby.Texture.Height / 2), 1f, SpriteEffects.None, 1);

            /*
             * Dit is om de colision te zien
             * subbyRects = calculateSubbyRect();
            Texture2D dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            foreach (Rectangle r in subbyRects)
            {
                spriteBatch.Draw(dummyTexture, r, Color.White);
            }*/
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
            if (state.IsKeyDown(Keys.Space))
            {
                Missile s = subby.Shoot();
                s.Texture = Content.Load<Texture2D>("missile");
                s.Position = new Vector2(subby.Position.X + scrollingPosition, subby.Position.Y);
                allSprites.Add(s);
            }

            return false;
        }

        private List<Rectangle> calculateSubbyRect()
        {
            int pixels = 1; // deze kan maximaal op 10 voor een goede collision
            int widthRadius = subby.Texture.Width/2;
            int heightRadius = subby.Texture.Height/2;
            List<Rectangle> values = new List<Rectangle>();

            for (int x = 0; x < widthRadius; x = x + pixels)
            {
                for (int y = 0; y < heightRadius; y = y + pixels)
                {
                    Point radiusPoint = PointOnCircle(x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    Point point = PointOnCircle(y, (int)subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point.X, point.Y, pixels, pixels));
                    radiusPoint = PointOnCircle(x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point.X, point.Y, pixels, pixels));
                    radiusPoint = PointOnCircle(-x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point.X, point.Y, pixels, pixels));
                    radiusPoint = PointOnCircle(-x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point.X, point.Y, pixels, pixels));
                }
            }
            return values;
        }
        public static Point PointOnCircle(int radius, int angleInDegrees, Point origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            int x = (int)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            int y = (int)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new Point(x,y);
        }
        private void checkCollisions()
        {
            int boundingLength;
            if (subby.Texture.Width > subby.Texture.Height)
            {
                boundingLength = subby.Texture.Width;
            }
            else
            {
                boundingLength = subby.Texture.Height;
            }
            Rectangle rectSubby = new Rectangle((int)subby.Position.X - (boundingLength/2), (int)subby.Position.Y - (boundingLength/2), boundingLength, boundingLength);

            List<Rectangle> subbyRects = calculateSubbyRect();
            foreach (ISprite s in allSprites)
            {
                Rectangle rectSprite = new Rectangle((int)s.Position.X - scrollingPosition, (int)s.Position.Y, s.Texture.Width, s.Texture.Height); 

                Rectangle overlap = Rectangle.Intersect(rectSubby, rectSprite);
                if (!overlap.IsEmpty)
                {
                    //collision
                    foreach (Rectangle r in subbyRects)
                    {
                        Rectangle collisionCheck = Rectangle.Intersect(r, rectSprite);
                        if(!collisionCheck.IsEmpty)
                        {
                            s.CollisionWith(subby);
                            subby.CollisionWith(s);
                        }
                    }
                }
            }
        }

        private void InitializeLevel1()
        {
            allSprites.Add(new TankStation { Color = Color.White, Position = new Vector2(800, 500), Tank = 300, Texture = Content.Load<Texture2D>("tankstation") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(400, 350), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(3000, 550), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });
            allSprites.Add(new Wrak { Color = Color.White, Position = new Vector2(5000, 550), Schade = 1, Texture = Content.Load<Texture2D>("wrak") });
        }
    }
}
