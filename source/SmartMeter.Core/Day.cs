using System;

namespace SmartMeter.Core
{
    public class Day
    {
        private DateTime _date;
        private double _measurement;

        public Day(DateTime date, double measurement)
        {
            _measurement = measurement;
            _date = date;
        }

        public override string ToString()
        {
            return $"_date: {_date.ToShortDateString()}, _measurement: {_measurement}";
        }

        virtual public string GetMarkdownString() => $"|{_date.ToShortDateString()} | {_measurement:0.###} ";

    }
}
