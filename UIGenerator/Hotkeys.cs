using Microsoft.Xna.Framework.Input;
using UIGenerator.UI.UIStates;

namespace UIGenerator
{
    public class Hotkeys
    {
        public static void Update()
        {
            if (Input.keyboard.IsKeyDown(Keys.LeftControl) || Input.keyboard.IsKeyDown(Keys.RightControl))
            {
                if (Main.SelectedElement != null)
                {
                    // copy
                    if (Input.keyboard.JustPressed(Keys.C))
                    {
                        System.Windows.Forms.Clipboard.SetText(Main.SelectedElement.BetterToString());
                    }

                    // cut
                    if (Input.keyboard.JustPressed(Keys.X))
                    {
                        System.Windows.Forms.Clipboard.SetText(Main.SelectedElement.BetterToString());
                        Main.SelectedElement.Remove();
                    }
                }

                // paste
                if (Input.keyboard.JustPressed(Keys.V))
                {
                    // c# parser
                }

                if (Input.keyboard.IsKeyDown(Keys.LeftShift) || Input.keyboard.IsKeyDown(Keys.RightShift))
                {
                    // redo
                    if (Input.keyboard.JustPressed(Keys.Z))
                    {

                    }
                }
                else
                {
                    // undo
                    if (Input.keyboard.JustPressed(Keys.Z))
                    {

                    }
                }
            }

            // Deleting Elements
            if (Main.SelectedElement != null && (Input.keyboard.IsKeyDown(Keys.Delete) || Input.keyboard.IsKeyDown(Keys.Back)))
            {
                if (Input.keyboard.IsKeyDown(Keys.Back) && Input.typing)
                    return;

                Main.SelectedElement.Remove();
                Main.SelectedElement = null;
                Main.SidebarUserinterface.SetState(new AddElements());
            }

            // toggle selected Element
            if (Input.mouseLeft && Main.MouseOverScene)
            {
                if (!Main.MouseOverUI)
                {
                    Main.SelectedElement = null;
                    Input.typing = false;
                }

                if (Main.SelectedElement == null)
                {
                    Main.SidebarUserinterface.SetState(new AddElements());
                }
                else
                {
                    Main.SidebarUserinterface.SetState(new SelectElement());
                }
            }
        }
    }
}
