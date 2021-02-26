using Microsoft.Xna.Framework.Graphics;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class SidebarUIState : UIState
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
                var proto = new UIPanel();
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UIPanel>(proto));
            };
            list.Add(panel);

            UIText text = new UIText("UIText", 1, true);
            text.HAlign = 0.5f;
            text.OnClick += (evt, elm) =>
            {
                var proto = new UIText("UIText", 1, true);
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UIText>(proto));
            };
            list.Add(text);

            UITextBox textBox = new UITextBox("UITextBox", 1, true);
            textBox.Width.Set(0, 0.8f);
            textBox.HAlign = 0.5f;
            textBox.OnClick += (evt, elm) =>
            {
                var proto = new UITextBox("UITextBox", 1, true);
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UITextBox>(proto));
            };
            list.Add(textBox);

            UITextPanel<string> textPanel = new UITextPanel<string>("UITextPanel", 1, true);
            textPanel.Width.Set(0, 0.8f);
            textPanel.HAlign = 0.5f;
            textPanel.OnClick += (evt, elm) =>
            {
                var proto = new UITextPanel<string>("UITextPanel", 1, true);
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UITextPanel<string>>(proto));
            };
            list.Add(textPanel);

            Texture2D lets = Main.instance.Content.Load<Texture2D>("LETSFUCKINGGOOOOO");
            UIImage image = new UIImage(lets);
            image.HAlign = 0.5f;
            image.OnClick += (evt, elm) =>
            {
                var proto = new UIImage(lets);
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UIImage>(proto));
            };
            list.Add(image);

            UIImageButton imageBtn = new UIImageButton(lets);
            imageBtn.HAlign = 0.5f;
            imageBtn.OnClick += (evt, elm) =>
            {
                var proto = new UIImageButton(lets);
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UIImageButton>(proto));
            };
            list.Add(imageBtn);

            UIImageFramed imageFramed = new UIImageFramed(lets, new Microsoft.Xna.Framework.Rectangle(5, 20, 70, 70));
            imageFramed.HAlign = 0.5f;
            imageFramed.OnClick += (evt, elm) =>
            {
                var proto = new UIImageFramed(lets, new Microsoft.Xna.Framework.Rectangle(5, 20, 70, 70));
                proto.Width.Set(elm.GetDimensions().Width, 0);
                proto.Height.Set(elm.GetDimensions().Height, 0);
                Main.MainUI.CurrentState.Append(new ProtoElement<UIImageFramed>(proto));
            };
            list.Add(imageFramed);

            base.OnInitialize();
        }
        public override void Recalculate()
        {
            Width.Set(Main.SidebarArea.Width, 0);
            base.Recalculate();
        }
    }
}
