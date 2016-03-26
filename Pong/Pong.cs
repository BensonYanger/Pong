﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // ball
        Texture2D ball;
        Vector2 ballPosition;
        Vector2 ballSpeed = new Vector2(200, 200);

        Texture2D paddle;
        Vector2 paddlePositionL;
        Vector2 paddlePositionR;

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // paddle
            paddle = new Texture2D(this.GraphicsDevice, 10, 50);
            Color[] paddleData = new Color[25 * 25];
            for (int i = 0; i < 500; i++)
            {
                paddleData[i] = Color.White;
            }
            paddle.SetData<Color>(paddleData);

            paddlePositionL = new Vector2(25 - paddle.Width, graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height);
            paddlePositionR = new Vector2(graphics.GraphicsDevice.Viewport.Width - 25, graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height);

            // ball
            ball = new Texture2D(this.GraphicsDevice, 25, 25);
            Color[] ballData = new Color[25 * 25];
            for (int i = 0; i < 625; i++)
            {
                ballData[i] = Color.White;
            }

            ballPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 + ball.Width, graphics.GraphicsDevice.Viewport.Height / 2 - ball.Height);

            ball.SetData<Color>(ballData);

            // TODO: Add your initialization logic here
            // make mouse visible
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            // paddle
            // move the paddle
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
                paddlePositionL.Y -= 5;
            else if (keyState.IsKeyDown(Keys.Down))
                paddlePositionL.Y += 5;

            // move the right paddle
            if (ballPosition.Y < paddlePositionR.Y + paddle.Height / 2)
                paddlePositionR.Y -= 5;
            else if (ballPosition.Y > paddlePositionR.Y + paddle.Height / 2)
                paddlePositionR.Y += 5;

            //paddle bounds checking
            int paddleMaxY = GraphicsDevice.Viewport.Height - paddle.Height;

            // left paddle
            if (paddlePositionL.Y < 0)
                this.paddlePositionL.Y = 0;
            else if (paddlePositionL.Y > paddleMaxY)
                this.paddlePositionL.Y = GraphicsDevice.Viewport.Height - paddle.Height;

            // right paddle
            if (paddlePositionR.Y < 0)
                this.paddlePositionR.Y = 0;
            else if (paddlePositionR.Y > paddleMaxY)
                this.paddlePositionR.Y = GraphicsDevice.Viewport.Height - paddle.Height;

            // ball
            // move the ball
            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // ball bounds checking
            int ballMaxX = GraphicsDevice.Viewport.Width - ball.Width;
            int ballMaxY = GraphicsDevice.Viewport.Height - ball.Height;

            //if (ballPosition.X > ballMaxX || ballPosition.X < 0)
            //    ballSpeed.X *= -1;
            // win condition
            if (ballPosition.X < 0)
            {
                // reset ball
                ballSpeed.X = 200;
                ballSpeed.Y = 200;
                ballPosition.X = graphics.GraphicsDevice.Viewport.Width / 2 + ball.Width;
                ballPosition.Y = graphics.GraphicsDevice.Viewport.Height / 2 - ball.Height;
                // reset paddles
                paddlePositionL.Y = graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height;
                paddlePositionR.Y = graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height;
            }
            else if (ballPosition.X > ballMaxX)
            {
                // reset ball
                ballSpeed.X = 200;
                ballSpeed.Y = 200;
                ballPosition.X = graphics.GraphicsDevice.Viewport.Width / 2 + ball.Width;
                ballPosition.Y = graphics.GraphicsDevice.Viewport.Height / 2 - ball.Height;
                // reset paddles
                paddlePositionL.Y = graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height;
                paddlePositionR.Y = graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height;
            }

            // bounce on walls
            if (ballPosition.Y > ballMaxY || ballPosition.Y < 0)
                ballSpeed.Y *= -1;

            // paddle collision
            Rectangle ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ball.Width, (int)ball.Height);
            Rectangle paddleRectL = new Rectangle((int)paddlePositionL.X, (int)paddlePositionL.Y, (int)paddle.Width, (int)paddle.Height);
            Rectangle paddleRectR = new Rectangle((int)paddlePositionR.X, (int)paddlePositionR.Y, (int)paddle.Width, (int)paddle.Height);

            if (ballRect.Intersects(paddleRectL))
            {
                // increase ball speed when hit
                ballSpeed.X += 50;
                if (ballSpeed.Y < 0)
                    ballSpeed.Y -= 10;
                else
                    ballSpeed.Y += 10;

                // bounce ball
                ballSpeed.X *= -1;
            }

            if (ballRect.Intersects(paddleRectR))
            {
                // increase ball speed when hit
                ballSpeed.X += 50;
                if (ballSpeed.Y < 0)
                    ballSpeed.Y -= 10;
                else
                    ballSpeed.Y += 10;

                // bounce ball
                ballSpeed.X *= -1;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            // ball
            spriteBatch.Begin();
            spriteBatch.Draw(paddle, paddlePositionL, Color.White);
            spriteBatch.Draw(paddle, paddlePositionR, Color.White);
            spriteBatch.Draw(ball, ballPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
