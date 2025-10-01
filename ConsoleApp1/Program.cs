
namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        var person1 = new Person("name1", 18, "abc@abc.com");
        var person2 = new Person("", 18, "abc@abc.com");
        var person3 = new Person("name3", 2, "abc@abc.com");
        var person4 = new Person("name4", 22, "abcabc.com");

        var validate1 = new PersonValidator();
        validate1.Validate(person1, out string errorMessage1);
        Console.WriteLine(errorMessage1);

        var validate2 = new PersonValidator();
        validate2.Validate(person2, out string errorMessage2);
        Console.WriteLine(errorMessage2);

        var validate3 = new PersonValidator();
        validate2.Validate(person2, out string errorMessage3);
        Console.WriteLine(errorMessage3);

        var validate4 = new PersonValidator();
        validate4.Validate(person3, out string errorMessage4);
        Console.WriteLine(errorMessage4);
    }
}
