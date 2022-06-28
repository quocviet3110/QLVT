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
    public partial class frmDangNhap : Form
    {
        private SqlConnection con_publisher = new SqlConnection();
        public frmDangNhap()
        {
            InitializeComponent();
        }
        private void LayDSPM( String cmd)
        {
            DataTable dt = new DataTable();
            if (con_publisher.State == ConnectionState.Closed) con_publisher.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd,con_publisher);
            da.Fill(dt);
            con_publisher.Close();
            Program.bds_dspm.DataSource = dt;
            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";

        }
        private int Kn_CSDLGOC()
        {
            if (con_publisher != null && con_publisher.State == ConnectionState.Open)
                con_publisher.Close();
            try 
            { 
                con_publisher.ConnectionString = Program.connstr_publisher;
                con_publisher.Open();
                return 1;
            } catch(Exception e) {
               
                MessageBox.Show("Lỗi kế nối về cơ sở dữ liệu gốc.\n Bạn xem lại Tên Server của Publisher, và tên CSDL", "Lỗi đăng nhập", MessageBoxButtons.OK);
                 return 0;
                
            }
        }
        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            if (Kn_CSDLGOC() == 0) return;
            LayDSPM("SELECT * FROM Get_Subscribes");
            cmbChiNhanh.SelectedIndex = 1; cmbChiNhanh.SelectedIndex = 0;
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                Program.servername = cmbChiNhanh.SelectedValue.ToString();
            } catch(Exception) { }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();Program.frmChinh.Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if(txtLogin.Text.Trim()==""|| txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoản và mật khẩu không được trống ", "", MessageBoxButtons.OK);
                return;
            }
            Program.mlogin = txtLogin.Text;
            Program.password = txtPass.Text;
            if (Program.KetNoi() == 0)
                return;
            Program.mChinhanh = cmbChiNhanh.SelectedIndex;
            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            string strLenh = "EXEC SP_DANGNHAP '" + Program.mlogin + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();
            Program.username = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login bạn không có quyền truy cập dữ liệu,\n bạn xem lại username và password", "", MessageBoxButtons.OK);
                return;
            }
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.con.Close();
            this.SetVisibleCore(false);
            
            Program.frmChinh.txtMaNV.Text = "Mã nhân viên :" + Program.username;
            Program.frmChinh.txtHOTEN.Text = "Họ tên : " + Program.mHoten;
            Program.frmChinh.txtNHOM.Text = "Nhóm: " + Program.mGroup;

            Program.frmChinh.rbDanhmuc.Visible = true;
            Program.frmChinh.rbBaoCao.Visible = true;
            Program.frmChinh.btnDangNhap.Enabled = false;
            if(Program.mGroup == "CONGTY" || Program.mGroup == "CHINHANH" ){
                Program.frmChinh.btnDangKy.Enabled = true;
            }
         
            Program.frmChinh.btnDangXuat.Enabled = true;
        }
    }
}
