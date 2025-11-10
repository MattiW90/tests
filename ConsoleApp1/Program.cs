namespace Day15_LinqRealWorld;

internal class Program
{
    static void Main(string[] args)
    {
        var customers = GetCustomers();
        var products = GetProducts();
        var orders = GetOrders();
        var orderItems = GetOrderItems();

        // TU BĘDĄ TWOJE QUERIES

        Console.WriteLine("\n=== QUERY 1: TOP 5 CUSTOMERS BY TOTAL SPENT ===");
        var top5Customers = customers.GroupJoin(
            orders,
            c => c.Id,
            o => o.CustomerId,
            (customer, order) => new
            {
                customer.Name,
                OrderTotal = order.Sum(o => o.TotalAmount)
            }).OrderByDescending(x => x.OrderTotal)
            .Take(5);
        foreach(var customer in top5Customers)
        {
            Console.WriteLine($"Customer Name {customer.Name}, total spent: { customer.OrderTotal:C}");
        }

        Console.WriteLine("\n=== QUERY 2: Best Selling Products ===");
        var top5Products = products.GroupJoin(
            orderItems,
            p => p.Id,
            o => o.ProductId,
            (p, o) => new
            {
                p.Name,
                orderCount = o.Sum(x=>x.Quantity)
            }).OrderByDescending(x=>x.orderCount)
            .Take(5);

        foreach (var product in top5Products)
        {
            Console.WriteLine($"ProductName: {product.Name}, count: {product.orderCount}");
        }

        Console.WriteLine("\n=== QUERY 3: Monthly Revenue(2024) ===");
        var monthlyRevenue = orders.Where(x => x.OrderDate.Year == 2024)
            .GroupBy(x => x.OrderDate.Month)
            .Select(
            g => new
            {
                g.Key,
                Total = g.Sum(e => e.TotalAmount)
            });
        foreach(var monthly in monthlyRevenue)
        {
            Console.WriteLine($"{monthly.Key}, Total: {monthly.Total:C}");
        }

        Console.WriteLine("\n=== QUERY 4: Category Performance ===");
        //Boss: "Która kategoria produktów ma najwyższą średnią wartość zamówienia?"

        var category = products.Join(orderItems, p => p.Id, o => o.Id, (p, o) => new { p.Category, o.OrderId })
            .Join(orders, oi => oi.OrderId, o => o.Id, (oi, o) => new { oi.Category, o.TotalAmount })
            .GroupBy(x => x.Category)
            .Select(x => new
            {
                x.Key,
                AverageTotal = x.Average(x => x.TotalAmount)
            }).OrderByDescending(x=>x.AverageTotal);

        foreach (var product in category)
        {
            Console.WriteLine($"{product.Key}, Average: {product.AverageTotal:C}");
        }

        Console.WriteLine("\n=== QUERY 5: Customer Retention (repeat customers) ===");
        //Boss: "Ile klientów złożyło więcej niż 1 zamówienie? (repeat customers)"

        var customersOrdersGreaterThan1 = customers.GroupJoin(orders,
            c => c.Id,
            o => o.CustomerId,
            (c, o) => new
            {
                c.Name,
                orderTotal = o.Sum(x => x.TotalAmount)
            }).Where(x => x.orderTotal > 1);
        foreach(var customer in customersOrdersGreaterThan1)
        {
            Console.WriteLine($"Customer: {customer.Name}, Total: {customer.orderTotal:C}");
        }

        Console.WriteLine("\n=== QUERY 6: Product Revenue Contribution ===");

        //Boss: "Pokaż każdy produkt z jego total revenue (Quantity × Price summed)."
    }

