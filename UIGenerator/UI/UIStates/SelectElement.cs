using Microsoft.Xna.Framework;
using System.Reflection;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class SelectElement : UIState
    {
        private UIDynamicInput MakeElements(System.Type type, object value)
        {
            if (type == typeof(bool))
            {
                return new UIDynamicInput((bool)value);
            }
            else if (type == typeof(int))
            {
                return new UIDynamicInput((int)value);
            }
            else if (type == typeof(float))
            {
                return new UIDynamicInput((float)value);
            }
            else if (type == typeof(Vector2))
            {
                return new UIDynamicInput((Vector2)value);
            }
            else if (type == typeof(Color))
            {
                return new UIDynamicInput((Color)value);
            }
            else if (type == typeof(StyleDimension))
            {
                return new UIDynamicInput((StyleDimension)value);
            }
            else if (type == typeof(CalculatedStyle))
            {
                return new UIDynamicInput((CalculatedStyle)value);
            }
            else if (type == typeof(UIElement))
            {
                var elm = (UIElement)value;
                if (elm != null)
                {
                    return new UIDynamicInput(elm);
                }

                return new UIDynamicInput("none");
            }
            else
            {
                return new UIDynamicInput((value ?? "").ToString());
            }
        }
        UIList list;
        public override void OnInitialize()
        {
            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.93f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);
            Append(scrollbar);

            list = new UIList();
            list.Width.Set(0, 1f);
            list.Height.Set(0, 0.8f);
            list.Top.Set(0, 0.1f);
            list.ListPadding = 20f;
            list.SetScrollbar(scrollbar);
            Append(list);

            var typeText = new UIText(Main.SelectedElement.Name, 1, true);
            typeText.HAlign = 0.5f;
            typeText.VAlign = 0.05f;
            Append(typeText);

            CreateList();

            UIText branding = new UIText("made by NotLe0n#7696", 1, true);
            branding.Left.Set(30, 0f);
            branding.Top.Set(0, 0.92f);
            Append(branding);

            base.OnInitialize();
        }
        private void CreateList()
        {
            list.Clear();

            var type = Main.SelectedElement.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = type.GetProperties(BindingFlags.SetProperty);

            for (int i = 0; i < fields.Length; i++)
            {
                var fieldText = new UIText(fields[i].Name, 1.5f);
                fieldText.HAlign = 0.3f;
                list.Add(fieldText);

                UIDynamicInput fieldInput = MakeElements(fields[i].FieldType, fields[i].GetValue(Main.SelectedElement));
                fieldInput.HAlign = 0.3f;
                fieldInput.Width.Set(0, 0.5f);
                fieldInput.OnValueChanged += (val, elm) =>
                {
                    if (Main.SelectedElement != null)
                    {
                        var type = Main.SelectedElement.GetType();
                        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                        for (int i = 0; i < list._items.Count; i++)
                        {
                            for (int k = 0; k < fields.Length; k++)
                            {
                                if (list._items[i] is UIText e && list._items[i + 1] == elm && fields[k].Name == e.Text)
                                {
                                    fields[k].SetValue(Main.SelectedElement, val);
                                }
                            }
                        }
                    }
                };
                list.Add(fieldInput);
            }
            for (int i = 0; i < properties.Length; i++)
            {
                var propertyText = new UIText(properties[i].Name);
                propertyText.HAlign = 0.3f;
                list.Add(propertyText);

                var propertyInput = new UIInput<string>(properties[i].GetValue(Main.SelectedElement).ToString());
                propertyInput.HAlign = 0.3f;
                propertyInput.Width.Set(0, 0.5f);
                list.Add(propertyInput);
            }
        }
        public override void Recalculate()
        {
            Width.Set(Main.SidebarArea.Width, 0);
            base.Recalculate();
        }
    }
}
