using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameSplineTest
{
    // Point2D type that stores 2 coordinates.
    struct Point2D
    {
        public int x;
        public int y;

        public Point2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D lineTex;
        Texture2D cpTex;
        Spline spline;

        public Game1()
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
            // TODO: Add your initialization logic here

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
            cpTex = Content.Load<Texture2D>("ControlPoint");
            lineTex = Content.Load<Texture2D>("LinePoint");

            // The list with the points and the current position of each one.
            Point2D[] points = new Point2D[]{new Point2D(50, 200), new Point2D(100, 200), new Point2D(150, 200), new Point2D(200, 200), new Point2D(250, 200),
                      new Point2D(300, 200), new Point2D(350, 200), new Point2D(400, 200), new Point2D(450, 200), new Point2D(500, 200) };

            // creates our Spline passing our control point list and the textures for our control points and line points.
            spline = new Spline(points, cpTex, lineTex);
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
                Exit();

            // TODO: Add your update logic here
            spline.Update(gameTime);

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
            spline.Draw(spriteBatch, GraphicsDevice);
            
            base.Draw(gameTime);
        }
    }
}
