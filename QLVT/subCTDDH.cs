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
    public partial class subCTDDH : Form
    {
        public subCTDDH()
        {
            InitializeComponent();
        }

        private void cTDDHBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsCTDDH.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSDATHANG);

        }

        private void subCTDDH_Load(object sender, EventArgs e)
        {
            dSDATHANG.EnforceConstraints = false;

            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dSDATHANG.CTDDH);
            String maCTPN = Program.maCTDDH.ToString().Trim();
            bdsCTDDH.Filter = "MasoDDH ='" + maCTPN + "'";
            gcCTDDH.DataSource = bdsCTDDH;

        }

      

        private void subCTDDH_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.frmChinh.Enabled = true;
        }

        private void BtnCTDDH_Click(object sender, EventArgs e)
        {
            DataRowView row = bdsCTDDH.Current as DataRowView;
            if(row != null)
            {
                String maVt = ((DataRowView)bdsCTDDH.Current)["MAVT"].ToString();
                String sl = ((DataRowView)bdsCTDDH.Current)["SOLUONG"].ToString();
                String dg = ((DataRowView)bdsCTDDH.Current)["DONGIA"].ToString();
                Program.frmCTPN.txtMaVT.Text = maVt;
                Program.frmCTPN.txtSL.Text = sl;
                Program.frmCTPN.txtDG.Text = dg;

            }
            
            this.Close();
        }
    }
}
