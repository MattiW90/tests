
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;

namespace DAY18_ECOMMERCE;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===================================\r\n  E-COMMERCE ANALYTICS SYSTEM\r\n===================================");

        Console.WriteLine("[MAIN MENU]");
        int i = 1;
        do
        {
            Console.WriteLine("1. Dashboard\r\n2. Customer Analytics\r\n3. Product Analytics\r\n4. Regional Reports\r\n5. Trend Analysis\r\n6. Search & Filter\r\n0. Exit");
            if (int.TryParse(Console.ReadLine(),out int result))
            {
                switch(result)
                {
                    case 0:
                        i = 0;
                        break;
                    case 1: 
                        Console.WriteLine("=== DASHBOARD ===");
                        
                        Dashboard.ShowStats(Sale.SaleList, Customer.CustomerList, Product.ProductList);
                        break;
                    case 2:
                        int vipThreshold = 3000;
                        int regularThreshold = 1100;
                        int newThreshold = 200;
                        Console.WriteLine("=== CUSTOMER ANALYTICS ===\r\n");
                        CustomerAnalytics.CustomerStats(Sale.SaleList, Customer.CustomerList);

                        Console.WriteLine("Customer Segmentation:");

                        var CustomerList = CustomerAnalytics.GetCustomersList(Sale.SaleList);
                        var CustomerCount = CustomerAnalytics.CustomersCount;
                        var VipCount = CustomerList.Where(s=>s.TotalSpent> vipThreshold).Count();                       
                        Console.WriteLine($"VIP ({vipThreshold:C}): {VipCount} customers, {100 * VipCount / CustomerCount} %");

                        var VipRegular = CustomerList.Where(s => s.TotalSpent >= regularThreshold && s.TotalSpent< vipThreshold).Count();
                        Console.WriteLine($"Regular ({regularThreshold:C} - {vipThreshold:C}): {VipRegular} customers, {100 * VipRegular / CustomerCount} %");

                        var NewCount = CustomerList.Where(s => s.TotalSpent < vipThreshold).Count();
                        Console.WriteLine($"New ({newThreshold:C}): {NewCount} customers, {100 * NewCount / CustomerCount} %");

                        var repeatCustomers = CustomerAnalytics.RepeatCustomers(Sale.SaleList);
                        var transactionsCount = CustomerAnalytics.TransactionsCount(Sale.SaleList);
                        Console.WriteLine($"Repeat Customers: {repeatCustomers} {(double)repeatCustomers/CustomerCount*100} %" );
                        Console.WriteLine($"Avg Purchases per Customer: {(double) transactionsCount/CustomerCount}");

                        break;

                    case 3:
                        Console.WriteLine("=== PRODUCT ANALYTICS ===\r\n");
                        ProductAnalytics.TopProducts(Sale.SaleList,Product.ProductList, 5);
                        
                        Console.WriteLine();
                        Console.WriteLine();

                        ProductAnalytics.ProductsByCategory(Sale.SaleList, Product.ProductList);
                        Console.WriteLine();
                        Console.WriteLine();

                        ProductAnalytics.UnderPerforming(Sale.SaleList, Product.ProductList);
                        Console.WriteLine();
                        Console.WriteLine();

                        break;

                    case 4:
                        Console.WriteLine("=== REGIONAL REPORTS ===");
                        RegionalReports.SalesByCity(Sale.SaleList, Customer.CustomerList);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Top Seller per City:");
                        RegionalReports.TopSellerByCity(Sale.SaleList,Customer.CustomerList);

                        Console.WriteLine();
                        Console.WriteLine();
                        break;

                    case 5:
                        Console.WriteLine("=== TREND ANALYSIS ===");
                        TrendAnalysis.MonthlySales(Sale.SaleList);

                        Console.WriteLine();
                        Console.WriteLine();

                        break;

                    case 6:

                        Console.WriteLine("=== SEARCH & FILTER ===");
                        SearchFilter.FindCustomer(Sale.SaleList, Customer.CustomerList, Product.ProductList);


                        break;
                }


            }
            else
            {
                Console.WriteLine("Write only number");
            }

            
        } while (i != 0);
    }
}

public class Sale(int saleId, int customerId, int productId, int quantity, decimal price, DateTime saleDate)
{
    public int SaleId { get; set; } = saleId;       // Unique ID
    public int CustomerId { get; set; } = customerId;    // Link to Customer
    public int ProductId { get; set; } = productId;     // Link to Product
    public int Quantity { get; set; } = quantity;     // Ile sztuk sprzedane
    public decimal Price { get; set; } = price;     // Cena jednostkowa
    public DateTime SaleDate { get; set; } = saleDate;    // Kiedy sprzedane

