using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utils;

namespace SmartMeter.Core
{
    public class Controller
    {
        private IDictionary<DateTime, string> _holidays;
        private List<Day> _measurements;
        private readonly string _inputFilePath;


        public Controller(string[] inputFileNames, string holidayFileName)
        {
            _inputFilePath = MyFile.GetFullFolderNameInApplicationTree("input");

            InitHolidays(holidayFileName);
            InitMeasurements(inputFileNames);
        }

        public int CountOfMeasurements => _measurements.Count;

        public string CreateMarkdownDump()
        {

            // SAMPLE:
            //
            //| Tables        | Are             | Cool  |
            //| ------------- |:---------------:| -----:|
            //| col 3 is      | right - aligned | $1600 |
            //| col 2 is      | centered        |   $12 |
            //| zebra stripes | are neat        |    $1 |

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("| Tables        | Are             | Cool  |");
            sb.AppendLine("| :-------------: |:---------------:| ----- |");

            foreach(Day measurementOfDay in _measurements)
            {
                sb.AppendLine(measurementOfDay.GetMarkdownString());
            }

            return sb.ToString();
        }

        private void InitHolidays(string holidayFileName)
        {
            _holidays = new Dictionary<DateTime, string>();

            string[] lines = File.ReadAllLines(Path.Combine(_inputFilePath, holidayFileName), Encoding.UTF8);

            foreach(string s in lines)
            {
                string[] parts = s.Split(";");
                string description = parts[0];
                DateTime date = DateTime.Parse(parts[1]);

                _holidays.Add(date, description);
            }
        }


        private void InitMeasurements(string[] inputFileNames)
        {
            _measurements = new List<Day>();
            Dictionary<DateTime, double> dailyMeasurements = new Dictionary<DateTime, double>();


            foreach(string inputFileName in inputFileNames)
            {
                string[] lines = File.ReadAllLines(Path.Combine(_inputFilePath, inputFileName), Encoding.UTF8);

                foreach(string line in lines)
                {
                    string[] parts = line.Split(";");
                    DateTime timestamp = DateTime.Parse(parts[0]);
                    double measurement = Double.Parse(parts[1]);

                    if(dailyMeasurements.ContainsKey(timestamp.Date))
                    {
                        dailyMeasurements[timestamp.Date] += measurement;
                    }
                    else
                    {
                        dailyMeasurements[timestamp.Date] = measurement;
                    }
                }
            }

            foreach(KeyValuePair<DateTime, double> entries in dailyMeasurements)
            {
                if(_holidays.ContainsKey(entries.Key))
                {
                    _measurements.Add(new Holiday(entries.Key, entries.Value, _holidays[entries.Key]));
                }
                else
                {
                    _measurements.Add(new Day(entries.Key, entries.Value));
                }
            }
        }

    }
}
