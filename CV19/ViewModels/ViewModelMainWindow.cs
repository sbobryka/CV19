using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
