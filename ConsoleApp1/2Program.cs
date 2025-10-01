//using System.Security.Cryptography.X509Certificates;

//namespace ConsoleApp1;

//internal class Program2
//{
//    static void Main(string[] args)
//    {
//        //var person = new Person("A",1);
//        //person.PrintInfo();
//        //var person1 = new Person("", -1);
//        //person1.PrintInfo();
//        //var dog1 = new Dog("dog1", "breed1");
//        //var dog2 = new Dog("dog2", "breed2");
//        //var owner1 = new Owner("owner1");
//        //owner1.AddDog(dog1);
//        //var owner2 = new Owner("owner2");
//        //owner2.AddDog(dog2);
//        //owner1.ShowDogs();

//        var car = new Car("brand1", "model1", 1);
//        var car2 = new Car("brand2", "model2", 2);
//        car.PrintInfo();
//        car.Model = "dsaa";



//    }
//}

//public class Car
//{

//    public string Brand { get; }
//    public string Model { get; set; }
//    public int Year { get; private set; }
//    public Car(string brand, string model, int year)
//    {
//        Brand = brand;
//        Model = model;
//        Year = year;
//    }

//    public void PrintInfo()
//    {
//        Console.WriteLine($"{Brand}, {Model}, {Year}");
//    }
//}
//public class Dog
//{
//    public string Name { get; set; }
//    public string Breed { get; set; }
//    public Dog(string name, string breed)
//    {
//        Name= name;
//        Breed= breed;
//    }
//}
//public class Owner
//{
//    public Owner(string name)
//    {
//        Name = name;
//    }
//    public string Name { get; set; }

//    public List<Dog> Dogs = new List<Dog>();

//    public void AddDog(Dog dog)
//    {
//        Dogs.Add(dog);
//    }
//    public void ShowDogs()
//    {
//        foreach (Dog dog in Dogs)
//        {
//            Console.WriteLine($"Dog: {dog.Name}, {dog.Breed}");
//        }      
//    }
//}
//public class Person
//{
//    public Person(string name, int age)
//    {
//        Age=age;
//        Name=name;
//    }
//    private string name;
//    private int age;
//    public string Name
//    {
//        get {
//          return  name;
//        }
//         set
//        {
//            if(String.IsNullOrEmpty(value))
//            {
//                name = "Unknown";
//            }
//            else
//            {
//                name = value;
//            }

//        }
//    }
//    public int Age { get
//        {
//            return age;
//        }
//        set
//        {
//            if (value < 0)
//                age = 0;
//            else
//            {
//                age = value;
//            }
//        }
//     }

//    public void PrintInfo()
//    {
//        Console.WriteLine($"Person Name: {name}, Age: {age}");
//    }
//}
