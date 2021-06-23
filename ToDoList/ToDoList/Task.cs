using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToDoList
{
    public class Task
    {

   
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        public bool TaskCompleted { get; set; }


        public DataTable GetAllTasks(int PageNumber)
        {
            SQLCrud db = new SQLCrud();
            return db.GetTasks(PageNumber);



        }


        public void DeleteTask()
        {
            SQLCrud db = new SQLCrud();
            db.DeleteTask(this);

        }


        public void SaveTask()
        {
            SQLCrud db = new SQLCrud();
            db.SaveTask(this);
        }


    }
}
