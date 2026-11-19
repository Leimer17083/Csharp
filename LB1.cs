using System;
class Program
{
    static void Main()
    {
        double a = ReadSide("a");
        double b = ReadSide("b");
        double c = ReadSide("c");

        if (a + b <= c || a + c <= b || b + c <= a)
        {
            Console.WriteLine("Ці сторони не можуть утворити трикутник.");
            return;
        }

        double perimeter = a + b + c;
        double p = perimeter / 2;
        double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

        Console.WriteLine($"Периметр: {perimeter}");
        Console.WriteLine($"Площа: {area}");

        if (a == b && b == c)
            Console.WriteLine("За сторонами: рівносторонній");
        else if (a == b || a == c || b == c)
            Console.WriteLine("За сторонами: рівнобедрений");
        else
            Console.WriteLine("За сторонами: довільний (різносторонній)");


        double[] sides = { a, b, c };
        Array.Sort(sides);

        bool isRight = Math.Abs(
            sides[0] * sides[0] + sides[1] * sides[1] - sides[2] * sides[2]
        ) < 1e-10 * Math.Max(1, sides[2] * sides[2]);

        Console.WriteLine(isRight
            ? "Трикутник прямокутний."
            : "Трикутник не є прямокутним.");
    }

    static double ReadSide(string name)
    {
        while (true)
        {
            Console.Write($"Введіть сторону {name}: ");

            if (double.TryParse(Console.ReadLine(), out double value) &&
                value > 0 &&
                !double.IsInfinity(value))
            {
                return value;
            }

            Console.WriteLine("Введіть додатне число.");
        }
    }
}