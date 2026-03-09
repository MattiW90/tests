
using System.Diagnostics.Tracing;
using System.Text;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        var result = FizzBuzz();
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();
        Console.WriteLine();
        
        Console.WriteLine(IsPalindrome("kajak2"));
        Console.WriteLine(IsPalindrome("Kajak"));
        Console.WriteLine(IsPalindrome("kajak"));
        Console.WriteLine(IsPalindrome("hello"));
        Console.WriteLine(IsPalindrome("A"));

        Console.WriteLine();
        Console.WriteLine();

        var resultOfSum = TwoSum([2, 8, 7, 15], 9);
        Console.WriteLine($"Result: {resultOfSum[0]}, {resultOfSum[1]}");
        Console.WriteLine();
        var resultOfSumOptional = TwoSumOptional([2, 8, 7, 15], 9);
        Console.WriteLine($"Result: {resultOfSumOptional[0]}, {resultOfSumOptional[1]}");

        Console.WriteLine();
        Console.WriteLine();
        ReverseSentence("ja   tak Sam");
        ReverseSentence("aj   kat maS");

        Console.WriteLine(ReverseWords("ja   tak Sam"));
        Console.WriteLine(ReverseWords("aj   kat maS"));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(IsValid("{[(}]}"));
        Console.WriteLine(IsValid("()"));
        Console.WriteLine(IsValid("()[]{}"));
        Console.WriteLine(IsValid("([]{})"));
        Console.WriteLine(IsValid("(]"));
        Console.WriteLine(IsValid("([)]"));
        Console.WriteLine(IsValid("((("));
        Console.WriteLine(IsValid("]"));
        Console.WriteLine(IsValid(""));
        Console.WriteLine(IsValid("{[]}"));

        Console.WriteLine();
        Console.WriteLine();

        var duplicatedValues = FindDuplicates([1, 2, 3, 2, 4, 3, 5]);
        foreach (var item in duplicatedValues)
        {
            Console.WriteLine(item);
        }
        var duplicatedValues2 = FindDuplicates([1, 1, 1, 1]);
        foreach (var item in duplicatedValues2)
        {
            Console.WriteLine(item);
        }

        var duplicatedValues3 = FindDuplicates2([1, 2, 3, 2, 4, 3, 5]);
        foreach (var item in duplicatedValues3)
        {
            Console.WriteLine(item);
        }
        var duplicatedValues4 = FindDuplicates2([1, 1, 1, 1]);
        foreach (var item in duplicatedValues4)
        {
            Console.WriteLine(item);
        }

    }

    public static List<string> FizzBuzz()
    {
        var list = new List<string>();

        for(int i = 1;i <= 100;i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                list.Add("FizzBuzz");
            }
            else if (i % 3 == 0)
            {
                list.Add("Fizz");

            }
            else if (i % 5 == 0)
            {
                list.Add("Buzz");
            }
            else
            {
                list.Add(i.ToString());
            }
        }


        return list;
    }

    public static bool IsPalindrome(string text)
    {
        var reversed = new StringBuilder();

        for (int i=text.Length-1; i>=0; i--)
        {
            reversed.Append(text[i]);
        }


        return reversed.ToString().ToLower() == text.ToLower();

    }


    public static int[] TwoSum(int[] nums, int target)
    {
        int[] num = new int[2];
        
        for (int i = 0; i < nums.Length; i++)
        {
            for (int k = i+1; k < nums.Length; k++)
            {
                if (nums[i] + nums[k] == target)
                { 
                    return new int[] {i, k};
                }

            }
            
        }
        return new int[] { -1, -1 };
    }

    public static int[] TwoSumOptional(int[] nums, int target)
    {
        var numbers = new Dictionary<int,int>();

        for (int i = 0; i < nums.Length; i++)
        {
            var expectedValue = target - nums[i];

            if (numbers.ContainsKey(expectedValue))
                return new int[] { numbers[expectedValue], i };
            numbers[nums[i]] = i;
        }

        return new int[] { -1, -1 };
    }

    public static void ReverseSentence(string sentence)
    {
        //mate   Usna das = etam   ansU sad
        var list = new List<string>();
        string reverse="";
        for (int i = 0; i < sentence.Length; i++)
        {
            if (sentence[i].ToString() == " ")
            {
                if (i != 0  && reverse.ToString()!="")
                {
                    list.Add(reverse.ToString());
                }
                list.Add(sentence[i].ToString());
                reverse = "";
            }
            else
            {

                reverse += sentence[i].ToString();
                if (i == sentence.Length-1)
                {
                    list.Add(reverse.ToString());
                }
            }
            //"ja tak Sam"
        }

        Console.WriteLine();

        string reversedSentence = "";
        foreach(var item in list)
        {
            for(int k = item.Length-1;k>=0; k--)
            {
                reversedSentence += item[k];
            }
        }
        Console.WriteLine(reversedSentence);
    }

    public static string ReverseWords(string sentence)
    {
        var words = sentence.Split(' '); // obsługuje wiele spacji!

        var reversedWords = new List<string>();

        foreach (var word in words)
        {
            char[] chars = word.ToCharArray();
            Array.Reverse(chars);
            reversedWords.Add(new string(chars));
        }

        return string.Join(" ", reversedWords);
    }

    public static string ReverseSentence2(string sentence)
    {
        var words = sentence.Split(' ');
        var reversedWords = new List<string>();

        foreach(var word in words)
        {
            var charr = word.ToCharArray();
            Array.Reverse(charr);

            reversedWords.Add(new string(charr));
        }
        return string.Join(" ",reversedWords);
    }


    //public static bool IsValid(string s)
    //{
    //    var stack = new Stack<char>();
    //    var i = 0;

    //    foreach (var item in s)
    //    {

    //        if(item.ToString() == "{" || item.ToString() == "[" || item.ToString() == "(")
    //        {
    //            stack.Push(item);
    //            i++;

    //        }
    //        if (stack.Count() > 0 && ((item.ToString() == "}" && stack.Peek().ToString() == "{")
    //                || (item.ToString() == ")" && stack.Peek().ToString() == "(")
    //                || (item.ToString() == "]" && stack.Peek().ToString() == "[")))
    //        { 
    //            stack.Pop(); 
    //        }

    //    }

    //    return (stack.Count == 0 && i >0) || s=="" ? true : false;


    //}

    public static bool IsValid(string s)
    {
        var stack = new Stack<char>();

        foreach (var item in s)
        {
            if (item == '{' || item == '[' || item == '(')
            {
                stack.Push(item);
            }
            else
            {
                if (stack.Count == 0)
                    return false;

                if ((item == '}' && stack.Peek() != '{') ||
                    (item == ')' && stack.Peek() != '(') ||
                    (item == ']' && stack.Peek() != '['))
                    return false;

                stack.Pop();
            }
        }

        return stack.Count == 0;
    }


    public static List<int> FindDuplicates(int[] nums)
    {
        var list = new List<int>();
        var listOfDuplicates = new List<int>();
        foreach (var item in nums)
        {
            if(list.Contains(item) && !listOfDuplicates.Contains(item))
            {
                listOfDuplicates.Add(item);
            }
            else
            {
                list.Add(item);
            }
        }

        return listOfDuplicates;
    }

    public static List<int> FindDuplicates2(int[] nums)
    {
        var seen = new HashSet<int>();
        var duplicates = new HashSet<int>();

        foreach (var item in nums)
        {
            if(!seen.Add(item))
            {
                duplicates.Add(item);
            }
        }
        return duplicates.ToList();
    }
}


