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
        public static Vector2 WindowPos => System.Windows.Forms.Control.FromHandle(instance.Window.Handle).Location.ToVector2();
        public static Viewport ViewPort => graphics.GraphicsDevice.Viewport;
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        // UI
        public static string? MouseText;
        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;
        public static Vector2 ScenePos;
        public static float SceneScale = 0.5f;
        public static Matrix SceneMatrix;
        public static Rectangle SidebarArea => new Rectangle(0, 0, ViewPort.Width / 5, ViewPort.Height);
        public static int SceneWidth;
        public static int SceneHeight;
        public static Rectangle SceneRect;
        public static bool MouseOverSidebar => SidebarArea.Contains(mouse.Position);
        public static bool MouseOverUI => SceneUI.CurrentState.Elements.Exists(x => x.IsMouseHovering) || SidebarUI.CurrentState.Elements.Exists(x => x.IsMouseHovering);
        public static bool UIActive = true;
        public static UserInterface SceneUI = new UserInterface();
        public static UserInterface SidebarUI = new UserInterface();
        public static UserInterface Options = new UserInterface();

        public static Texture2D[] Backgrounds;
        public static BackgroundID[] currentBackground = {
            BackgroundID.Default,
            BackgroundID.Hotbar,
            BackgroundID.Minimap,
            BackgroundID.Inventory,
            BackgroundID.None,
            BackgroundID.None,
            BackgroundID.None
        };

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
        /// <summary>
        /// How much the mouse has moved since the last frame
        /// </summary>
        public static bool mouseMoved;
        public static Vector2 mousedelta;
        public static Vector2 MouseWorld => InvertTranslate(mouse.Position);
        #endregion

        public static InteractableElement? SelectedElement = null;

        public enum BackgroundID
        {
            None = 0,
            Default = 1,
            Hotbar = 2,
            Minimap = 3,
            Inventory = 4,
            NPC = 5,
            Angler = 6,
            Shop = 7
        }

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
            base.Initialize();

            ScenePos = new Vector2(ViewPort.Width, ViewPort.Height);
            SceneWidth = 1920;
            SceneHeight = 1080;

            SceneMatrix = Matrix.Identity
              * Matrix.CreateTranslation(ScenePos.X, ScenePos.Y, 0)
              * Matrix.CreateScale(SceneScale);

            SceneUI.SetState(new SceneUIState());
            SidebarUI.SetState(new AddElements());
            Options.SetState(new Options());
        }

        public static FontSystem fontSystem;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MagicPixel = Content.Load<Texture2D>("solid");
            toggle = Content.Load<Texture2D>("Settings_Toggle");

            fontSystem = FontSystemFactory.Create(GraphicsDevice);
            fontSystem.AddFont(File.ReadAllBytes(CurrentDirectory + @"/Fonts/Andy Bold.ttf"));

            fontMouseText = fontSystem.GetFont(20);
            fontDeathText = fontSystem.GetFont(40);

            Backgrounds = new Texture2D[]
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

            // Zooming
            if (scrollwheel != 0 && !SidebarArea.Contains(mouse.Position))
            {
                SceneScale -= scrollwheel;
            }
            SceneScale = Math.Clamp(SceneScale, 0.2f, 10);

            // Moving the scene
            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                ScenePos -= mousedelta / SceneScale;
            }
            ScenePos = Vector2.Clamp(ScenePos,
                ViewPort.Bounds.VectorSize() / -4 * SceneScale,
                ViewPort.Bounds.VectorSize() / 2 / SceneScale);

            // Update Camera
            SceneMatrix = Matrix.Identity
                  * Matrix.CreateTranslation(ScenePos.X, ScenePos.Y, 0)
                  * Matrix.CreateScale(SceneScale);

            SidebarUI.Update(gameTime);
            SceneUI.Update(gameTime);
            Options.Update(gameTime);

            // toggle selected Element
            if (mouseLeft && !MouseOverSidebar)
            {
                if (!MouseOverUI)
                {
                    SelectedElement = null;
                }

                if (SelectedElement == null)
                {
                    SidebarUI.SetState(new AddElements());
                }
                else
                {
                    SidebarUI.SetState(new SelectElement());
                }
            }

            base.Update(gameTime);
        }

        public Color sexyGray = new Color(33, 33, 33);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));

            // Draw Scene
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: SceneMatrix);
            SceneRect = new Rectangle(0, 0, SceneWidth, SceneHeight);

            // Draw Background elements
            spriteBatch.Draw(MagicPixel, SceneRect, new Color(88, 88, 88));
            for (int i = 0; i < currentBackground.Length; i++)
            {
                switch (currentBackground[i])
                {
                    case BackgroundID.Default:
                        spriteBatch.Draw(Backgrounds[0], SceneRect, Color.White);
                        break;
                    case BackgroundID.Hotbar:
                        spriteBatch.Draw(Backgrounds[1], SceneRect, Color.White);
                        break;
                    case BackgroundID.Minimap:
                        spriteBatch.Draw(Backgrounds[2], SceneRect, Color.White);
                        break;
                    case BackgroundID.Inventory:
                        if (currentBackground[2] == BackgroundID.Minimap)
                        {
                            spriteBatch.Draw(Backgrounds[3], SceneRect, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(Backgrounds[4], SceneRect, Color.White);
                        }
                        break;
                    case BackgroundID.NPC:
                        spriteBatch.Draw(Backgrounds[5], SceneRect, Color.White);
                        break;
                    case BackgroundID.Angler:
                        spriteBatch.Draw(Backgrounds[6], SceneRect, Color.White);
                        break;
                    case BackgroundID.Shop:
                        spriteBatch.Draw(Backgrounds[7], SceneRect, Color.White);
                        break;
                }
            }

            // Draw Scene elements
            SceneUI.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            // Draw UI
            UIScaleMatrix = Matrix.CreateScale(UIScale);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: UIScaleMatrix);

            spriteBatch.Draw(MagicPixel, SidebarArea, sexyGray);
            SidebarUI.Draw(spriteBatch, gameTime);
            Options.Draw(spriteBatch, gameTime);

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
            Matrix invMatrix = Matrix.Invert(SceneMatrix);
            return Vector2.Transform(vector / SceneScale, invMatrix);
        }
        public static Vector2 InvertTranslate(Point point)
        {
            Matrix invMatrix = Matrix.Invert(SceneMatrix);
            return Vector2.Transform(point.ToVector2(), invMatrix);
        }
    }
}
