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
    public partial class frmDangKyTaiKhoan : Form
    {
        public frmDangKyTaiKhoan()
        {
            InitializeComponent();
        }

        private void frmDangKyTaiKhoan_Load(object sender, EventArgs e)
        {
            this.hOTENNVTableAdapter.Connection.ConnectionString = Program.connstr;
            this.hOTENNVTableAdapter.Fill(this.dS.HOTENNV);
            cmbHoten.DataSource = hOTENNVBindingSource;
            cmbHoten.DisplayMember = "HOTEN";
            cmbHoten.ValueMember = "MANV";
            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            //Phân quyền
            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = true;
                 cmbQuyen.Items.Add("CONGTY");
                 cmbQuyen.SelectedIndex = 0;
            }
            else if (Program.mGroup.Equals("CHINHANH"))
                {
                     cmbChiNhanh.Enabled = false;
                    cmbQuyen.Items.Add("CHINHANH");
                    cmbQuyen.SelectedIndex = 0;
                    cmbQuyen.Items.Add("USER");
                   
                }

            }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;
                Program.servername = cmbChiNhanh.SelectedValue.ToString();
                if (cmbChiNhanh.SelectedIndex != Program.mChinhanh)
                {
                    Program.mlogin = Program.remotelogin;
                    Program.password = Program.remotepassword;
                }
                else
                {
                    Program.mlogin = Program.mloginDN;
                    Program.password = Program.passwordDN;
                }
                if (Program.KetNoi() == 0)
                    MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
                else
                {
                    this.hOTENNVTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.hOTENNVTableAdapter.Fill(this.dS.HOTENNV);
                   
                }
            }
            catch (NullReferenceException exp) { }

        }

        private void btn_DangKy_Click(object sender, EventArgs e)
        {
            string maNV = cmbHoten.SelectedValue.ToString();
            string taikhoan = txtTaiKhoan.Text.ToString().Trim();
            string matKhau = txtMatKhau.Text.ToString().Trim();
            string quyen = cmbQuyen.Text.ToString();
            if (taikhoan.Equals(""))
            {
                MessageBox.Show("Tên đăng nhập không được để trống !");
                txtTaiKhoan.Focus();
                return;
            }
            if (matKhau.Equals(""))
            {
                MessageBox.Show("Mật khẩu không được để trống !");
                txtMatKhau.Focus();
                return;
            }
            string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[SP_TAOACCOUNT] N'" + taikhoan +"',N'" + matKhau + "',N'"  + maNV + "',N'" + quyen + "' SELECT  'Return Value' = @return_value";
            SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
            if (myReader == null) return;
            myReader.Read();
            int result = myReader.GetInt32(0);
            myReader.Close();
            if (result == 1)
            {
                MessageBox.Show("Login name bị trùng.", "Lỗi thêm tài khoản", MessageBoxButtons.OK);
                txtTaiKhoan.Focus();
                return;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

        }
    }
}
