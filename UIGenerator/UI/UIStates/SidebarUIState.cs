using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UIGenerator.UI.UIStates
{
    class SidebarUIState : UIState
    {
        UIList list;
        public override void OnInitialize()
        {
            list = new UIList();
            list.Width.Set(0, 0.2f);
            list.Height.Set(0, 1f);
            list.Top.Set(0, 0.02f);
            list.ListPadding = 20f;
            Append(list);

            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.19f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);

            list.SetScrollbar(scrollbar);
            Append(scrollbar);

            UIPanel panel = new UIPanel();
            panel.Width.Set(0, 0.8f);
            panel.Height.Set(100, 0);
            panel.HAlign = 0.5f;
            list.Add(TurnIntoProto(panel));

            UIText text = new UIText("UIText", 1, true);
            text.HAlign = 0.5f;
            list.Add(TurnIntoProto(text));

            UITextBox textBox = new UITextBox("UITextBox", 1, true);
            textBox.Width.Set(0, 0.8f);
            textBox.HAlign = 0.5f;
            list.Add(TurnIntoProto(textBox));

            UITextPanel<string> textPanel = new UITextPanel<string>("UITextPanel", 1, true);
            textPanel.Width.Set(0, 0.8f);
            textPanel.HAlign = 0.5f;
            list.Add(TurnIntoProto(textPanel));

            UIImage image = new UIImage(Main.instance.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("LETSFUCKINGGOOOOO"));
            image.HAlign = 0.5f;
            list.Add(TurnIntoProto(image));

            UIImageButton imageBtn = new UIImageButton(Main.instance.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("LETSFUCKINGGOOOOO"));
            imageBtn.HAlign = 0.5f;
            list.Add(TurnIntoProto(imageBtn));

            base.OnInitialize();
        }
        private UIElement TurnIntoProto(UIElement element)
        {
            UIElement initialElement = element;
            Vector2 offset = Vector2.Zero;
            element.OnClick += (evt, elm) =>
            {
                list.Remove(elm);
                Append(elm);
            };

            element.OnMouseDown += (evt, elm) =>
            {
                offset = new Vector2(evt.MousePosition.X - elm.Left.Pixels, evt.MousePosition.Y - elm.Top.Pixels);

                elm.Left.Set(Main.mouse.X - offset.X, 0f);
                elm.Top.Set(Main.mouse.Y - offset.Y, 0f);
                elm.Recalculate();
            };

            element.OnMouseUp += (evt, elm) =>
            {
                elm.Left.Set(evt.MousePosition.X - offset.X, 0f);
                elm.Top.Set(evt.MousePosition.Y - offset.Y, 0f);
                elm.Recalculate();

                list.Add(initialElement);
            };
            return element;
        } 
    }
}
