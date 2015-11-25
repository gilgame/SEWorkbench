using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class OutputItemViewModel : BaseViewModel
    {
        private Models.OutputItem _Model;
        public Models.OutputItem Model
        {
            get
            {
                return _Model;
            }
        }

        public int Line
        {
            get
            {
                return _Model.Line;
            }
        }

        public int Column
        {
            get
            {
                return _Model.Column;
            }
        }

        public string Error
        {
            get
            {
                return _Model.Error;
            }
        }

        public string Message
        {
            get
            {
                return _Model.Message;
            }
        }

        public OutputItemViewModel(Models.OutputItem item, BaseViewModel parent) : base(parent)
        {
            _Model = item;
        }
    }
}
