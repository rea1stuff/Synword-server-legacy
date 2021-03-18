using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.InternalUserToken;

namespace SynWord_Server_CSharp.UserDataHandlers {
    public class UnauthUserApplicationDataHandler : IUserDataHandler<UnauthUserApplicationDataModel> {
        IDao<UnauthUserApplicationDataModel> _db;
        string token;
        public UnauthUserApplicationDataHandler() {
            _db = new UnauthUserApplicationDataDao();
        }

        public void CreateUser(string ip) {
            do {
                token = TokenGenerator.Generate();
            } while (IsUserTokenExist(token));

            string date = DateTime.Now.ToString();

            UnauthUserApplicationDataModel userData = new UnauthUserApplicationDataModel {
                uId = token,
                ip = ip,
                coins = UserLimits.Coins,
                uniqueCheckMaxSymbolLimit = UserLimits.UniqueCheckMaxSymbolLimit,
                uniqueUpMaxSymbolLimit = UserLimits.UniqueUpMaxSymbolLimit,
                documentUniqueUpMaxSymbolLimit = UserLimits.DocumentUniqueUpMaxSymbolLimit,
                lastVisitDate = date,
                creationDate = date
            };

            _db.Create(userData);
        }

        public UnauthUserApplicationDataModel GetUserData(string uId) {
            return _db.GetUserDataById(uId);
        }

        public UnauthUserApplicationDataModel GetUserDataByIp(string ip) {
            UnauthUserApplicationDataDao unauthDao = (UnauthUserApplicationDataDao)_db;
            return unauthDao.GetUserDataByIp(ip);
        }

        public void SetUserData(UnauthUserApplicationDataModel userData) {
            _db.SetUserData(userData);
        }
        public void DeleteUserData(string ip) {
            _db.DeleteUserData(ip);
        }
        public bool IsUserExist(string ip) {
            try {
                UnauthUserApplicationDataDao _getUserDataByIp = new UnauthUserApplicationDataDao();
                _getUserDataByIp.GetUserDataByIp(ip);
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public bool IsUserTokenExist(string uId) {
            try {
                UnauthUserApplicationDataDao _getUserDataByIp = new UnauthUserApplicationDataDao();
                _db.GetUserDataById(uId);
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public string GetToken() {
            if (token != null) {
                return token;
            } else {
                throw new Exception("Token is null");
            }
        }
    }
}
