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
    public partial class frmHoatDongCuaNhanVien : Form
    {
        public static String maNV;
        public static int manv;
        public static String hoTen;
        public static String ngaySinh;
        public static String diaChi;
        public static int luong;
        public static String macn;
        public frmHoatDongCuaNhanVien()
        {
            InitializeComponent();
        }

        private void frm_HoatDongCuaNhanVien_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;

            this.hOTENNVTableAdapter.Connection.ConnectionString = Program.connstr;
            this.hOTENNVTableAdapter.Fill(this.dS.HOTENNV);
            this.sP_ThongTinNhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_ThongTinNhanVienTableAdapter.Fill(this.dS.SP_ThongTinNhanVien, manv);
            cmbHoTen.DataSource = hOTENNVBindingSource;
            cmbHoTen.DisplayMember = "HOTEN";
            cmbHoTen.ValueMember = "MANV";
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
                else
                {
                    this.hOTENNVTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.hOTENNVTableAdapter.Fill(this.dS.HOTENNV);
                    this.sP_ThongTinNhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sP_ThongTinNhanVienTableAdapter.Fill(this.dS.SP_ThongTinNhanVien, manv);
                }
            }
            catch (NullReferenceException exp) { }
        }

   

        private void hOTENNVComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String maNV = cmbHoTen.SelectedValue.ToString();
            String query = "EXEC SP_ThongTinNhanVien "+maNV ;
            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
            dataReader.Read();

            // Gán giá trị cho các label bên report
            manv = int.Parse(dataReader.GetValue(0).ToString());
            hoTen = dataReader.GetValue(1).ToString();
            ngaySinh = dataReader.GetDateTime(2).ToString("dd/MM/yyyy");
            diaChi = dataReader.GetValue(3).ToString();
            luong = int.Parse(dataReader.GetValue(4).ToString());
            macn = dataReader.GetValue(5).ToString();
            dataReader.Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (cmbLoai.Text.Equals(""))
            {
                MessageBox.Show("Loại không được để trống!", "", MessageBoxButtons.OK);
                cmbLoai.Focus();
                return;
            }
            if (dateTo.Text.Length == 0)
            {
                MessageBox.Show("Ngày bắt đầu không được để trống", "", MessageBoxButtons.OK);
                dateTo.Focus();
                return;
            }
            if (dateFrom.Text.Length == 0)
            {
                MessageBox.Show("Ngày kết thúc không được để trống", "", MessageBoxButtons.OK);
                dateFrom.Focus();
                return;
            }
            if (dateFrom.DateTime > dateTo.DateTime)
            {
                MessageBox.Show("Ngày kết thúc phải lớn hơn ngày bắt đầu!", "", MessageBoxButtons.OK);
                dateFrom.Focus();
                return;
            }
            Xtra_HoatDongCuaNhanVien rpt = new Xtra_HoatDongCuaNhanVien(manv, dateFrom.DateTime, dateTo.DateTime, cmbLoai.Text.Substring(0,1));
            rpt.xrtMANV.Text = manv.ToString().Trim();
            rpt.xrtHOTEN.Text = hoTen;
            rpt.xrtNGAYSINH.Text = ngaySinh;
            rpt.xrtDIACHI.Text = diaChi;
            rpt.xrtLUONG.Text = luong.ToString().Trim();
            rpt.xrtMACN.Text = macn;
            /*rpt.xrTitle.Text = "BẢNG KÊ CHỨNG TỪ PHIẾU ";
            rpt.xrTitle.Text += (type == "N") ? "NHẬP" : "XUẤT";*/

            ReportPrintTool print = new ReportPrintTool(rpt);
            rpt.ShowPreviewDialog();
        }
    }
}
