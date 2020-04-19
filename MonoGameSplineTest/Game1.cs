using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameSplineTest
{
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

        Spline path;

        List<Point2D> points;
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

            var p0 = new Point2D();
            p0.x = 100;
            p0.y = 100;

            var p1 = new Point2D();
            p1.x = 200;
            p1.y = 200;

            var p2 = new Point2D();
            p2.x = 300;
            p2.y = 300;

            var p3 = new Point2D();
            p3.x = 400;
            p3.y = 400;

            var p4 = new Point2D();
            p4.x = 500;
            p4.y = 100;

            var p5 = new Point2D();
            p5.x = 600;
            p5.y = 100;

            //var p0 = new Point2D();
            //p0.x = 100;
            //p0.y = 100;

            points = new List<Point2D>();
            points.Add(p0);
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
            points.Add(p4);
            points.Add(p5);

            // TODO: use this.Content to load your game content here
            cpTex = Content.Load<Texture2D>("ControlPoint");
            lineTex = Content.Load<Texture2D>("LinePoint");


            spline = new Spline(points.ToArray(), cpTex, lineTex);
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
