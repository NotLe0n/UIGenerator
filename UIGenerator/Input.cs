using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UIGenerator
{
    public class Input
    {
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
        public static Vector2 MouseWorld => Helper.InvertTranslate(mouse.Position);
        public static Vector2 MouseWorldPercent => Helper.GetPrecent(MouseWorld, new Vector2(Main.SceneUI.SceneWidth, Main.SceneUI.SceneHeight));

        /// <summary>
        /// Updates input variables
        /// </summary>
        public static void Update()
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

            hasFocus = Main.instance.IsActive;
        }
    }
}
