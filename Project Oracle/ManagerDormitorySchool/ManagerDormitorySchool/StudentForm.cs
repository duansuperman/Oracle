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
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
            LoadAll();
        }
        void LoadAll()
        {
            LoadComboboxRoom();
            LoadComboboxProvince();
            LoadDataStudent();
            LoadComboboxDepartment();
            DefineDtpk();
        }
        ManagerEntities db = new ManagerEntities();
        #region ConvertToDataTableStudent
        void ConvertToDataTableStudent(List<STUDENT_TB> list)
        {
            dtgvStudent.DataSource = null;
            DataTable tb = new DataTable();
            tb.Columns.Add("Student ID", typeof(string));
            tb.Columns.Add("Student Name", typeof(string));
            tb.Columns.Add("Class", typeof(string));
            tb.Columns.Add("Room", typeof(string));
            tb.Columns.Add("Status", typeof(int));
            tb.Columns.Add("Date In", typeof(string));
            tb.Columns.Add("Date Out", typeof(string));
            tb.Columns.Add("Place", typeof(string));
            tb.Columns.Add("Priority", typeof(int));
            tb.Columns.Add("Select ", typeof(bool));
            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["Student ID"] = item.STUDENT_ID;
                dr["Student Name"] = item.STUDENTNAME;
                var classItem = db.CLASS_TB.SingleOrDefault(s => s.CLASS_ID == item.CLASS);
                dr["Class"] = classItem.CLASSNAME;
                var room = db.ROOM_TB.SingleOrDefault(s => s.ROOMID == item.ROOM);
                dr["Room"] = room.ROOMNAME;
                dr["Status"] = item.STATUS;
                dr["Date In"] = item.DATEIN;
                dr["Date Out"] = item.DATEOUT;
                dr["Place"] = item.PLACE;
                dr["Priority"] = item.PRIORITY;
                tb.Rows.Add(dr);
            }
            dtgvStudent.DataSource = tb;
        }
        #endregion
        #region ConvertToDataTableProvince
        DataTable ConvertToDataTableProvince(List<DEVVN_TINHTHANHPHO> list)
        {

            DataTable tb = new DataTable();
            tb.Columns.Add("MaTP", typeof(string));
            tb.Columns.Add("Name Province", typeof(string));


            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["MaTP"] = item.MATP;
                dr["Name Province"] = item.NAME;
                tb.Rows.Add(dr);
            }
            return tb;
        }
        #endregion
        #region ConvertToDataTableDistrict
        DataTable ConvertToDataTableDistrict(List<DEVVN_QUANHUYEN> list)
        {

            DataTable tb = new DataTable();
            tb.Columns.Add("MaQH", typeof(string));
            tb.Columns.Add("Name District", typeof(string));


            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["MaQH"] = item.MAQH;
                dr["Name District"] = item.NAME;
                tb.Rows.Add(dr);
            }
            return tb;
        }
        #endregion
        #region ConvertToDataTableVillage
        DataTable ConvertToDataTableVillage(List<DEVVN_XAPHUONGTHITRAN> list)
        {

            DataTable tb = new DataTable();
            tb.Columns.Add("MaX", typeof(string));
            tb.Columns.Add("Name Village", typeof(string));


            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["MaX"] = item.XAID;
                dr["Name Village"] = item.NAME;
                tb.Rows.Add(dr);
            }
            return tb;
        }
        #endregion
        #region LoadComboboxProvince
        void LoadComboboxProvince()
        {
            DataTable tb = new DataTable();
            var list = db.DEVVN_TINHTHANHPHO.ToList();
            tb = ConvertToDataTableProvince(list);
            cbProvince.DataSource = tb;
            cbProvince.DisplayMember = "Name Province";
            cbProvince.ValueMember = "MaTP";
        }
        #endregion
        #region LoadComboboxDistrict
        void LoadComboboxDistrict(string MaTP)
        {
            DataTable tb = new DataTable();
            var list = db.DEVVN_QUANHUYEN.Where(w => w.MATP == MaTP).ToList();
            tb = ConvertToDataTableDistrict(list);
            cbDistrict.DataSource = tb;
            cbDistrict.DisplayMember = "Name District";
            cbDistrict.ValueMember = "MaQH";
        }
        #endregion
        #region LoadComboboxVillage
        void LoadComboboxVillage(string MaQH)
        {
            DataTable tb = new DataTable();
            var list = db.DEVVN_XAPHUONGTHITRAN.Where(w => w.MAQH == MaQH).ToList();
            tb = ConvertToDataTableVillage(list);
            cbVillage.DataSource = tb;
            cbVillage.DisplayMember = "Name Village";
            cbVillage.ValueMember = "MaX";
        }
        #endregion
        #region LoadComboboxClass
        void LoadComboboxClass( int departmentID)
        {
            var query = db.CLASS_TB.Where(w=>w.DEPARTMENT==departmentID);
            DataTable tb = new DataTable();
            tb.Columns.Add("Class_Id");
            tb.Columns.Add("ClassName");
            foreach (var item in query)
            {
                DataRow dr = tb.NewRow();
                dr["Class_Id"] = item.CLASS_ID;
                dr["ClassName"] = item.CLASSNAME;
                tb.Rows.Add(dr);
            }
            cbClass.DataSource = tb;
            cbClass.DisplayMember = "ClassName";
            cbClass.ValueMember = "Class_Id";
        }
        #endregion
        #region LoadComboboxDepartment
        void LoadComboboxDepartment()
        {
            var query = db.DEPARTMENT_TB;
            DataTable tb = new DataTable();
            tb.Columns.Add("Depart_Id",typeof(int));
            tb.Columns.Add("DepartName",typeof(string));
            foreach (var item in query)
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
        #region DefineDtpk
        void DefineDtpk()
        {
            DateTime datenow = DateTime.Now;
            dtpkDateFrom.Value = new DateTime(datenow.Year,datenow.Month,1);
            dtpkDateTo.Value = dtpkDateFrom.Value.AddMonths(1).AddDays(-1);
        }
        #endregion

        #region GetListMssv
        List<string> GetListMssv()
        {
            List<string> list = new List<string>();
            for(int i = 0; i < dtgvStudent.RowCount - 1; i++)
            {
                if (dtgvStudent.Rows[i].Cells[9].Value.ToString() == "True"&& dtgvStudent.Rows[i].Cells[0].Value.ToString()!="")
                {
                    list.Add(dtgvStudent.Rows[i].Cells[0].Value.ToString());
                }
            }
            return list;
        }
        #endregion
        #region DeleteOneOfRows
        void DeleteOneOfRows(string mssv)
        {
            
            var query = db.STUDENT_TB.SingleOrDefault(s => s.STUDENT_ID == mssv);
            db.STUDENT_TB.Remove(query);
           
           
        }
        #endregion
        #region GetMssvClassForInsert
        string GetMssvStudent()
        {
            int dem = 0;
            
            while (true)
            {
                string mssv = 'C' + numCourse.Value.ToString() + '.' + cbDepartment.Text + '.' + dem;
                if (CheckDataInstudentTB(mssv) == 0)
                {
                    return mssv;
                }
                dem++;
            }
        }
        #endregion
        #region LoadComboboxRoom
        void LoadComboboxRoom()
        {
            var query = db.ROOM_TB;
            DataTable tb = new DataTable();
            tb.Columns.Add("Room_Id");
            tb.Columns.Add("RoomName");
            foreach (var item in query)
            {
                DataRow dr = tb.NewRow();
                dr["Room_Id"] = item.ROOMID;
                dr["RoomName"] = item.ROOMNAME;
                tb.Rows.Add(dr);
            }
            cbRoom.DataSource = tb;
            cbRoom.DisplayMember = "RoomName";
            cbRoom.ValueMember = "Room_Id";
        }
        #endregion
        #region LoadDataStudent
        void LoadDataStudent()
        {
            var query = db.STUDENT_TB.ToList();
            ConvertToDataTableStudent(query);
        }
        #endregion
        #region CheckDataInStudentTB
        int CheckDataInstudentTB(string t)
        {
        
            var query = db.STUDENT_TB.SingleOrDefault(w => w.STUDENT_ID.ToUpper() == t.ToUpper());
            if (query!=null)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                STUDENT_TB add = new STUDENT_TB();
                add.STUDENT_ID = GetMssvStudent();
                add.STUDENTNAME = txtStudentName.Text;
                add.CLASS = int.Parse(cbClass.SelectedValue.ToString());
                add.ROOM = int.Parse(cbRoom.SelectedValue.ToString());
                if (checkStatus.Checked == true)
                {
                    add.STATUS = 1;
                    add.DATEOUT = dtpkDateOut.Value;
                }
                else
                {
                    add.STATUS = 0;

                }
                add.DATEIN = dtpkDateIn.Value;
               
                add.PLACE = cbVillage.Text + " - " + cbDistrict.Text + " - " + cbProvince.Text;
                try
                {
                    add.PRIORITY = int.Parse(cbPriority.Text);
                }
                catch
                {

                }
                db.STUDENT_TB.Add(add);
                db.SaveChanges();
                LoadDataStudent();
            }
            catch
            {
                MessageBox.Show("Please fill information !");
            }
        }

        private void cbProvince_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void cbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string MaTP = cbProvince.SelectedValue.ToString();
            LoadComboboxDistrict(MaTP);
        }

        private void cbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            string MaQH = cbDistrict.SelectedValue.ToString();
            LoadComboboxVillage(MaQH);
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string t = cbDepartment.SelectedValue.ToString();
                int departmentID = int.Parse(t);
                LoadComboboxClass(departmentID);
            }
            catch
            {

            }

        }

        private void dtgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtMSSV.Text = dtgvStudent.Rows[row].Cells[0].Value.ToString();
                txtStudentName.Text = dtgvStudent.Rows[row].Cells[1].Value.ToString();
                cbClass.Text = dtgvStudent.Rows[row].Cells[2].Value.ToString();
                cbRoom.Text = dtgvStudent.Rows[row].Cells[3].Value.ToString();
                if (int.Parse(dtgvStudent.Rows[row].Cells[4].Value.ToString()) == 1)
                {
                    checkStatus.Checked = true;
                }
                else
                {
                    checkStatus.Checked = false;
                }
                dtpkDateIn.Text = dtgvStudent.Rows[row].Cells[5].Value.ToString();
                dtpkDateOut.Text = dtgvStudent.Rows[row].Cells[6].Value.ToString();
                string[] place = dtgvStudent.Rows[row].Cells[7].Value.ToString().Split('-');
                cbProvince.Text = place[2];
                cbDistrict.Text = place[1];
                cbVillage.Text = place[0];
                cbPriority.Text = dtgvStudent.Rows[row].Cells[8].Value.ToString();
            }
            catch
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> arr = GetListMssv();
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
            db.SaveChanges();
            LoadAll();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var update = db.STUDENT_TB.SingleOrDefault(s => s.STUDENT_ID == txtMSSV.Text);
            if (update != null)
            {
                
                update.STUDENTNAME = txtStudentName.Text;
                update.CLASS = int.Parse(cbClass.SelectedValue.ToString());
                update.ROOM = int.Parse(cbRoom.SelectedValue.ToString());
                if (checkStatus.Checked == true)
                {
                    update.STATUS = 1;
                    update.DATEOUT = dtpkDateOut.Value;
                }
                else
                {
                    update.STATUS = 0;

                }
                update.DATEIN = dtpkDateIn.Value;

                update.PLACE = cbVillage.Text + " - " + cbDistrict.Text + " - " + cbProvince.Text;
                try
                {
                    update.PRIORITY = int.Parse(cbPriority.Text);
                }
                catch
                {

                }
                db.SaveChanges();
                LoadAll();
            }

        }

        private void checkSelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkSelectAll.Checked == true)
            {
                for (int i = 0; i < dtgvStudent.RowCount - 1; i++)
                {
                    dtgvStudent.Rows[i].Cells[9].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dtgvStudent.RowCount - 1; i++)
                {
                    dtgvStudent.Rows[i].Cells[9].Value = false;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string t = txtSearch.Text;
            if (t != "")
            {
                var query = db.STUDENT_TB.Where(w => w.STUDENT_ID.ToUpper().Contains(t.ToUpper())).ToList();
                ConvertToDataTableStudent(query);
            }
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            var query = db.STUDENT_TB.Where(w => w.DATEIN >= dtpkDateFrom.Value && w.DATEIN <= dtpkDateTo.Value).ToList() ;
            ConvertToDataTableStudent(query);
        }

        private void statisticSubmitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatisticForm f = new StatisticForm();
            f.ShowDialog();
        }

        private void departmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentForm f = new DepartmentForm();
            f.ShowDialog();
        }

        private void classToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassForm f = new ClassForm();
            f.ShowDialog();
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoomForm f = new RoomForm();
            f.ShowDialog();
        }

        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadAll();
        }
    }
}
