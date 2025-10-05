namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {

        var employeeList = new List<Employee>()
        {
            new Developer("Mateusz",10000,new DateTime(2022,10,01),".net", 4),
            new Developer("Grzesiek", 11000, new DateTime(2022, 10, 31), ".net", 16),
            new Manager("Piotr",30000, new DateTime(2021,09,01), 50, "IT"),
            new Manager("Kacper",20000, new DateTime(2022,09,01), 20, "IT"),
            new Intern("Michal", 5000, new DateTime(2024,12,12),"uł",new DateTime(2025,11,11)),
            new Intern("Jarek", 8000, new DateTime(2024,12,30),"pł",new DateTime(2025,12,11))
        };
        foreach(Employee employee in employeeList)
        {
            Console.WriteLine(employee.GetEmployeeInfo());
            Console.WriteLine($" Bonus: {employee.CalculateBonus():C}");
            Console.WriteLine($" Total: {employee.TotalCompensation:C}");
            Console.WriteLine();
        }

        Console.WriteLine("\n=== STATISTICS ===");
        var sumOfBonus = employeeList.Sum(a => a.CalculateBonus());
        Console.WriteLine($"Sum bonus of all employees: {sumOfBonus}");
        var sumOfBaseSalary = employeeList.Sum(a => a.BaseSalary);
        Console.WriteLine($"Sum base salary of all employees: {sumOfBaseSalary}");

        Console.WriteLine("\n=== Developers ===");
        var listOfDevelopers = employeeList.OfType<Developer>().ToList();
        foreach(var employee in listOfDevelopers)
        {
            Console.WriteLine(" Developers: " + employee.GetEmployeeInfo());
        }

        Console.WriteLine("\n=== Managers ===");
        var listOfManagers = employeeList.OfType<Manager>().ToList();
        foreach (var employee in listOfManagers)
        {
            Console.WriteLine(" Managers: " + employee.GetEmployeeInfo());
        }

        Console.WriteLine("\n=== Interns ===");
        var listOfInterns = employeeList.OfType<Intern>().ToList();
        foreach (var employee in listOfInterns)
        {
            Console.WriteLine(" Interns:" + employee.GetEmployeeInfo());
        }

        Console.WriteLine("\n=== Top 3 employees ===");
        var top3totalCompensation = employeeList.OrderByDescending(a => a.TotalCompensation).Take(3);
        foreach (var employee in top3totalCompensation)
        {
            Console.WriteLine("top 3 total compensation: " + employee.GetEmployeeInfo());
        }

        var averageSalaryDeveloper = listOfDevelopers.Average(b => b.BaseSalary);
        var averageSalaryManager = listOfManagers.Average(b => b.BaseSalary);
        var averageSalaryIntern= listOfInterns.Average(b => b.BaseSalary);
        Console.WriteLine($"Average salary for developers: {averageSalaryDeveloper}, managers: {averageSalaryManager}, interns: {averageSalaryIntern}");
    }
}

public abstract class Employee
{
    private static int _nextId = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal BaseSalary { get; set; }
    public DateTime HireDate { get; set; }

    protected Employee(string name, decimal baseSalary, DateTime hireDate)
    {
        if(string.IsNullOrEmpty(name)) 
            throw new ArgumentException("Name cannot be empty or null");
        if (baseSalary < 0)
            throw new ArgumentException("Base salary cannot be negative");
        Id = _nextId++;
        Name = name;
        BaseSalary = baseSalary;
        HireDate = hireDate;


    }
    public abstract decimal CalculateBonus();
    public virtual string GetEmployeeInfo()
    {
        string employeeInfo = $"Employee details: Name: {Name}, BaseSalary:{BaseSalary}, HireDate: {HireDate}";
        return employeeInfo;
    }

    public decimal TotalCompensation => BaseSalary + CalculateBonus();
}

public class Developer : Employee
{
    private const decimal BaseBonusSalary = 0.2m;
    private const decimal ExperienceBonusSalary = 0.02m;
    public string ProgrammingLanguage { get; set; } = default!;
    public int YearsOfExperience {  get; set; }
    public Developer(string name, decimal baseSalary, DateTime hireDate, string programmingLanguage, int yearsOfExperience) : base(name, baseSalary, hireDate)
    {
        ProgrammingLanguage = programmingLanguage;
        YearsOfExperience = yearsOfExperience;
    }
    public override decimal CalculateBonus()
    {
        decimal bonus = BaseBonusSalary * BaseSalary + ExperienceBonusSalary * BaseSalary * YearsOfExperience;
        return bonus;
    }
    public override string GetEmployeeInfo()
    {
        string baseEmployeeInfo = base.GetEmployeeInfo();
        string employeeInfo = $"{baseEmployeeInfo}, Language:{ProgrammingLanguage}, yearofExperience: {YearsOfExperience}";
        return employeeInfo;
    }
}
public class Manager : Employee
{
    private const decimal BaseBonusSalary = 0.3m;
    private const decimal BaseTeamSalaryBonus = 0.05m;
    public int TeamSize { get; set; }
    public string Department { get; set; }

    public Manager(string name, decimal baseSalary, DateTime hireDate, int teamSize, string department) : base(name, baseSalary, hireDate)
    {
        TeamSize = teamSize;
        Department = department;
    }

    public override decimal CalculateBonus()
    {
        decimal bonus = BaseBonusSalary * BaseSalary + BaseTeamSalaryBonus * BaseSalary* TeamSize;
        return bonus;

    }
    public override string GetEmployeeInfo()
    {
        string employeeInfo = base.GetEmployeeInfo() + $", team size: {TeamSize}, department = {Department}";
        return employeeInfo;
    }
}

public class Intern : Employee
{
    private const decimal BaseBonusSalary = 0.05m;
    public string University { get; set; }
    public DateTime InternshipEndDate { get; set; }
    public Intern(string name, decimal baseSalary, DateTime hireDate, string university, DateTime internshipEndDate) : base(name, baseSalary, hireDate)
    {
        University = university;
        InternshipEndDate = internshipEndDate;
    }
    public override decimal CalculateBonus()
    {
        decimal bonus = BaseBonusSalary * BaseSalary;
        return bonus;
    }
    public override string GetEmployeeInfo()
    {
        string employeeInfo = base.GetEmployeeInfo() + $", university: {University}";
        return employeeInfo;
    }
}