    public decimal Total => Quantity * Price; // Calculated total

    public static List<Sale> SaleList = new List<Sale>()
    {
        new Sale(1, 1, 1, 2, 1500, new DateTime(2025, 10, 15)), // John - 2 × Laptop
        new Sale(2, 2, 2, 5, 25, new DateTime(2025, 10, 16)),   // Jane - 5 × Mouse
        new Sale(3, 3, 3, 3, 80, new DateTime(2025, 9, 10)),    // Bob - 3 × Keyboard
        new Sale(4, 1, 4, 1, 900, new DateTime(2025, 8, 21)),   // John - Monitor
        new Sale(5, 4, 5, 2, 120, new DateTime(2025, 11, 03)),  // Anna - Desk Lamp
        new Sale(6, 5, 6, 1, 750, new DateTime(2025, 10, 12)),  // Marek - Chair
        new Sale(7, 6, 7, 4, 300, new DateTime(2025, 7, 19)),   // Ewa - Headphones
        new Sale(8, 7, 8, 6, 40, new DateTime(2025, 9, 22)),    // Piotr - USB‑C
        new Sale(9, 8, 9, 1, 3200, new DateTime(2025, 9, 30)),  // Alicja - Smartphone
        new Sale(10, 3, 10, 2, 200, new DateTime(2025, 10, 10)),// Bob - Backpack
        new Sale(11, 2, 7, 1, 300, new DateTime(2025, 10, 05)), // Jane - Headphones
        new Sale(12, 8, 2, 3, 25, new DateTime(2025, 10, 20)),  // Alicja - Mouse
    };

}

public class Customer(int customerId, string name, string email, string city)
{
    public int CustomerId { get; set; } = customerId;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string City { get; set; } = city;

    public static List<Customer> CustomerList = new List<Customer>()
    {
        new Customer (1, "John Doe", "john@example.com", "Warsaw"),
        new Customer (2, "Jane Smith", "jane@example.com", "Krakow"),
        new Customer (3, "Bob Johnson", "bob@example.com", "Gdansk"),
        new Customer (4, "Anna Kowalska", "anna.k@example.com", "Lublin"),
        new Customer (5, "Marek Nowak", "marek.nowak@example.com", "Poznan"),
        new Customer (6, "Ewa Wisniewska", "ewa.w@example.com", "Warsaw"),
        new Customer (7, "Piotr Zielinski", "piotr.z@example.com", "Wroclaw"),
        new Customer (8, "Alicja Kowalczyk", "alicja@example.com", "Gdynia"),
        new Customer (9, "Alicja Kowalczykowna", "alicjak@example.com", "Belchatow")
    };

}


public class Product(int productId, string name, string category, decimal price)
{
    public int ProductId { get; set; } = productId;    // Unique ID
    public string Name { get; set; } = name;     // "Laptop", "Mouse"
    public string Category { get; set; } = category;  // "Electronics", "Accessories"
    public decimal Price { get; set; } = price;    // 1500, 25, 80

    public static List<Product> ProductList = new List<Product>()
    {
        new Product(1, "Laptop", "Electronics", 1500),
        new Product(2, "Mouse", "Accessories", 25),
        new Product(3, "Keyboard", "Accessories", 80),
        new Product(4, "Monitor", "Electronics", 900),
        new Product(5, "Desk Lamp", "Home", 120),
        new Product(6, "Office Chair", "Furniture", 750),
        new Product(7, "Headphones", "Electronics", 300),
        new Product(8, "USB-C Cable", "Accessories", 40),
        new Product(9, "Smartphone", "Electronics", 3200),
        new Product(10, "Backpack", "Bags", 200),
        new Product(11, "Something", "New", 20000000)
    };

}

