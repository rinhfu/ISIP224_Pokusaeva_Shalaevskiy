using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    public abstract class Person
    {
        private string _name;
        private int _age;
        private string _email;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым.");
                _name = value.Trim();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 16 || value > 100)
                    throw new ArgumentException("Возраст должен быть от 16 до 100.");
                _age = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Некорректный email.");
                _email = value.Trim();
            }
        }

        protected Person(string name, int age, string email)
        {
            Name = name;
            Age = age;
            Email = email;
        }

        public abstract string GetInfo();
        public override string ToString() => GetInfo();
    }

    public class Student : Person
    {
        private static int _counter = 1;
        private string _groupName;

        public int Id { get; }
        public string GroupName
        {
            get => _groupName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Группа не может быть пустой.");
                _groupName = value.Trim();
            }
        }

        public Student(string name, int age, string email, string groupName)
            : base(name, age, email)
        {
            GroupName = groupName;
            Id = _counter++;
        }

        public override string GetInfo()
            => $"[Студент #{Id}] {Name}, {Age} лет, группа: {GroupName}, email: {Email}";
    }

    public class Teacher : Person
    {
        private static int _counter = 1;
        private string _department;
        private string _degree;

        public int Id { get; }
        public string Department
        {
            get => _department;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Кафедра не может быть пустой.");
                _department = value.Trim();
            }
        }

        public string Degree
        {
            get => _degree;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Степень не может быть пустой.");
                _degree = value.Trim();
            }
        }

        public Teacher(string name, int age, string email, string department, string degree)
            : base(name, age, email)
        {
            Department = department;
            Degree = degree;
            Id = _counter++;
        }

        public override string GetInfo()
            => $"[Преподаватель #{Id}] {Name}, {Age} лет, кафедра: {Department}, степень: {Degree}, email: {Email}";
    }
    public class Course
    {
        private static int _counter = 1;
        private string _title;
        private decimal _price;
        private int _hours;

        public int Id { get; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; } = new List<Student>();

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название курса не может быть пустым.");
                _title = value.Trim();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной.");
                _price = value;
            }
        }

        public int Hours
        {
            get => _hours;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Количество часов должно быть положительным.");
                _hours = value;
            }
        }

        public Course(string title, decimal price, int hours)
        {
            Title = title;
            Price = price;
            Hours = hours;
            Id = _counter++;
        }

        public void Enroll(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (Students.Contains(student))
                throw new InvalidOperationException("Студент уже записан на этот курс.");
            Students.Add(student);
        }

        public string GetInfo()
        {
            string teacherInfo = Teacher != null ? Teacher.Name : "не назначен";
            return $"[Курс #{Id}] {Title}, {Hours} ч., цена: {Price:C}, преподаватель: {teacherInfo}, студентов: {Students.Count}";
        }
    }
    public class University
    {
        private readonly List<Student> _students = new List<Student>();
        private readonly List<Teacher> _teachers = new List<Teacher>();
        private readonly List<Course> _courses = new List<Course>();

        public IReadOnlyList<Student> Students => _students;
        public IReadOnlyList<Teacher> Teachers => _teachers;
        public IReadOnlyList<Course> Courses => _courses;

        public void AddStudent(Student s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            _students.Add(s);
        }

        public void AddTeacher(Teacher t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            _teachers.Add(t);
        }

        public void AddCourse(Course c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            _courses.Add(c);
        }

        public Student FindStudent(int id) => _students.FirstOrDefault(s => s.Id == id);
        public Teacher FindTeacher(int id) => _teachers.FirstOrDefault(t => t.Id == id);
        public Course FindCourse(int id) => _courses.FirstOrDefault(c => c.Id == id);

        public void EnrollStudent(int studentId, int courseId)
        {
            var student = FindStudent(studentId)
                ?? throw new InvalidOperationException("Студент не найден.");
            var course = FindCourse(courseId)
                ?? throw new InvalidOperationException("Курс не найден.");
            course.Enroll(student);
        }

        public void AssignTeacher(int teacherId, int courseId)
        {
            var teacher = FindTeacher(teacherId)
                ?? throw new InvalidOperationException("Преподаватель не найден.");
            var course = FindCourse(courseId)
                ?? throw new InvalidOperationException("Курс не найден.");
            course.Teacher = teacher;
        }

        public List<Course> GetStudentCourses(int studentId)
        {
            var student = FindStudent(studentId);
            if (student == null) return new List<Course>();
            return _courses.Where(c => c.Students.Contains(student)).ToList();
        }
    }
    public class Program
    {
        private static readonly University uni = new University();

        public static void Main()
        {
            while (true)
            {
                PrintMenu();
                int choice = ReadInt("Выберите пункт: ", 0, 13);

                try
                {
                    switch (choice)
                    {
                        case 0: return;
                        case 1: AddStudent(); break;
                        case 2: AddTeacher(); break;
                        case 3: AddCourse(); break;
                        case 4: ShowAllStudents(); break;
                        case 5: ShowAllTeachers(); break;
                        case 6: ShowAllCourses(); break;
                        case 7: ShowStudentInfo(); break;
                        case 8: ShowTeacherInfo(); break;
                        case 9: ShowCourseInfo(); break;
                        case 10: EnrollStudent(); break;
                        case 11: AssignTeacher(); break;
                        case 12: ShowStudentCourses(); break;
                        case 13: ShowCourseStudents(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("========== УПРАВЛЕНИЕ УНИВЕРСИТЕТОМ ==========");
            Console.WriteLine("1.  Добавить студента");
            Console.WriteLine("2.  Добавить преподавателя");
            Console.WriteLine("3.  Добавить курс");
            Console.WriteLine("4.  Показать всех студентов");
            Console.WriteLine("5.  Показать всех преподавателей");
            Console.WriteLine("6.  Показать все курсы");
            Console.WriteLine("7.  Информация о студенте");
            Console.WriteLine("8.  Информация о преподавателе");
            Console.WriteLine("9.  Информация о курсе");
            Console.WriteLine("10. Записать студента на курс");
            Console.WriteLine("11. Назначить преподавателя на курс");
            Console.WriteLine("12. Курсы студента");
            Console.WriteLine("13. Студенты курса");
            Console.WriteLine("0.  Выход");
            Console.WriteLine("==============================================");
        }

        private static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) return input.Trim();
                Console.WriteLine("Поле не может быть пустым. Попробуйте снова.");
            }
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Введите целое число от {min} до {max}.");
            }
        }

        private static decimal ReadDecimal(string prompt, decimal min)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= min)
                    return value;
                Console.WriteLine($"Введите число не меньше {min}.");
            }
        }

        private static void AddStudent()
        {
            string name = ReadString("Имя: ");
            int age = ReadInt("Возраст: ", 16, 100);
            string email = ReadString("Email: ");
            string group = ReadString("Группа: ");
            uni.AddStudent(new Student(name, age, email, group));
            Console.WriteLine("Студент добавлен.");
        }

        private static void AddTeacher()
        {
            string name = ReadString("Имя: ");
            int age = ReadInt("Возраст: ", 16, 100);
            string email = ReadString("Email: ");
            string dept = ReadString("Кафедра: ");
            string degree = ReadString("Степень: ");
            uni.AddTeacher(new Teacher(name, age, email, dept, degree));
            Console.WriteLine("Преподаватель добавлен.");
        }

        private static void AddCourse()
        {
            string title = ReadString("Название курса: ");
            decimal price = ReadDecimal("Цена: ", 0);
            int hours = ReadInt("Часы: ", 1, 1000);
            uni.AddCourse(new Course(title, price, hours));
            Console.WriteLine("Курс добавлен.");
        }

        private static void ShowAllStudents()
        {
            if (!uni.Students.Any()) { Console.WriteLine("Студентов нет."); return; }
            foreach (var s in uni.Students) Console.WriteLine(s.GetInfo());
        }

        private static void ShowAllTeachers()
        {
            if (!uni.Teachers.Any()) { Console.WriteLine("Преподавателей нет."); return; }
            foreach (var t in uni.Teachers) Console.WriteLine(t.GetInfo());
        }

        private static void ShowAllCourses()
        {
            if (!uni.Courses.Any()) { Console.WriteLine("Курсов нет."); return; }
            foreach (var c in uni.Courses) Console.WriteLine(c.GetInfo());
        }

        private static void ShowStudentInfo()
        {
            int id = ReadInt("ID студента: ", 1, int.MaxValue);
            var s = uni.FindStudent(id);
            Console.WriteLine(s != null ? s.GetInfo() : "Студент не найден.");
        }

        private static void ShowTeacherInfo()
        {
            int id = ReadInt("ID преподавателя: ", 1, int.MaxValue);
            var t = uni.FindTeacher(id);
            Console.WriteLine(t != null ? t.GetInfo() : "Преподаватель не найден.");
        }

        private static void ShowCourseInfo()
        {
            int id = ReadInt("ID курса: ", 1, int.MaxValue);
            var c = uni.FindCourse(id);
            Console.WriteLine(c != null ? c.GetInfo() : "Курс не найден.");
        }

        private static void EnrollStudent()
        {
            int sid = ReadInt("ID студента: ", 1, int.MaxValue);
            int cid = ReadInt("ID курса: ", 1, int.MaxValue);
            uni.EnrollStudent(sid, cid);
            Console.WriteLine("Студент записан на курс.");
        }

        private static void AssignTeacher()
        {
            int tid = ReadInt("ID преподавателя: ", 1, int.MaxValue);
            int cid = ReadInt("ID курса: ", 1, int.MaxValue);
            uni.AssignTeacher(tid, cid);
            Console.WriteLine("Преподаватель назначен на курс.");
        }

        private static void ShowStudentCourses()
        {
            int sid = ReadInt("ID студента: ", 1, int.MaxValue);
            var courses = uni.GetStudentCourses(sid);
            if (!courses.Any()) { Console.WriteLine("Студент ни на один курс не записан."); return; }
            foreach (var c in courses) Console.WriteLine(c.GetInfo());
        }

        private static void ShowCourseStudents()
        {
            int cid = ReadInt("ID курса: ", 1, int.MaxValue);
            var course = uni.FindCourse(cid);
            if (course == null) { Console.WriteLine("Курс не найден."); return; }
            if (!course.Students.Any()) { Console.WriteLine("На курс никто не записан."); return; }
            foreach (var s in course.Students) Console.WriteLine(s.GetInfo());
        }
    }

}
