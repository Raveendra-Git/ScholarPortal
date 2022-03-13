using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScholarPortal.Library
{
    public class dbclass
    {
        string constr = ConfigurationManager.ConnectionStrings["rydbConnection"].ConnectionString;
        SqlDataAdapter sda;
        DataTable dt;
        DataSet ds;

        //*******************************************************************
        //             Asynchronous methods
        //*******************************************************************
        //Select datatable by command
        public async Task<DataTable> ExcuteDT_CMDAsync(SqlCommand cmd)
        {
            dt = new DataTable();
            cmd.CommandType = CommandType.Text;            
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                con.Open();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return await Task.Run(() => { return dt; });
        }

        public async Task<int> ExcuteNonquerySPAsync(SqlCommand cmd)
        {
            int result = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlConnection con=new SqlConnection(constr))
            {
                cmd.Connection = con;
                con.Open();
                result = await cmd.ExecuteNonQueryAsync();
            }
            return result;
        }
              

        //Insert , Update , Delete  operation by command
        public async Task<int> ExcuteNonqueryCMDAsync(SqlCommand cmd)
        {
            int result = 0;
            cmd.CommandType = CommandType.Text;
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                //open connection
                con.Open();
                result = await cmd.ExecuteNonQueryAsync();
            }
            return result;
        }

        //Select single column  by command
        public async Task<string> ExcuteScalerCMDAsync(SqlCommand cmd)
        {
            string result = "";
            cmd.CommandType = CommandType.Text;            
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;                
                con.Open();
                object rst = await cmd.ExecuteScalarAsync();
                if (rst != null)
                {
                    result = Convert.ToString(rst);
                }
            }
            return result;
        }

        //Select single column  by command
        public async Task<string> ExcuteScalerSPAsync(SqlCommand cmd)
        {
            string result = "";
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                con.Open();
                object rst = await cmd.ExecuteScalarAsync();
                if (rst != null)
                {
                    result = Convert.ToString(rst);
                }
            }
            return result;
        }

        //Select datatable by procedure
        public async Task<DataTable> ExcuteDT_SPAsync(SqlCommand cmd)
        {
            dt = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;                        
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                con.Open();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return await Task.Run(() => { return dt; });
        }

        //Select dataset by procedure
        public async Task<DataSet> ExcuteDS_SPAsync(SqlCommand cmd)
        {
            ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                //open connection
                con.Open();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            return await Task.Run(() => { return ds; });
        }
        //Select dataset by comand
        public async Task<DataSet> ExcuteDS_CMDAsync(SqlCommand cmd)
        {
            ds = new DataSet();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                //open connection
                con.Open();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            return await Task.Run(() => { return ds; });
        }


        //*******************************************************************
        //             Synchronous methods
        //*******************************************************************
        //Select datatable by command

        //Insert , Update , Delete  operation by command
        public int ExcuteNonqueryCMD(SqlCommand cmd)
        {
            int result = 0;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                cmd.Connection = con;
                //open connection
                con.Open();
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

    }
}