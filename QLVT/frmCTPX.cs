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
    public partial class frmCTPX : Form
    {
        public frmCTPX()
        {
            InitializeComponent();
        }

        private void cTPXBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.cTPXBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSPHIEUXUAT);

        }

        private void frmCTPX_Load(object sender, EventArgs e)
        {
            dSPHIEUXUAT.EnforceConstraints = false;
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.dSPHIEUXUAT.CTPX);

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

                cTPXBindingSource.EndEdit();
                cTPXBindingSource.ResetCurrentItem();
                if (dSPHIEUXUAT.HasChanges())
                {
                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPXTableAdapter.Update(this.dSPHIEUXUAT.CTPX);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã vật tư bị trùng.", "Lỗi thêm chi tiết phiếu nhập!", MessageBoxButtons.OK);
                return;
            }
            string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_SuaSLVattu] " + txtSL.Text.Trim() + ",'" + txtMaVT.Text.Trim() + "' ,'HIEU' " + " SELECT  'Return Value' = @return_value";
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

        private void frmCTPX_Shown(object sender, EventArgs e)
        {
            this.cTPXBindingSource.AddNew();
            txtMaPX.Text = Program.mapx;
        }

        private void txtDG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Program.subVattu = new subVATTU();
            Program.checkVT = "PX";
            Program.subVattu.Show();
        }
    }
}
