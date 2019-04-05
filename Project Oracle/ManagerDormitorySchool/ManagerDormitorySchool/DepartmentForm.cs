using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ManagerDormitorySchool
{
    public partial class DepartmentForm : Form
    {
        public DepartmentForm()
        {
            InitializeComponent();
            LoadData();
            
        }
        ManagerEntities db = new ManagerEntities();
        #region LoadData
        void LoadData()
        {
            dtgvDepartment.DataSource = null;
            var loadData = db.DEPARTMENT_TB.ToList();
            //dtgvDepartment.DataSource = loadData;
            ConvertToDataTable(loadData);
        }
        #endregion
        #region GetIdForAdd
        int GetIdForAdd()
        {
            int temp = 1;
            while (true)
            {
                var query = db.DEPARTMENT_TB.Where(w => w.DEPART_ID == temp);
                if (query.Count() == 0)
                {
                    return temp;
                }
                temp++;
            }
            
        }
        #endregion
        #region GetListDepartment
        List<int> GetListDepartment()
        {
            List<int> arr = new List<int>();
            for(int i = 0; i < dtgvDepartment.RowCount - 1; i++)
            {
                if (dtgvDepartment.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    if (dtgvDepartment.Rows[i].Cells[0].Value.ToString() != "")
                    {
                        arr.Add(int.Parse(dtgvDepartment.Rows[i].Cells[0].Value.ToString()));
                    }
                }
            }
            return arr;
        }
        #endregion
        #region DeleteOneOfRows
        void DeleteOneOfRows(int id)
        {
            var query = db.DEPARTMENT_TB.SingleOrDefault(s => s.DEPART_ID == id);
            db.DEPARTMENT_TB.Remove(query);
            db.SaveChanges();
        }
        #endregion
        #region AddCheckBoxInDtgv
        void AddCheckboxInDtgv()
        {

            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.Name = "check";
            check.ValueType = typeof(bool);
            check.HeaderText = "Select";
            check.TrueValue = "1";
            check.FalseValue = "0";
            dtgvDepartment.DataSource = check;
        }
        #endregion
        #region CheckDataByDepartName
        int CheckDataByDepartName(string t)
        {
            string text = txtDepartName.Text;
            text = text.First().ToString().ToUpper() + text.Substring(1).ToLower();
            var query = db.DEPARTMENT_TB.Where(w => w.DEPARTNAME == text);
            if (query.Count() > 0)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        #region LoadResult

        #endregion
        #region ConvertToDataTable
        void ConvertToDataTable(List<DEPARTMENT_TB> list)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("DepartID", typeof(int));
            tb.Columns.Add("NameDepart", typeof(string));
            tb.Columns.Add("Select", typeof(bool));
            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["DepartID"] = item.DEPART_ID;
                dr["NameDepart"] = item.DEPARTNAME;
                tb.Rows.Add(dr);
            }

            dtgvDepartment.DataSource = tb;

        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DEPARTMENT_TB de = new DEPARTMENT_TB();
            int id = GetIdForAdd();
        
            de.DEPART_ID = GetIdForAdd();
            if (CheckDataByDepartName(txtDepartName.Text) == 0)
            {
                
                if (txtDepartName.Text != "")
                {
                    string text = txtDepartName.Text;
                    text = text.First().ToString().ToUpper() + text.Substring(1).ToLower();
                    de.DEPARTNAME = text;
                    db.DEPARTMENT_TB.Add(de);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Have a exist row data !");
            }
            LoadData();
        }

        private void dtgvDepartment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtDepartID.Text = dtgvDepartment.Rows[row].Cells[0].Value.ToString();
                txtDepartName.Text = dtgvDepartment.Rows[row].Cells[1].Value.ToString();
            }
            catch
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> arr = GetListDepartment();
            DialogResult result = MessageBox.Show("Do you want delete " + arr.Count + " rows data ?","Notification",MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                for (int i = 0; i < arr.Count; i++)
                {

                    DeleteOneOfRows(arr[i]);
                }
              
            }
            LoadData();
        }

        private void SelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            if (SelectAll.Checked == true)
            {
                for (int i = 0; i < dtgvDepartment.RowCount - 1; i++)
                {
                    dtgvDepartment.Rows[i].Cells[2].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dtgvDepartment.RowCount - 1; i++)
                {
                    dtgvDepartment.Rows[i].Cells[2].Value =false;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtDepartID.Text);
                var query = db.DEPARTMENT_TB.Where(s => s.DEPART_ID == id).SingleOrDefault();
                if (query != null)
                {
                    query.DEPART_ID = int.Parse(txtDepartID.Text);
                    query.DEPARTNAME = txtDepartName.Text;

                }
                db.SaveChanges();
                LoadData();
            }
            catch
            {
                MessageBox.Show("Please choose row data !");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string text = txtSearch.Text;
           
            var query = db.DEPARTMENT_TB.Where(w => w.DEPARTNAME.ToUpper().Contains(text.ToUpper())).ToList();
            dtgvDepartment.DataSource = null;
            ConvertToDataTable(query);


        }
    }
}
