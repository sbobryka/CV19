﻿using CV19.Comands;
using CV19.Models;
using CV19.Models.Decanat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        #region Набор данных для визуализации графика

        private IEnumerable<DataPoint> dataPoints;

        public IEnumerable<DataPoint> DataPoints
        {
            get => dataPoints;
            set => Set(ref dataPoints, value);
        }

        #endregion

        #region Группы и студенты

        public ObservableCollection<Group> Groups { get; }

        #endregion

        #region Команды

        /// <summary>
        /// Завершает работу приложения
        /// </summary>
        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object property)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object property) => true;

        /// <summary>
        /// Устанавливает статус
        /// </summary>
        public ICommand SetStatusCommand { get; }

        private void OnSetStatusCommandExecuted(object property)
        {
            Status = property.ToString();
        }

        private bool CanSetStatusCommandExecute(object property) => true;

        #endregion

        public ViewModelMainWindow()
        {
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SetStatusCommand = new RelayCommand(OnSetStatusCommandExecuted, CanSetStatusCommandExecute);

            // Заполнение тестовыми данными
            List<DataPoint> testDataPoints = new List<DataPoint>((int)(360 / 0.1));
            const double TO_RAD = Math.PI / 180;
            for (double x = 0; x < 360; x += 0.1)
            {
                double y = Math.Sin(x * TO_RAD);
                testDataPoints.Add(new DataPoint(x, y));
            }
            DataPoints = testDataPoints;

            // Заполнение тестовыми данным группы студентов
            int studentIndex = 1;
            var students = Enumerable.Range(1, 10).Select(s => new Student
            {
                Name = $"Name {studentIndex}",
                Surname = $"Surname {studentIndex}",
                Patronymic = $"Patronymic {studentIndex++}",
                Birthday = DateTime.Now,
                Rating = 0
            });
            var groups = Enumerable.Range(1, 20).Select(g => new Group
            {
                Name = $"Группа {g}",
                Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);
        }
    }
}
