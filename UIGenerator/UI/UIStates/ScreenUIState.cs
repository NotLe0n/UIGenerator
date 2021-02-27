using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class ScreenUIState : UIState
    {
        public override void Recalculate()
        {
            Width.Set(Main.ViewPort.Width * Main.SceneScale, 0f);
            Height.Set(Main.ViewPort.Height * Main.SceneScale, 0f);
            base.Recalculate();
        }
    }
}
