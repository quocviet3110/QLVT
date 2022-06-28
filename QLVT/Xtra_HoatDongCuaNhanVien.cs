using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT
{
    public partial class Xtra_HoatDongCuaNhanVien : DevExpress.XtraReports.UI.XtraReport
    {
        public Xtra_HoatDongCuaNhanVien(int manv,DateTime bd,DateTime kt,string loai)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = manv;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = bd;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = kt;
            this.sqlDataSource1.Queries[0].Parameters[3].Value = loai;
            this.sqlDataSource1.Fill();
        }

        
    }
}
