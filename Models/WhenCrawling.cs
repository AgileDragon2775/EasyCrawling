using EasyCrawling.Enums;
using System;

namespace EasyCrawling.Models
{
    [Serializable]
    public class WhenCrawling
    {
        public TimeSpanType When { get; set; }
        public Week Week { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }


        public WhenCrawling() 
        {
            Week = Week.NONE;
            Hour = -1;
            Min = -1;
            Sec = -1;
        }
       
        public WhenCrawling(WhenCrawling when)
        {
            When = when.When;
            Week = when.Week;
            Hour = when.Hour;
            Min = when.Min;
            Sec = when.Sec;
        }
    }   
}
