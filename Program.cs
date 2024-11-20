using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Choose a task to run (1, 2, or 3):");
        Console.WriteLine("1 - String Manipulation");
        Console.WriteLine("2 - If-Else Conversion");
        Console.WriteLine("3 - Uzbek to English Weekday Translation");
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                StringManipulation();
                break;
            case "2":
                IfElseConversion();
                break;
            case "3":
                WeekdayTranslation();
                break;
            default:
                Console.WriteLine("Invalid choice. Please run the program again.");
                break;
        }
    }

    static void StringManipulation()
    {
        Console.Write("Enter a number (x): ");
        int x = int.Parse(Console.ReadLine());
        Console.Write("Enter a string: ");
        string str = Console.ReadLine();

        if (str.Length > x)
        {
            str = str.ToUpper();
        }
        else
        {
            str = str.ToLower();
        }

        Console.WriteLine("Output: " + str);
    }

    static void IfElseConversion()
    {
        Console.Write("Enter first number (x): ");
        int x = int.Parse(Console.ReadLine());
        Console.Write("Enter second number (y): ");
        int y = int.Parse(Console.ReadLine());

        string result;

        if (x > y)
        {
            result = "x is greater than y";
        }
        else if (x < y)
        {
            result = "x is less than y";
        }
        else
        {
            result = "x is equal to y";
        }

        Console.WriteLine(result);
    }

    static void WeekdayTranslation()
    {
        Console.Write("Enter a weekday in Uzbek: ");
        string uzbekDay = Console.ReadLine()?.ToLower();

        string englishDay = uzbekDay switch
        {
            "dushanba" => "Monday",
            "seshanba" => "Tuesday",
            "chorshanba" => "Wednesday",
            "payshanba" => "Thursday",
            "juma" => "Friday",
            "shanba" => "Saturday",
            "yakshanba" => "Sunday",
            _ => "Invalid day"
        };

        Console.WriteLine("English: " + englishDay);
    }
}
