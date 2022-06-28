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
    public partial class subKHO : Form
    {
        public subKHO()
        {
            InitializeComponent();
        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dSKHO);

        }

        private void subKho_Load(object sender, EventArgs e)
        {
            dSKHO.EnforceConstraints = false;

            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.dSKHO.Kho);

        }

  

        private void BtnKho_Click_1(object sender, EventArgs e)
        {
            DataRowView row = bdsKho.Current as DataRowView;
            if(row != null) { 
            String maKho = ((DataRowView)bdsKho.Current)["MAKHO"].ToString();
                if (Program.checKMK.Equals("DH"))
                {
                    Program.frmDH.txtMaKho.Text = maKho;
                }
                else if (Program.checKMK.Equals("PX"))
                {
                    Program.frmPX.txtMaKho.Text = maKho;
                }
            }
           
            
            this.Close();
        }

        private void subKHO_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.frmChinh.Enabled = true;
        }

      
    }
}
