using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TestRefactory
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 驗證密碼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <history>
        ///1. Use "parameters" to avoid sql injection attack
        ///2. extract data access and business logic
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string id = this.txtBoxUserName.Text;
            string password = this.txtBoxPassword.Text;

            var status = VerifypasswordById(id, password);

            
            string result = string.Empty;
            switch (status)
            {
                case VerifyStatus.Passed:
                case VerifyStatus.Failed:
                    result = status.ToString();
                    break;

                case VerifyStatus.NotExist:
                    result = "帳號或密碼錯誤!";
                    break;
                case VerifyStatus.None:
                default:
                    break;
            }

            this.txtBoxResult.Text = result;
        }

        //private enum VerifyStatus
        //{
        //    None = 0,
        //    Passed,
        //    Failed,
        //    NotExist
        //}

        private VerifyStatus VerifypasswordById(string id, string password)
        {
            DataTable dt = QueryPasswordById(id, password);

            if (dt.Rows.Count > 0)
            {
                if (password == dt.Rows[0]["password"].ToString())
                {
                    //this.txtBoxResult.Text = "Pass !!";
                    return VerifyStatus.Passed;
                }
                else
                {
                    //this.txtBoxResult.Text = "Fail !!";
                    return VerifyStatus.Failed;
                }
            }
            else
            {
                //this.txtBoxResult.Text = "帳號或密碼錯誤";
                return VerifyStatus.NotExist;
            }
        }//private void VerifypasswordById(string id, string password)

        private DataTable QueryPasswordById(string id, string password)
        {
            DataTable dt = new DataTable();
            
            string connectStr = @"Data Source=rogerchao-PC;Initial Catalog=Northwind;" +
                                 "Integrated Security=true;";

            using (SqlConnection conn = new SqlConnection(connectStr))
            {
                conn.Open();

                string sqlString = @"select password from Users where id=@id";
                SqlCommand sqlCommand = new SqlCommand(sqlString, conn);
                sqlCommand.Parameters.AddWithValue("@id", id);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                dt = new DataTable();
                adapter.Fill(dt);

                
            }

            return dt;
        }
    }
}