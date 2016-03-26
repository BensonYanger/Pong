using Microsoft.Xna.Framework;
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
        Vector2 ballPosition = new Vector2(250, 250);
        Vector2 ballSpeed = new Vector2(150, 150);

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

            paddlePositionL = new Vector2(25, graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height);
            paddlePositionR = new Vector2(graphics.GraphicsDevice.Viewport.Width - 25, graphics.GraphicsDevice.Viewport.Height / 2 - paddle.Height);

            // ball
            ball = new Texture2D(this.GraphicsDevice, 25, 25);
            Color[] ballData = new Color[25 * 25];
            for (int i = 0; i < 625; i++)
            {
                ballData[i] = Color.White;
            }

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
            //move the paddles
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
                paddlePositionL.Y -= 5;
            else if (keyState.IsKeyDown(Keys.Down))
                paddlePositionL.Y += 5;

            //paddle bounds checking


            // ball
            // move the ball
            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            int maxX = GraphicsDevice.Viewport.Width - ball.Width;
            int maxY = GraphicsDevice.Viewport.Height - ball.Height;

            // ball bounds checking
            if (ballPosition.X > maxX || ballPosition.X < 0)
                ballSpeed.X *= -1;
            if (ballPosition.Y > maxY || ballPosition.Y < 0)
                ballSpeed.Y *= -1;

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
