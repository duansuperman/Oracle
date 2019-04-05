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
    public partial class RoomForm : Form
    {
        public RoomForm()
        {
            InitializeComponent();
            LoadDataRoom();
        }
        ManagerEntities db = new ManagerEntities();
        #region LoadDataRoom
        void LoadDataRoom()
        {
            var query = db.ROOM_TB.ToList();
            ConvertToDataTable(query);
        }
        #endregion
        #region ConvertToDataTable
        void ConvertToDataTable(List<ROOM_TB> list)
        {
            dtgvRoom.DataSource = null;
            DataTable tb = new DataTable();
            tb.Columns.Add("Room ID",typeof(int));
            tb.Columns.Add("Room Name",typeof(string));
            tb.Columns.Add("Select", typeof(bool));
            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr["Room ID"] = item.ROOMID;
                dr["Room Name"] = item.ROOMNAME;
                tb.Rows.Add(dr);
            }
            dtgvRoom.DataSource = tb;
        }
        #endregion
        #region GetIdRoom
        int GetIdRoom()
        {
            int dem = 0;
            while (true)
            {
                var query = db.ROOM_TB.SingleOrDefault(w => w.ROOMID == dem);
                if (query==null) 
                {
                    return dem;
                }
                dem++;
            }
        }
        #endregion
        #region CheckDataInRoom
        int CheckDataInRoom(string t)
        {
            var query = db.ROOM_TB.Where(w => w.ROOMNAME == t).ToList();
            if (query.Count!=0)
            {
                return 1;
            }
            return 0;
        }
        #endregion
        #region GetListId
        List<int> GetListId()
        {
            List<int> arr= new List<int>();
            for(int i = 0; i < dtgvRoom.RowCount - 1; i++)
            {

                if (dtgvRoom.Rows[i].Cells[2].Value.ToString()=="True"&& dtgvRoom.Rows[i].Cells[0].Value.ToString() != "")
                {
                    arr.Add(int.Parse(dtgvRoom.Rows[i].Cells[0].Value.ToString()));
                }
            }
            return arr;
        }
        #endregion
        #region DeleteOneOfRows
        void DeleteOneOfRows(int id)
        {
            var query = db.ROOM_TB.SingleOrDefault(s => s.ROOMID == id);
            if (query != null)
            {
                db.ROOM_TB.Remove(query);
                db.SaveChanges();
            }
           
        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbBuilding.SelectedIndex >=0)
            {
                string t = cbBuilding.Text.ToUpper() + numFloor.Value + "." + numRoomNumber.Value;
               
                if (CheckDataInRoom(t)==1)
                {
                    MessageBox.Show("Row data have existed !");
                }
                else
                {
                    
                    ROOM_TB room = new ROOM_TB();
                    room.ROOMID = GetIdRoom();
                    room.ROOMNAME = t;
                    db.ROOM_TB.Add(room);
                    db.SaveChanges();
                }
            }
            LoadDataRoom();
        }

        private void checkSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkSelect.Checked == true)
            {
                for(int i = 0; i < dtgvRoom.RowCount - 1; i++)
                {
                    dtgvRoom.Rows[i].Cells[2].Value = true;
                }
                
            }
            else
            {
                for (int i = 0; i < dtgvRoom.RowCount - 1; i++)
                {
                    dtgvRoom.Rows[i].Cells[2].Value = false;
                }
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
            LoadDataRoom();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtRoomId.Text != "")
            {
                int id = int.Parse(txtRoomId.Text);
                var query = db.ROOM_TB.SingleOrDefault(w => w.ROOMID == id);
                if (query != null)
                {
                    query.ROOMNAME = cbBuilding.Text.ToUpper() + numFloor.Value + "." + numRoomNumber.Value;
                    db.SaveChanges();
                }
            }
            LoadDataRoom();
        }

        private void dtgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtRoomId.Text = dtgvRoom.Rows[row].Cells[0].Value.ToString();
                string[] t = dtgvRoom.Rows[row].Cells[1].Value.ToString().Split('.');
                cbBuilding.Text = t[0].First().ToString();
                numFloor.Value = int.Parse(t[0].Substring(1));
                numRoomNumber.Value = int.Parse(t[1]);
            }
            catch
            {

            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            string t = cbBuilding.Text;
            if (t != "")
            {
                var query = db.ROOM_TB.Where(w => w.ROOMNAME.ToUpper().Contains(t.ToUpper())).ToList();
                ConvertToDataTable(query);
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string t = txtSearch.Text;
            if (t != "")
            {
                var query = db.ROOM_TB.Where(w => w.ROOMNAME.ToUpper().Contains(t.ToUpper())).ToList();
                ConvertToDataTable(query);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
