using Pizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public Orders Order { get; set; } = new Orders();
        public bool IsExpand { get; set; } = false;

        public OrderViewModel()
        {

        }

        internal void Update()
        {
            OnPropertyChanged(nameof(Order));
            OnPropertyChanged(nameof(IsExpand));
        }
    }
}
