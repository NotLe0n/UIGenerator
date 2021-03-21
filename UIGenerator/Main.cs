using DiscordRPC;
using DiscordRPC.Logging;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using UIGenerator.UI;
using UIGenerator.UI.UIElements.Interactable;
using UIGenerator.UI.UIStates;

namespace UIGenerator
{
    public class Main : Game
    {
        // Game Stuff
        public static Main instance;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Texture2D MagicPixel;
        public static Texture2D toggle;
        public static SpriteFontBase fontMouseText;
        public static SpriteFontBase fontDeathText;
        public static Viewport ViewPort => graphics.GraphicsDevice.Viewport;
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public DiscordRpcClient client;

        // UI
        public static string? MouseText;
        public static bool MouseOverScene => SceneUI.SceneRect.Contains(mouse.Position) && !SidebarArea.Contains(mouse.Position);
        public static bool MouseOverUI => SceneUI.Elements.Exists(x => x.IsMouseHovering) || SidebarUserinterface.CurrentState.Elements.Exists(x => x.IsMouseHovering) || OptionsUserinterface.CurrentState.Elements.Exists(x => x.IsMouseHovering);
        public static bool UIActive = true;
        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;
        public static Rectangle SidebarArea => new Rectangle(0, 0, ViewPort.Width / 5, ViewPort.Height);
        public static SceneUIState SceneUI;
        public static UserInterface SceneUserinterface = new UserInterface();
        public static UserInterface SidebarUserinterface = new UserInterface();
        public static UserInterface OptionsUserinterface = new UserInterface();

        #region input
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
        public static bool hasFocus;
        public static bool typing;
        /// <summary>
        /// How much the mouse has moved since the last frame
        /// </summary>
        public static bool mouseMoved;
        public static Vector2 mousedelta;
        public static Vector2 MouseWorld => InvertTranslate(mouse.Position);
        public static Vector2 MouseWorldPercent => Helper.GetPrecent(MouseWorld, new Vector2(SceneUI.SceneWidth, SceneUI.SceneHeight));
        #endregion

        public static InteractableElement? SelectedElement = null;

        public Main()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "UI Generator"; // title
            IsMouseVisible = true; // mouse is visible

            // Maximize Window
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 30;
        }

        protected override void Initialize()
        {
            SceneUI = new SceneUIState();
            base.Initialize();

            SceneUserinterface.SetState(SceneUI);
            SidebarUserinterface.SetState(new AddElements());
            OptionsUserinterface.SetState(new Options());

            client = new DiscordRpcClient("822869370543013888")
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            //Connect to the RPC
            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Editing UIState0.cs", // instead of "UIState0.cs" replace it with the current selected File in the future
                Timestamps = Timestamps.Now,
                Assets = new Assets() { LargeImageKey = "rpcimg" }
            });
        }

        public static FontSystem fontSystem;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MagicPixel = Content.Load<Texture2D>("solid");
            toggle = Content.Load<Texture2D>("Settings_Toggle");

            fontSystem = FontSystemFactory.Create(GraphicsDevice);
#if DEBUG
            fontSystem.AddFont(File.ReadAllBytes(CurrentDirectory + @"/Fonts/Andy Bold.ttf"));
#else
            fontSystem.AddFont(File.ReadAllBytes(Environment.CurrentDirectory + "/Andy Bold.ttf"));
#endif

            fontMouseText = fontSystem.GetFont(20);
            fontDeathText = fontSystem.GetFont(40);

            SceneUI.Backgrounds = new Texture2D[]
            {
                Content.Load<Texture2D>("Background/Idle"),
                Content.Load<Texture2D>("Background/Hotbar"),
                Content.Load<Texture2D>("Background/Minimap"),
                Content.Load<Texture2D>("Background/InventoryMinimap"),
                Content.Load<Texture2D>("Background/InventoryNoMinimap"),
                Content.Load<Texture2D>("Background/NPC"),
                Content.Load<Texture2D>("Background/Angler"),
                Content.Load<Texture2D>("Background/Shop")
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update deltaTime
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Mouse variables
            UpdateInput();

            SidebarUserinterface.Update(gameTime);
            SceneUserinterface.Update(gameTime);
            OptionsUserinterface.Update(gameTime);

            // toggle selected Element
            if (mouseLeft && MouseOverScene)
            {
                if (!MouseOverUI)
                {
                    SelectedElement = null;
                    typing = false;
                }

                if (SelectedElement == null)
                {
                    SidebarUserinterface.SetState(new AddElements());
                }
                else
                {
                    SidebarUserinterface.SetState(new SelectElement());
                }
            }

            client.Invoke();
            base.Update(gameTime);
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            client.Dispose();
            base.OnExiting(sender, args);
        }
        public Color sexyGray = new Color(33, 33, 33);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));

            // Draw Scene
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: SceneUI.SceneMatrix);

            // Draw Scene elements
            SceneUserinterface.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            // Draw UI
            UIScaleMatrix = Matrix.CreateScale(UIScale);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: UIScaleMatrix);

            spriteBatch.Draw(MagicPixel, SidebarArea, sexyGray);
            SidebarUserinterface.Draw(spriteBatch, gameTime);
            OptionsUserinterface.Draw(spriteBatch, gameTime);

            // Draw Mousetext
            if (MouseText != null)
            {
                spriteBatch.DrawString(fontMouseText, MouseText, mouse.Position.ToVector2() + new Vector2(10), Color.White);
            }
            if (!MouseOverUI)
            {
                MouseText = null;
            }

            spriteBatch.End();

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

            mousedelta = (lastmouse.Position - mouse.Position).ToVector2();
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

        public static Vector2 InvertTranslate(Vector2 vector)
        {
            Matrix invMatrix = Matrix.Invert(SceneUI.SceneMatrix);
            return Vector2.Transform(vector / SceneUI.SceneScale, invMatrix);
        }
        public static Vector2 InvertTranslate(Point point)
        {
            Matrix invMatrix = Matrix.Invert(SceneUI.SceneMatrix);
            return Vector2.Transform(point.ToVector2(), invMatrix);
        }
    }
}
