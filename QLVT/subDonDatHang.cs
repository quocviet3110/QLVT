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
    public partial class subDonDatHang : Form
    {
        public subDonDatHang()
        {
            InitializeComponent();
        }

        private void subDonDatHang_Load(object sender, EventArgs e)
        {
            dSDATHANG.EnforceConstraints = false;
            this.masoDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.masoDDHTableAdapter.Fill(this.dSDATHANG.MasoDDH);

        }

        private void BtnDDH_Click(object sender, EventArgs e)
        {
            String masoddh = ((DataRowView)bdsDDH.Current)["MasoDDH"].ToString();
            String maKho = ((DataRowView)bdsDDH.Current)["MAKHO"].ToString();

            if (Program.checKMK.Equals("PN"))
            {
                Program.frmPN.txtMaKho.Text = maKho;
                Program.frmPN.txtMasoDDH.Text = masoddh;
            }
           
            this.Close();
        }

        private void subDonDatHang_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.frmChinh.Enabled = true;
        }
    }
}