public class Dashboard()
{
    public static void ShowStats(List<Sale> sales, List<Customer> customers, List<Product> products)
    {
        if(!sales.Any())
        {
            Console.WriteLine("No sales");
            return;
        }
        var totalSales = sales.Sum(s=>s.Total);
        var totalTransactions = sales.Count();
        var avgTransaction = totalTransactions > 0 ? totalSales / totalTransactions : 0;

        var topCustomer = sales.Join(customers, 
            sale => sale.CustomerId, customer => customer.CustomerId, 
            (sale,customer) => new { Name = customer.Name, Total = sale.Total } )
            .GroupBy(c => c.Name)
                                .Select(g=>new
                                {
                                    CustomerName = g.Key,
                                    TotalSpent = g.Sum(s => s.Total)
                                })
                                .OrderByDescending(g=>g.TotalSpent)
                                .FirstOrDefault();

        var topProduct = sales.Join(products,
            sale => sale.ProductId, product => product.ProductId,
            (sale, product) => new { Name = product.Name, Quantity = sale.Quantity })
            .GroupBy(p => p.Name)
            .Select(g => new
            {
                Name = g.Key,
                TotalQuantity = g.Sum(q => q.Quantity)
            })
            .OrderByDescending(g => g.TotalQuantity)
            .FirstOrDefault();

        var activeCustomers = sales.Select(c => c.CustomerId).Distinct().Count();
        var productsSold = sales.Select(p => p.ProductId).Distinct().Count();
        var uniqueCities = sales.Join(customers, s => s.CustomerId, c => c.CustomerId,
            (s, c) => new
            {
                City = c.City
            }).Distinct().Count();

        Console.WriteLine($"Total Sales: {totalSales}");
        Console.WriteLine($"Total Transactions: {totalTransactions}");
        Console.WriteLine($"Avg Transaction: {avgTransaction}");
        if ( topCustomer != null )
        {
            Console.WriteLine($"Top Customer: {topCustomer.CustomerName}, Spent: {topCustomer.TotalSpent}");
        }
        else
        {
            Console.WriteLine("Top Customer: NA");
        }
        if (topProduct != null)
        {

            Console.WriteLine($"Top Product: {topProduct.Name}, Quantity: {topProduct.TotalQuantity}");
        }
        else
        {
            Console.WriteLine("Total Product: NA");
        }

        Console.WriteLine($"ActiveCustomers: {activeCustomers}");
        Console.WriteLine($"ProductsSold: {productsSold}");
        Console.WriteLine($"UniqueCities: {uniqueCities}");

        
    }
}
public class CustomerSpending
{
    public int CustomerId { get; set; }
    public decimal TotalSpent { get; set; }
}
public class CustomerAnalytics()
{
    public static void CustomerStats(List<Sale> sales, List<Customer> customers)
    {
        var topCustomers = sales.Join(customers, s => s.CustomerId, c => c.CustomerId,
            (s, c) => new
            {
                CustomerName = c.Name,
                Total = s.Total,
                City = c.City,
                Quantity = s.Quantity
            })
            .GroupBy(c=> new { c.CustomerName, c.City })
            .Select(g => new
            {
                Name=g.Key.CustomerName,
                TotalSum = g.Sum(s => s.Total),
                City=g.Key.City,
                TotalQuantity = g.Sum(s=>s.Quantity)
            })
            .OrderByDescending(o=>o.TotalSum)
            .Take(6);


        Console.WriteLine("Top 6 Customers by Spending:");
        foreach (var item in topCustomers)
        {
            Console.WriteLine($"{item.Name} ({item.City}): {item.TotalSum:C} ({item.TotalQuantity})");
        }
        Console.WriteLine("\r\n");

    }

    public static List<CustomerSpending> GetCustomersList(List<Sale> sales)
    {
        var vipCustomersList = sales.GroupBy(c => c.CustomerId)
            .Select(s => new CustomerSpending
            {
                CustomerId = s.Key,
                TotalSpent = s.Sum(g => g.Total)
            })
        .ToList();


        return vipCustomersList;
    }


    public static int CustomersCount
    {
        get
        {
            return Sale.SaleList.Select(s => s.CustomerId).Distinct().Count();
        }
    }

    public static int RepeatCustomers(List<Sale> sales)
    {
        var repeatCustomers = sales.GroupBy(c => c.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                PurchaseCount = g.Count()
            })
            .Where(p => p.PurchaseCount > 1)
            .Count();
            
        return repeatCustomers;
    }

    public static int TransactionsCount(List<Sale> sales)
    {
        return sales.Count();
       
        
    }
}

