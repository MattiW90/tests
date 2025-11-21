
using System.Data.Common;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("");
        var text = Console.ReadLine();

        var splittedText = text.Split(" ");

        Dictionary<string, int> dict = new Dictionary<string, int>();
        

        foreach(var kv in splittedText)
        {
            if(!string.IsNullOrEmpty(kv))
            {
                if (dict.TryGetValue(kv, out int count))
                {
                    dict[kv] = count + 1;
                }
                else
                {
                    dict[kv] = 1;
                }
            }
            
        }

        foreach (var kv in dict) 
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
        Console.WriteLine("\n\n\nTop 5 Most Frequent:");


        var top5Count = dict.OrderByDescending(x => x.Value).Take(5);
        foreach (var kv in top5Count)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }

        Console.WriteLine("\n\nPROJEKT 2: Unique Visitors Tracker ");

        var names = new HashSet<string>();
        RecordVisit(names);
        RecordVisit(names);
        RecordVisit(names);
        GetUniqueVisitorsCount(names);
        GetAllVisitors(names);
        HasVisited(names, "a");
        HasVisited(names, "b");

        Console.WriteLine("\n\nPROJEKT 3: Task Queue System ");
        //1.AddTask(taskName) - dodaj task do kolejki
        //2.ProcessNextTask() - pobierz i "wykonaj" następny
        //3.ViewQueue() - pokaż wszystkie taski
        //4.PeekNext() - patrz na następny bez usuwania
        //5.GetQueueSize() - ile tasków w kolejce
        //6.IsEmpty() - czy kolejka pusta
        //7.Menu interaktywne

        var queue = new Queue<string>();
        AddTaskToQueue(queue, "Send email to customer");
        AddTaskToQueue(queue, "Generate monthly report");
        AddTaskToQueue(queue, "Update database");
        ProcessNextTaskFromQueue(queue);
        ViewQueue(queue);



        Console.WriteLine("\n\nPROJEKT 4: Undo Tasks System");

        var stack = new Stack<string>();
        AddStack(stack, "Created file Document.txt");
        AddStack(stack, "Deleted file OldData.csv");
        AddStack(stack, "Modified file Config.json");
        ShowHistory(stack);
        UndoStack(stack);
        ShowHistory(stack);

        Peek(stack);
        ClearHistory(stack);
        ShowHistory(stack);



        Console.WriteLine($"\n\n PROJEKT 5: Cache System!");

        Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>();
        var entry = new CacheEntry();
        entry.Set("John Doe", 11111);
        cache["user123"] = entry;

        var entry2 = new CacheEntry();
        entry2.Set("Laptop", 1);
        cache["product:456"] = entry2;

        GetAllDataFromCache(cache);
        GetCacheEntry(cache, "user123");
        GetCacheEntry(cache, "product:456");

        Stats(cache);
        CleanExpired(cache);
        GetAllDataFromCache(cache);


    }
    public static void Stats(Dictionary<string, CacheEntry> cache)
    {
        var total = cache.Count;
        var expired = cache.Where(c => c.Value.IsExpired()).Count();
        var active = total - expired;

        Console.WriteLine($"Total entries: {total}");
        Console.WriteLine($"Active: {active}");
        Console.WriteLine($"Expired: {expired}");
    }
    public static void CleanExpired(Dictionary<string, CacheEntry> cache)
    {
        var expiredKeys = cache.Where(c => c.Value.IsExpired())
             .Select(c => c.Key)
             .ToList();
        foreach (var key in expiredKeys)
        {
            cache.Remove(key);
        }

    }
    public static void GetCacheEntry(Dictionary<string, CacheEntry> cache, string key)
    {
        if(!cache.TryGetValue(key, out var entry))
        {
            Console.WriteLine($"{key} not found");
        }
        else
        {
            if(entry.IsExpired())
            {
                
                Console.WriteLine($"Entry {key} not found, expired");
            }
            else
            {
                var timeLeft = (entry.ExpiresAt - DateTime.UtcNow).TotalSeconds;
                Console.WriteLine($"Found, {key}: {entry.Value}, timeLeft: {timeLeft}");
            }
        }
    }
    public static void GetAllDataFromCache(Dictionary<string, CacheEntry> cache)
    {
        if(!cache.Any())
        {
            Console.WriteLine("Cache is empty");
            return;
        }
        Console.WriteLine("=== CACHE CONTENT ===");
        int position = 1;
        foreach (var entry in cache)
        {
            var status = entry.Value.IsExpired() ? "Expired" : "Active";
            var timeLeft = entry.Value.IsExpired() ? 0 : (entry.Value.ExpiresAt - DateTime.UtcNow).TotalSeconds;
            Console.WriteLine($"{position}: {entry.Key}");
            Console.WriteLine($"Value: {entry.Value.Value}");
            Console.WriteLine($"   Status: {status}");
            Console.WriteLine($"   Created: {entry.Value.CreatedAt}");
            Console.WriteLine($"   Expires: {entry.Value.ExpiresAt}, timeleft:{timeLeft}");
            position++;

        }
    }
    public static void ClearHistory(Stack<string> stack)
    {
        stack.Clear();

    }
    public static void Peek(Stack<string> stack)
    {
        if (stack.Count > 0)
        {
            var name = stack.Peek();
            Console.WriteLine(name);
        }
            
    }
    public static void ShowHistory(Stack<string> stack)
    {
        if (stack.Count > 0)
        {
            int position = stack.Count;
            foreach (var item in stack)
            {
                Console.WriteLine($"{position}: {item}");
                position--;
            }

        }
    }
    public static void UndoStack(Stack<string> stack)
    {
        if (stack.Count > 0)
        {
            stack.Pop();
        }

    }
    public static void AddStack(Stack<string> stack, string taskName)
    {
        stack.Push(taskName);
    }
    public static void ViewQueue(Queue<string> queue)
    {
        if (queue.Count > 0)
        {
            int position = 1;
            foreach (var item in queue)
            {
                Console.WriteLine($"Task {position}: {item}");
                position++;
            }
        }
    }
    public static void PeekNextTask(Queue<string> queue)
    {
        if (queue.Count > 0)
        {
            string task = queue.Peek();
            Console.WriteLine($"Next task: {task}");
        }
    }
    public static void ProcessNextTaskFromQueue(Queue<string> queue)
    {
        if (queue.Count > 0)
        {
            string task = queue.Dequeue();
            Console.WriteLine($"Processing task: {task}");
        }
    }
    public static void AddTaskToQueue(Queue<string> queue, string taskName)
    {
        queue.Enqueue(taskName);
        Console.WriteLine($"Task added to queue (Position:{queue.Count})");
    }
    public static void HasVisited(HashSet<string> names, string userId)
    {
        Console.WriteLine(names.Contains(userId) ? "Visited" : "Not visited");
    }
    public static void GetAllVisitors(HashSet<string> names)
    {
        Console.WriteLine("\nAll visitors:");
        foreach (var name in names)
        {
            Console.WriteLine($"{name}");
        }
    }
    public static void RecordVisit(HashSet<string> names)
    {
        Console.WriteLine("Enter User ID:");
        string userId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userId)) return;

        userId= userId.Trim();
        var isNew = names.Add(userId);
        Console.WriteLine(isNew? "Visit recorded - First time visitor!" : "Visit recorded - Returning visitor");
        

    }
    public static void GetUniqueVisitorsCount(HashSet<string> names)
    {
        Console.WriteLine($"Unique: {names.Count}");
    }
}

public class CacheEntry
{
    public string Value { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsExpired() => DateTime.Now > ExpiresAt;

   
    public void Set(string value, int ttl)
    {

        this.Value = value;
        this.CreatedAt = DateTime.Now;
        this.ExpiresAt = DateTime.Now.AddSeconds(ttl);
        

    }
}




