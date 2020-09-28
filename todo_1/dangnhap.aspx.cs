using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using todo_1.App_Code.BLL;

namespace todo_1
{
    public partial class dangnhap : System.Web.UI.Page
    {
        public Filter dbcon = new Filter();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Account ac = new Account();
            ac.taikhoan = Login1.UserName;
            ac.matkhau = Login1.Password;
            DataTable dt = ac.GetAccount();
            if(dt.Rows.Count==1)
            {
                string day = DateTime.Now.ToString();
                Session["User_ID"] = dt.Rows[0][0];
                dbcon.Setdayfilter(DateTime.Now.ToString());
                Response.Redirect("index.aspx");
            }else
            {
                Login1.FailureText = "Login faild";
            }
            
        }
        protected void ButtonLogin_Click(object sender,EventArgs e)
        {
           
        }
    }
}