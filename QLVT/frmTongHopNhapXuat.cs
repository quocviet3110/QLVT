using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmTongHopNhapXuat : Form
    {
        public frmTongHopNhapXuat()
        {

            InitializeComponent();
            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            //Phân quyền
            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = true;

            }
            else
            {
                cmbChiNhanh.Enabled = false;
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
            }
            catch (NullReferenceException exp) { }

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (dateBD.Text.Length == 0)
            {
                MessageBox.Show("Ngày bắt đầu không được để trống", "", MessageBoxButtons.OK);
                dateBD.Focus();
                return;
            }
            if (dateKT.Text.Length == 0)
            {
                MessageBox.Show("Ngày kết thúc không được để trống", "", MessageBoxButtons.OK);
                dateKT.Focus();
                return;
            }
            if (dateBD.DateTime > dateKT.DateTime)
            {
                MessageBox.Show("Vui lòng chọn thời gian kết thúc lớn hơn ngày bắt đầu!!", "", MessageBoxButtons.OK);
                return;
            }
            Xtra_TongHopNhapXuat rpt = new Xtra_TongHopNhapXuat(dateBD.DateTime, dateKT.DateTime);
            ReportPrintTool print = new ReportPrintTool(rpt);
            rpt.ShowPreviewDialog();
        }

      
    }
}
