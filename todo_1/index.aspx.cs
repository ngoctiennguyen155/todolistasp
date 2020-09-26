using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using todo_1.App_Code.BLL;

namespace todo_1
{
    public partial class index : System.Web.UI.Page
    {
        public string abc = "xyz";
        public string title = "";
        public DateTime day = DateTime.Now;
        public DataTable dt = new DataTable();

        public string todo;
        //array contacts
        public string[,] contacts = new string[200,200];
        public int[,] idcontacts = new int[200, 200];

        public string daySelected = DateTime.Now.ToString();
        public DateTime dayshowonindex = new DateTime();
        
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        protected void Page_Load(object sender, EventArgs e) 
        {
            title = " abc";
            Jobs jobHandle = new Jobs();
          
            if(Request.QueryString["daysearch"]!=null)
            {
                title = Request.QueryString["daysearch"];
                string daytam = Request.QueryString["daysearch"];
                daySelected = daytam;
            }
            title = GetDayStartOfWeek(daySelected);
           // dayshowonindex = DateTime.ParseExact(daySelected.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (Request.QueryString["iddelete"]!=null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["iddelete"]) ;
                jobHandle.Delete();
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Xóa công việc thành công');window.location='index.aspx';</script>");
            }


            if (Request.QueryString["idupdate"] != null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["idupdate"]);
                jobHandle.job_title = Request.QueryString["textupdate"];

                jobHandle.Update();
                //title += Request.QueryString["idupdate"] + "-"+Request.QueryString["textupdate"].Trim()+";";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Cập nhật công việc thành công');window.location='index.aspx';</script>");
            }


            if (Request.QueryString["namecontact"] != null)
            {
                int idnv = int.Parse(Request.QueryString["namecontact"]);
                jobHandle.job_id = int.Parse(Request.QueryString["namecontact_idjob"]);


                jobHandle.DeleteOneContact(idnv);
                //title += Request.QueryString["namecontact"].Trim() + "-"+Request.QueryString["namecontact_idjob"] +";";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Xóa cộng tác công việc thành công');window.location='index.aspx';</script>");
            }

            if (Request.QueryString["contact"] != null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["idjob"]);
                jobHandle.AddContact(Request.QueryString["contact"].Trim());
                //title += Request.QueryString["contact"].Trim() + "-"+Request.QueryString["idjob"] +";";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Thêm cộng tác công việc thành công');window.location='index.aspx';</script>");
            }

            if (Request.QueryString["job"] != null)
            {
//                title+= Request.QueryString["job"] +":"+ Request.QueryString["day"]+";";
                string day = Request.QueryString["day"];
                DateTime curday = new DateTime();
                foreach (DateTime dayloop in EachDay(DateTime.Now, DateTime.Now.AddDays(5)))
                {
                    curday = dayloop;
                    if (dayloop.DayOfWeek.ToString().Equals("Sunday")) break;

                    if (dayloop.DayOfWeek.ToString().Equals(day)) break;
                }
                if (!curday.DayOfWeek.ToString().Equals(day))
                {
                    ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Cảnh báo: ", "<script>alert('Không thể thêm công việc cho quá khứ !!!');window.location='index.aspx';</script>");
                }else
                {
                    jobHandle.job_id = jobHandle.CreateIdJobNext();
                    jobHandle.job_title = Request.QueryString["job"].Trim();
                    jobHandle.job_date = curday;
                    jobHandle.Insert((int)Session["User_ID"]);
                    ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Thêm công việc thành công.');window.location='index.aspx';</script>");
                }

              
            }


            Account ac = new Account();
            ac.id = Session["User_ID"].ToString();
            
            dt= ac.GetJobByID(GetDayStartOfWeek(daySelected));
            if (dt.Rows.Count > 0)
            {
                day = (DateTime)dt.Rows[0][2];
                // fill array contacts
                DataTable cont = new DataTable();
                Jobs job = new Jobs();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    job.job_id = (int)dt.Rows[i][0];
                    cont = job.GetContact(Session["User_ID"].ToString());
                    int run = 0;
                    for (int j = 0; j < cont.Rows.Count; j++)
                    {
                        idcontacts[(int)dt.Rows[i][0], run] = (int)cont.Rows[j][0];
                        contacts[(int)dt.Rows[i][0], run] = cont.Rows[j][1].ToString();
                        run++;
                    }
                }
            }
            



            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }
       
        public string GetDayStartOfWeek(string dayinput)
        {
            DateTime daycur = DateTime.Parse(dayinput);
            if(daycur.DayOfWeek.ToString().Equals("Monday"))
            {
                daycur = daycur.AddDays(-1);
            }
            if (daycur.DayOfWeek.ToString().Equals("Tuesday"))
            {
                daycur = daycur.AddDays(-2);
            }
            if (daycur.DayOfWeek.ToString().Equals("Wednesday"))
            {
                daycur = daycur.AddDays(-3);
            }
            if (daycur.DayOfWeek.ToString().Equals("Thursday"))
            {
                daycur = daycur.AddDays(-4);
            }
            if (daycur.DayOfWeek.ToString().Equals("Friday"))
            {
                daycur = daycur.AddDays(-5);
            }
            if (daycur.DayOfWeek.ToString().Equals("Saturday"))
            {
                daycur = daycur.AddDays(-6);
            }
            return daycur.ToString();
        }
      
    }
}
