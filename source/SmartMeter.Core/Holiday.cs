using System;

namespace SmartMeter.Core
{
    internal class Holiday : Day
    {
        private string _holidayDesc;

        public Holiday(DateTime date, double measurement, string v) : base(date, measurement)
        {
            _holidayDesc = v;
        }

        public override string ToString()
        {
            return $" {base.ToString()} {nameof(_holidayDesc)} {_holidayDesc}";
        }

        public override string GetMarkdownString() => $"{base.GetMarkdownString()} | {_holidayDesc}"; 

    }
}