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
    public partial class frmKho : Form
    {
        string macn = "";
        int vitri = 0;
        bool checkSua = false;
        public frmKho()
        {
            InitializeComponent();
        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DSKHO);

        }

        private void frmKho_Load(object sender, EventArgs e)
        {
            DSKHO.EnforceConstraints = false;

            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.DSKHO.Kho);
            // TODO: This line of code loads data into the 'dSKHO.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DSKHO.PhieuNhap);
            // TODO: This line of code loads data into the 'dSKHO.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DSKHO.DatHang);
            // TODO: This line of code loads data into the 'dSKHO.PhieuXuat' table. You can move, or remove it, as needed.
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DSKHO.PhieuXuat);

            macn = ((DataRowView)bdsKho[0])["MACN"].ToString().Trim();
            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
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

            }


        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsKho.Position;
            panelControl2.Enabled = true;
            gcKho.Enabled = true;
            bdsKho.AddNew();
            txtMaCN.Text = macn;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = false;
            txtMaKho.Focus();

        }

        private void btnCapNhat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtMaCN.Enabled = false;
            panelControl2.Enabled = true;
            vitri = bdsKho.Position;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            txtMaKho.Enabled = false;
            checkSua = true;
            txtMaKho.Focus();

        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaKho.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã kho không thể để trống!", "", MessageBoxButtons.OK);
                txtMaKho.Focus();
                return;
            }
            if (txtTen.Text.Trim().Equals(""))
            {
                MessageBox.Show("Tên kho không thể để trống!", "", MessageBoxButtons.OK);
                txtTen.Focus();
                return;
            }
            if (TxtDiaChi.Text.Trim().Equals(""))
            {
                MessageBox.Show("Địa chỉ không thể để trống!", "", MessageBoxButtons.OK);
                TxtDiaChi.Focus();
                return;
            }
            if (checkSua== false)
            {
                string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_KiemTraMaKhoTonTai] " + txtMaKho.Text.Trim() + " SELECT  'Return Value' = @return_value";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã kho bị trùng.", "Lỗi kho nhân viên", MessageBoxButtons.OK);
                    txtMaKho.Focus();
                    return;
                }
            }
            try
            {
                bdsKho.EndEdit();
                bdsKho.ResetCurrentItem();
                this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.khoTableAdapter.Update(this.DSKHO.Kho);
            }
            catch
            {
                MessageBox.Show("Mã kho bị trùng.", "Lỗi sửa kho", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = false;
            gcKho.Enabled = true;

        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsKho.CancelEdit();
            if (btnThem.Enabled == false) bdsKho.Position = vitri;

            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = false;

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String makho = "";
            if (bdsDH.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho này vì đã lập đơn hàng", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsPN.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho này vì đã lập phiếu nhập", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsPX.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho này vì đã lập Phiếu xuất", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa kho này ?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    makho = ((DataRowView)bdsKho[bdsKho.Position])["MAKHO"].ToString();
                    bdsKho.RemoveCurrent();
                    this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.khoTableAdapter.Update(this.DSKHO.Kho);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa kho, bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.khoTableAdapter.Fill(this.DSKHO.Kho);
                    bdsKho.Position = bdsKho.Find("MAKHO", makho);
                    return;
                }
            }
            if (bdsKho.Count == 0) btnXoa.Enabled = false;
        }

  

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.khoTableAdapter.Fill(this.DSKHO.Kho);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Visible = false;
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                        this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.khoTableAdapter.Fill(this.DSKHO.Kho);
                        macn = ((DataRowView)bdsKho[0])["MACN"].ToString().Trim();
                    }
                }
                catch (NullReferenceException exp) { }
            }
        }
    }
}
