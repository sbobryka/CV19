using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModelBase
    {
        private ViewModelMainWindow MainViewModel { get; }

        public CountriesStatisticViewModel(ViewModelMainWindow mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
}
