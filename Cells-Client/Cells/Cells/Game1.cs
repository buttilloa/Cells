using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Cells
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D circle;
        public static Map map;
        public static Random randy;
        public static Rectangle Bounds;
        public static Camera camera;
        public static ServerHandle client;
        public static Game1 instance;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;
            client = new ServerHandle();
            instance = this;


        }
        protected override void Initialize()
        {
            camera = new Camera(Window.ClientBounds.Height, Window.ClientBounds.Width);
            randy = new Random(System.Environment.TickCount);
            Bounds = this.Window.ClientBounds;
            map = new Map();
            base.Initialize();
        }
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            circle = Content.Load<Texture2D>("Circle");
            map.setTexture(Content.Load<Texture2D>("RECT"));
        }
        protected override void UnloadContent()
        {
            client.disconect();
        }
        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();
            map.update(gameTime);
            base.Update(gameTime);
            Bounds = this.Window.ClientBounds;
            camera.pos = map.player.Pos;
            if (Keyboard.GetState().IsKeyDown(Keys.L)) camera.Zoom -= 0.1f;
           
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, camera.gettransformation(GraphicsDevice)); // moveable objects
            map.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(); //Static
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
