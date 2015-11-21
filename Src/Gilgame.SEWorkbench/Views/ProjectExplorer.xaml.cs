using Gilgame.SEWorkbench.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class ProjectExplorer : UserControl
    {
        // TODO if any one knows a decent way to invoke methods with a binding, please message me
        private ViewModels.ProjectViewModel _Project;

        private EditorViewModel _Editor;

        public event BlueprintSelectedEventHandler BlueprintSelected;
        
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        public ProjectItemViewModel SelectedFile
        {
            get
            {
                if (_Project == null)
                {
                    return null;
                }
                else
                {
                    return _Project.GetSelectedFile();
                }
            }
        }

        public void SetEditor(EditorViewModel editor)
        {
            // TODO there's probably a better MVVM way of doing this
            if (editor != null)
            {
                _Editor = editor;
            }
        }

        public void NewProject()
        {
            _Project = ViewModels.ProjectViewModel.NewProject();

            if (_Project != null)
            {
                _Project.SaveProject();

                DataContext = _Project;
            }
        }

        public void OpenProject()
        {
            _Project = new ViewModels.ProjectViewModel();
            _Project.OpenProject();

            if (_Project.First != null)
            {
                DataContext = _Project;
            }
        }

        public void SaveProject()
        {
            if (_Project != null)
            {
                _Project.SaveProject();
            }
        }

        public void OpenSelected()
        {
            if (_Project != null)
            {
                ProjectItemViewModel selected = _Project.SelectedItem;
                if (selected != null)
                {
                    if (selected.Type == Models.ProjectItemType.File && _Editor != null)
                    {
                        _Editor.OpenProjectFile(selected);
                    }
                }
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (_Project != null)
            {
                _Project.SearchCommand.Execute(null);
            }
        }

        private void PojectTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenSelected();
        }

        private void PojectTreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (_Project != null)
            {
                ProjectItemViewModel selected = _Project.GetSelectedBlueprint();
                BlueprintSelectedEventArgs args = new BlueprintSelectedEventArgs();
                if (selected != null)
                {
                    if (selected.Type == Models.ProjectItemType.Blueprints)
                    {
                        args.Item = selected;
                    }
                    else
                    {
                        args.Item = null;
                    }
                }
                if (BlueprintSelected != null)
                {
                    BlueprintSelected(this, args);
                }
            }
        }
    }
}
