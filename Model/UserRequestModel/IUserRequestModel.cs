namespace SynWord_Server_CSharp.Model {
    public interface IUserRequestModel {
        public string Uid { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }
}
