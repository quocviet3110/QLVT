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
    public partial class frmPhieuXuat : Form
    {
        string macn = "";
        string mapx = "";
        int vitri = 0;
        bool checkSua = false;
        public frmPhieuXuat()
        {
            InitializeComponent();
        }

        private void phieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPX.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSPHIEUXUAT);

        }

        private void frmPHIEUXUAT_Load(object sender, EventArgs e)
        {
            dSPHIEUXUAT.EnforceConstraints = false;
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.dSPHIEUXUAT.PhieuXuat);

            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.dSPHIEUXUAT.CTPX);

            DataRowView row = bdsPX.Current as DataRowView;
            if (row != null)
            {
                mapx = ((DataRowView)bdsPX[0])["MAPX"].ToString().Trim();
                bdsCTPX.Filter = "MAPX ='" + mapx + "'";
                gcCTPX.DataSource = bdsCTPX;
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
                panel1.Enabled = false;

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
                panel1.Enabled = false;
                gcCTPX.Enabled = true;
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
                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Fill(this.dSPHIEUXUAT.PhieuXuat);

                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPXTableAdapter.Fill(this.dSPHIEUXUAT.CTPX);

                    DataRowView row = bdsPX.Current as DataRowView;
                    if (row != null)
                    {
                        mapx = ((DataRowView)bdsPX[0])["MAPX"].ToString().Trim();
                        bdsCTPX.Filter = "MAPX ='" + mapx + "'";
                        gcCTPX.DataSource = bdsCTPX;
                    }
                }
            }
            catch (NullReferenceException exp) { }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsPX.Position;
            bdsPX.AddNew();
            panel1.Enabled = true;
            txtDate.DateTime = DateTime.Now;
            txtDate.Enabled = false;
            
            txtMaNV.Text = Program.username;
            txtMaNV.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = false;
            txtMAPX.Focus();

        }

        private void btnCapNhat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaNV.Text != Program.username)
            {
                MessageBox.Show("Bạn không có quyền sửa đơn hàng của nhân viên khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            vitri = bdsPX.Position;
            panel2.Enabled = true;
            gcPhieuXuat.Enabled = false;
     
            txtMaNV.Text = Program.username;
            txtMaNV.Enabled = false;
            txtMAPX.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = true;


        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMAPX.Text.Trim().Equals(""))
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
                string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_KiemTraMaPhieuXuat] " + txtMAPX.Text.Trim() + " SELECT  'Return Value' = @return_value";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã đơn đặt hàng bị trùng.", "Lỗi thêm đơn đặt hàng", MessageBoxButtons.OK);
                    txtMAPX.Focus();
                    return;
                }
            }
            try
            {
                bdsPX.EndEdit();
                bdsPX.ResetCurrentItem();
                if (dSPHIEUXUAT.HasChanges())
                {

                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.dSPHIEUXUAT.PhieuXuat);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã đơn đặt hàng bị trùng.", "Lỗi nhân viên", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcPhieuXuat.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panel1.Enabled = false;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsPX.CancelEdit();
            if (btnThem.Enabled == false) bdsPX.Position = vitri;

            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcPhieuXuat.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panel1.Enabled = false;

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String mapx = "";
            if (txtMaNV.Text != Program.username)
            {
                MessageBox.Show("Bạn không có quyền xóa đơn hàng của nhân viên khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (bdsCTPX.Count > 0)
            {
                MessageBox.Show("Không thể xóa đơn phiếu xuất vì đã lập CTPX", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn muốn xóa phiếu xuất!", "",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    mapx = ((DataRowView)bdsPX[bdsPX.Position])["MAPX"].ToString().Trim();
                    bdsPX.RemoveCurrent();
                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.dSPHIEUXUAT.PhieuXuat);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa đơn đặt hàng. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.phieuXuatTableAdapter.Fill(this.dSPHIEUXUAT.PhieuXuat);
                    bdsPX.Position = bdsPX.Find("MAPX", mapx);
                    return;
                }

            }
            if (bdsPX.Count == 0) btnXoa.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.phieuXuatTableAdapter.Fill(this.dSPHIEUXUAT.PhieuXuat);

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

        private void btnThemCTPN_Click(object sender, EventArgs e)
        {
            Program.frmCTPX = new frmCTPX();
            Program.mapx = txtMAPX.Text.ToString().Trim();
            Program.frmCTPX.Show();
            if (dSPHIEUXUAT.HasChanges())
            {

                this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPXTableAdapter.Update(this.dSPHIEUXUAT.CTPX);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.subKho = new subKHO();
            Program.checKMK = "PX";
            Program.subKho.Show();
            Program.frmChinh.Enabled = false;
        }

        private void txtMAPX_TextChanged(object sender, EventArgs e)
        {
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.dSPHIEUXUAT.CTPX);

            mapx = txtMAPX.Text.ToString().Trim();
            bdsCTPX.Filter = "MAPX ='" + mapx + "'";
            gcCTPX.DataSource = bdsCTPX;
           
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            if (txtMaNV.Text.ToString().Equals(Program.username))
            {
                btnThemCTPX.Enabled  = true;
            }
            else
            {
                btnThemCTPX.Enabled  = false;
            }
        }
    }
}
