
using System.Numerics;

namespace ConsoleApp1;

internal class Day12_StoreSystem
{
    static void Main(string[] args)
    {
        var shoppingCart = new ShoppingCart();
        var catalog = new List<ProductType>()
        {
            new DigitalProduct("kurs", "kurs description", 2000.20M),
            new DigitalProduct("kurs2", "kurs2 description", 5000.20M),
            new PhysicalProduct("zabawka1", "zabawka1", 2000.20M, 1),
            new PhysicalProduct("drukarka", "epson", 200.20M,10),
            new PhysicalProduct("laptop", "toshiba", 10000.20M,4.99)
        };



        shoppingCart.AddProduct(catalog[0]);
        shoppingCart.AddProduct(catalog[2]);
        shoppingCart.AddProduct(catalog[3]);
        shoppingCart.AddProduct(catalog[4]);

        shoppingCart.ShowAllProducts();

        Console.WriteLine($"Suma produktow bez dostawy w koszyku wynosi: {shoppingCart.SumOfProductsWithoutShipment().ToString("C")}");

        shoppingCart.GetDiscount(15);
        shoppingCart.ShowAllProducts();
        var products = shoppingCart.GetProductsList();

        Console.WriteLine($"Suma produktow bez dostawy w koszyku wynosi: {shoppingCart.SumOfProductsWithoutShipment().ToString("C")}");
        Console.WriteLine($"Suma wysylki kosztuje: {shoppingCart.SumOfShipment().ToString("C")}");
        Console.WriteLine($"suma produktow i wysylki: {(shoppingCart.SumOfProductsWithoutShipment() + shoppingCart.SumOfShipment()).ToString("C")}");


        shoppingCart.RemoveProduct(shoppingCart.GetProductsList()[1]);
        Console.WriteLine("\nPo usunieciu produktu:");
        Console.WriteLine($"Suma produktow bez dostawy w koszyku wynosi: {shoppingCart.SumOfProductsWithoutShipment().ToString("C")}");
        Console.WriteLine($"Suma wysylki kosztuje: {shoppingCart.SumOfShipment().ToString("C")}");
        Console.WriteLine($"suma produktow i wysylki: {(shoppingCart.SumOfProductsWithoutShipment() + shoppingCart.SumOfShipment()).ToString("C")}");


        Console.WriteLine("\n=== CATALOG STATISTICS ===");


        Console.WriteLine($"count wszystkich produktow w katalogu: {catalog.Count()}");
        var physicalCount = catalog.OfType<PhysicalProduct>().Count();
        var digitalCount = catalog.OfType<DigitalProduct>().Count();
        Console.WriteLine($"physical count in catalog: {physicalCount}");
        Console.WriteLine($"digital count in catalog: {digitalCount}");

        Console.WriteLine($"PhysicalCount in shopping cart: {shoppingCart.PhysicalProducts()}");
        Console.WriteLine($"DigitalCount in shopping cart: {shoppingCart.DigitalProducts()}");

        var average = shoppingCart.GetProductsList().Average(p => p.Price);
        Console.WriteLine($"Srednia cena wszystkich produktow w koszyku: {average.ToString("C")}");

        Console.WriteLine($"Najdroszy produkt w koszyku: {products.OrderByDescending(p=>p.Price).First()}");
        Console.WriteLine($"Najtanszy produkt w koszyku: {products.OrderBy(p=>p.Price).First()}");

        Console.WriteLine($"Suma wszystkich produktow w katalogu: {catalog.Sum(p=>p.Price).ToString("C")}");

        Console.WriteLine($"Count produktow powyzej sredniej ceny: {products.Where(p=>p.Price>average).Count()}");
    }

}

public abstract class ProductType
{
    public virtual bool ShipmentAvailable { get; }
    public virtual bool DiscountAvailable { get; }
    public virtual string ProductName { get; }
    public virtual string Description { get; }
    public virtual decimal Price { get; }
    public decimal? DiscountedPrice { get; protected set; }
    public void ApplyDiscount(decimal discountPercent)
    {
        if (DiscountAvailable)
        {
            DiscountedPrice = Price * (1 - discountPercent / 100m);
        }
    }
    public abstract decimal CalculateShippingCost();

    protected ProductType(string productName, string description, decimal price)
    {
        if (string.IsNullOrEmpty(productName))
            throw new ArgumentNullException("Product name cannot be null");
        ProductName = productName;
        Description = description;
        if (price < 0)
            throw new ArgumentException("Price cannot be less than 0");
        Price = price;

    }

}

public class PhysicalProduct : ProductType
{

    public PhysicalProduct(string productName, string description, decimal price, double weight) : base(productName, description, price)
    {
        if (weight < 0)
            throw new ArgumentException("Weight cannot be less than 0");
        Weight = weight;
    }
    public override decimal CalculateShippingCost()
    {
        if (Weight < 1) return 10m;
        if (Weight < 5) return 25m;
        return 50m;
    }

    public override bool ShipmentAvailable { get; } = true;
    public override bool DiscountAvailable { get; } = true;
    //public override string ProductName { get; } = default!;
    //public override string Description { get; } = default!;
    //public override decimal Price { get; }
    public double Weight { get; }

}


public class DigitalProduct : ProductType
{

    public DigitalProduct(string productName, string description, decimal price) : base(productName, description, price)
    {
    }

    public override decimal CalculateShippingCost()
    {
        return 0;
    }
    public override bool ShipmentAvailable { get; } = false;
    public override bool DiscountAvailable { get; } = false;
    //public override string ProductName { get; } = default!;
    //public override string Description { get; } = default!;
    //public override decimal Price { get; }
}

public class ShoppingCart
{    
    private List<ProductType> Products { get; set; } = new List<ProductType>();

    public int DigitalProducts() => Products.OfType<DigitalProduct>().Count();
    public int PhysicalProducts() => Products.OfType<PhysicalProduct>().Count();

    public IReadOnlyList<ProductType> GetProductsList()
    {
        return Products.AsReadOnly();
    }

    public void AddProduct(ProductType product)
    {
        Products.Add(product);
    }

    public void RemoveProduct(ProductType product)
    {
        Products.Remove(product);
    }

    public void ShowAllProducts()
    {
        foreach (var item in Products)
        {
            Console.WriteLine($"ProductName: {item.ProductName} and Price: {item.Price.ToString("C")}");
        }
    }

    public void GetDiscount(decimal discountPercent)
    {
        //Products.Where(p => p.DiscountAvailable == true).Sum(p => p.Price * (1 - discountPercent / 100));
        foreach(var item in Products)
        {
            item.ApplyDiscount(discountPercent);
        }
    }

    public decimal SumOfProductsWithoutShipment()
    {
        return Products.Sum(p => p.DiscountedPrice ?? p.Price);
    }

    public decimal SumOfShipment()
    {
        
        return  GetProductsList().Where(p => p.ShipmentAvailable == true).Sum(s => s.CalculateShippingCost());
        
    }

}
