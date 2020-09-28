using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using todo_1.App_Code.DAL;

namespace todo_1.App_Code.BLL
{
    public class Filter
    {
        public string Getcurrentday()
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select day from day ";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                return cmd.ExecuteScalar().ToString();

                //return ds.Tables[0];
            }
        }
        public void Setdayfilter(string day)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "update day set day='" + day + "' ";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();

                //return ds.Tables[0];
            }
        }
    }
}