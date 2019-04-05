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
    public partial class StatisticForm : Form
    {
        public StatisticForm()
        {
            InitializeComponent();
            LoadAll();
            
        }
        void LoadAll()
        {
            LoadDataStudent();
            LoadDataStatistic();
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
            
            tb.Columns.Add("Priority", typeof(int));
            tb.Columns.Add("Select ", typeof(bool));
            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["Student ID"] = item.STUDENT_ID;
                dr["Student Name"] = item.STUDENTNAME;
                
                dr["Priority"] = item.PRIORITY;
                tb.Rows.Add(dr);
            }
            dtgvStudent.DataSource = tb;
        }
        #endregion
        #region LoadDataStudent
        void LoadDataStudent()
        {
            var query = db.STUDENT_TB.ToList().Where(w=>w.DATEOUT==null).ToList();
            ConvertToDataTableStudent(query);
        }
        #endregion
        #region LoadDataStatistic
        void LoadDataStatistic()
        {
            var list = db.STATISTIC_TB.ToList();
            ConvertToDataTableStatistic(list);
        }
        #endregion
        #region ConvertToDataTableStatistic
        void ConvertToDataTableStatistic(List<STATISTIC_TB> list)
        {
            dtgvStatistic.DataSource = null;
            DataTable tb = new DataTable();
            tb.Columns.Add("ID", typeof(int));
            tb.Columns.Add("Month", typeof(string));

            tb.Columns.Add("Student", typeof(string));
            tb.Columns.Add("Select ", typeof(bool));
            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["ID"] = item.STA_ID;
                dr["Student"] = item.STUDENT;

                dr["Month"] = item.MONTH;
                tb.Rows.Add(dr);
            }
            dtgvStatistic.DataSource = tb;
        }
        #endregion
        
        #region DefineDtpk
        void DefineDtpk()
        {
            DateTime datenow = DateTime.Now;
            DateTime BeginMonth = new DateTime(datenow.Year, datenow.Month, 1);
            dtpkDateSub.Value = BeginMonth.AddMonths(1).AddDays(-1);
        }
        #endregion
        #region GetListMssv
        List<string> GetListMssv()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dtgvStudent.RowCount - 1; i++)
            {
                if (dtgvStudent.Rows[i].Cells[3].Value.ToString() == "True" && dtgvStudent.Rows[i].Cells[0].Value.ToString() != "")
                {
                    list.Add(dtgvStudent.Rows[i].Cells[0].Value.ToString());
                }
            }
            return list;
        }
        #endregion
        #region GetListID
        List<int> GetListID()
        {
            List<int> list = new List<int>();
            for(int i = 0; i < dtgvStatistic.RowCount-1; i++)
            {
                if(dtgvStatistic.Rows[i].Cells[3].Value.ToString()=="True"&& dtgvStatistic.Rows[i].Cells[0].Value.ToString() != "")
                {
                    list.Add(int.Parse(dtgvStatistic.Rows[i].Cells[0].Value.ToString()));
                }
            }
            return list;
        }
        #endregion
        #region DeleteOneOfRows
        void DeleteOneOfRows(int id)
        {
            
            var query = db.STATISTIC_TB.SingleOrDefault(s => s.STA_ID == id);
            db.STATISTIC_TB.Remove(query);
            
        }
        #endregion
        #region CheckDataInStatistic
        int CheckDataInStatistic(int id)
        {
            var query = db.STATISTIC_TB.SingleOrDefault(s => s.STA_ID == id);
            if (query != null)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        #region GetIdInStatistic
        int GetIdInStatistic()
        {
            int dem = 0;

            while (true)
            {
               
                if (CheckDataInStatistic(dem) == 0)
                {
                    return dem;
                }
                dem++;
            }
        }
        #endregion
        #region InsertMoreRows
        void InsertMoreRows(List<string>list)
        {
            for(int i =0;i<list.Count;i++)
            {
                InsertOneRows(list[i]);
            }
        }
        #endregion
        #region InsertOneRows
        void InsertOneRows(string studentid)
        {
            STATISTIC_TB add = new STATISTIC_TB();

            var query = db.STATISTIC_TB.SingleOrDefault(s => s.STUDENT == studentid && s.MONTH == dtpkDateSub.Value);
            if (query == null)
            {
                add.STA_ID = GetIdInStatistic();
                add.STUDENT = studentid;
                add.MONTH = dtpkDateSub.Value;
                db.STATISTIC_TB.Add(add);
            }
            db.SaveChanges();
        }
        #endregion
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string t = txtSearch.Text;
            if (t != "")
            {
                var query = db.STUDENT_TB.Where(w => w.STUDENT_ID.ToUpper().Contains(t.ToUpper())).ToList();
                ConvertToDataTableStudent(query);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> list = GetListMssv();
            InsertMoreRows(list);
            
            LoadAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> list = GetListID();
            for(int i = 0; i < list.Count; i++)
            {
                DeleteOneOfRows(list[i]);
            }
            db.SaveChanges();
            LoadAll();
        }

        private void checkSelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkSelectAll.Checked == true)
            {
                for (int i = 0; i < dtgvStudent.RowCount - 1; i++)
                {
                    dtgvStatistic.Rows[i].Cells[3].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dtgvStudent.RowCount - 1; i++)
                {
                    dtgvStatistic.Rows[i].Cells[3].Value = false;
                }
            }
        }

        private void dtpkDateSub_ValueChanged(object sender, EventArgs e)
        {
            var query = db.STATISTIC_TB.Where(w => w.MONTH >= dtpkDateSub.Value).ToList();
            ConvertToDataTableStatistic(query);
        }
    }
}
