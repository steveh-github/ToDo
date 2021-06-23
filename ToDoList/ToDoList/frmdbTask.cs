using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ToDoList
{
    public partial class frmdbTask : Form
    {

        private Task task;
        
        public Task Task
        {
            
            set
            {
                task = value;
            }

        }

        private void SetControls()
        {
            txtID.Text = task.ID.ToString();
            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description;

            if (task.DueDate.HasValue)
            {
                dtpDueDate.Value = (DateTime)task.DueDate;
                dtpDueDate.Checked = true;
            
            }
            else
            {
                // we arent using it unless it is ticked!
                dtpDueDate.Value = DateTime.Now;
                dtpDueDate.Checked = false;

            }

            chkCompleted.Checked = task.TaskCompleted;

            // spec says completed task can not be performed again or reverse a completed flag
            grpDetails.Enabled = !chkCompleted.Checked;
            btnSave.Enabled = grpDetails.Enabled;

        }

        public frmdbTask()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {

            if (txtTitle.Text.Trim() == "")
            {
                MessageBox.Show("Title is mandatory.");
                return false;
            }

            if (dtpDueDate.Checked)
            {
                if (dtpDueDate.Value < DateTime.Now )
                {
                    MessageBox.Show("Cannot schedule a task in the past.");
                    return false;
                }

            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {



            try
            {

                if (ValidateInput())
                {


                    Task t = new Task();
                    t.ID = Convert.ToInt32(txtID.Text);
                    t.Title = txtTitle.Text;
                    t.Description = txtDescription.Text;

                    if (dtpDueDate.Checked)
                    {
                        t.DueDate = dtpDueDate.Value;
                    }

                    t.TaskCompleted = chkCompleted.Checked;


                    // save it
                    t.SaveTask();
                    MessageBox.Show("Task added successfully");
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }




        }


      
        private void frmdbTask_Load(object sender, EventArgs e)
        {
            dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpDueDate.CustomFormat = "MM/dd/yyyy hh:mm:ss";


            if (!(this.task is null))
            {
                // we have passed in a task object therefore we are updating a task!
                SetControls();
                
            }
            else
            {
                txtID.Text = "0";
            }
           
            
        }
    }
}
