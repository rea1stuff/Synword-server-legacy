namespace SynWord_Server_CSharp.Model.UniqueCheck {
    public class ContentWatchApiModel {
        public string Error { get; set; }
        public string Text { get; set; }
        public float Percent { get; set; }
        public Match[] Matches { get; set; }
        public int[][] Highlight { get; set; }
    }
}
