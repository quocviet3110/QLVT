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
    public partial class frmCTDDH : Form
    {
        
        public frmCTDDH()
        {
            InitializeComponent();
        }

        private void cTDDHBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsCTDH.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSDATHANG);

        }

        private void frmCTDDH_Load(object sender, EventArgs e)
        {
            dSDATHANG.EnforceConstraints = false;
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dSDATHANG.CTDDH);
     
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.frmChinh.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Program.subVattu = new subVATTU();
            Program.checkVT = "DDH";
            Program.subVattu.Show();
        }

        private void frmCTDDH_Shown(object sender, EventArgs e)
        {
            this.bdsCTDH.AddNew();
            txtMaDDH.Text = Program.maddh;
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
                string strLenh = "SELECT COUNT(MAVT) FROM dbo.CTDDH WHERE MAVT='"+txtMaVT.Text.ToString()+ "' AND MasoDDH='" + txtMaDDH.Text.Trim()+"'";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã vật tư bị trùng.", "Lỗi thêm vật tư", MessageBoxButtons.OK);
                    return;
                }

                bdsCTDH.EndEdit();
                bdsCTDH.ResetCurrentItem();
                if (dSDATHANG.HasChanges())
                {

                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTDDHTableAdapter.Update(this.dSDATHANG.CTDDH);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã vật tư bị trùng.", "Lỗi thêm chi tiết đơn đặt hàng!", MessageBoxButtons.OK);
                return;
            }
            this.Close();
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
    }
}
