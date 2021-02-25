using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using UIGenerator.UI;
using UIGenerator.UI.UIStates;

namespace UIGenerator
{
    public class Main : Game
    {
        // Engine Stuff
        public static Main instance { get; private set; }
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Texture2D solid;
        public static SpriteFont font;

        public static Vector2 WindowPos => System.Windows.Forms.Control.FromHandle(instance.Window.Handle).Location.ToVector2();
        public static Viewport ViewPort => graphics.GraphicsDevice.Viewport;
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string? MouseText;

        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;
        public static float GameScale = 0.5f;
        public static Matrix GameScreenMatrix;

        public static long globalTimer;
        public static bool UIActive = true;

        // input stuff
        /// <summary>
        /// The mouse relative to the screen
        /// </summary>
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static KeyboardState lastKeyboard;
        /// <summary>
        /// How much the scroll wheel has changed since the last frame
        /// </summary>
        public static float scrollwheel;
        public static bool LeftHeld;
        public static bool RightHeld;
        public static bool LeftReleased;
        public static bool RightReleased;
        public static bool mouseLeft;
        public static bool mouseRight;
        public static bool mouseMiddle;
        public static bool mouseXButton1;
        public static bool mouseXButton2;
        public static float mouseX;
        public static float mouseY;
        public static bool hasFocus;
        /// <summary>
        /// How much the mouse has moved since the last frame
        /// </summary>
        public static bool mouseMoved;

        public static UserInterface MainUI = new UserInterface();

        public Main()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "UI Generator"; // title
            IsMouseVisible = true; // mouse is visible
            Window.AllowUserResizing = true; // user can resize window

            // Maximize Window
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        protected override void Initialize()
        {
            MainUI.SetState(new MainUIState());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            solid = Content.Load<Texture2D>("solid");
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            globalTimer++;

            // Update deltaTime
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Mouse variables
            UpdateInput();

            UserInterface.ActiveInstance.Update(gameTime);

            base.Update(gameTime);
        }

        public Color sexyGray = new Color(33, 33, 33);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));

            Rectangle SidebarArea = new Rectangle(0, 0, ViewPort.Width / 5, ViewPort.Height);
            UIScaleMatrix = Matrix.CreateScale(UIScale);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: UIScaleMatrix);

            spriteBatch.Draw(solid, SidebarArea, sexyGray);

            spriteBatch.End();

            GameScreenMatrix = Matrix.CreateScale(GameScale);
            GameScreenMatrix.Translation = new Vector3(ViewPort.Width / 3, ViewPort.Height / 4, 0);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: GameScreenMatrix);

            spriteBatch.Draw(solid, ViewPort.Bounds, new Color(88, 88, 88));
            UserInterface.ActiveInstance.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            UserInterface.ActiveInstance.Recalculate();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates input variables
        /// </summary>
        private void UpdateInput()
        {
            lastmouse = mouse;
            mouse = Mouse.GetState();
            lastKeyboard = keyboard;
            keyboard = Keyboard.GetState();
            mouseMoved = mouse.Position != lastmouse.Position;
            scrollwheel = (lastmouse.ScrollWheelValue - mouse.ScrollWheelValue) / 8000f;

            LeftHeld = mouse.LeftButton == ButtonState.Pressed;
            RightHeld = mouse.RightButton == ButtonState.Pressed;
            LeftReleased = mouse.LeftButton == ButtonState.Released;
            RightReleased = mouse.RightButton == ButtonState.Released;
            mouseLeft = LeftReleased && lastmouse.LeftButton == ButtonState.Pressed;
            mouseRight = RightReleased && lastmouse.RightButton == ButtonState.Pressed;
            mouseMiddle = mouse.MiddleButton == ButtonState.Pressed;
            mouseXButton1 = mouse.XButton1 == ButtonState.Pressed;
            mouseXButton2 = mouse.XButton2 == ButtonState.Pressed;
            hasFocus = IsActive;
        }
    }
}
