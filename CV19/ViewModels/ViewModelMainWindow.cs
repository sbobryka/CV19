using CV19.Comands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        #region Заголовок окна

        private string _Title = "Анализ статистики CV19";

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Статус

        private string _Status = "Готов";

        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        #endregion

        #region Команды

        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object property)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object property) => true;

        #endregion

        public ViewModelMainWindow()
        {
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }
    }
}