    static List<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer(1, "Jan Kowalski", "Warsaw", new DateTime(2023, 1, 15)),
            new Customer(2, "Anna Nowak", "Krakow", new DateTime(2023, 3, 20)),
            new Customer(3, "Piotr Wiśniewski", "Warsaw", new DateTime(2023, 5, 10)),
            new Customer(4, "Maria Lewandowska", "Gdansk", new DateTime(2023, 7, 5)),
            new Customer(5, "Krzysztof Dąbrowski", "Poznan", new DateTime(2024, 1, 12)),
            new Customer(6, "Ewa Kamińska", "Warsaw", new DateTime(2024, 2, 18)),
            new Customer(7, "Tomasz Kowalczyk", "Wroclaw", new DateTime(2024, 3, 25))
        };
    }

    static List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product(1, "Laptop", "Electronics", 3500m),
            new Product(2, "Mouse", "Electronics", 150m),
            new Product(3, "Keyboard", "Electronics", 350m),
            new Product(4, "Monitor", "Electronics", 1200m),
            new Product(5, "Desk", "Furniture", 800m),
            new Product(6, "Chair", "Furniture", 600m),
            new Product(7, "Lamp", "Furniture", 200m),
            new Product(8, "Notebook", "Stationery", 15m),
            new Product(9, "Pen", "Stationery", 5m),
            new Product(10, "Backpack", "Accessories", 250m)
        };
    }

    static List<Order> GetOrders()
    {
        return new List<Order>
        {
            // 2023 orders
            new Order(1, 1, new DateTime(2023, 6, 15), 4000m),
            new Order(2, 2, new DateTime(2023, 7, 20), 1550m),
            new Order(3, 3, new DateTime(2023, 8, 10), 950m),
            new Order(4, 1, new DateTime(2023, 9, 5), 200m),
            new Order(5, 4, new DateTime(2023, 10, 12), 1400m),
            
            // 2024 orders
            new Order(6, 1, new DateTime(2024, 1, 15), 650m),
            new Order(7, 2, new DateTime(2024, 2, 20), 3850m),
            new Order(8, 5, new DateTime(2024, 3, 10), 1550m),
            new Order(9, 6, new DateTime(2024, 4, 5), 2000m),
            new Order(10, 3, new DateTime(2024, 5, 12), 850m),
            new Order(11, 7, new DateTime(2024, 6, 18), 1200m),
            new Order(12, 4, new DateTime(2024, 7, 25), 600m),
            new Order(13, 1, new DateTime(2024, 8, 30), 3500m),
            new Order(14, 2, new DateTime(2024, 9, 15), 250m),
            new Order(15, 5, new DateTime(2024, 10, 20), 450m)
        };
    }

    static List<OrderItem> GetOrderItems()
    {
        return new List<OrderItem>
        {
            // Order 1 (Jan - 4000)
            new OrderItem(1, 1, 1, 1, 3500m),  // Laptop
            new OrderItem(2, 1, 2, 2, 150m),   // Mouse x2
            new OrderItem(3, 1, 8, 5, 15m),    // Notebook x5
            
            // Order 2 (Anna - 1550)
            new OrderItem(4, 2, 3, 1, 350m),   // Keyboard
            new OrderItem(5, 2, 4, 1, 1200m),  // Monitor
            
            // Order 3 (Piotr - 950)
            new OrderItem(6, 3, 5, 1, 800m),   // Desk
            new OrderItem(7, 3, 7, 1, 200m),   // Lamp
            
            // Order 4 (Jan - 200)
            new OrderItem(8, 4, 9, 40, 5m),    // Pen x40
            
            // Order 5 (Maria - 1400)
            new OrderItem(9, 5, 6, 2, 600m),   // Chair x2
            new OrderItem(10, 5, 7, 1, 200m),  // Lamp
            
            // Order 6 (Jan - 650)
            new OrderItem(11, 6, 3, 1, 350m),  // Keyboard
            new OrderItem(12, 6, 2, 2, 150m),  // Mouse x2
            
            // Order 7 (Anna - 3850)
            new OrderItem(13, 7, 1, 1, 3500m), // Laptop
            new OrderItem(14, 7, 3, 1, 350m),  // Keyboard
            
            // Order 8 (Krzysztof - 1550)
            new OrderItem(15, 8, 4, 1, 1200m), // Monitor
            new OrderItem(16, 8, 3, 1, 350m),  // Keyboard
            
            // Order 9 (Ewa - 2000)
            new OrderItem(17, 9, 5, 1, 800m),  // Desk
            new OrderItem(18, 9, 6, 2, 600m),  // Chair x2
            
            // Order 10 (Piotr - 850)
            new OrderItem(19, 10, 3, 1, 350m), // Keyboard
            new OrderItem(20, 10, 2, 2, 150m), // Mouse x2
            new OrderItem(21, 10, 8, 10, 15m), // Notebook x10
            
            // Order 11 (Tomasz - 1200)
            new OrderItem(22, 11, 4, 1, 1200m), // Monitor
            
            // Order 12 (Maria - 600)
            new OrderItem(23, 12, 6, 1, 600m),  // Chair
            
            // Order 13 (Jan - 3500)
            new OrderItem(24, 13, 1, 1, 3500m), // Laptop
            
            // Order 14 (Anna - 250)
            new OrderItem(25, 14, 10, 1, 250m), // Backpack
            
            // Order 15 (Krzysztof - 450)
            new OrderItem(26, 15, 2, 3, 150m)   // Mouse x3
        };
    }
}

public class Customer
{
    public int Id { get; }
    public string Name { get; }
    public string City { get; }
    public DateTime RegisterDate { get; }

    public Customer(int id, string name, string city, DateTime registerDate)
    {
        Id = id;
        Name = name;
        City = city;
        RegisterDate = registerDate;
    }
}

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Category { get; }
    public decimal Price { get; }

    public Product(int id, string name, string category, decimal price)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
    }
}

public class Order
{
    public int Id { get; }
    public int CustomerId { get; }
    public DateTime OrderDate { get; }
    public decimal TotalAmount { get; }

    public Order(int id, int customerId, DateTime orderDate, decimal totalAmount)
    {
        Id = id;
        CustomerId = customerId;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
    }
}

public class OrderItem
{
    public int Id { get; }
    public int OrderId { get; }
    public int ProductId { get; }
    public int Quantity { get; }
    public decimal Price { get; }

    public OrderItem(int id, int orderId, int productId, int quantity, decimal price)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}