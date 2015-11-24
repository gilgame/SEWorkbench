using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class OutputViewModel : BaseViewModel
    {
        private Services.ObservableSortedList<OutputItemViewModel> _Items;
        public Services.ObservableSortedList<OutputItemViewModel> Items
        {
            get
            {
                return _Items;
            }
        }

        public OutputViewModel(BaseViewModel parent) : base(parent)
        {
            _Items = new Services.ObservableSortedList<OutputItemViewModel>(
                new Comparers.OutputItemComparer()
            );
        }

        public void AddItem(Models.OutputItem model)
        {
            OutputItemViewModel item = new OutputItemViewModel(model, this);
            Items.Add(item);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
