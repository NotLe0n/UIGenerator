using Microsoft.Xna.Framework;

namespace UIGenerator.UI
{
    public class UIEvent
    {

        public readonly UIElement Target;
        public UIEvent(UIElement target)
        {
            Target = target;
        }
    }

    public class UIMouseEvent : UIEvent
    {
        public readonly Vector2 MousePosition;

        public UIMouseEvent(UIElement target, Vector2 mousePosition) : base(target)
        {
            MousePosition = mousePosition;
        }
    }

    public class UIScrollWheelEvent : UIMouseEvent
    {
        public readonly float ScrollWheelValue;
        public UIScrollWheelEvent(UIElement target, Vector2 mousePosition, float scrollWheelValue) : base(target, mousePosition)
        {
            ScrollWheelValue = scrollWheelValue;
        }
    }
}
