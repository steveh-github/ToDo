using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class SQLCrud
    {

        private string connectionString;

        public int PageNumber { get; set; }



        public SQLCrud()
        {
            // in the constructor get the database string
            connectionString = ConfigurationManager.ConnectionStrings["ToDoListConnectionString"].ToString();
        }


        public DataTable GetTasks(int PageNumber)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand("GetTasks", connection);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@pageNumber", PageNumber);

                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                return dt;

            }
        }

        public void DeleteTask(Task task)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand("DeleteTask", connection);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ID", task.ID);
                sqlcmd.ExecuteNonQuery();

            }
        }

        public void SaveTask(Task task)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand("SaveTask", connection);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ID", task.ID);
                sqlcmd.Parameters.AddWithValue("@Title", task.Title);
                sqlcmd.Parameters.AddWithValue("@Description", task.Description);

                if (task.DueDate.HasValue)
                {
                    sqlcmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@DueDate", DBNull.Value);
                }
                sqlcmd.Parameters.AddWithValue("@TaskCompleted", task.TaskCompleted);
                sqlcmd.ExecuteNonQuery();

            }
        }
    }
}
