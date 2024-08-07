using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the radius: ");
        if (double.TryParse(Console.ReadLine(), out double radius))
        {
            double area = Math.PI * Math.Pow(radius, 2);
            double circumference = 2 * Math.PI * radius;

            Console.WriteLine($"S={area}, L={circumference}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number for the radius.");
        }
    }
}