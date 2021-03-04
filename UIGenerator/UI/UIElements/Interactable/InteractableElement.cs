using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class InteractableElement : UIElement
    {
        protected Action ValueChanged;
        internal string Name => GetType().Name.Replace("Interactable", "");

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

            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
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
        }
    }
}
