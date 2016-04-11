using System;
using System.Collections.Generic;

using Gilgame.SEWorkbench.Services.IO;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Gilgame.SEWorkbench.Services
{
    public class DockLayoutManager
    {
        private List<LayoutAnchorable> _Panels = new List<LayoutAnchorable>();

        public void AddPanel(LayoutAnchorable panel)
        {
            if (!_Panels.Contains(panel))
            {
                _Panels.Add(panel);
            }
        }

        public void Save(DockingManager manager)
        {
            string path = GetDockLayoutPath();
            XmlLayoutSerializer layoutSerializer = new XmlLayoutSerializer(manager);
            using (var writer = new System.IO.StreamWriter(path))
            {
                layoutSerializer.Serialize(writer);
            }
        }

        public void Load(DockingManager manager)
        {
            string path = GetDockLayoutPath();
            if (VerifyLayout(path))
            {
                XmlLayoutSerializer layoutSerializer = new XmlLayoutSerializer(manager);
                layoutSerializer.LayoutSerializationCallback += (s, args) =>
                {
                    args.Content = args.Content;
                };

                using (var reader = new System.IO.StreamReader(path))
                {
                    layoutSerializer.Deserialize(reader);
                }
            }
        }

        private bool VerifyLayout(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            string contents = File.Read(path);
            foreach (LayoutAnchorable panel in _Panels)
            {
                string contentid = String.Format("ContentId=\"{0}\"", panel.ContentId);
                if (!contents.Contains(contentid))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetDockLayoutPath()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string workbench = Path.Combine(appdata, "SEWorkbench");
            string layout = Path.Combine(workbench, "layout.xml");

            if (!Directory.Exists(workbench))
            {
                Directory.CreateDirectory(workbench);
            }

            return layout;
        }
    }
}
