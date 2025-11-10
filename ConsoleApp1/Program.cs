namespace Day14_LinqAdvanced;

internal class Program
{
    static void Main(string[] args)
    {
        var students = GetStudents();
        var courses = GetCourses();
        var enrollments = GetEnrollments();

        //Query 1: Student - Enrollment JOIN
        Console.WriteLine("\n=== QUERY 1: STUDENT GRADES (JOIN) ===");
        var studentEnrollment = students.Join(enrollments,
            student => student.Id,
            enrollments => enrollments.StudentId,
            (student, grade) => new
            {
                StudentName = student.Name,
                StudentGrade = grade.Grade,
            });
        foreach(var cnt in studentEnrollment)
        {
            Console.WriteLine($"{cnt.StudentName}, grades: {cnt.StudentGrade}");
        }

        Console.WriteLine("\n=== QUERY 2: FULL INFO (3-WAY JOIN) ===");
        //Query 2: Three-way JOIN
        var studentCourseEnrollment = students.Join(enrollments,
            student => student.Id,
            enrollment => enrollment.StudentId,
            (student, grade) => new
            {
                StudentName = student.Name,
                StudentGrade = grade.Grade,
                grade.CourseId
            }).Join(courses,
            enrollment => enrollment.CourseId,
            course => course.Id,
            (student, course) => new
            {
                student.StudentName,
                course.CourseName,
                studentgrade = student.StudentGrade
            });

        foreach (var cnt in studentCourseEnrollment)
        {
            Console.WriteLine($"{cnt.StudentName}, courseName: {cnt.CourseName}, grades: {cnt.studentgrade}");
        }

        //Query 3: Students with their grades (GROUP JOIN)
        Console.WriteLine("\n=== QUERY 3: STUDENTS WITH GRADES (GROUP JOIN) ===");
        var studentGrades = students.GroupJoin(enrollments,
            student => student.Id,
            enrollment => enrollment.StudentId,
            (student, enrollment) => new
            {
                studentName = student.Name,
                studentGrades = enrollment.Select(g => g.Grade).ToList(),
                gradesCount = enrollment.Count(),
                average = enrollment.Any() ? enrollment.Average(e => e.Grade) : 0
            });

        foreach (var cnt in studentGrades)
        {
            var studentGradesList = string.Join(",", cnt.studentGrades);
            Console.WriteLine($"{cnt.studentName}, studentGrades: [{studentGradesList}], count: {cnt.gradesCount}, average: {cnt.average}");
        }

        //Query 4: Courses with enrolled students count
        Console.WriteLine("\n=== Query 4: Courses with enrolled students count ===");
        var courseStudents = courses.GroupJoin(
            enrollments,
            c => c.Id,
            e => e.CourseId,
            (c, e) => new
            {
                c.CourseName,
                c.Instructor,               
                studentsCount = e.Select(e=>e.StudentId).Distinct().Count()
            });
        foreach (var course in courseStudents)
        {
            Console.WriteLine($"CourseName: {course.CourseName}, Instructor:{course.Instructor}, StudentsCount: {course.studentsCount}");
        }

        //Query 5: All grades (flat list)
        Console.WriteLine("\n=== Query 5: All grades (flat list) ===");
        Console.WriteLine(string.Join(", ",studentGrades.SelectMany(g=>g.studentGrades)));

        //Query 6: All student-course pairs (SELECTMANY with projection)
        Console.WriteLine("\n=== Query 6: All student-course pairs (SELECTMANY with projection) ===");
        var studentWithCourses = students.SelectMany(
            student => enrollments.Where(e => e.StudentId == student.Id),
            (student, enrollment) => new
            {
                StudentName =  student.Name,
                CourseId = enrollment.CourseId,
                Grade = enrollment.Grade
            });

        foreach(var st in studentWithCourses)
        {
            Console.WriteLine($"StudentName: {st.StudentName}, CourseID: {st.CourseId}, Grades: {st.Grade}");
        }

        //Query 7: Product of all grades(iloczyn)
        Console.WriteLine("\n=== Query 7: Product of all grades(iloczyn) ===");

        var allGrades = enrollments.Select(g => g.Grade);

        var product = allGrades.Aggregate(1, (a, b) => a * b);

        Console.WriteLine($"Product: {product}");

        //Query 8: Concatenate all student names
        Console.WriteLine("\n=== Query 8: Concatenate all student names ===");
        var allNames = students.Select(s => s.Name);
        var studentNames = allNames.Aggregate((acc, name) => acc + ", " + name);

        Console.WriteLine($"StudentNames: {studentNames}");

        //Query 9: Students with average > 4.0
        Console.WriteLine("\n=== Query 9: Students with average > 4.0 ===");
        var topStudents = students.GroupJoin(
            enrollments,
            st => st.Id,
            en => en.StudentId,
            (student, enrollment) => new
            {
                StudentName = student.Name,
                average = enrollment.Any()? enrollment.Average(g => g.Grade): 0
            }).Where(g => g.average > 4.0)
            .OrderByDescending(g => g.average);
        
        foreach(var student in topStudents)
        {
            Console.WriteLine($"{student.StudentName} : {student.average}");
        }

        //Query 10: Course statistics
        Console.WriteLine("\n=== Query 10: Course statistics ===");
        var courseStats = courses.GroupJoin(
            enrollments,
            c => c.Id,
            e => e.CourseId,
            (course, enroll) => new
            {
                CourseName = course.CourseName,
                StudentsCount = enroll.Select(st => st.StudentId).Distinct().Count(),
                AverageGrade = enroll.Any() ? enroll.Average(e => e.Grade) : 0,
                MaxGrade = enroll.Any() ? enroll.Max(e => e.Grade) : 0,
                MinGrade = enroll.Any() ? enroll.Min(e => e.Grade) : 0

            }).OrderByDescending(g=>g.AverageGrade);

        foreach(var course in courseStats)
        {
            Console.WriteLine($"CourseName: {course.CourseName}, StudentsCount: {course.StudentsCount}, AverageGrade: {course.AverageGrade}, MaxGrade: {course.MaxGrade}, MinGrade:{course.MinGrade}");
        }

    }

