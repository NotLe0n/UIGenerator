using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIStates
{
    class MainUIState : UIState
    {
        public override void OnInitialize()
        {
            var panel = new UIPanel();
            panel.Width.Set(500, 0);
            panel.Height.Set(500, 0);
            Append(panel);

            var img = new UIImage(Main.instance.Content.Load<Texture2D>("LETSFUCKINGGOOOOO"));
            img.Left.Set(40, 0.5f);
            img.Top.Set(120, 0);
            panel.Append(img);

            var text = new UIText("LETSFUCKINGGOOO");
            text.Left.Precent = 0.5f;
            text.Top.Precent = 0.5f;
            panel.Append(text);

            var textbox = new UITextPanel<int>(4123123);
            panel.Append(textbox);

            var input = new UITextBox("hi");
            input.Top.Set(400, 0);
            input.Width.Set(400, 0);
            panel.Append(input);

            base.OnInitialize();
        }
    }
}
