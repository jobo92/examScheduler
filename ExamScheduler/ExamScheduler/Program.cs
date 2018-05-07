using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExamScheduler
{
    class Program
    {

        static void Main(string[] args)
        {
            List<String> students = new List<string>();

            /*
             * To make the csv file copy data from Users page then run these find-and-replace
             * Select .+ -> ""
             * \t+ -> ","
             * ^, -> ""
             * [\r\n]+ -> \r\n
             * " Options Menu: Username " -> ""
             */

            int month;
            do
            {
                Console.Write("Insert start month of exams: ");
                month = int.Parse(Console.ReadLine());
            } while (month < 1 || month > 12);

            int date;
            do
            {
                Console.Write("Insert start date of exams: ");
                date = int.Parse(Console.ReadLine());
            } while (date < 1 || date > 31);

            int days = 0;
            do
            {
                Console.Write("Insert number of days: ");
                days = int.Parse(Console.ReadLine());
            } while (days < 0);

            int startingHour = 0;
            do
            {
                Console.Write("Insert what time you want to start the exams (hour): ");
                startingHour = int.Parse(Console.ReadLine());
            } while (startingHour < 0 || startingHour > 12);

            DateTime currentDate = DateTime.Now;
            currentDate = new DateTime(currentDate.Year, month, date, startingHour, 0, 0);

            using (var reader = new StreamReader(@"students.txt"))
            {
                // We countinue reading one line at a time until we get to the end of the file
                while (!reader.EndOfStream)
                {
                    // Each line is stored in the 'line' variable, and then split using the comma (CSV = Comma Separated Values)
                    String line = reader.ReadLine();
                    String[] values = line.Split(',');

                    students.Add(values[0] + "\t" + values[1] + " " + values[2]);
                }
            }

            // shuffle list
            var rnd = new Random();
            students = students.OrderBy(item => rnd.Next()).ToList<string>();

            int timeSlot;
            do
            {
                Console.Write("How long should the exam be (minutes)? ");
                timeSlot = int.Parse(Console.ReadLine());
            } while (timeSlot < 5);

            int studentsPerDay = (int) Math.Ceiling((double) students.Count / (double) days);

            int currentDay = 0;
            Console.WriteLine(String.Format("{0:d/M/yyyy}", currentDate));
            for (int i = 0; i < students.Count; i++)
            {
                if (i != 0 && currentDay < days-1 && i % studentsPerDay == 0)
                {
                    currentDate = currentDate.AddDays(1);
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startingHour, 0, 0);
                    currentDay++;
                    Console.WriteLine(String.Format("{0:d/M/yyyy}", currentDate));
                }
                if (currentDate.AddMinutes(timeSlot).Hour == 12 && currentDate.AddMinutes(timeSlot).Minute > 0)
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 0, 0);
                Console.WriteLine(String.Format("{0:HH:mm}", currentDate) + "\t" + students[i]);
                currentDate = currentDate.AddMinutes(timeSlot);
            }

            Console.WriteLine("\n----------\nPress any key to close the program");
            Console.ReadLine();
        }
    }
}
