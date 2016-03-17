using System;
using System.Collections.Generic;
using System.Windows.Input;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ClassViewModel : BaseViewModel
    {
        private IEnumerator<ClassItemViewModel> _MatchingItemEnumerator;
        private IEnumerator<ClassItemViewModel> _SelectedItemEnumerator;

        private ObservableSortedList<ClassItemViewModel> _First;
        public ObservableSortedList<ClassItemViewModel> First
        {
            get
            {
                return _First;
            }
        }

        private string _SearchText = String.Empty;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (value == _SearchText)
                {
                    return;
                }

                _SearchText = value;
                _MatchingItemEnumerator = null;
            }
        }

        public ClassItemViewModel SelectedItem
        {
            get
            {
                return FindSelectedItem();
            }
        }

        public Models.ClassItemType SelectedItemType
        {
            get
            {
                return (SelectedItem == null) ? Models.ClassItemType.None : SelectedItem.Type;
            }
        }

        public ClassItemViewModel RootItem
        {
            get
            {
                if (First != null && First.Count > 0)
                {
                    return First[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public ClassViewModel(BaseViewModel parent) : base(parent)
        {
            _First = new Services.ObservableSortedList<ClassItemViewModel>(
                new ClassItemViewModel[] { },
                new Comparers.ClassItemComparer()
            );

            _SearchCommand = new Commands.DelegateCommand(PerformSearch);

            StartTree();
        }

        public void AddNamespaces(List<Interop.AssemblyObject> namespaces)
        {
            foreach (Interop.AssemblyObject n in namespaces)
            {
                ClassItemViewModel nitem = new ClassItemViewModel(
                    new Models.ClassItem()
                    {
                        Name = n.Name,
                        Namespace = n.Namespace,
                        Type = Models.ClassItemType.Namespace
                    },
                    RootItem
                );

                foreach (Interop.AssemblyObject child in n.Children)
                {
                    ClassItemViewModel citem = new ClassItemViewModel(
                        new Models.ClassItem()
                        {
                            Name = child.Name,
                            Namespace = child.Namespace,
                            Type = Models.ClassItemType.Object
                        },
                        nitem
                    );

                    foreach (Interop.AssemblyObject field in child.Fields)
                    {
                        ClassItemViewModel fitem = new ClassItemViewModel(
                            new Models.ClassItem()
                            {
                                Name = field.Name,
                                Namespace = field.Namespace,
                                Type = Models.ClassItemType.Field
                            },
                            citem
                        );
                        citem.AddChild(fitem);
                    }

                    foreach (Interop.AssemblyObject property in child.Properties)
                    {
                        ClassItemViewModel pitem = new ClassItemViewModel(
                            new Models.ClassItem()
                            {
                                Name = property.Name,
                                Namespace = property.Namespace,
                                Type = Models.ClassItemType.Property
                            },
                            citem
                        );
                        citem.AddChild(pitem);
                    }

                    foreach (Interop.AssemblyObject method in child.Methods)
                    {
                        ClassItemViewModel mitem = new ClassItemViewModel(
                            new Models.ClassItem()
                            {
                                Name = method.Name,
                                Namespace = method.Namespace,
                                Type = Models.ClassItemType.Method
                            },
                            citem
                        );
                        citem.AddChild(mitem);
                    }

                    nitem.AddChild(citem);
                }

                RootItem.AddChild(nitem);
            }
        }

        private void StartTree()
        {
            ClassItemViewModel root = new ClassItemViewModel(new Models.ClassItem() { Name = "Classes", Type = Models.ClassItemType.Root });

            _First.Add(root);

            _First[0].IsExpanded = true;
        }

        #region FindSelectedItem

        private ClassItemViewModel FindSelectedItem()
        {
            VerifySelectedItemEnumerator();

            return _SelectedItemEnumerator.Current;
        }

        private void VerifySelectedItemEnumerator()
        {
            ClassItemViewModel root = RootItem;

            var matches = FindSelected(root);

            _SelectedItemEnumerator = matches.GetEnumerator();
            if (!_SelectedItemEnumerator.MoveNext())
            {
                // none selected
            }
        }

        private IEnumerable<ClassItemViewModel> FindSelected(ClassItemViewModel item)
        {
            if (item == null)
            {
                yield return null;
            }

            if (item.IsSelected)
            {
                yield return item;
            }

            foreach (ClassItemViewModel child in item.Children)
            {
                foreach (ClassItemViewModel match in FindSelected(child))
                {
                    // TODO fix collection modified exception
                    yield return match;
                }
            }
        }

        #endregion

        #region Search Command

        private readonly ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand;
            }
        }

        public void PerformSearch()
        {
            if (_MatchingItemEnumerator == null || !_MatchingItemEnumerator.MoveNext())
            {
                VerifyMatchingItemEnumerator();
            }
            if (_MatchingItemEnumerator == null)
            {
                return;
            }

            var item = _MatchingItemEnumerator.Current;
            if (item == null)
            {
                return;
            }

            if (item.Parent != null)
            {
                ClassItemViewModel parent = (ClassItemViewModel)item.Parent;
                parent.IsExpanded = true;
            }

            // TODO switch search to filter (hide unmatched items), probably handled by the UI instead

            item.IsSelected = true;
        }

        private void VerifyMatchingItemEnumerator()
        {
            if (First == null || First.Count < 1)
            {
                return;
            }

            var matches = FindMatches(_SearchText, First[0]);

            _MatchingItemEnumerator = matches.GetEnumerator();
            if (!_MatchingItemEnumerator.MoveNext())
            {
                // none found, do nothing for now
            }
        }

        private IEnumerable<ClassItemViewModel> FindMatches(string text, ClassItemViewModel item)
        {
            if (item == null)
            {
                yield return null;
            }
            if (item.NameContainsText(text))
            {
                yield return item;
            }

            foreach (ClassItemViewModel child in item.Children)
            {
                foreach (ClassItemViewModel match in FindMatches(text, child))
                {
                    yield return match;
                }
            }
        }

        #endregion
    }
}
