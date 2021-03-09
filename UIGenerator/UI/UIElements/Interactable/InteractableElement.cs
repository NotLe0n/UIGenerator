using Microsoft.Xna.Framework;
using System;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class InteractableElement : UIElement
    {
        protected Action ValueChanged;
        internal string Name {
            get
            {
                var type = GetType();
                var count = type.GenericTypeArguments.Length;
                return count > 0
                    ? type.Name.Replace("Interactable", "").Replace($"`{count}",$"<{type.GenericTypeArguments[count - 1].Name}>")
                    : type.Name.Replace("Interactable", "");
            }
        }

        internal object clone;
        internal string constructor = "()";

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            Main.SelectedElement = this;
        }

        private Vector2 offset;
        private bool dragging;
        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);

            offset = evt.MousePosition - GetDimensions().Position();
            dragging = true;

            //Parent = Main.SceneUI.Elements.Find(x => x.ContainsPoint(GetDimensions().Position()));
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            dragging = false;

            Left.Set(evt.MousePosition.X - offset.X, 0f);
            Top.Set(evt.MousePosition.Y - offset.Y, 0f);
            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            if (dragging)
            {
                Left.Set(Main.MouseWorld.X - offset.X, 0f);
                Top.Set(Main.MouseWorld.Y - offset.Y, 0f);
                Recalculate();
                ValueChanged?.Invoke();
            }

            if (Main.SceneUI.snapElements.Item1 || Main.SceneUI.snapElements.Item2)
            {
                for (int i = 0; i < Main.SceneUI.snapIntervals.Length; i++)
                {
                    if (Main.SceneUI.snapElements.Item1 
                        && GetDimensions().X > (Main.SceneUI.SceneWidth * Main.SceneUI.snapIntervals[i]) - Main.SceneUI.snapRange
                        && GetDimensions().X < (Main.SceneUI.SceneWidth * Main.SceneUI.snapIntervals[i]) + Main.SceneUI.snapRange)
                    {
                        Left.Set(0, Main.SceneUI.snapIntervals[i]);
                    }
                    if (Main.SceneUI.snapElements.Item2 
                        && GetDimensions().Y > (Main.SceneUI.SceneHeight * Main.SceneUI.snapIntervals[i]) - Main.SceneUI.snapRange
                        && GetDimensions().Y < (Main.SceneUI.SceneHeight * Main.SceneUI.snapIntervals[i]) + Main.SceneUI.snapRange)
                    {
                        Top.Set(0, Main.SceneUI.snapIntervals[i]);
                    }
                }
            }
        }
    }
}
