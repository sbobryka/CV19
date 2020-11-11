using System;
using System.Text;

namespace CV19.Models.Decanat
{
    internal class Student
    {
        private static Random random = new Random();

        public static string[] MaleNames = new string[] { "Сергей", "Андрей", "Михаил", "Алексей", "Владимир", "Денис", "Антон" };
        public static string[] FemaleNames = new string[] { "Ольга", "Ирина", "Светлана", "Мария", "Оксана", "Марина", "Надежда" };

        public static string[] MalePatronymics = new string[] { "Сергеевич", "Андреевич", "Михайлович", "Алексеевич", "Владимирович", "Денисович" };
        public static string[] FemalePatronymics = new string[] { "Сергеевна", "Андреевна", "Михайловна", "Алексеевна", "Владимировна", "Денисовна" };

        public static string[] MaleSurnames = new string[] { "Рогов", "Иванов", "Петров", "Сидоров", "Дубов", "Васильев" };
        public static string[] FemaleSurnames = new string[] { "Рогова", "Иванова", "Петрова", "Сидорова", "Дубова", "Васильева" };

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }

        public Student() { }

        public static Student GenerateRandomStudent()
        {
            return random.Next(2) == 0 ?
            new Student()
            {
                Name = MaleNames[random.Next(MaleNames.Length)],
                Surname = MaleSurnames[random.Next(MaleSurnames.Length)],
                Patronymic = MalePatronymics[random.Next(MalePatronymics.Length)],
                Birthday = DateTime.Now.AddDays(random.NextDouble() * -10000),
                Rating = random.Next(10),
                Description = ""
            }
            :
            new Student()
            {
                Name = FemaleNames[random.Next(FemaleNames.Length)],
                Surname = FemaleSurnames[random.Next(FemaleSurnames.Length)],
                Patronymic = FemalePatronymics[random.Next(FemalePatronymics.Length)],
                Birthday = DateTime.Now.AddDays(random.NextDouble() * -10000),
                Rating = random.Next(10),
                Description = ""
            };
        }
    }
}
