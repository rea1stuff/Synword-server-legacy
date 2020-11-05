namespace SynWord_Server_CSharp.Model {
    class WordModel {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public WordModel(int start, int end) {
            StartIndex = start;
            EndIndex = end;
        }
    }
}
