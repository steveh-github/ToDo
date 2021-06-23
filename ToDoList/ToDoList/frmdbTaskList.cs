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
    public partial class frmdbTaskList : Form
    {
        public frmdbTaskList()
        {
            InitializeComponent();
        }

        private int PageNumber = 0;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bntNew_Click(object sender, EventArgs e)
        {

            frmdbTask frm = new frmdbTask();
            // this.Hide();
            frm.Text = "Create New Task";
            frm.ShowDialog();
            //this.Visible = true;
            ShowData();
        }

        private void ShowData()
        {

            // get the page size from the settings
            // we can pass this to SQL to change the page size /or recompile the SQL server SP with it soft coded
            // for testing not implemented and use a deafult number in the proc which can simply change and recompile the SP
            int pageSize = Properties.Settings.Default.PageSize;

            
            Task t = new Task();
            dgvTasks.DataSource = t.GetAllTasks(PageNumber);

            // we ned these but we dont want to display them
            dgvTasks.Columns["Page"].Visible = false;
            dgvTasks.Columns["ID"].Visible = false;

            if (dgvTasks.Rows.Count > 0)
            {
                PageNumber = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["Page"].Value);
                
            }


            lblPageNumber.Text = PageNumber.ToString();
            btnPrevious.Enabled = (PageNumber != 0);
        }


        private void frmdbTaskList_Load(object sender, EventArgs e)
        {

            dgvTasks.RowHeadersVisible = false; //no need for these
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.ReadOnly = true;
            dgvTasks.MultiSelect = false;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


            ShowData();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // we are moving Next
            // make sure we have ot gone of the scale
            PageNumber += 1;
            ShowData();
            if (dgvTasks.Rows.Count == 0)
            {
                btnNext.Enabled = false;
                PageNumber -= 1;
                ShowData();
            }
            else
            {
                btnNext.Enabled = true;
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UpdateTask()
        {
            // build a task from the row on the grid and pass it to the form via the Property


            if (dgvTasks.SelectedRows.Count == 1)
            {

                // build the object

                Task task = new Task();

                task.ID = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["ID"].Value);
                task.Title = dgvTasks.SelectedRows[0].Cells["Title"].Value.ToString();
                task.Description = dgvTasks.SelectedRows[0].Cells["Description"].Value.ToString();

                if (dgvTasks.SelectedRows[0].Cells["DueDate"].Value != DBNull.Value)
                {
                    task.DueDate = (DateTime)dgvTasks.SelectedRows[0].Cells["DueDate"].Value;
                }

                task.TaskCompleted = (bool)dgvTasks.SelectedRows[0].Cells["Completed"].Value;


                frmdbTask frm = new frmdbTask();
                frm.Text = "Update Task";
                frm.Task = task;
                frm.ShowDialog();
                ShowData();

            }

            else
            {
                MessageBox.Show("You must select a row on the grid in order to Update!", "Task Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateTask();
            
        }

        private void bntDelete_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 1)
            {


                if (MessageBox.Show("Are you sure you want to delete the selected task ?", "Confirm Task Delete", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Task task = new Task();
                    task.ID = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["ID"].Value);
                    task.DeleteTask();
                    MessageBox.Show("Task deleted!");
                    ShowData();
                }
                }
            else
            {
                
                MessageBox.Show("You must select a row on the grid in order to Delete!", "Task Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (PageNumber >= 1)
            {
                PageNumber -= 1;
            }
            btnNext.Enabled = true;
            ShowData();
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTask();
        }
    }
}
