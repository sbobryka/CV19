﻿using CV19.Comands;
using CV19.Models;
using CV19.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModelBase
    {
        private ViewModelMainWindow mainViewModel;

        private DataService dataService;

        #region Countries

        private IEnumerable<CountryInfo> countries;

        public IEnumerable<CountryInfo> Countries
        {
            get => countries;
            private set => Set(ref countries, value);
        }

        #endregion

        #region SelectedCountry

        private CountryInfo selectedCountry;

        public CountryInfo SelectedCountry
        {
            get => selectedCountry;
            set => Set(ref selectedCountry, value);
        }

        #endregion

        #region RefreshDataCommand

        public ICommand RefreshDataCommand { get; }

        private void OnRefreshDataCommandExecuted(object property) => Countries = dataService.GetData();

        #endregion

        public CountriesStatisticViewModel(ViewModelMainWindow mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            dataService = new DataService();

            RefreshDataCommand = new RelayCommand(OnRefreshDataCommandExecuted);
        }
    }
}
