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
    public partial class frmVatTu : Form
    {
        int vitri = 0;
        string macn = "";
        bool checkSua = false;
        public frmVatTu()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVT.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DSVT);

        }

        private void VatTu_Load(object sender, EventArgs e)
        {
            DSVT.EnforceConstraints = false;

            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.DSVT.Vattu);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.DSVT.CTDDH);
            // TODO: This line of code loads data into the 'DSVT.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.DSVT.CTPN);
            // TODO: This line of code loads data into the 'DSVT.CTPX' table. You can move, or remove it, as needed.
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.DSVT.CTPX);
            // TODO: This line of code loads data into the 'qLVTDataSet.Vattu' table. You can move, or remove it, as needed.
            if (Program.mGroup == "CONGTY")
            {
              
                btnThem.Enabled = false;
                btnCapNhat.Enabled = false;
                btnGhi.Enabled = false;
                btnXoa.Enabled = false;
                btnPhucHoi.Enabled = false;
                btnReload.Enabled = false;
                panelControl1.Enabled = false;

            }
            else
            {
                
                btnThem.Enabled = true;
                btnCapNhat.Enabled = true;
                btnGhi.Enabled = false;
                btnXoa.Enabled = true;
                btnPhucHoi.Enabled = false;
                btnReload.Enabled = true;
                panelControl1.Enabled = false;
                gcVT.Enabled = true;
            }


        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsVT.Position;
            panelControl1.Enabled = true;
            bdsVT.AddNew();
            txtDVT.Text = "cái";
            txtSLT.Text = "0";
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled =barButtonItem1.Enabled = false ;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            checkSua = false;
            txtMaVT.Focus();

        }

        private void btnCapNhat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsVT.Position;
            panelControl1.Enabled = true;
            gcVT.Enabled = false;
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            txtMaVT.Enabled = false;
            checkSua = true;
            txtMaVT.Focus();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaVT.Text.Trim() == "")
            {
                MessageBox.Show("Mã vật tư không được để trống!");
                txtMaVT.Focus();
            }
             if (txtTenVT.Text.Trim() == "")
            {
                MessageBox.Show("Tên vật tư không được để trống!");
                txtTenVT.Focus();
            }
            if (txtDVT.Text.Trim() == "")
            {
                MessageBox.Show("Đơn vị tính không được để trống!");
                txtDVT.Focus();
            }
            if (checkSua == false)
            {
                string strLenh = "SELECT COUNT(MAVT) FROM dbo.Vattu WHERE MAVT='" + txtMaVT.Text.ToString() + "'";
                SqlDataReader myReader = Program.ExecSqlDataReader(strLenh);
                if (myReader == null) return;
                myReader.Read();
                int result = myReader.GetInt32(0);
                myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Mã kho bị trùng.", "Lỗi kho nhân viên", MessageBoxButtons.OK);
                    txtMaVT.Focus();
                    return;
                }
            }
            try
            {
                bdsVT.EndEdit();
                bdsVT.ResetCurrentItem();
                this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                this.vattuTableAdapter.Update(this.DSVT.Vattu);
            }
            catch
            {
                MessageBox.Show("Mã vật tư bị trùng.", "Lỗi sửa vật tư", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = gcVT.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = panelControl1.Enabled = false;


        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsVT.CancelEdit();
            if (btnThem.Enabled == false) bdsVT.Position = vitri;

            btnThem.Enabled = btnCapNhat.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled= barButtonItem1.Enabled = true;
            btnGhi.Enabled = btnPhucHoi.Enabled = false;

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String mavt = "";
            if (bdsCTDDH.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư vì đã lập đơn đặt hàng", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsCTPN.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư vì đã lập phiếu nhập", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsCTPX.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư vì đã lập phiếu xuất", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa vật tư!", "",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    mavt = ((DataRowView)bdsVT[bdsVT.Position])["MAVT"].ToString().Trim();
                    bdsVT.RemoveCurrent();
                    this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vattuTableAdapter.Update(this.DSVT.Vattu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa nhân viên. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.vattuTableAdapter.Fill(this.DSVT.Vattu);
                    bdsVT.Position = bdsVT.Find("MAVT", mavt);
                    return;
                }

            }
            if (bdsVT.Count == 0) btnXoa.Enabled = false;

        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.vattuTableAdapter.Fill(this.DSVT.Vattu);

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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Xtra_DSVT dsvt = new Xtra_DSVT();
            ReportPrintTool print = new ReportPrintTool(dsvt);
            print.ShowPreviewDialog();
        }

       
    }
}
