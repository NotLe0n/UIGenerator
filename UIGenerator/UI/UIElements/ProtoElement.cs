using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIGenerator.UI.UIStates;

namespace UIGenerator.UI.UIElements
{
    public class ProtoElement<T> : UIElement where T : UIElement
    {
        public T element;
        public ProtoElement(T t)
        {
            element = t;
            CopyStyle(t);
            Append(t);
        }
        public ProtoElement(UIElement elm)
        {
            element = elm as T;
            CopyStyle(element);
            Append(element);
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            Main.SelectedElement = element;
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
            }
        }
    }
}
