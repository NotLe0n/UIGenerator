using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UIGenerator.UI;
using UIGenerator.UI.UIElements.Interactable;

namespace UIGenerator
{
    static class CodeGenerator
    {
        public static string Generate()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("using Terraria.GameContent.UI.Elements;");
            s.AppendLine("using Terraria.UI;");
            s.AppendLine("using Terraria.Graphics;");
            s.AppendLine("using Terraria.ModLoader;");
            s.AppendLine("using Microsoft.Xna.Framework;\n");

            s.AppendLine("class MyUIState : UIState");
            s.AppendLine("{");
            s.AppendLine("\tpublic override void OnInitialize()");
            s.AppendLine("\t{");
            s.BetterAppendJoin('\n', Main.SceneUI.Elements);
            s.AppendLine("\t}");
            s.AppendLine("}");
            return s.ToString();
        }

        public static StringBuilder BetterAppendJoin(this StringBuilder s, char seperator, List<UIElement> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                s.Append((values[i] as InteractableElement).BetterToString() + seperator);
                for (int k = 0; k < values[i].Elements.Count; k++) 
                {
                    s.Append((values[i].Elements[k] as InteractableElement).BetterToString() + seperator);
                }
            }
            s.Replace("String", "string")
             .Replace("True", "true")
             .Replace("False", "false")
             .Replace("UIInput<string>", "UITextBox");
            return s;
        }

        public static string BetterToString(this InteractableElement elm)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"\t\t{elm.Name} {elm.Id} = new {elm.Name}{elm.constructor};");


            var fields = elm.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            var cloneFields = elm.clone.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                for (int j = 0; j < cloneFields.Length; j++)
                {
                    var val1 = fields[i].GetValue(elm);
                    var val2 = cloneFields[j].GetValue(elm.clone);

                    if (fields[i].Name == "Parent" || fields[i].Name == "Id")             
                        break;

                    if (fields[i].Name == cloneFields[j].Name && !val1.Equals(val2))
                    {
                        if (val1 is StyleDimension)
                        {
                            s.AppendLine($"\t\t{elm.Id}.{fields[i].Name}.Set({val1});");
                        }
                        else if (val1 is Color)
                        {
                            var col = (val1 as Color?).Value;
                            s.AppendLine($"\t\t{elm.Id}.{fields[i].Name} = new Color({col.R}, {col.G}, {col.B}, {col.A});");
                        }
                        else
                        {
                            s.AppendLine($"\t\t{elm.Id}.{fields[i].Name} = {val1};");
                        }
                        break;
                    }
                }
            }
            var parent = elm.Parent.GetType().Name == "SceneUIState" ? "" : elm.Parent.Id + ".";
            s.AppendLine($"\t\t{parent}Append({elm.Id});");
            return s.ToString();
        }
    }
}
