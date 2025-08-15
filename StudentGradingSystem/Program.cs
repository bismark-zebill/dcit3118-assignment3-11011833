using System;
using System.Collections.Generic;
using System.IO;

namespace StudentGradingSystem
{
   
    // a. Student class
   
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70 && Score <= 79) return "B";
            if (Score >= 60 && Score <= 69) return "C";
            if (Score >= 50 && Score <= 59) return "D";
            return "F";
        }
    }

    
    // b. Custom exceptions
    
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

   
    // d. StudentResultProcessor
    
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    string[] parts = line.Split(',');

                    // Validate fields
                    if (parts.Length != 3)
                        throw new MissingFieldException($"Line {lineNumber}: Missing data fields.");

                    try
                    {
                        int id = int.Parse(parts[0].Trim());
                        string fullName = parts[1].Trim();
                        int score;

                        if (!int.TryParse(parts[2].Trim(), out score))
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format for '{parts[2]}'.");

                        students.Add(new Student(id, fullName, score));
                    }
                    catch (FormatException ex)
                    {
                        throw new InvalidScoreFormatException($"Line {lineNumber}: Score format error. {ex.Message}");
                    }
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }

   
    // e. Main Program
   
    public class Program
    {
        public static void Main(string[] args)
        {
            string inputFilePath = "students.txt";     
            string outputFilePath = "report.txt";       

            StudentResultProcessor processor = new StudentResultProcessor();

            try
            {
                List<Student> students = processor.ReadStudentsFromFile(inputFilePath);
                processor.WriteReportToFile(students, outputFilePath);
                Console.WriteLine("Report generated successfully!");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: Input file not found. {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
