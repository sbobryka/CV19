using CV19.Comands;
using CV19.Models;
using CV19.Models.Decanat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        private Random random = new Random();

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

        #region Groups

        public ObservableCollection<Group> Groups { get; }

        private readonly CollectionViewSource groupsCollectionView = new CollectionViewSource();

        public ICollectionView GroupsCollectionView { get => groupsCollectionView?.View; }

        private void GroupsCollectionView_Filter(object sender, FilterEventArgs e)
        {
            Group group = e.Item as Group;

            if (string.IsNullOrEmpty(GroupTextFilter)) return;
            if (!(group.Name.Contains(GroupTextFilter, StringComparison.OrdinalIgnoreCase))) e.Accepted = false;
        }

        #endregion

        #region GroupStudents

        private readonly CollectionViewSource groupStudentsCollectionView = new CollectionViewSource();

        public ICollectionView GroupStudentsCollectionView { get => groupStudentsCollectionView.View; }

        private void GroupStudentsCollectionView_Filter(object sender, FilterEventArgs e)
        {
            Student student = e.Item as Student;

            if (string.IsNullOrEmpty(StudentTextFilter)) return;
            if (IsStudentsNotFound(student, StudentTextFilter)) e.Accepted = false;
        }

        private bool IsStudentsNotFound(Student student, string textFilter) => (
            !(student.Name.Contains(textFilter, StringComparison.OrdinalIgnoreCase)) &&
            !(student.Surname.Contains(textFilter, StringComparison.OrdinalIgnoreCase)) &&
            !(student.Patronymic.Contains(textFilter, StringComparison.OrdinalIgnoreCase))
            );

        #endregion

        #region SelectedGroup

        private Group _SelectedGroup;

        public Group SelectedGroup
        {
            get => _SelectedGroup;
            set
            {
                if (Set(ref _SelectedGroup, value))
                {
                    groupStudentsCollectionView.Source = value?.Students;
                    GroupStudentsCollectionView?.Refresh();
                    OnPropertyChanged(nameof(GroupStudentsCollectionView));

                    groupStudentsCollectionView.SortDescriptions.Add(new SortDescription("Surname", ListSortDirection.Ascending));
                    groupStudentsCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    groupStudentsCollectionView.SortDescriptions.Add(new SortDescription("Patronymic", ListSortDirection.Ascending));
                }
            }
        }

        #endregion

        #region GroupTextFilter

        private string groupTextFilter;

        public string GroupTextFilter
        {
            get => groupTextFilter;
            set
            {
                if (Set(ref groupTextFilter, value))
                    GroupsCollectionView.Refresh();
            }
        }

        #endregion

        #region StudentTextFilter

        private string studentTextFilter;

        public string StudentTextFilter
        {
            get => studentTextFilter;
            set
            {
                if (Set(ref studentTextFilter, value))
                    GroupStudentsCollectionView?.Refresh();
            }
        }

        #endregion

        #region Команды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object property)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object property) => true;

        #endregion

        #region SetStatusCommand

        public ICommand SetStatusCommand { get; }

        private void OnSetStatusCommandExecuted(object property)
        {
            Status = property.ToString();
        }

        private bool CanSetStatusCommandExecute(object property) => true;

        #endregion

        #region CreateGroupCommand

        public ICommand CreateGroupCommand { get; }

        private void OnCreateGroupCommandExecuted(object property)
        {
            Group group = new Group()
            {
                Name = $"Группа №{random.Next(10000, 99999)}",
                Students = new ObservableCollection<Student>()
            };
            Groups.Insert(0, group);
            SelectedGroup = group;
        }

        private bool CanCreateGroupCommandExecute(object property) => true;

        #endregion

        #region RemoveGroupCommand

        public ICommand RemoveGroupCommand { get; }

        private void OnRemoveGroupCommandExecuted(object property)
        {
            if (!(property is Group group)) return;
            int index = Groups.IndexOf(group);
            Groups.Remove(group);
            if (index < Groups.Count)
                SelectedGroup = Groups[index];
        }

        private bool CanRemoveGroupCommandExecute(object property) => property is Group group && Groups.Contains(group);

        #endregion

        #region ClearGroupTextFilterCommand

        public ICommand ClearGroupTextFilterCommand { get; }

        private void OnClearGroupTextFilterCommandExecuted(object property) => GroupTextFilter = "";

        private bool CanClearGroupTextFilterCommandExecute(object property) => true;

        #endregion

        #region ClearStudentTextFilterCommand

        public ICommand ClearStudentTextFilterCommand { get; }

        private void OnClearStudentTextFilterCommandExecuted(object property) => StudentTextFilter = "";

        private bool CanClearStudentTextFilterCommandExecute(object property) => true;

        #endregion

        public ICommand SelectDirectoryCommand { get; }

        private void OnSelectDirectoryCommandExecuted(object property)
        {
            SelectedDirectory = property as DirectoryViewModel;
        }

        private bool CanSelectDirectoryCommandExecute(object property) => true;

        #endregion

        #region Разнотипный набор данных

        public object[] CompositeCollection { get; }

        private object _SelectedCompositeValue;

        public object SelectedCompositeValue { get => _SelectedCompositeValue; set => Set(ref _SelectedCompositeValue, value); }

        #endregion

        #region RootDirectoriesDisk

        public DirectoryViewModel RootDirectoriesDisk { get; } = new DirectoryViewModel("c:\\");

        #endregion

        #region SelectedDirectory

        private DirectoryViewModel selectedDirectory;

        public DirectoryViewModel SelectedDirectory
        {
            get => selectedDirectory;
            set => Set(ref selectedDirectory, value);
        } 

        #endregion


        #region Конструктор

        public ViewModelMainWindow()
        {
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SetStatusCommand = new RelayCommand(OnSetStatusCommandExecuted, CanSetStatusCommandExecute);
            CreateGroupCommand = new RelayCommand(OnCreateGroupCommandExecuted, CanCreateGroupCommandExecute);
            RemoveGroupCommand = new RelayCommand(OnRemoveGroupCommandExecuted, CanRemoveGroupCommandExecute);
            ClearGroupTextFilterCommand = new RelayCommand(OnClearGroupTextFilterCommandExecuted, CanClearGroupTextFilterCommandExecute);
            ClearStudentTextFilterCommand = new RelayCommand(OnClearStudentTextFilterCommandExecuted, CanClearStudentTextFilterCommandExecute);
            SelectDirectoryCommand = new RelayCommand(OnSelectDirectoryCommandExecuted, CanSelectDirectoryCommandExecute);

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
            //int studentIndex = 1;
            //var students = Enumerable.Range(1, 10).Select(s => new Student
            //{
            //    Name = $"Имя {studentIndex}",
            //    Surname = $"Фамилия {studentIndex}",
            //    Patronymic = $"Отчество {studentIndex++}",
            //    Birthday = DateTime.Now,
            //    Rating = random.Next(10)
            //});
            var students = Enumerable.Range(1, 10).Select(s => Student.GenerateRandomStudent());
            var groups = Enumerable.Range(1, 10).Select(g => new Group
            {
                Name = $"Группа №{random.Next(10000, 99999)}",
                //Name = $"Группа {g}",
                Description = $"Описание {g}",
                Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);

            // Заполнение разнотипного набора данными
            List<object> list = new List<object>();
            list.Add("Какой-то текст");
            list.Add(32);
            list.Add(groups.FirstOrDefault());
            list.Add(students.FirstOrDefault());
            CompositeCollection = list.ToArray();

            // Заполнение коллекции с возможностью фильтрации
            groupsCollectionView.Source = Groups;
            //groupsCollectionView.View.Refresh();
            groupsCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            groupsCollectionView.Filter += GroupsCollectionView_Filter;

            groupStudentsCollectionView.Filter += GroupStudentsCollectionView_Filter;
        }

        #endregion
    }
}
