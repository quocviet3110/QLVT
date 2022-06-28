using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QLVT
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static SqlConnection con = new SqlConnection();
        public static String connstr;
        public static String connstr_publisher= "Data Source= DESKTOP-ENQCQOC;Initial Catalog=QLVT;Integrated Security=True";

        public static SqlDataReader myReader;
        public static String servername ="";
        public static String username ="" ;
        public static String mlogin = "";
        public static String password = "";

        public static String database = "QLVT";
        public static String remotelogin = "HTKN";
        public static String remotepassword = "123456";
        public static String mloginDN = "";
        public static String passwordDN = "";
        public static String mGroup = "";
        public static String mHoten = "";
        public static int mChinhanh = 1;


        public static BindingSource bds_dspm = new BindingSource();
        public static frmMain frmChinh;
        public static subKHO subKho;
        public static subDonDatHang subDDH;
        public static subCTDDH subCTDDH;

        public static frmDangNhap frmDangNhap;
        public static frmDangKyTaiKhoan frmDangKy;
        public static frmDatHang frmDH;
        public static frmPhieuNhap frmPN;
        public static subVATTU subVattu;
        public static frmCTDDH frmCTDH;
        public static frmCTPN frmCTPN;
        public static frmCTPX frmCTPX;
        public static frmPhieuXuat frmPX;

        public static string checkVT = "";
        public static string checKMK = "";
        //Giữ mã số các đơn hàng
        public static string maddh = "";
        public static string mapn = "";
        public static string maCTDDH = "";
        public static string mapx = "";

        public static int KetNoi()
        {
            if (Program.con != null && Program.con.State == System.Data.ConnectionState.Open)
                Program.con.Close();
            try
            {
                Program.connstr = "Data Source=" + Program.servername + ";Initial Catalog=" + Program.database +
                    ";User ID=" + Program.mlogin + ";Password=" + Program.password;
                Program.con.ConnectionString = Program.connstr;
                Program.con.Open();
                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }
        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myReader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.con);
            sqlcmd.CommandType = CommandType.Text;
            if (Program.con.State == ConnectionState.Closed) Program.con.Open();
            try
            {
                myReader = sqlcmd.ExecuteReader();
                return myReader;
            }
            catch (SqlException e)
            {
                Program.con.Close();
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public static DataTable ExecSqlDataTable(String cmd)
        {
            DataTable dt = new DataTable();
            if (Program.con.State == ConnectionState.Closed) Program.con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, con);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public static  int ExecSqlNonQuery(String strLenh)
        {
            SqlCommand sqlcmd = new SqlCommand(strLenh,con);
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandTimeout = 600;
            if (con.State == ConnectionState.Closed) ; con.Open();
            try
            {
                sqlcmd.ExecuteNonQuery();con.Close();
                return 0;

            }
            catch(SqlException ex)
            {
                if (ex.Message.Contains("Error converting data type varchar to int"))
                    MessageBox.Show("Bạn format cell lại cột \"Ngày thi\" qua kiểu Number hoặc mở file Excel.");
                else MessageBox.Show(ex.Message);
                con.Close();
                return ex.State;
            }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmChinh = new frmMain();
            Application.Run(frmChinh);
        }
       
    }
}
