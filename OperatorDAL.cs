using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HSMS.Model;
using System.Data.SqlClient;
using System.Data;

namespace HSMS.DAL
{
    public class OperatorDAL
    {

        public static void DeleteById(Guid id)
        {
            SqlHelper.ExcuteNonQuery("update T_Operator set IsDelete=1 where id=@id",
                 new SqlParameter("@id", id));
            
        }
        public static Operator[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Operator where IsDelete=0");
            Operator[] operators = new Operator[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                operators[i] = ToOperator(dt.Rows[i]);
            }
            return operators;
        }

        public static Operator GetById(Guid id)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(@"select * from T_Operator where
                                id=@id ",
                                             new SqlParameter("@id", id));
            if (dt.Rows.Count < 0)
            {
                return null;
            }
            else if (dt.Rows.Count > 1)
            {
                throw new Exception("用户id重复");
            }
            else
                return ToOperator(dt.Rows[0]);
        }
        public static void Update(Guid id,string userName,string realName)
        {
            SqlHelper.ExcuteNonQuery(@"update T_Operator set UserName=@UserName,RealName=@RealName 
                                where Id=@id",new SqlParameter("@id",id),
                                             new SqlParameter("@UserName",userName),
                                             new SqlParameter("@RealName",realName));
        }

        public static void Update(Guid id, string userName, string realName,string password)
        {
            SqlHelper.ExcuteNonQuery(@"update T_Operator set UserName=@UserName,RealName=@RealName ,Password=@Password
                                where Id=@id", new SqlParameter("@id", id),
                                             new SqlParameter("@UserName", userName),
                                             new SqlParameter("@RealName", realName),
                                             new SqlParameter("@Password",password));
        }
        public static void Insert(Operator op)
        {
            SqlHelper.ExcuteNonQuery(@"insert into T_Operator(Id,UserName,Password,IsDelete,RealName,IsNocked)
                                        values(newid(),@UserName,@Password,0,@RealName,0)",
                                                       new SqlParameter("@UserName", op.UserName),
                                                       new SqlParameter("@Password", op.Password),
                                                       new SqlParameter("@RealName",op.RealName));
        }

        private static  Operator ToOperator(DataRow row)
        {
            Operator op = new Operator();
            op.UserName = (string)row["userName"];
            op.Id = (Guid)row["Id"];
            op.Password=(string)row["Password"];
            op.IsDelete = (bool)row["IsDelete"];
            op.RealName = (string)row["RealName"];
            op.IsNocked = (bool)row["IsNocked"];
            return op;
        }
        public static Operator GetByUserName(string userName)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(@"select * from T_Operator where
                                userName=@userName and IsDelete=0 ",
                                             new SqlParameter("@userName", userName));
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else if(dt.Rows.Count>1)
            {
                throw new Exception("用户名重复");
            }
            else
            return ToOperator(dt.Rows[0]);

        }
    }
}
