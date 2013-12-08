using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HSMS.Model;
using System.Data;
using System.Data.SqlClient;

namespace HSMS.DAL
{
   public class IdNameDAL
    {
        public static IdName[] GetByCatagory(string category)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_IdName where Category=@Category",
                new SqlParameter("@Category", category));
            IdName[] idNames = new IdName[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                IdName idName = new IdName();
                idName.Id = (Guid)row["Id"];
                idName.Name = (string)row["Name"];
                idNames[i] = idName;
            }
            return idNames;
        }
    }
}