    static List<Student> GetStudents()
    {
        return new List<Student>
        {
            new Student(1, "Jan Kowalski", 2003),
            new Student(2, "Anna Nowak", 2002),
            new Student(3, "Piotr Wiśniewski", 2003),
            new Student(4, "Maria Lewandowska", 2004),
            new Student(5, "Krzysztof Dąbrowski", 2003)
        };
    }

    static List<Course> GetCourses()
    {
        return new List<Course>
        {
            new Course(1, "Mathematics", "Dr. Smith"),
            new Course(2, "Physics", "Dr. Johnson"),
            new Course(3, "Chemistry", "Dr. Williams"),
            new Course(4, "Computer Science", "Dr. Brown"),
            new Course(5, "English", "Dr. Davis")
        };
    }

    static List<Enrollment> GetEnrollments()
    {
        return new List<Enrollment>
        {
            // Jan Kowalski
            new Enrollment(1, 1, 1, 5),
            new Enrollment(2, 1, 2, 4),
            new Enrollment(3, 1, 4, 5),
            
            // Anna Nowak
            new Enrollment(4, 2, 1, 5),
            new Enrollment(5, 2, 3, 4),
            new Enrollment(6, 2, 4, 5),
            new Enrollment(7, 2, 5, 4),
            
            // Piotr Wiśniewski
            new Enrollment(8, 3, 2, 3),
            new Enrollment(9, 3, 3, 3),
            
            // Maria Lewandowska
            new Enrollment(10, 4, 1, 5),
            new Enrollment(11, 4, 2, 5),
            new Enrollment(12, 4, 3, 5),
            new Enrollment(13, 4, 4, 5),
            new Enrollment(14, 4, 5, 5),
            
            // Krzysztof Dąbrowski - BRAK ENROLLMENTS!
        };
    }
}

public class Student
{
    public int Id { get; }
    public string Name { get; }
    public int BirthYear { get; }

    public Student(int id, string name, int birthYear)
    {
        Id = id;
        Name = name;
        BirthYear = birthYear;
    }
}

public class Course
{
    public int Id { get; }
    public string CourseName { get; }
    public string Instructor { get; }

    public Course(int id, string courseName, string instructor)
    {
        Id = id;
        CourseName = courseName;
        Instructor = instructor;
    }
}

public class Enrollment
{
    public int Id { get; }
    public int StudentId { get; }
    public int CourseId { get; }
    public int Grade { get; }

    public Enrollment(int id, int studentId, int courseId, int grade)
    {
        Id = id;
        StudentId = studentId;
        CourseId = courseId;
        Grade = grade;
    }
}