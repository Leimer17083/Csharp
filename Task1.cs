using System;

class Program
{
    static void Main()
    {
        Random rand = new Random();
        int groupsCount = rand.Next(3, 6);
        int[][] streamGrades = new int[groupsCount][];
        int streamMin = int.MaxValue;
        int streamMax = int.MinValue;
        double streamSum = 0;
        int totalStudents = 0;

        Console.WriteLine($"Кількість груп на потоці: {groupsCount}\n");

        for (int i = 0; i < streamGrades.Length; i++)
        {
            int studentsCount = rand.Next(10, 31);
            streamGrades[i] = new int[studentsCount];
            totalStudents += studentsCount;

            int groupMin = int.MaxValue;
            int groupMax = int.MinValue;
            double groupSum = 0;

            for (int j = 0; j < streamGrades[i].Length; j++)
            {
                int grade = rand.Next(1, 101);
                streamGrades[i][j] = grade;

                if (grade < groupMin) groupMin = grade;
                if (grade > groupMax) groupMax = grade;
                groupSum += grade;

                if (grade < streamMin) streamMin = grade;
                if (grade > streamMax) streamMax = grade;
                streamSum += grade;
            }

            double groupAvg = groupSum / studentsCount;

            Console.WriteLine($"Група {i + 1} (Студентів: {studentsCount}):");
            Console.WriteLine($"- Мінімальна оцінка: {groupMin}");
            Console.WriteLine($"- Максимальна оцінка: {groupMax}");
            Console.WriteLine($"- Середній бал:      {groupAvg:F2}\n");
        }

        double streamAvg = streamSum / totalStudents;


        Console.WriteLine("======================================");
        Console.WriteLine($"ЗАГАЛЬНА СТАТИСТИКА ПОТОКУ (Студентів: {totalStudents}):");
        Console.WriteLine($"- Мінімальна оцінка: {streamMin}");
        Console.WriteLine($"- Максимальна оцінка: {streamMax}");
        Console.WriteLine($"- Середній бал:      {streamAvg:F2}");
    }
}