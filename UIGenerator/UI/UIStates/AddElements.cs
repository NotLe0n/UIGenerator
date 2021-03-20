using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UIGenerator.UI.UIElements;
using UIGenerator.UI.UIElements.Interactable;

namespace UIGenerator.UI.UIStates
{
    class AddElements : UIState
    {
        public UIList list;
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

            UIPanel panel = new UIPanel();
            panel.Width.Set(0, 0.8f);
            panel.Height.Set(100, 0);
            panel.HAlign = 0.5f;
            panel.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractablePanel());
            };
            panel.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIPanel element";
            panel.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(panel);

            UIText text = new UIText("UIText", 1, true);
            text.HAlign = 0.5f;
            text.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableText("UIText", 1, true));
            };
            text.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIText element";
            text.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(text);

            UITextPanel<string> textPanel = new UITextPanel<string>("UITextPanel", 1, true);
            textPanel.Width.Set(0, 0.8f);
            textPanel.HAlign = 0.5f;
            textPanel.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableTextPanel<string>("UITextPanel", 1, true));
            };
            textPanel.OnMouseOver += (evt, elm) => Main.MouseText = "Create UITextPanel element";
            textPanel.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(textPanel);

            UITextBox textBox = new UITextBox("UITextBox", 1, true);
            textBox.Width.Set(0, 0.8f);
            textBox.HAlign = 0.5f;
            textBox.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableInput<string>("UITextBox", 1, true));
            };
            textBox.OnMouseOver += (evt, elm) => Main.MouseText = "Create UITextBox element";
            textBox.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(textBox);

            Texture2D lets = Main.instance.Content.Load<Texture2D>("LETSFUCKINGGOOOOO");
            UIImage image = new UIImage(lets);
            image.HAlign = 0.5f;
            image.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableImage(lets));
            };
            image.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIImage element";
            image.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(image);

            UIImageButton imageBtn = new UIImageButton(lets);
            imageBtn.HAlign = 0.5f;
            imageBtn.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableImageButton(lets));
            };
            imageBtn.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIImageButton element";
            imageBtn.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(imageBtn);

            UIImageFramed imageFramed = new UIImageFramed(lets, new Rectangle(5, 20, 70, 70));
            imageFramed.HAlign = 0.5f;
            imageFramed.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableImageFramed(lets, new Rectangle(5, 20, 70, 70)));
            };
            imageFramed.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIImageFramed element";
            imageFramed.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(imageFramed);

            UIToggleImage toggle = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            toggle.HAlign = 0.5f;
            toggle.OnClick += (evt, elm) =>
            {
                AddElement(elm, new UIInteractableToggleImage(Main.toggle));
            };
            toggle.OnMouseOver += (evt, elm) => Main.MouseText = "Create UIToggleImage element";
            toggle.OnMouseOut += (evt, elm) => Main.MouseText = null;
            list.Add(toggle);

            UIText branding = new UIText("made by NotLe0n#7696", 1, true);
            branding.Left.Set(30, 0f);
            branding.Top.Set(0, 0.92f);
            Append(branding);

            base.OnInitialize();
        }
        private void AddElement<T>(UIElement elm, T element) where T : InteractableElement
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
