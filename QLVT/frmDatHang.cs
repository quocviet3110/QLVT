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
    public partial class frmDatHang : Form
    {
        string macn = "";
        string masoddh = "";
        int vitri = 0;
        bool checkSua = false;
        
        public frmDatHang()
        {
            InitializeComponent();
 
        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDH.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSDATHANG);

        }

        private void DatHang_Load(object sender, EventArgs e)
        {
            dSDATHANG.EnforceConstraints = false;
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.dSDATHANG.DatHang);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dSDATHANG.CTDDH);
            DataRowView row = bdsDH.Current as DataRowView;
            if(row != null)
            {
                masoddh = ((DataRowView)bdsDH[0])["MasoDDH"].ToString().Trim();
                bdsCTDH.Filter = "MasoDDH ='" + masoddh + "'";
                gcCTDH.DataSource = bdsCTDH;

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
                gcDatHang.Enabled = true;
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
                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Fill(this.dSDATHANG.DatHang);
                    //macn = ((DataRowView)bdsDH[0])["MasoDDH"].ToString().Trim();
                }
            }
            catch (NullReferenceException exp) { }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsDH.Position;
            panelControl2.Enabled = true;
            bdsDH.AddNew();
            txtNgay.DateTime = DateTime.Now;
            txtNgay.Enabled = false;

            txtMaNV.Text = Program.username;           
            txtMaNV.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            btnTCTDDH.Enabled = false;
            checkSua = false;
            txtMaDH.Focus();

        }


        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaDH.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã số đơn đặt hàng không thể để trống!", "", MessageBoxButtons.OK);
                txtMaNV.Focus();
                return;
            }
            if (txtNCC.Text.Trim().Equals(""))
            {
                MessageBox.Show("Nhà cung cấp không thể để trống!", "", MessageBoxButtons.OK);
                txtNCC.Focus();
                return;
            }
            if (txtMaKho.Text.Trim().Equals(""))
            {
                MessageBox.Show("Mã kho không thể để trống!", "", MessageBoxButtons.OK);
                txtMaKho.Focus();
                return;
            }
            if(checkSua == false)
            {
                string strLenh = "DECLARE @return_value int EXEC @return_value = [dbo].[sp_KiemTraMaSoDonDatHang] " + txtMaDH.Text.Trim() + " SELECT  'Return Value' = @return_value";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã đơn đặt hàng bị trùng.", "Lỗi thêm đơn đặt hàng", MessageBoxButtons.OK);
                    txtMaDH.Focus();
                    return;
                }
            }
            try
            {
                bdsDH.EndEdit();
                bdsDH.ResetCurrentItem();
                if (dSDATHANG.HasChanges())
                {

                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Update(this.dSDATHANG.DatHang);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã đơn đặt hàng bị trùng.", "Lỗi nhân viên", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcDatHang.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = panelControl2.Enabled = false;

        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsDH.CancelEdit();
            if (btnThem.Enabled == false) bdsDH.Position = vitri;

            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcDatHang.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl2.Enabled = false;

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String madh = "";
            if (txtMaNV.Text != Program.username)
            {
                MessageBox.Show("Bạn không có quyền xóa đơn hàng của nhân viên khác.", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (bdsCTDH.Count > 0)
            {
                MessageBox.Show("Không thể xóa đơn đặt hàng vì đã lập CTDDH", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn muốn xóa đơn đặt hàng", "",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    madh = ((DataRowView)bdsDH[bdsDH.Position])["MasoDDH"].ToString().Trim();
                    bdsDH.RemoveCurrent();
                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Update(this.dSDATHANG.DatHang);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa đơn đặt hàng. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.datHangTableAdapter.Fill(this.dSDATHANG.DatHang);
                    bdsDH.Position = bdsDH.Find("MasoDDH", madh);
                    return;
                }

            }
            if (bdsDH.Count == 0) btnXoa.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.datHangTableAdapter.Fill(this.dSDATHANG.DatHang);

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

        private void button1_Click(object sender, EventArgs e)
        { 
            Program.subKho = new subKHO();
            Program.checKMK = "DH";
            Program.subKho.Show();
            Program.frmChinh.Enabled = false;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Xtra_DDHChuaCoPN dsdh = new Xtra_DDHChuaCoPN();
            ReportPrintTool print = new ReportPrintTool(dsdh);
            dsdh.labelDH.Text = "DANH SÁCH ĐƠn ĐẶT HÀNG CHƯA CÓ PHIẾU NHẬP CỦA " + cmbChiNhanh.Text.ToUpper();
            print.ShowPreviewDialog();

        }

        private void txtMaDH_TextChanged(object sender, EventArgs e)
        {
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dSDATHANG.CTDDH);
            masoddh = txtMaDH.Text.ToString().Trim();
            bdsCTDH.Filter = "MasoDDH ='" + masoddh + "'";
            gcCTDH.DataSource = bdsCTDH;
        }

        private void btnTCTDDH_Click(object sender, EventArgs e)
        {
            Program.frmCTDH = new frmCTDDH();
            Program.maddh = txtMaDH.Text.ToString().Trim();
            Program.frmCTDH.Show();
            if (dSDATHANG.HasChanges())
            { 
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTDDHTableAdapter.Update(this.dSDATHANG.CTDDH);
            }
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            if (txtMaNV.Text.ToString().Equals(Program.username))
            {
                btnTCTDDH.Enabled = true;
            }
            else
            {
                btnTCTDDH.Enabled  = false;
            }
        }
    }
}