public class ProductAnalytics()
{
    public static void TopProducts(List<Sale> sales, List<Product> products, int numberOfProducts)
    {
        var i = 0;
        var topProducts = sales.Join(products,
            s => s.ProductId,
            p => p.ProductId,
            (s, p) => new
            {
                ProductName = p.Name,
                ProductCategory = p.Category,
                ProductPrice = s.Price,
                ProductQuantity = s.Quantity
            })
            .GroupBy(p => new { p.ProductName, p.ProductCategory })
            .Select(g => new
            {
                ProductName = g.Key.ProductName,
                ProductCategory = g.Key.ProductCategory,
                ProductRevenue = g.Sum(x => x.ProductQuantity * x.ProductPrice),
                ProductQuantity = g.Sum(x => x.ProductQuantity)

            }).OrderByDescending(g => g.ProductRevenue)
            .Take(numberOfProducts);

        
        Console.WriteLine($"Top {numberOfProducts} Products by Revenue:");
        foreach (var item in topProducts)
        {
            i = i+1;
            Console.WriteLine($"{i}. {item.ProductName}: {item.ProductRevenue:C} ({item.ProductQuantity}) - {item.ProductCategory}");

        }
    }

    public static void ProductsByCategory(List<Sale> sales, List<Product> products)
    {
        var productsByCategory = sales.Join(products,
            s => s.ProductId,
            p => p.ProductId,
            (s, p) => new
            {
                Category = p.Category,
                Revenue = s.Total,
                ProductID= p.ProductId

            }).GroupBy(x=>x.Category)
            .Select(g => new
            {
                Category = g.Key,
                TotalRevenue = g.Sum(x => x.Revenue),
                ProductCount = g.Select(x => x.ProductID).Distinct().Count()
            }).OrderByDescending(g => g.TotalRevenue);

        Console.WriteLine("Products by Category:");

        foreach (var item in productsByCategory) {
            Console.WriteLine($"{item.Category}: {item.ProductCount} products, {item.TotalRevenue:C} total revenue");
        }

    }
    
    public static void UnderPerforming(List<Sale> sales, List<Product> products)
    {
        var distinctProductsFromSales = sales.Select(s => s.ProductId).Distinct();
        var underPerforming = products.Where(p => !distinctProductsFromSales.Contains(p.ProductId));

        Console.WriteLine($"Underperforming (no sales): {underPerforming.Count()} products");

        foreach (var item in underPerforming)
        {
            Console.WriteLine($"{item.Name} : {item.Category}");
        }
    }


}

public class RegionalReports
{
    public static void SalesByCity(List<Sale> sales, List<Customer> customers)
    {
        var salesTotal = sales.Sum(x => x.Total);
        var salesByCity = sales.Join(customers,
            s => s.CustomerId,
            c => c.CustomerId,
            (s, c) => new
            {
                c.City,
                s.Total,
                s.Quantity
            }).
            GroupBy(c => c.City)
            .Select(g => new
            {
                City = g.Key,
                Total = g.Sum(x => x.Total),
                TransactionCount = g.Count()
            }).OrderByDescending(g => g.Total);

        foreach (var item in salesByCity)
        {
            Console.WriteLine($"{item.City}: {item.Total:C} ({item.Total / salesTotal*100}%) - {item.TransactionCount} transactions");
        }
        
    }
    public static void TopSellerByCity(List<Sale> sales, List<Customer> customers)
    {
        var customerSpending = sales.Join(customers,
            s => s.CustomerId,
            c => c.CustomerId,
            (s, c) => new
            {
                c.City,
                s.Total,
                c.Name
            }).GroupBy(x => new { x.City ,x.Name})
            .Select(x => new
            {
                Name = x.Key.Name,
                x.Key.City,
                TotalSpent = x.Sum(g => g.Total)
            }).ToList();


        var topSellers = customerSpending.GroupBy(x => x.City).
            Select(g => new
            {
                City = g.Key,
                TopCustomer = g.OrderByDescending(c => c.TotalSpent).First()
            }).OrderBy(x => x.City);

        foreach (var item in topSellers)
        {
            Console.WriteLine($"{item.City}: {item.TopCustomer.Name} ({item.TopCustomer.TotalSpent:C})");
        }
    }
}

public class TrendAnalysis()
{

