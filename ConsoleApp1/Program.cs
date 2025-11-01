using System.Collections.Generic;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        List<Shape> shapes = new List<Shape>()
        {
            new Circle(2,"circle"),
            new Rectangle(2,3,"rectangle")
        };
        double totalArea = shapes.Sum(c => c.GetArea());
        Console.WriteLine($"Sumaryczne pole: {totalArea}");
        var theLargest = shapes.OrderByDescending(c => c.GetArea()).FirstOrDefault();
        if (theLargest != null)
        {
            Console.WriteLine($"The largest shape is : {theLargest.GetInfo()}");
        }



        List<IPaymentMethod> payments = new List<IPaymentMethod>()
        {
            new CreditCardPayment("123456789","Mateusz Mateusz"),
            new PayPalPayment("mateusz@gmail.com")
        };

        foreach(var pay in payments)
        {
            pay.ProcessPayment(100.50M);
            Console.WriteLine(pay.GetPaymentDetails());
        }


        /*
    PYTANIE: Dlaczego Shape to abstract class, a IPaymentMethod to interface?

    Moja odpowiedź:
    - Shape: 
    - IPaymentMethod: 
    */
    }
}
public abstract class Shape
{
    private readonly string _name;
    protected Shape(string name)
    {
        _name = name;
    }
    public abstract double GetArea();


    public virtual string GetInfo()
    {
        return $"Shape: {_name}, Area: {GetArea()}";
    }
}
public class Circle : Shape
{
    private readonly double _radius;
    public Circle(double radius, string name) : base( name)
    {
        if (radius <= 0)
            throw new ArgumentException("Radius cannot be less and equal than zero");
        _radius = radius;
        
    }
    public override double GetArea()
    {
        return double.Pi * _radius * _radius;
    }
}

public class Rectangle : Shape
{
    private readonly double _width;
    private readonly double _height;

    public Rectangle(double width, double height, string name) : base(name)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("width or/and height cannot be less or equal to zero");

        _width = width;
        _height = height;
    }
    public override double GetArea()
    {
        return _width * _height;   
    }
}

public interface IPaymentMethod
{
    bool ProcessPayment(decimal amount);
    string GetPaymentDetails();

}
public class CreditCardPayment : IPaymentMethod
{
    private readonly string _cardNumber;
    private readonly string _cardHolder;

    public CreditCardPayment(string cardNumber, string cardHolder)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("Card number cannot be empty");
        if (string.IsNullOrWhiteSpace(cardHolder))
            throw new ArgumentException("Card holder cannot be empty");
        if (cardNumber.Length < 4)  // Dla [^4..] potrzebujemy min 4 znaki!
            throw new ArgumentException("Card number too short");
        _cardHolder = cardHolder;
        _cardNumber = cardNumber;
    }
    public string GetPaymentDetails()
    {
        return $"Credit Card: **** {_cardNumber[^4..]}";
    }


    public bool ProcessPayment(decimal amount)
    {

        Console.WriteLine($"Processing ${amount} via Credit Card{_cardNumber[^4..]}");
        if(amount >0)
        {
            return true;
        }
        Console.WriteLine("Amount cannot be <=0");
        return false;
       
    }

   
}

public class PayPalPayment : IPaymentMethod
{
    private readonly string _email;

    public PayPalPayment(string email)
    {
        _email = email;
    }
    public string GetPaymentDetails()
    {
        return $"PayPal: {_email}";
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via PayPal ({_email})");
        if (amount<=0)
        {
            Console.WriteLine("Amount cannot be <=0");
            return false;
        }
        return true;
    }
}
