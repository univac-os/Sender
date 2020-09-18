using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace Sender
{
    public class Sender
    {

        static void Main(string[] args)
        {
            string path = "C:/Users/Udayh/Desktop/Sample.csv";
            while (!PathChecker.DoesPathExists(path))
            {
                path = Console.ReadLine();                                 // Modify to reduce Complexity
            }
            //handle exception
            path = CleanCopy.Create(path);                                 // What if data has comma in type text-take file and make new file

            string[] columns = CsvInfo.GetColumns(path);                   // get the columns array

            var reviewColumnIndex = CsvInfo.GetColumnIndex(columns,"Comments");

            Action<string, string> sendWordsWithTimestamp = (string timestamp, string line) =>
            {
                string[] words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    Console.WriteLine(timestamp +" "+ words[i]);
                }
            };

            if (Timestamp.DoesTimestampExists(columns))
            {
                var timestampColumnIndex = CsvInfo.GetColumnIndex(columns, "ReviewDate");

                
                FilterAndSend(sendWordsWithTimestamp,timestampColumnIndex,reviewColumnIndex,path);
            }
            else
            {
                FilterAndSend(sendWordsWithTimestamp,reviewColumnIndex, path);
            }

        }

        public static void FilterAndSend(Action<string, string> sendWordsWithTimestamp, int timestampColumn, int reviewColumn, string filePath)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    sendWordsWithTimestamp.Invoke(row[timestampColumn], row[reviewColumn]);
                }
            }
        }

        public static void FilterAndSend(Action<string, string> sendWordsWithoutTimestamp, int reviewColumn, string filePath)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    sendWordsWithoutTimestamp.Invoke("NoDateTime", row[reviewColumn]);
                }
            }
        }

    }
}
