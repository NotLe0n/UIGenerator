using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class InteractableElement : UIElement
    {
        protected Action ValueChanged;
        internal string Name
        {
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
        internal string Constructor => GetConstructor();

        public virtual string GetConstructor()
        {
            return "()";
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            Main.SelectedElement = this;
        }

        private Vector2 offset;
        private bool dragging;
        public override void MouseDown(UIMouseEvent evt)
        {
            offset = evt.MousePosition - GetDimensions().Position();
            dragging = true;

            if (Parent != Main.SceneUI)
            {
                Main.SceneUI.Append(this);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            dragging = false;
            Left.Set(evt.MousePosition.X - offset.X, 0f);
            Top.Set(evt.MousePosition.Y - offset.Y, 0f);
            Recalculate();

            if (Parent == Main.SceneUI)
            {
                var parents = Main.SceneUI.Elements.Where(x => x.ContainsPoint(GetDimensions().Position())).ToList();
                if (parents.Count > 0)
                {
                    parents[0].Append(this);
                    Left.Set(Left.Pixels - parents[0].Left.Pixels - 10, 0f);
                    Top.Set(Top.Pixels - parents[0].Top.Pixels - 10, 0f);
                }
            }
            dragging = false;
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
            if (Main.SelectedElement == this && (Main.keyboard.IsKeyDown(Keys.Delete) || Main.keyboard.IsKeyDown(Keys.Back)))
            {
                if (Main.keyboard.IsKeyDown(Keys.Back) && Main.typing)
                    return;

                Remove();
                Main.SelectedElement = null;
                Main.SidebarUserinterface.SetState(new UIStates.AddElements());
            }

            if (Main.SceneUI.keepElementsInBounds)
            {
                Left.Pixels = Math.Clamp(Left.Pixels, 0, Main.SceneUI.SceneWidth - Width.Pixels);
                Top.Pixels = Math.Clamp(Top.Pixels, 0, Main.SceneUI.SceneHeight - Height.Pixels);
            }
        }
    }
}
