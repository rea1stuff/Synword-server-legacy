using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.DAO {
    public interface IUnauthUserDao : IDao<UnauthUserApplicationDataModel> {
        public UnauthUserApplicationDataModel GetUserDataByIp(string ip);
    }
}