    public static void MonthlySales(List<Sale> sales)
    {

        var monthlySales = sales.GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
            .Select(s => new
            {
                Year = s.Key.Year,
                MonthNumber = s.Key.Month,
                MonthName = new DateTime(s.Key.Year, s.Key.Month, 1).ToString("MMMM"),
                TotalSales = s.Sum(x => x.Total),
                Transactions = s.Sum(x => x.Quantity)
            }).OrderBy(s => s.Year).ThenBy(s => s.MonthNumber).ToList();


        var bestMonth = monthlySales
            .OrderByDescending(s => s.TotalSales).FirstOrDefault();

        var worstMonth = monthlySales
            .OrderBy(s => s.TotalSales).FirstOrDefault();

        var averageMonth = monthlySales
            .Average(s => s.TotalSales);

        Console.WriteLine("Monthly Sales:");

        foreach (var item in monthlySales)
        {
            Console.WriteLine($"{item.MonthName} {item.Year}: {item.TotalSales:C} ({item.Transactions} transactions)");
        }
        Console.WriteLine();

        Console.WriteLine($"Best Month: {bestMonth.MonthName} {bestMonth.Year} {bestMonth.TotalSales:C} ");
        Console.WriteLine();
        Console.WriteLine($"Worst Month: {worstMonth.MonthName} {worstMonth.Year} {worstMonth.TotalSales:C} ");
        Console.WriteLine();
        Console.WriteLine($"Avg month: {averageMonth:C}");
        Console.WriteLine();
        if (monthlySales.Count >= 2)
        {
            var lastMonth = monthlySales[^1];
            var previousMonth = monthlySales[^2];

            var growth = (lastMonth.TotalSales - previousMonth.TotalSales) / previousMonth.TotalSales * 100;


            Console.WriteLine($"Growth {lastMonth.MonthName} vs {previousMonth.MonthName}: {Math.Round(growth,2)} %");
        }
    }
}
public class SearchFilter()
{


    public static void FindCustomer(List<Sale> sales, List<Customer> customers, List<Product> products)
    {
        Console.Write("Enter customer name (partial): ");
        string searchTerm = Console.ReadLine();

        var findCustomers = customers
            .Where(c => c.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
            .Select(c => new
            {
                Customer = c,
                TotalSpent = sales.Where(s => s.CustomerId == c.CustomerId).Sum(s => s.Total),
                Cnt = sales.Count(s => s.CustomerId == c.CustomerId)
            }
                );

        
        foreach (var customer in findCustomers)
        {
            Console.WriteLine($"{customer.Customer.Name} ({customer.Customer.City}): {customer.TotalSpent:C} total, {customer.Cnt} purchases");
        }

        Console.WriteLine("Enter Category name: ");
        string category = Console.ReadLine();
        var findProductsCategory = products
            .Where(p => p.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase))
            .Select(s => new
            {
                Products = s,
                TotalSales = sales.Where(x => x.ProductId == s.ProductId).Sum(s => s.Total),
                Cnt = sales.Count(s => s.ProductId == s.ProductId)
            });

        foreach (var product in findProductsCategory)
        {
            Console.WriteLine($"{product.Products.Name}: {product.TotalSales:C}, {product.Cnt} sold");
        }

        Console.WriteLine("Enter min price: ");
        int minPrice = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter max price: ");
        int maxPrice = int.Parse(Console.ReadLine());

        var priceRange = sales.Join(products,
            s => s.ProductId,
            p => p.ProductId,
            (s, p) => new
            {
                s,
                p
            }).Join(customers,
            s => s.s.CustomerId,
            c => c.CustomerId,
            (ss, cc) => new
            {
                ss,
                cc
            }).Where(x => x.ss.s.Price >= minPrice && x.ss.s.Price <= maxPrice);

        var priceRangeCount = priceRange.Count();

        Console.WriteLine($"Results ({priceRangeCount} sales found)");
        foreach(var item in priceRange)
        {
            Console.WriteLine($"{item.ss.s.SaleId}: {item.cc.Name} bought {item.ss.p.Name} ({item.ss.s.Quantity} x {item.ss.s.Price:C}) = {item.ss.s.Total:C}");
        }

        Console.WriteLine("Enter City: ");
        string city = Console.ReadLine();

        var allByCity = sales.Join(products,
            s => s.ProductId,
            p => p.ProductId,
            (s, p) => new
            {
                s,
                p
            }).Join(customers,
            s => s.s.CustomerId,
            c => c.CustomerId,
            (ss, cc) => new
            {
                ss,
                cc
            }).Where(x=>x.cc.City.Equals(city,StringComparison.CurrentCultureIgnoreCase)).ToList();

        var allByCityCount = allByCity.Count();

        Console.WriteLine($"Results ({allByCityCount} sales found)");
        foreach (var item in allByCity)
        {
            Console.WriteLine($"{item.cc.Name} bought {item.ss.p.Name}: {item.ss.s.Total}");
        }
        Console.WriteLine($"Total from {allByCity.Sum(x=>x.ss.s.Total):C}");
    }
}