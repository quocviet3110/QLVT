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
    public partial class frmPhieuNhap : Form
    {
        string macn = "";
        string mapn = "";
        string masoddh = "";
        int vitri = 0;
        bool checkSua = false;
        public frmPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPN.EndEdit();

        }

        private void frmPhieuNhap_Load(object sender, EventArgs e)
        {
         

            dSDATHANG.EnforceConstraints = false;
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dSDATHANG.PhieuNhap);

            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.dSDATHANG.CTPN);

            mapn = ((DataRowView)bdsPN[0])["MAPN"].ToString().Trim();
            cTPNBindingSource.Filter = "MAPN ='" + mapn + "'";
            gcCTPN.DataSource = cTPNBindingSource;

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
                gcPhieuNhap.Enabled = true;
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
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Fill(this.dSDATHANG.PhieuNhap);
            
                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                   this.cTPNTableAdapter.Fill(this.dSDATHANG.CTPN);


                    mapn = ((DataRowView)bdsPN[0])["MAPN"].ToString().Trim();
                    cTPNBindingSource.Filter = "MAPN ='" + mapn + "'";
                    gcCTPN.DataSource = cTPNBindingSource;
                }
            }
            catch (NullReferenceException exp) { }

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsPN.Position;
            panelControl2.Enabled = true;
            gcPhieuNhap.Enabled = false;
            bdsPN.AddNew();
            txtNgay.DateTime = DateTime.Now;
            txtNgay.Enabled = false;

            txtMaNV.Text = Program.username;
            txtMaNV.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = false;
            txtMaPN.Focus();


        }

        private void btnCapNhat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaNV.Text != Program.username)
            {
                MessageBox.Show("Bạn không có quyền sửa đơn hàng của nhân viên khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            vitri = bdsPN.Position;
            panelControl2.Enabled = true;
            gcPhieuNhap.Enabled = false;
            txtNgay.DateTime = DateTime.Now;
            txtNgay.Enabled = false;
            txtMaNV.Text = Program.username;
            txtMaNV.Enabled = false;
            txtMaPN.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = true;

        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaPN.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã số đơn đặt hàng không thể để trống!", "", MessageBoxButtons.OK);
                txtMaNV.Focus();
                return;
            }
            if (txtMaKho.Text.Trim().Equals(""))
            {
                MessageBox.Show("Nhà cung cấp không thể để trống!", "", MessageBoxButtons.OK);
                return;
            }
            if (txtMaKho.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã kho không thể để trống!", "", MessageBoxButtons.OK);
                txtMaKho.Focus();
                return;
            }
            if (checkSua == false)
            {
                string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_KiemTraMaPhieuNhap] " + txtMaPN.Text.Trim() + " SELECT  'Return Value' = @return_value";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã đơn đặt hàng bị trùng.", "Lỗi thêm đơn đặt hàng", MessageBoxButtons.OK);
                    txtMaPN.Focus();
                    return;
                }
            }
            try
            {
                bdsPN.EndEdit();
                bdsPN.ResetCurrentItem();
                if (dSDATHANG.HasChanges())
                {

                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dSDATHANG.PhieuNhap);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã đơn phiếu nhập bị trùng.", "Lỗi nhân viên", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcPhieuNhap.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = panelControl2.Enabled = false;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            bdsPN.CancelEdit();
            if (btnThem.Enabled == false) bdsPN.Position = vitri;

            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcPhieuNhap.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String mapn = "";
            if (txtMaNV.Text != Program.username)
            {
                MessageBox.Show("Bạn không có quyền xóa đơn hàng của nhân viên khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (cTPNBindingSource.Count > 0)
            {
                MessageBox.Show("Không thể xóa đơn phiếu nhập vì đã lập CTPN", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn muốn xóa phiếu nhập!", "",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    mapn = ((DataRowView)bdsPN[bdsPN.Position])["MAPN"].ToString().Trim();
                    bdsPN.RemoveCurrent();
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dSDATHANG.PhieuNhap);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa đơn đặt hàng. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.phieuNhapTableAdapter.Fill(this.dSDATHANG.PhieuNhap);
                    bdsPN.Position = bdsPN.Find("MAPN", mapn);
                    return;
                }

            }
            if (bdsPN.Count == 0) btnXoa.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.phieuNhapTableAdapter.Fill(this.dSDATHANG.PhieuNhap);

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

   

        private void txtMaPN_TextChanged(object sender, EventArgs e)
        {
           this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
           this.cTPNTableAdapter.Fill(this.dSDATHANG.CTPN);
           mapn = txtMaPN.Text.ToString().Trim();
           cTPNBindingSource.Filter = "MAPN ='" + mapn + "'";
           gcCTPN.DataSource = cTPNBindingSource;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnFindDDH_Click(object sender, EventArgs e)
        {
            Program.subDDH = new subDonDatHang();
            Program.subDDH.Show();
            Program.checKMK = "PN";
            Program.frmChinh.Enabled = false;
        }

        private void btnThemCTPN_Click(object sender, EventArgs e)
        {
            Program.frmCTPN = new frmCTPN();
            Program.mapn = txtMaPN.Text.ToString().Trim();
            Program.maCTDDH = txtMasoDDH.Text.ToString().Trim();
            Program.frmCTPN.Show();
            if (dSDATHANG.HasChanges())
            {

                this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPNTableAdapter.Update(this.dSDATHANG.CTPN);
            }
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            if (txtMaNV.Text.ToString().Equals(Program.username))
            {
                btnThemCTPN.Enabled  = true;
            }
            else
            {
                btnThemCTPN.Enabled = false;
            }
        }
    }
}
