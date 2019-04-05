using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace ManagerDormitorySchool
{
    public partial class ClassForm : Form
    {
        public ClassForm()
        {
            InitializeComponent();
            LoadDataClass();
            LoadComboboxDepartment();
        }
        ManagerEntities db = new ManagerEntities();
        #region LoadDataClass
        void LoadDataClass()
        {
            dtgvClass.DataSource = null;
            var query = db.CLASS_TB.ToList();
            ConvertToDataTable(query);
        }
        #endregion
        #region LoadComboboxDepartment
        void LoadComboboxDepartment()
        {
            var query = db.DEPARTMENT_TB;
            DataTable tb = new DataTable();
            tb.Columns.Add("Depart_Id");
            tb.Columns.Add("DepartName");
            foreach(var item in query)
            {
                DataRow dr = tb.NewRow();
                dr["Depart_Id"] = item.DEPART_ID;
                dr["DepartName"] = item.DEPARTNAME;
                tb.Rows.Add(dr);
            }
            cbDepartment.DataSource = tb;
            cbDepartment.DisplayMember = "DepartName";
            cbDepartment.ValueMember = "Depart_Id";
        }
        #endregion
        #region GetIdClass
        int GetIdClass()
        {
            int dem = 0;
            while (true)
            {
                if (db.CLASS_TB.Where(w => w.CLASS_ID == dem).SingleOrDefault() == null)
                {
                    return dem;
                }
                dem++;
            }
        }
        #endregion
        #region CheckDataInClassTB
        int CheckDataInClassTB(string t)
        {
            int departID = int.Parse(cbDepartment.SelectedValue.ToString());
            var query = db.CLASS_TB.Where(w => w.CLASSNAME.ToUpper() == t.ToUpper()&&w.DEPARTMENT==departID).ToList();
            if (query.Count()>0)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        #region GetListIdFromDtgvClass
        List<int> GetListId()
        {
            List<int> arr = new List<int>();
            for(int i = 0; i < dtgvClass.RowCount - 1; i++)
            {
                if (dtgvClass.Rows[i].Cells[3].Value.ToString() == "True" && dtgvClass.Rows[i].Cells[0].Value.ToString() != "")
                {
                    arr.Add(int.Parse(dtgvClass.Rows[i].Cells[0].Value.ToString()));
                }
            }
            return arr;
        }
        #endregion
        #region DeleteOneOfRows
        void DeleteOneOfRows(int id)
        {
            var del = db.CLASS_TB.SingleOrDefault(w => w.CLASS_ID == id);
            db.CLASS_TB.Remove(del);
            db.SaveChanges();
            
        }
        #endregion
        #region Search
        void Search(string t)
        {

            dtgvClass.DataSource = null;
            var query = db.CLASS_TB.Where(w => w.CLASSNAME.ToUpper().Contains(t.ToUpper())).ToList();
            dtgvClass.DataSource = null;
            ConvertToDataTable(query);
        }
        #endregion
        #region ConvertToDataTable
        void ConvertToDataTable(List<CLASS_TB> list)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("Class ID", typeof(int));
            tb.Columns.Add("Class Name", typeof(string));
            tb.Columns.Add("Department of Name", typeof(string));
            tb.Columns.Add("Select", typeof(bool));
            foreach (var item in list)
            {
                var result = db.DEPARTMENT_TB.Where(w => w.DEPART_ID == item.DEPARTMENT).ToList();
                DataRow dt = tb.NewRow();
                dt["Class ID"] = item.CLASS_ID;
                dt["Class Name"] = item.CLASSNAME;
                dt["Department of Name"] = result[0].DEPARTNAME;
                tb.Rows.Add(dt);
            }
            dtgvClass.DataSource = tb;
        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string t="";
            if (cbSystem.Text == ""||cbClassName.Text=="") {
                MessageBox.Show("Please choose system !");
                return;
            }
            else
            {
                if (cbSystem.SelectedIndex == 0)
                {
                    t += "SP." + cbClassName.Text.ToUpper();
                }
                else if(cbSystem.SelectedIndex == 1)
                {
                    t += cbDepartment.Text.First().ToString().ToUpper() + cbDepartment.Text.Substring(1) + "." + cbClassName.Text.ToUpper();
                }
                else
                {
                    return;
                }
            }
           
          
            if (CheckDataInClassTB(t)==1)
            {
                MessageBox.Show("Have a row data exist !");
            }
            else
            {
                 CLASS_TB add = new CLASS_TB();
                add.CLASS_ID = GetIdClass();
                add.CLASSNAME = t;
                add.DEPARTMENT = int.Parse(cbDepartment.SelectedValue.ToString());
                db.CLASS_TB.Add(add);
                db.SaveChanges();
            }
            LoadDataClass();
        }
        private void dtgvClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtClassId.Text = dtgvClass.Rows[row].Cells[0].Value.ToString();
                string[] t = dtgvClass.Rows[row].Cells[1].Value.ToString().Split('.');
                cbClassName.Text = t[1];
                if (t[0] == "SP")
                {
                    cbSystem.SelectedIndex = 0;
                }
                else
                {
                    cbSystem.SelectedIndex = 1;
                }
                cbDepartment.Text = dtgvClass.Rows[row].Cells[2].Value.ToString();
            }
            catch
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> arr = GetListId();
            if (arr.Count == 0)
            {
                MessageBox.Show("Please select rows to delete !");
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want delete " + arr.Count + " rows data ?", "Notification", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DeleteOneOfRows(arr[i]);
                    }

                }
 
            }
            LoadDataClass();

        }

        private void checkSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkSelect.Checked)
            {
                for (int i = 0; i < dtgvClass.RowCount - 1; i++)
                {
                    dtgvClass.Rows[i].Cells[3].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dtgvClass.RowCount - 1; i++)
                {
                    dtgvClass.Rows[i].Cells[3].Value = false;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cbClassName.Text != "" && cbDepartment.Text != "" && cbSystem.Text != "")
            {
                int id = int.Parse(txtClassId.Text);
                var update = db.CLASS_TB.SingleOrDefault(s => s.CLASS_ID ==id );
                if (update!=null){
                    string t = "";
                    if (cbSystem.SelectedIndex == 0)
                    {
                        t += "SP." + cbClassName.Text.ToUpper();
                    }
                    else
                    {
                        t += cbDepartment.Text.First().ToString().ToUpper() + cbDepartment.Text.Substring(1) + "." + cbClassName.Text.ToUpper();
                    }
                    update.CLASSNAME = t;
                    update.DEPARTMENT = int.Parse(cbDepartment.SelectedValue.ToString());

                    db.SaveChanges();
                }
            }
            LoadDataClass();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search(txtSearch.Text);
        }
        
        private void btnSort_Click(object sender, EventArgs e)
        {
            int idDepartment = int.Parse(cbDepartment.SelectedValue.ToString());
            var query = db.CLASS_TB.Where(w => w.DEPARTMENT == idDepartment).ToList();
            ConvertToDataTable(query);
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
