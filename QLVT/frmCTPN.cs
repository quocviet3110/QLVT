using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmCTPN : Form
    {
        public frmCTPN()
        {
            InitializeComponent();
        }

        private void cTPNBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.cTPNBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSDATHANG);

        }

        private void frmCTPN_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dSDATHANG.CTPN' table. You can move, or remove it, as needed.
            dSDATHANG.EnforceConstraints = false;
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.dSDATHANG.CTPN);

        }
        private void frmCTPN_Shown(object sender, EventArgs e)
        {
            this.cTPNBindingSource.AddNew();
            txtMaPN.Text = Program.mapn;
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            if (txtMaVT.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã vật tư không thể để trống!", "", MessageBoxButtons.OK);
                return;
            }

            if (Int32.Parse(txtSL.Text) < 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "", MessageBoxButtons.OK);
                txtSL.Focus();
                return;
            }
            if (Int32.Parse(txtDG.Text) < 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0!", "", MessageBoxButtons.OK);
                txtSL.Focus();
                return;
            }
            try
            {

                cTPNBindingSource.EndEdit();
                cTPNBindingSource.ResetCurrentItem();
                if (dSDATHANG.HasChanges())
                {
                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.dSDATHANG.CTPN);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã vật tư bị trùng.", "Lỗi thêm chi tiết phiếu nhập!", MessageBoxButtons.OK);
                return;
            }
            string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_SuaSLVattu] " + txtSL.Text.Trim()+",'"+txtMaVT.Text.Trim()+"' ,'CONG' " + " SELECT  'Return Value' = @return_value";
            SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
            if (myReader == null) return;
            myReader.Read();
            int result = myReader.GetInt32(0);
            myReader.Close();
            if (result == 0)
            {
                MessageBox.Show("Lỗi thêm số lượng vào vật tư!", "Lỗi thêm số lượng vào vật tư!", MessageBoxButtons.OK);
                return;
            }
            this.Close();

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.frmChinh.Enabled = true;

        }

       

        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Program.subCTDDH = new subCTDDH();
            Program.maCTDDH = Program.maCTDDH.ToString().Trim();
            Program.subCTDDH.Show();
        }
    }
}
