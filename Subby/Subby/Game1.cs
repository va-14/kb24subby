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

namespace Subby
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Player subby;
        Background background;
        Level level;

        KeyboardState oldState;

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
            using (FileStream reader = new FileStream("level1.xml", FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                level = (Level)ser.ReadObject(reader);
            }
            foreach (ISprite sprite in level.SpriteList)
            {
                if (sprite is Player)
                {
                    subby = (Player)sprite;
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("SpriteFontTemPlate");
            level.Load(Content, GraphicsDevice);
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
            level.Update(gameTime);

            base.Update(gameTime);
        }
        private void DrawText()
        {
            spriteBatch.DrawString(font, "Health: " + subby.Health, new Vector2(20, 45), Color.White);
            spriteBatch.DrawString(font, "Fuel: " + subby.Fuel, new Vector2(20, 70), Color.White);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(); 
            
            level.Draw(spriteBatch);
            DrawText();
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
            if (state.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    createMissile();
                }
            }
            if (state.IsKeyDown(Keys.P))
            {
                using (FileStream writer = new FileStream("level1.xml", FileMode.Create, FileAccess.Write))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(Level));
                    ser.WriteObject(writer, level);
                }
            }

            oldState = state;

            return false;
        }

        private void createMissile()
        {
            Missile s = subby.Shoot();
            s.Texture = Content.Load<Texture2D>("missile");
            s.Position = new Vector2(subby.Position.X + level.ScrollingPosition, subby.Position.Y);
            level.SpriteList.Add(s);
            if (level.MissileList == null)
                level.MissileList = new List<Missile>();
            level.MissileList.Add(s);
        }
        private List<Rectangle> calculateSubbyRect()
        {
            int pixels = 10; // deze kan maximaal op 10 voor een goede collision
            Point size = new Point(pixels, pixels);
            int widthRadius = subby.Texture.Width / 2;
            int heightRadius = subby.Texture.Height / 2;
            List<Rectangle> values = new List<Rectangle>();

            for (int x = 0; x < widthRadius; x = x + pixels)
            {
                for (int y = 0; y < heightRadius; y = y + pixels)
                {
                    Point radiusPoint = PointOnCircle(x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    Point point = PointOnCircle(y, (int)subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees - 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
                    radiusPoint = PointOnCircle(-x, (int)subby.AngleDegrees, new Point((int)subby.Position.X, (int)subby.Position.Y));
                    point = PointOnCircle(y, (int)subby.AngleDegrees + 90, new Point((int)radiusPoint.X, (int)radiusPoint.Y));
                    values.Add(new Rectangle(point, size));
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
            if (subby.Width > subby.Height)
            {
                boundingLength = subby.Width;
            }
            else
            {
                boundingLength = subby.Height;
            }
            Rectangle rectSubby = new Rectangle((int)subby.Position.X - (boundingLength/2), (int)subby.Position.Y - (boundingLength/2), boundingLength, boundingLength);
            
            foreach (ISprite sprite in level.SpriteList)
            {

                //collision voor subby is apart, want deze wordt opgedeeld in meerdere vierkantjes
                Rectangle rectSprite = new Rectangle((int)sprite.Position.X - level.ScrollingPosition, (int)sprite.Position.Y, sprite.Width, sprite.Height); 
                Rectangle overlap = Rectangle.Intersect(rectSubby, rectSprite);
                if (!overlap.IsEmpty)
                {
                    List<Rectangle> partRectsSubby = calculateSubbyRect();
                    
                    foreach (Rectangle partRectSubby in partRectsSubby)
                    {
                        Rectangle collisionCheck = Rectangle.Intersect(partRectSubby, rectSprite);
                        if (!collisionCheck.IsEmpty)
                        {
                            subby.CollisionWith(sprite);
                            sprite.CollisionWith(subby);
                        }
                    }
                }
                if (level.MissileList != null)
                {
                    foreach (ISprite missile in level.MissileList)
                    {
                        if (!sprite.GetType().Name.Equals("Missile"))
                            checkCollisionRectangleAction(missile, sprite);
                    }
                }
            }
            
            
        }
        private void checkCollisionRectangleAction(ISprite s1, ISprite s2)
        {
            Rectangle r1 = new Rectangle((int)s1.Position.X - level.ScrollingPosition, (int)s1.Position.Y, s1.Width, s1.Height);
            Rectangle r2 = new Rectangle((int)s2.Position.X - level.ScrollingPosition, (int)s2.Position.Y, s2.Width, s2.Height); 

            Rectangle collisionCheck = Rectangle.Intersect(r1, r2);
            if (!collisionCheck.IsEmpty)
            {
                s2.CollisionWith(s1);
                s1.CollisionWith(s2);
            }
        }


    }
}
