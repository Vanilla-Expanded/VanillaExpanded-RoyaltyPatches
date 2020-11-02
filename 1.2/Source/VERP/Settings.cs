using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using Verse;

namespace VERP
{
    class VERP_Mod : Mod
    {
        // public static VERP_ModSettings settings;
        private Vector2 scrollPosition = Vector2.zero;

        public VERP_Mod(ModContentPack content) : base(content)
        {
            // settings = GetSettings<VERP_ModSettings>();
        }

        public override string SettingsCategory() => "Vanilla Expanded - Royalty Patches";

        public override void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard list = new Listing_Standard();
            Widgets.BeginScrollView(rect, ref scrollPosition, rect, true);
            list.Begin(rect);

            Text.Anchor = TextAnchor.MiddleCenter;
            list.Label("NeedRestart".Translate());
            list.Gap();
            Text.Anchor = TextAnchor.UpperLeft;

            foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods orderby m.OverwritePriority select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
            {
                this.AddButton(list, modContentPack, rect);
            }
            
            list.End();
            Widgets.EndScrollView();
        }

        static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        private void AddButton(Listing_Standard list, ModContentPack modContentPack, Rect rect)
        {
            foreach (PatchOperation patchOperation in modContentPack.Patches)
            {
                if (patchOperation is VERP.PatchOperationToggable p && p != null)
                {
                    bool flag = p.enabled;
                    if (list.ButtonTextLabeled(p.label, p.enabled.ToString()))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(p.sourceFile);

                        string xpath = "Patch/Operation[@Class=\"VERP.PatchOperationToggable\" and label=\"" + p.label + "\"]/enabled/text()";
                        if (p.enabled) { xmlDocument.SelectSingleNode(xpath).Value = "False"; p.enabled = false; }
                        else { xmlDocument.SelectSingleNode(xpath).Value = "True"; p.enabled = true; }

                        File.WriteAllText(p.sourceFile, PrettyXml(xmlDocument.OuterXml));
                    }
                }
            }
        }
    }
}
