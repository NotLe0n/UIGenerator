using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UIGenerator.UI.UIElements;
using UIGenerator.UI.UIElements.Interactable;

namespace UIGenerator.UI.UIStates
{
    class AddElements : UIState
    {
        public UIList list;
        Texture2D lets = Main.instance.Content.Load<Texture2D>("LETSFUCKINGGOOOOO");

        public override void OnInitialize()
        {
            list = new UIList();
            list.Width.Set(0, 1f);
            list.Height.Set(0, 1f);
            list.Top.Set(0, 0.02f);
            list.ListPadding = 20f;
            Append(list);

            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.93f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);
            list.SetScrollbar(scrollbar);
            Append(scrollbar);

            // Add elements to the list

            UIPanel panel = new UIPanel();
            panel.Width.Set(0, 0.8f);
            panel.Height.Set(100, 0);
            AddElement(panel);

            AddElement(new UIText("UIText", 2));

            UITextPanel<string> textPanel = new UITextPanel<string>("UITextPanel", 2);
            textPanel.Width.Set(0, 0.8f);
            AddElement(textPanel);

            UITextBox textBox = new UITextBox("UITextBox", 2);
            textBox.Width.Set(0, 0.8f);
            AddElement(textBox);

            AddElement(new UIImage(lets));
            AddElement(new UIImageButton(lets));
            AddElement(new UIImageFramed(lets, new Rectangle(5, 20, 70, 70)));
            AddElement(new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1)));

            /////////////////////////

            UIText branding = new UIText("made by NotLe0n#7696", 2);
            branding.Left.Set(30, 0f);
            branding.Top.Set(0, 0.92f);
            Append(branding);

            base.OnInitialize();
        }

        private void AddElement(UIElement element)
        {
            element.HAlign = 0.5f;
            element.OnClick += (evt, elm) =>
            {
                if (element.GetType() == typeof(UIPanel))
                {
                    NewInteractable(elm, new UIInteractablePanel());
                }
                else if (element.GetType() == typeof(UIText))
                {
                    NewInteractable(elm, new UIInteractableText("UIText", 2));
                }
                else if (element.GetType() == typeof(UITextPanel<string>))
                {
                    NewInteractable(elm, new UIInteractableTextPanel<string>("UITextPanel", 2));
                }
                else if (element.GetType() == typeof(UITextBox))
                {
                    NewInteractable(elm, new UIInteractableInput<string>("UITextBox", 2));
                }
                else if (element.GetType() == typeof(UIImage))
                {
                    NewInteractable(elm, new UIInteractableImage(lets));
                }
                else if (element.GetType() == typeof(UIImageButton))
                {
                    NewInteractable(elm, new UIInteractableImageButton(lets));
                }
                else if (element.GetType() == typeof(UIImageFramed))
                {
                    NewInteractable(elm, new UIInteractableImageFramed(lets, new Rectangle(5, 20, 70, 70)));
                }
                else if (element.GetType() == typeof(UIToggleImage))
                {
                    NewInteractable(elm, new UIInteractableToggleImage(Main.toggle));
                }
            };
            element.OnMouseOver += (evt, elm) => Main.MouseText = "Create " + element.GetType().Name + " element";
            element.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(element);
        }

        private void NewInteractable(UIElement elm, InteractableElement element)
        {
            element.Width.Set(elm.GetDimensions().Width, 0);
            element.Height.Set(elm.GetDimensions().Height, 0);

            element.clone = element.Clone();
            element.Id = "element" + Main.SceneUI.Elements.Count;
            Main.SceneUI.Append(element);
        }
        public override void Recalculate()
        {
            Width.Set(Main.SidebarArea.Width, 0);
            base.Recalculate();
        }
    }
}
