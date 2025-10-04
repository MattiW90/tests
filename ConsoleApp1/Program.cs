
namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        var validate = new PersonValidator();
        var People = new List<Person>();
        {
            var person1 = new Person("name1", 18, "abc@abc.com");
            var person2 = new Person("", 18, "abc@abc.com");
            var person3 = new Person("name3", 2, "abc@abc.com");
            var person4 = new Person("name4", 22, "abcabc.com");
        }

        foreach (var person in People)
        {
            if(validate.Validate(person, out string errorMessage1))
            {
                Console.WriteLine($"{person.Name} ({person.Age} {person.Email}) - VALID");
            }
            else
            {
                Console.WriteLine($"{person.Name} ({person.Age} {person.Email}) - ERROR");
            }

        }
    }
}
