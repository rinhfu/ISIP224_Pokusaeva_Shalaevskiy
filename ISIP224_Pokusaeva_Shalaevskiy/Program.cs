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
}
