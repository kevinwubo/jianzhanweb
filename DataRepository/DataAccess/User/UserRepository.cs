﻿using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.User
{
    public class UserRepository:DataAccessBase
    {
        public List<UserInfo> GetAllUser()
        {
            List<UserInfo> result = new List<UserInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.GetAllUsers, "Text"));
            result = command.ExecuteEntityList<UserInfo>();
            return result;
        }

        public List<UserInfo> GetUsersByRule(string name,string nickname, int status,string roleIDs)
        {
            List<UserInfo> result = new List<UserInfo>();
            string sqlText = UserStatement.GetAllUsersByRule;
            if (!string.IsNullOrEmpty(name))
            {
                sqlText += " AND UserName LIKE '%'+@key+'%' or NickName LIKE '%'+@key+'%'  ";
            }
            if (!string.IsNullOrEmpty(nickname))
            {
                sqlText += " AND NickName =@NickName";
            }
            if (status > -1)
            {
                sqlText += " AND Status=@Status";
            }
            if (!string.IsNullOrEmpty(roleIDs))
            {
                sqlText += " AND RoleIDs in(" + roleIDs + ")";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(name))
            {
                command.AddInputParameter("@key", DbType.String, name);
            }
            if (!string.IsNullOrEmpty(nickname))
            {
                command.AddInputParameter("@NickName", DbType.String, nickname);
            }
            if (status > -1)
            {
                command.AddInputParameter("@Status", DbType.Int32, status);
            }

            result = command.ExecuteEntityList<UserInfo>();
            return result;
        }

        public UserInfo GetUserByKey(long uid)
        {
            UserInfo result = new UserInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.GetUserByKey, "Text"));
            command.AddInputParameter("@UserID", DbType.String, uid);
            result = command.ExecuteEntity<UserInfo>();
            return result;
        }

        public int CreateNew(UserInfo user)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.CreateNewUser, "Text"));
            command.AddInputParameter("@UserName", DbType.String, user.UserName);
            command.AddInputParameter("@NickName", DbType.String, user.NickName);
            command.AddInputParameter("@Telephone", DbType.String, user.Telephone);
            command.AddInputParameter("@PrivateTelephone", DbType.String, user.PrivateTelephone);
            command.AddInputParameter("@Password", DbType.String, user.Password);
            command.AddInputParameter("@RoleIDs", DbType.String, user.RoleIDs);
            command.AddInputParameter("@GroupIDs", DbType.String, user.GroupIDs);
            command.AddInputParameter("@CityName", DbType.String, user.CityName);
            command.AddInputParameter("@Status", DbType.Int32, user.Status);
            command.AddInputParameter("@SalesCount", DbType.Int32, user.SalesCount);
            command.AddInputParameter("@CreateDate", DbType.DateTime, user.CreateDate);

            return command.ExecuteNonQuery();
        }

        public int ModifyUser(UserInfo user)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.ModifyUser, "Text"));
            command.AddInputParameter("@UserID", DbType.Int64, user.UserID);
            command.AddInputParameter("@UserName", DbType.String, user.UserName);
            command.AddInputParameter("@NickName", DbType.String, user.NickName);
            command.AddInputParameter("@Telephone", DbType.String, user.Telephone);
            command.AddInputParameter("@PrivateTelephone", DbType.String, user.PrivateTelephone);
            command.AddInputParameter("@RoleIDs", DbType.String, user.RoleIDs);
            command.AddInputParameter("@GroupIDs", DbType.String, user.GroupIDs);
            command.AddInputParameter("@CityName", DbType.String, user.CityName);
            command.AddInputParameter("@SalesCount", DbType.Int32, user.SalesCount);
            command.AddInputParameter("@Status", DbType.Int32, user.Status);

            return command.ExecuteNonQuery();
        }

        public int RemoveUser(long uid)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.Remove, "Text"));
            command.AddInputParameter("@UserID", DbType.Int64, uid);
            int result = command.ExecuteNonQuery();
            return result;
        }

        public UserInfo GetLoginUser(string name,string password)
        {
            UserInfo result = new UserInfo();
            try
            {
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.GetLoginUser, "Text"));
                command.AddInputParameter("@UserName", DbType.String, name);
                command.AddInputParameter("@Password", DbType.String, password);
                result = command.ExecuteEntity<UserInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
            return result;
        }

        public int ModifyPassword(long userid, string password)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.ModifyPassword, "Text"));
            command.AddInputParameter("@UserID", DbType.Int64, userid);
            command.AddInputParameter("@Password", DbType.String, password);

            return command.ExecuteNonQuery();
        }

            
        public int ModifySalesCountByID(long userid, int salesCount)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(UserStatement.ModifySalesCountByID, "Text"));
            command.AddInputParameter("@UserID", DbType.Int64, userid);
            command.AddInputParameter("@SalesCount", DbType.Int32, salesCount);

            return command.ExecuteNonQuery();
        }
    }
}
