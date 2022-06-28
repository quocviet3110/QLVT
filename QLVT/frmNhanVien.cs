using DevExpress.XtraReports.UI;
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
    public partial class frmNhanVien : Form
    {
        int vitri = 0;
        string macn = "";
        bool checkSua = false;
      

        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS);

        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
     
            DS.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DS.NhanVien);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DS.PhieuXuat);

            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DS.DatHang);
            // TODO: This line of code loads data into the 'DS.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DS.PhieuNhap);
            // TODO: This line of code loads data into the 'DS.PhieuXuat' table. You can move, or remove it, as needed.
            

            DataRowView row = bdsNhanVien.Current as DataRowView;
            if(row != null)
            {
                macn = ((DataRowView)bdsNhanVien[0])["MACN"].ToString().Trim();
            }
            

            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            //Phân quyền
            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = true;
                btnThem.Enabled = false;
                btnCapNhat.Enabled = false;
                btnGhi.Enabled = false;
                btnXoa.Enabled = false;
                btnPhucHoi.Enabled = false;
                btnReload.Enabled = false;
                panelControl2.Enabled = false;

            } 
            else
            {
                cmbChiNhanh.Enabled = false;
                btnThem.Enabled = true;
                btnCapNhat.Enabled = true;
                btnGhi.Enabled = false;
                btnXoa.Enabled = true;
                btnPhucHoi.Enabled = false;
                btnReload.Enabled = true;
                panelControl2.Enabled = false;
                gcNhanVien.Enabled = true;
            }
            if (Program.mGroup == "USER")
            {
                txtTTX.Enabled = false;
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsNhanVien.Position;
            panelControl2.Enabled = true;
            gcNhanVien.Enabled = false;
            bdsNhanVien.AddNew();
            txtMaCN.Text = macn;
            txtNgaySinh.EditValue = "";
            txtTTX.Text = "0";
            btnThem.Enabled = btnCapNhat.Enabled  = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = btnInDSNV.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = false;
            txtMaNV.Focus();
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsNhanVien.CancelEdit();
            if (btnThem.Enabled == false) bdsNhanVien.Position = vitri;
            
            btnThem.Enabled = btnCapNhat.Enabled  = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcNhanVien.Enabled = btnInDSNV.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled= panelControl2.Enabled = false;

        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.nhanVienTableAdapter.Fill(this.DS.NhanVien);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnCapNhat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtTTX.Text.Trim().Equals("1"))
            {
                MessageBox.Show("Nhân viên đã bị xóa! Không được sửa.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            txtTTX.Enabled = false;
            txtMaCN.Enabled = false;
            panelControl2.Enabled = true;
            vitri = bdsNhanVien.Position;        
            btnThem.Enabled = btnCapNhat.Enabled  = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            txtMaNV.Enabled = false;
            checkSua = true;
            txtHo.Focus();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaNV.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã nhân viên không thể để trống!", "", MessageBoxButtons.OK);
                txtMaNV.Focus();
                return;
            }
            if (txtHo.Text.Trim().Equals(""))
            {
                MessageBox.Show("Họ nhân viên không thể để trống!", "", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim().Equals(""))
            {
                MessageBox.Show("Tên nhân viên không thể để trống!", "", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtNgaySinh.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ngày sinh không thể để trống!", "", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (float.Parse(txtLuong.Text.Trim()) < 4000000)
            {
                MessageBox.Show("Lương phải lớn hơn hoặc bằng 4 triệu đồng!", "", MessageBoxButtons.OK);
                txtLuong.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim().Equals(""))
            {
                MessageBox.Show("Địa chỉ không thể để trống!", "", MessageBoxButtons.OK);
                txtDiaChi.Focus();
                return;
            }
           
            if (checkSua == false)
            {
                string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_KiemTraMaNVTonTai] " + txtMaNV.Text.Trim() + " SELECT  'Return Value' = @return_value";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã nhân viên bị trùng.", "Lỗi thêm nhân viên", MessageBoxButtons.OK);
                    txtMaNV.Focus();
                    return;
                }
            }
            try
            {
                bdsNhanVien.EndEdit();
                bdsNhanVien.ResetCurrentItem();
                if (DS.HasChanges())
                {

                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.DS.NhanVien);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã nhân viên bị trùng.", "Lỗi nhân viên", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled=gcNhanVien.Enabled = btnInDSNV.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = panelControl2.Enabled =  false;


        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Visible = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Int32 manv = 0;
            if (datHangBindingSource.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập đơn hàng", "", MessageBoxButtons.OK);
                return;
            }
            if (phieuNhapBindingSource.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu nhập", "", MessageBoxButtons.OK);
                return;
            }
            if (phieuNhapBindingSource.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập Phiếu xuất", "", MessageBoxButtons.OK);
                return;
            }
            if (Program.frmChinh.txtMaNV.Text == txtMaNV.Text)
            {
                MessageBox.Show("Không thể xóa chính mình!", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa nhân viên này ??", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    manv = int.Parse(((DataRowView)bdsNhanVien[bdsNhanVien.Position])["MANV"].ToString());
                    bdsNhanVien.RemoveCurrent();
                    String query = "EXEC [dbo].[SP_XoaTaiKhoan] @MANV = N'" + manv + "'";
                    Program.ExecSqlNonQuery(query);
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.DS.NhanVien);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa nhân viên, bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Fill(this.DS.NhanVien);
                    bdsNhanVien.Position = bdsNhanVien.Find("MANV", manv);
                    return;
                }
            }
            if (bdsNhanVien.Count == 0) btnXoa.Enabled = false;
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
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Fill(this.DS.NhanVien);
                    macn = ((DataRowView)bdsNhanVien[0])["MACN"].ToString().Trim();
                }
            }
            catch (NullReferenceException exp) { }
        }

        private void btnInDSNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Xtra_DSNV dsnv = new Xtra_DSNV();
            dsnv.lbTT.Text = "DANH SÁCH NHÂN VIÊN CỦA " + cmbChiNhanh.Text.ToUpper();
            ReportPrintTool print = new ReportPrintTool(dsnv);
            print.ShowPreviewDialog();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtTTX.Text.Trim().Equals("1"))
            {
                MessageBox.Show("Nhân viên đã bị xóa! Không được sửa.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (macn.Equals("CN1"))
            {
                string strLenh = " EXEC [dbo].[SP_CHUYENCHINHANH] "+int.Parse(txtMaNV.Text.ToString())+", 'CN2"+"'";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
            }
            else
            {
                string strLenh = " EXEC [dbo].[SP_CHUYENCHINHANH] " + int.Parse(txtMaNV.Text.ToString()) + ", 'CN1" + "'";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
            }

            MessageBox.Show("Chuyển chi nhánh cho nhân viên thành công", "", MessageBoxButtons.OK);
            if (DS.HasChanges())
            {
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Update(this.DS.NhanVien);
            }
        }
    }
}
