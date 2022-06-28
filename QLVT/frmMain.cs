using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

       public Form checkExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;

        }

        private void btnDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmDangNhap));
            if (frm != null) frm.Activate();
            else
            {
                frmDangNhap f = new frmDangNhap();
                f.Show();
            }    
        }

        private void btnNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                frmNhanVien f = new frmNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmKho));
            if (frm != null) frm.Activate();
            else
            {
                frmKho f = new frmKho();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmVatTu));
            if (frm != null) frm.Activate();
            else
            {
                frmVatTu f = new frmVatTu();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmDatHang));
            if (frm != null) frm.Activate();
            else
            {
                Program.frmDH = new frmDatHang();
                Program.frmDH.MdiParent = this;
                Program.frmDH.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmPhieuNhap));
            if (frm != null) frm.Activate();
            else
            {
                Program.frmPN = new frmPhieuNhap();
                Program.frmPN.MdiParent = this;
                Program.frmPN.Show();
            }

        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmBangKeChiTiet));
            if (frm != null) frm.Activate();
            else
            {
                frmBangKeChiTiet f = new frmBangKeChiTiet();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmTongHopNhapXuat));
            if (frm != null) frm.Activate();
            else
            {
                frmTongHopNhapXuat f = new frmTongHopNhapXuat();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmPhieuXuat));
            if (frm != null) frm.Activate();
            else
            {
                Program.frmPX = new frmPhieuXuat();
                Program.frmPX.MdiParent = this;
                Program.frmPX.Show();
            }

        }

        private void btnDangXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();

        }

        private void btnDangKy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmDangKyTaiKhoan));
            if (frm != null) frm.Activate();
            else
            {
                Program.frmDangKy = new frmDangKyTaiKhoan();
                Program.frmDangKy.Show();
            }
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.checkExists(typeof(frmHoatDongCuaNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                frmHoatDongCuaNhanVien f = new frmHoatDongCuaNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}
