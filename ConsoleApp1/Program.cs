using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Day13_LinqPractice;

internal class Program
{
    static void Main(string[] args)
    {
        var products = GetProducts();

        // TU BĘDĄ TWOJE QUERIES

        var electronics = products.Where(e => e.Category.Equals("Electronics",StringComparison.CurrentCultureIgnoreCase));
        Console.WriteLine("=== ELECTRONICS ===");
        foreach (var p in electronics) Console.WriteLine(p);

        
        var expensive = products.Where(e=>e.Price >500);
        Console.WriteLine("\nProdukty drozsze niz 500zł");
        foreach (var p in expensive) Console.WriteLine(p);

        var lowStock = products.Where(l => l.Stock<20);
        Console.WriteLine("\nProdukty ktore maja mniej niz 20 sztuk w stocku");
        foreach (var p in lowStock) Console.WriteLine(p);


        var expensiveFurniture = products.Where(e => e.Category.Equals("furniture", StringComparison.CurrentCultureIgnoreCase) && e.Price>700);
        Console.WriteLine("\nProdukty z kategorii \"Furniture\" I (AND) cena > 700 zł");
        foreach (var p in expensiveFurniture) Console.WriteLine(p);


        var names = products.Select(n => n.Name);
        Console.WriteLine("\n=== PRODUCT NAMES ===");
        foreach (var name in names) Console.WriteLine(name);

        Console.WriteLine("\nLista produktów z 20% zniżką (nowy anonimowy obiekt)");
        var discountedProductsList = products.Select(p => new
        {
            Name = p.Name,
            OriginalPrice = p.Price,
            DiscountedPrice = p.Price * 0.8m,
            Category = p.Category,

        });
        foreach (var name in discountedProductsList) Console.WriteLine(name);

        var uniqueCategory = products.Select(p => p.Category).Distinct();
        Console.WriteLine("\nTylko unikalne kategorie (Distinct)");
        foreach (var name in uniqueCategory) Console.WriteLine(name);


        var ascendingProducts = products.OrderBy(p => p.Price);
        Console.WriteLine("\nQuery 8: Produkty posortowane po cenie rosnaco");
        foreach (var name in ascendingProducts) Console.WriteLine(name);

        var descendingProducts = products.OrderByDescending(p=>p.Price);
        Console.WriteLine("\nQuery 8: Produkty posortowane po cenie malejaco");
        foreach (var name in descendingProducts) Console.WriteLine(name);



        var sortedProducts = products.OrderBy(p => p.Category).ThenBy(p=>p.Price);
        Console.WriteLine("\nQuery 10: Sortowanie dwupoziomowe: najpierw Category (A-Z), potem Price (rosnąco)");
        foreach (var name in sortedProducts) Console.WriteLine(name);

        var maxPrice = products.Max(p => p.Price);
        var maxProduct = products.First(p=>p.Price==maxPrice);
        Console.WriteLine("\nQuery 11: Najdroższy produkt (Max)");

        Console.WriteLine(maxProduct);

        var minPrice = products.OrderBy(p=>p.Price).First();
        Console.WriteLine("\nQuery 12: Najtanszy produkt (Min)");
        Console.WriteLine(minPrice);

        var averagePrice = products.Average(p => p.Price);
        Console.WriteLine("\nQuery 13: Średnia cena wszystkich produktów(Average)");
        Console.WriteLine(averagePrice);

        var sumPriceStock = products.Sum(p => p.Price * p.Stock);
        Console.WriteLine("\nQuery 14: Suma wartości magazynu(Price × Stock dla każdego, potem suma)");
        Console.WriteLine(sumPriceStock);


        var groupedProducts = products.GroupBy(p => p.Category);
        Console.WriteLine("\nQuery 15: Produkty pogrupowane po kategorii(GroupBy)");
        foreach(var group in groupedProducts)
        {
            Console.WriteLine($"Category Name: {group.Key}");
            foreach (var p in group)
            {
                Console.WriteLine($"{p.Name} : {p.Price:C}");
            }

            Console.WriteLine($"Total items: {group.Count()}");
            Console.WriteLine($"Average price :{group.Average(p=>p.Price):C}\n");
        }
        

    }

    static List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product(1, "Laptop", "Electronics", 3500m, 15),
            new Product(2, "Mouse", "Electronics", 150m, 50),
            new Product(3, "Keyboard", "Electronics", 350m, 30),
            new Product(4, "Monitor", "Electronics", 1200m, 20),
            new Product(5, "Desk", "Furniture", 800m, 10),
            new Product(6, "Chair", "Furniture", 600m, 15),
            new Product(7, "Lamp", "Furniture", 200m, 25),
            new Product(8, "Notebook", "Stationery", 15m, 100),
            new Product(9, "Pen", "Stationery", 5m, 200),
            new Product(10, "Pencil", "Stationery", 3m, 150),
            new Product(11, "Headphones", "Electronics", 450m, 40),
            new Product(12, "Webcam", "Electronics", 350m, 35),
            new Product(13, "Bookshelf", "Furniture", 950m, 8),
            new Product(14, "Eraser", "Stationery", 2m, 300),
            new Product(15, "Ruler", "Stationery", 4m, 250)
        };
    }
}

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Category { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public Product(int id, string name, string category, decimal price, int stock)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        Stock = stock;
    }

    public override string ToString()
    {
        return $"{Name} ({Category}) - {Price:C}, Stock: {Stock}";
    }
}