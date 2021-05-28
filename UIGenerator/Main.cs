using FontStashSharp;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public DiscordRichPresence rpc;
        public static FontSystem fontSystem;

        // UI
        public static string? MouseText;
        public static bool MouseOverUI => SceneUI.Elements.Exists(x => x.IsMouseHovering) || SidebarUserinterface.CurrentState.IsMouseHovering || OptionsUserinterface.CurrentState.IsMouseHovering;
        public static Rectangle SidebarArea => new Rectangle(0, 0, ViewPort.Width / 5, ViewPort.Height);
        public static Matrix UIScaleMatrix = Matrix.CreateScale(1);
        public static SceneUIState SceneUI;
        public static UserInterface SceneUserinterface = new UserInterface();
        public static UserInterface SidebarUserinterface = new UserInterface();
        public static UserInterface OptionsUserinterface = new UserInterface();

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
            Window.AllowUserResizing = true;

            Maximize();
        }

        protected override void Initialize()
        {
            SceneUI = new SceneUIState();
            base.Initialize();

            SceneUserinterface.SetState(SceneUI);
            SidebarUserinterface.SetState(new AddElements());
            OptionsUserinterface.SetState(new Options());

            // Discord Rich presence
            rpc = new DiscordRichPresence();
        }

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
            fontDeathText = fontSystem.GetFont(80);

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
            // Update Mouse variables
            Input.Update();

            SidebarUserinterface.Update(gameTime);
            SceneUserinterface.Update(gameTime);
            OptionsUserinterface.Update(gameTime);

            // Update Hotkeys
            Hotkeys.Update();

            rpc.client.Invoke();
            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            rpc.client.Dispose();
            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));

            // Draw Scene
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: SceneUI.SceneMatrix);

            // Draw Scene elements
            SceneUserinterface.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            // Draw UI
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: UIScaleMatrix);

            spriteBatch.Draw(MagicPixel, SidebarArea, new Color(33, 33, 33));
            SidebarUserinterface.Draw(spriteBatch, gameTime);
            OptionsUserinterface.Draw(spriteBatch, gameTime);

            // Draw Mousetext
            if (MouseText != null)
            {
                spriteBatch.DrawString(fontMouseText, MouseText, Input.mouse.Position.ToVector2() + new Vector2(10), Color.White);
            }
            if (!MouseOverUI)
            {
                MouseText = null;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void Maximize()
        {
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(instance.Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 30;
        }
    }
}
