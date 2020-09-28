using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace todo_1.App_Code.DAL
{
    public class Job_DAL
    {
        public DataTable GetJobById(string id,string dt)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select* from CongViec cv, (select * from PHANCONG where nv_id ='"+id+"') nt where cv.job_id = nt.job_id and job_datestart>='"+dt+"' and job_datestart<='"+DateTime.Parse(dt).AddDays(7).ToString()+"'";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
        public DataTable GetAllJobs()
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select * from CongViec ";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
        public DataTable GetJobsPulbicToday()
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select * from CongViec where job_public=1";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
        public DataTable GetContacts(string idjob,string idnv)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select nv.nv_id, nv_name from NhanVien nv,PHANCONG pc where nv.nv_id= pc.nv_id and pc.job_id='"+idjob+"' and nv.nv_id!='"+idnv+"'";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
        public DataTable GetComments(string idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select * from BINHLUAN bl,NhanVien nv where job_id ='"+idjob+"' and nv.nv_id = bl.nv_id";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
        public void Insert(int jobid,string title,DateTime day)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // 
                string sQuery = "insert into CongViec(job_id,job_title,job_datestart,job_datefinish,job_status,job_public) values('" + jobid+"',N'"+title+"','"+day+ "','" + day + "','" + 0+"','"+0+"')";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();

            }
        }
        public void Delete(string id)
        {

            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete job in table phancong
                string sQuery = "delete from PHANCONG where job_id='" + Convert.ToInt32(id) + "'";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();
                //delete job in table Congviec
                 sQuery = "delete from CongViec where job_id='"+Convert.ToInt32(id)+"'";
                 cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();
                
            }
        }
        public void Update(string textupdate,int idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // update job in table cong viec
                string sQuery = "update CongViec set job_title=N'"+textupdate+"' where job_id='"+idjob+"'";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();
                
            }
        }
        public void DeleteContact(int idnv,int idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "delete from PHANCONG where nv_id ='"+idnv+"' and job_id='"+idjob+"'; ";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();

            }
        }
        public void AddContact(string email, int idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "insert into PHANCONG(job_id,nv_id) values('"+idjob+"',(select nv_id from NhanVien where nv_email='"+email+"'))";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();

            }
        }
        public void AddContactById(int idnv, int idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "insert into PHANCONG(job_id,nv_id) values('" + idjob + "','"+idnv+"')";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                cmd.ExecuteNonQuery();
            }
        }
        public int CreateIDJob()
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "select MAX(job_id) as maxcurjob from CongViec";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                return (int)cmd.ExecuteScalar() +1;
            }
        }

        // 
        public int CheckEmailExist(string email)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "select Count(nv_email) from NhanVien where nv_email='"+email+"'";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                return (int)cmd.ExecuteScalar();
            }
        }
        public int EmailExistWithJobId(string email,int idjob)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                //
                string sQuery = "select COUNT(*) from PHANCONG where job_id ='"+idjob+"' and nv_id=(select nv_id from NhanVien where nv_email='"+email +"')";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                return (int)cmd.ExecuteScalar();
            }
        }
        public int CheckEmailById(int id)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                // delete phan cong cua 1 nhan vien
                string sQuery = "select COUNT(*) from NhanVien where nv_id ='" + id + "' ";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                return (int)cmd.ExecuteScalar();
            }
        }

        public DataTable GetContactsJobPublic(int id)
        {
            ConnectDB.OpenConect();
            using (ConnectDB.con)
            {
                string sQuery = "select * from NhanVien where nv_id in (select nv.nv_id from NhanVien nv,PHANCONG pc where nv.nv_id =pc.nv_id and pc.job_id='"+id+"')";
                SqlCommand cmd = new SqlCommand(sQuery, ConnectDB.con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(ds);

                return ds.Tables[0];
            }
        }
    }
    
}