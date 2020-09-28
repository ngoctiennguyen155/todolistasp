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
using todo_1.App_Code.DAL;

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
        public string[,] comments = new string[100, 200];
        public string[,] idnvcomments = new string[100, 200];
        public string[,] namenvcomments = new string[100, 200];
        public string[,] contactjobpublic = new string[100, 200];


        public DataTable comment = new DataTable();

        public string daySelected = DateTime.Now.ToString();
        public ConnectDB dbcon = new ConnectDB();
        
        
        public DateTime dayshowonindex = new DateTime();
        // jobpulic 
        public Jobs jobPublics;
        public DataTable jobPublicData = new DataTable();
            //
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        protected void Page_Load(object sender, EventArgs e) 
        {
            title = " abc";
            
            Jobs jobHandle = new Jobs();
            
            if (Request.QueryString["daysearch"]!=null)
            {
                string daytam = Request.QueryString["daysearch"];
                if (Request.QueryString["daysearch"] == "")
                {
                    dbcon.Setdayfilter(daySelected);
                }
                else
                {
                    dbcon.Setdayfilter(Request.QueryString["daysearch"]);
                }
                
            }
            title = convetday(dbcon.Getcurrentday());
            Response.Write("<script>alert('" + title + "');</script>");
            //title = GetDayStartOfWeek(daySelected);
            if (Request.QueryString["iddelete"]!=null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["iddelete"]) ;
                jobHandle.Delete();
                //ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Thông báo: ", "<script>alert('Xóa công việc thành công');window.location='index.aspx';</script>");
                Response.Write("<script>alert('Xóa công việc thành công');window.location='index.aspx';</script>");
            }


            if (Request.QueryString["idupdate"] != null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["idupdate"]);
                jobHandle.job_title = Request.QueryString["textupdate"];

                jobHandle.Update();
                Response.Write("<script>alert('Cập nhật công việc thành công');window.location='index.aspx';</script>");
            }


            if (Request.QueryString["namecontact"] != null)
            {
                int idnv = int.Parse(Request.QueryString["namecontact"]);
                jobHandle.job_id = int.Parse(Request.QueryString["namecontact_idjob"]);


                jobHandle.DeleteOneContact(idnv);
               Response.Write("<script>alert('Xóa cộng tác thành công');window.location='index.aspx';</script>");
            }

            if (Request.QueryString["contact"] != null)
            {
                jobHandle.job_id = int.Parse(Request.QueryString["idjob"]);
                jobHandle.AddContact(Request.QueryString["contact"].Trim());
                Response.Write("<script>alert('Thêm cộng tác thành công');window.location='index.aspx';</script>");
            }

            if (Request.QueryString["job"] != null)
            {
//                title+= Request.QueryString["job"] +":"+ Request.QueryString["day"]+";";
                string day = Request.QueryString["day"];
                if (Request.QueryString["day"] == "") day = DateTime.Now.ToString();
                DateTime curday= new DateTime();
                if(DateTime.Parse(dbcon.Getcurrentday()) >= DateTime.Now.AddDays(-1))
                {
                    foreach (DateTime dayloop in EachDay(DateTime.Parse(GetDayStartOfWeek(dbcon.Getcurrentday())), DateTime.Parse(GetDayStartOfWeek(dbcon.Getcurrentday())).AddDays(6)))
                    {
                        curday = dayloop;
                        if (dayloop.DayOfWeek.ToString().Equals("Sunday")) continue;

                        if (dayloop.DayOfWeek.ToString().Equals(day)) break;
                    }
                    if (!curday.DayOfWeek.ToString().Equals(day))
                    {
                        //ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Cảnh báo: ", "<script>alert('Không thể thêm công việc cho quá khứ !!!');window.location='index.aspx';</script>");
                        Response.Write("<script>alert('Không thể thêm công việc cho quá khứ !!!');window.location='index.aspx';</script>");
                    }
                    else
                    {
                        jobHandle.job_id = jobHandle.CreateIdJobNext();
                        jobHandle.job_title = Request.QueryString["job"].Trim();
                        jobHandle.job_datestart = curday;
                        jobHandle.Insert((int)Session["User_ID"]);
                        Response.Write("<script>alert('Thêm công việc thành công');window.location='index.aspx';</script>");
                    }
                }else
                {
                    Response.Write("<script>alert('Không thể thêm công việc cho quá khứ !!!');</script>");

                }
                //Response.Write("<script>alert('"+curday+ "');alert('" + daySelected + "');</script>");
            }
            // get cong viec public here
            jobPublics = new Jobs();
            jobPublicData = jobPublics.GetJobsPulic();
            for(int i=0;i<jobPublicData.Rows.Count;i++)
            {
                Jobs abc = new Jobs();
                abc.job_id = (int)jobPublicData.Rows[i][0];
                DataTable zzz = abc.GetAllContactsJobsPublic();
                int run = 0;
                for(int j=0;j<zzz.Rows.Count;j++)
                {
                    contactjobpublic[(int)jobPublicData.Rows[i][0], run++] = (string)zzz.Rows[j][1];
                }    
            }    
            //

            Account ac = new Account();
            ac.id = Session["User_ID"].ToString();
            
            dt= ac.GetJobByID(GetDayStartOfWeek(dbcon.Getcurrentday()));
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
                    //fill comment to arr
                    comment = job.Comments();
                    run = 0;
                    for (int j = 0; j < comment.Rows.Count; j++)
                    {
                        comments[(int)dt.Rows[i][0], run] = (string)comment.Rows[j][2];
                        idnvcomments[(int)dt.Rows[i][0], run] = comment.Rows[j][3].ToString();
                        namenvcomments[(int)dt.Rows[i][0], run] = (string)comment.Rows[j][4];
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
        public string convetday(string str)
        {

            return str;
        }
    }
}
