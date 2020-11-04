

using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.Collections.Generic;

namespace UniqueCheck
{
    public class ContentWatchAPI_Model
    {
        public string error { get; set; }
        public string text { get; set; }
        public float percent { get; set; }
        public Matches[] matches { get; set; }
        public int[][] highlight { get; set; }

    }

    //массив с информацией о страницах, на которых найдены совпадения
    public class Matches
    {
        public string url { get; set; }
        public float percent { get; set; }
    }


}
