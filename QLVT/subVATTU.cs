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
    public partial class subVATTU : Form
    {
        public subVATTU()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVT.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSDATHANG);

        }

        private void subVATTU_Load(object sender, EventArgs e)
        {
            dSDATHANG.EnforceConstraints = false;
            vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dSDATHANG.Vattu);

        }

        private void BtnKho_Click(object sender, EventArgs e)
        {
            DataRowView row = bdsVT.Current as DataRowView;
            if(row != null)
            {
                String maVt = ((DataRowView)bdsVT.Current)["MAVT"].ToString();
                if (Program.checkVT.Equals("DDH")) { Program.frmCTDH.txtMaVT.Text = maVt; }
                else if (Program.checkVT.Equals("PX")) { Program.frmCTPX.txtMaVT.Text = maVt; }
                this.Close();
            }
           
        }

        private void subVATTU_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.frmChinh.Enabled = true;
        }

      
    }
}
