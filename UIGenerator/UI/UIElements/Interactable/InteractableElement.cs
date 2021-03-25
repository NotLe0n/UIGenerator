using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

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
                    ? type.Name.Replace("Interactable", "").Replace($"`{count}", $"<{type.GenericTypeArguments[^1].Name}>")
                    : type.Name.Replace("Interactable", "");
            }
        }

        internal object clone;
        internal string Constructor => GetConstructor();

        public InteractableElement()
        {
            Id = "element" + Main.SceneUI.ElementCount;
        }

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
        private (bool w, bool h) resizing;
        private Vector2 TriggerArea => Vector2.Clamp(new Vector2(GetDimensions().ToRectangle().Width / 10, GetDimensions().ToRectangle().Height / 10), Vector2.One, new Vector2(10, 10));
        public override void MouseDown(UIMouseEvent evt)
        {
            offset = Main.SceneUI.usePrecent ?
                Helper.GetPrecent(evt.MousePosition - GetDimensions().Position(), new Vector2(Main.SceneUI.SceneWidth, Main.SceneUI.SceneHeight)) :
                evt.MousePosition - GetDimensions().Position();

            dragging = true;
            // change cursor
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;

            // resizing
            var dim = GetDimensions().ToRectangle();
            if (Input.MouseWorld.X < dim.Right + TriggerArea.X && Input.MouseWorld.X > dim.Right - TriggerArea.X)
            {
                resizing.w = true;
                dragging = false;
            }
            if (Input.MouseWorld.Y < dim.Bottom + TriggerArea.Y && Input.MouseWorld.Y > dim.Bottom - TriggerArea.Y)
            {
                resizing.h = true;
                dragging = false;
            }

            if (Parent != Main.SceneUI)
            {
                Main.SceneUI.Append(this);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            // dragging
            if (dragging)
            {
                if (Main.SceneUI.usePrecent)
                {
                    Left.Set(0, Input.MouseWorldPercent.X - offset.X);
                    Top.Set(0, Input.MouseWorldPercent.Y - offset.Y);
                }
                else
                {
                    Left.Set(evt.MousePosition.X - offset.X - 10, 0f);
                    Top.Set(evt.MousePosition.Y - offset.Y - 10, 0f);
                }
            }

            // resizing
            if (resizing.w)
            {
                if (Main.SceneUI.usePrecent)
                    Width.Set(0, Input.MouseWorldPercent.X - Left.Precent);
                else
                    Width.Set(Input.MouseWorld.X - Left.Pixels, 0);
            }
            if (resizing.h)
            {
                if (Main.SceneUI.usePrecent)
                    Height.Set(0, Input.MouseWorldPercent.Y - Top.Percent);
                else
                    Height.Set(Input.MouseWorld.Y - Top.Pixels, 0f);

            }

            // reset
            dragging = false;
            resizing = (false, false);
            Recalculate();

            // Append
            if (Parent == Main.SceneUI)
            {
                // all elements below this
                var parents = Main.SceneUI.Elements.Where(x => x.ContainsPoint(GetDimensions().Position())).ToList();
                if (parents.Count > 0)
                {
                    if (Main.SceneUI.usePrecent)
                    {
                        Left.Set(0, Helper.GetPrecent(GetDimensions().X - parents[0].GetDimensions().X, parents[0].GetDimensions().Width));
                        Top.Set(0, Helper.GetPrecent(GetDimensions().Y - parents[0].GetDimensions().Y, parents[0].GetDimensions().Height));
                    }
                    else
                    {
                        Left.Set(GetDimensions().X - parents[0].GetDimensions().X, 0);
                        Top.Set(GetDimensions().Y - parents[0].GetDimensions().Y, 0);
                    }
                    parents[0].Append(this);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            // Dragging
            if (dragging)
            {
                if (Main.SceneUI.usePrecent)
                {
                    Left.Set(0, Input.MouseWorldPercent.X - offset.X);
                    Top.Set(0, Input.MouseWorldPercent.Y - offset.Y);
                }
                else
                {
                    Left.Set(Input.MouseWorld.X - offset.X, 0f);
                    Top.Set(Input.MouseWorld.Y - offset.Y, 0f);
                }
                Recalculate();
                ValueChanged?.Invoke();
            }

            // Resizing
            if (resizing.w)
            {
                if (Main.SceneUI.usePrecent)
                    Width.Set(0, Input.MouseWorldPercent.X - Left.Precent);
                else
                    Width.Set(Input.MouseWorld.X - Left.Pixels, 0);
            }
            if (resizing.h)
            {
                if (Main.SceneUI.usePrecent)
                    Height.Set(0, Input.MouseWorldPercent.Y - Top.Percent);
                else
                    Height.Set(Input.MouseWorld.Y - Top.Pixels, 0f);
            }

            // Snapping
            if (Main.SceneUI.snapElements.x || Main.SceneUI.snapElements.y)
            {
                for (int i = 0; i < Main.SceneUI.snapIntervals.Length; i++)
                {
                    if (Main.SceneUI.snapElements.x
                        && GetDimensions().X > (Main.SceneUI.SceneWidth * Main.SceneUI.snapIntervals[i]) - Main.SceneUI.snapRange
                        && GetDimensions().X < (Main.SceneUI.SceneWidth * Main.SceneUI.snapIntervals[i]) + Main.SceneUI.snapRange)
                    {
                        Left.Set(0, Main.SceneUI.snapIntervals[i]);
                    }
                    if (Main.SceneUI.snapElements.y
                        && GetDimensions().Y > (Main.SceneUI.SceneHeight * Main.SceneUI.snapIntervals[i]) - Main.SceneUI.snapRange
                        && GetDimensions().Y < (Main.SceneUI.SceneHeight * Main.SceneUI.snapIntervals[i]) + Main.SceneUI.snapRange)
                    {
                        Top.Set(0, Main.SceneUI.snapIntervals[i]);
                    }
                }
            }

            // Keep element in bounds
            if (Main.SceneUI.keepElementsInBounds)
            {
                Width.Set(Math.Clamp(Width.Pixels, 0, Main.SceneUI.SceneWidth), Math.Clamp(Width.Precent, 0, 1));
                Height.Set(Math.Clamp(Height.Pixels, 0, Main.SceneUI.SceneHeight), Math.Clamp(Height.Precent, 0, 1));
                Left.Set(Math.Clamp(Left.Pixels, 0, Main.SceneUI.SceneWidth - Width.Pixels), Math.Clamp(Left.Precent, 0, Helper.GetPrecent(Main.SceneUI.SceneWidth - Width.Pixels, Main.SceneUI.SceneWidth)));
                Top.Set(Math.Clamp(Top.Pixels, 0, Main.SceneUI.SceneHeight - Height.Pixels), Math.Clamp(Top.Precent, 0, Helper.GetPrecent(Main.SceneUI.SceneHeight - Height.Pixels, Main.SceneUI.SceneHeight)));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var dim = GetDimensions().ToRectangle();

            if (Main.SelectedElement == this)
            {
                spriteBatch.DrawBoundary(Main.MagicPixel, dim, Color.White * 0.75f, 2);

                // change cursor
                if (IsMouseHovering)
                {
                    if (Input.MouseWorld.X < dim.Right + TriggerArea.X && Input.MouseWorld.X > dim.Right - TriggerArea.X)
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    }
                    if (Input.MouseWorld.Y < dim.Bottom + TriggerArea.Y && Input.MouseWorld.Y > dim.Bottom - TriggerArea.Y)
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                    }
                    if (Input.MouseWorld.X < dim.Right + TriggerArea.X && Input.MouseWorld.X > dim.Right - TriggerArea.X
                        && Input.MouseWorld.Y < dim.Bottom + TriggerArea.Y && Input.MouseWorld.Y > dim.Bottom - TriggerArea.Y)
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNWSE;
                    }
                }
            }
        }

        public UIElement ToElement(string text)
        {
            return null;
        }
    }
}
