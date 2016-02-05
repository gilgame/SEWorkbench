using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gilgame.SEWorkbench.ViewModels.Config
{
    public class TextEditorViewModel : BaseViewModel
    {
        public bool ConvertTabsToSpaces
        {
            get
            {
                return Configuration.TextEditor.ConvertTabsToSpaces;
            }
            set
            {
                Configuration.TextEditor.ConvertTabsToSpaces = value;
                RaisePropertyChanged("ConvertTabsToSpaces");
            }
        }

        public int TabSize
        {
            get
            {
                return Configuration.TextEditor.TabSize;
            }
            set
            {
                Configuration.TextEditor.TabSize = value;
                RaisePropertyChanged("TabSize");
            }
        }

        public FontViewModel FontFamily
        {
            get
            {
                FontViewModel item = _InstalledFonts.FirstOrDefault(f => f.Name == Configuration.TextEditor.FontFamily);
                if (item != null)
                {
                    return item;
                }
                return null;
            }
            set
            {
                Configuration.TextEditor.FontFamily = value.Name;
                RaisePropertyChanged("FontFamily");
            }
        }

        public double FontSize
        {
            get
            {
                return Configuration.TextEditor.FontSize;
            }
            set
            {
                Configuration.TextEditor.FontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }

        private Services.ObservableSortedList<FontViewModel> _InstalledFonts;
        public Services.ObservableSortedList<FontViewModel> InstalledFonts
        {
            get
            {
                return _InstalledFonts;
            }
        }

        public event EventHandler Saved;
        private void RaiseSaved()
        {
            if (Saved != null)
            {
                Saved(this, EventArgs.Empty);
            }
        }

        public TextEditorViewModel(BaseViewModel parent) : base(parent)
        {
            LoadSystemFonts();
        }

        private void LoadSystemFonts()
        {
            _InstalledFonts = new Services.ObservableSortedList<FontViewModel>(
                new FontViewModel[] { },
                new Comparers.FontComparer()
            );

            try
            {
                System.Drawing.Text.InstalledFontCollection installed = new System.Drawing.Text.InstalledFontCollection();
                foreach (System.Drawing.FontFamily family in installed.Families)
                {
                    AddFont(family.Name);
                }
            }
            catch (Exception ex)
            {
                Services.MessageBox.ShowError("Failed to load system fonts", ex);
                AddFont("Consolas");
            }
        }

        private void AddFont(string name)
        {
            FontViewModel font = new FontViewModel(this)
            {
                Name = name
            };
            InstalledFonts.Add(font);
        }
    }
}
