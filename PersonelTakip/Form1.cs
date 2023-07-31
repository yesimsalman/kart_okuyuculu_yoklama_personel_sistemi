using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Data.SqlClient;

namespace PersonelTakip
{
    public partial class Form1 : Form
     {
        SqlConnection con;
        SqlCommand kmt;
        SqlDataReader dr;
        SqlCommand kmt1;
        public static string SqlCon = @"Data Source=LAPTOP-VUJS54LS\SQLEXPRESS;Initial Catalog=PersonelTekip;Integrated Security=True";

        public static string portismi, banthizi;
        string[] ports = SerialPort.GetPortNames();
        //var olan port isimlerini buraya attık.


        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void silme() {
        


        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            timer1.Start();
            portismi = comboBox1.Text;
            banthizi = comboBox2.Text;

            try
            {
                serialPort1.PortName = portismi;
                serialPort1.BaudRate = Convert.ToInt16(banthizi);

                serialPort1.Open();
                //başka bir program da aynı anda portu kullanıyorsa hata verir.
                //(System.UnauthorizedAccessException: ''COM3' bağlantı noktasına erişim reddedildi.')
                label1.Text = "Bağlantı Sağlandı";
                label1.ForeColor = Color.Green;


            }
            catch
            {
                serialPort1.Close();
                serialPort1.Open();
                MessageBox.Show("Bağlantı Zaten Açık!");
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                label1.Text = "Bağlantı Kesildi!";
                label1.ForeColor = Color.Red;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(string port in ports)
            {
                comboBox1.Items.Add(port);
                //bilgisayarımıza bağlı olan bütün portlar combobox1 e aktarılıyor.

            }
            //kullandığımız bant hızını combobox2 ye eklıyoruz.
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("115200");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 2;

            
        }
      

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Timerlar durduruluncaya kadar aktiftir.
            string sonuc;
            sonuc = serialPort1.ReadExisting();

            if (sonuc != "")
            {
                label2.Text = sonuc;
                string sql2 = "select * from tablo where kid='" + sonuc + "'";
                con = new SqlConnection(SqlCon);
                kmt = new SqlCommand(sql2, con);

               // label2.Text = sonuc;
                con.Open();
                kmt.Connection = con;
               // kmt.CommandText = "select * from tablo where kid='" + sonuc + "'";
                dr = kmt.ExecuteReader();
                if (dr.Read())
                {
                    DateTime bugun = DateTime.Now;
                    pictureBox1.Image = Image.FromFile("foto\\" + dr["resim"].ToString());
                    label9.Text = dr["isim"].ToString();
                    label10.Text = dr["sinif"].ToString();
                    label8.Text = dr["bolum"].ToString();
                    label12.Text = bugun.ToLongTimeString();
                    label11.Text = bugun.ToShortDateString();
                    con.Close();
                    



                    try
                    {

                        con = new SqlConnection(SqlCon);
                        string sql3 = "insert into zaman (isim, tarih, saat) values ('" + label9.Text + "', @tarih , @saat)";


                        kmt1 = new SqlCommand(sql3, con);
                        con.Open();
                        kmt1.Connection = con;
                        kmt1.Parameters.AddWithValue( "@tarih" , Convert.ToDateTime(label11.Text));
                        kmt1.Parameters.AddWithValue("@saat", Convert.ToDateTime(label12.Text));

                        kmt1.ExecuteNonQuery();

                        label14.Text = "Giriş Yapıldı";
                        label14.ForeColor = Color.Green;


                      con.Close();
                     }
                     catch
                     {
                         con.Close();
                         MessageBox.Show("HATA");
                     }
                }


                con.Close();
            }
            


        }
       
       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (portismi==null || banthizi==null)
            {
                //Bağlan butonuna tıklamadan kayıt yapma işlemini gerçeklestirmesin.
                MessageBox.Show(" Bağlantıyı Kontrol Ediniz!");
            }
            else
            {
                timer1.Stop();
                serialPort1.Close();
                label1.Text = "Bağlantı Kapalı!";
                label1.ForeColor = Color.Red;


                Kayit kyt = new Kayit();
                kyt.ShowDialog();

            }
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var durum = MessageBox.Show("Çıkış yapmak istiyor musunuz?", "Çıkış Yapılıyor..", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (durum == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }



        //eventslarda bulamadığım için form kapandığında port bağlantısının kesilmesi işlemini elle yazdım.

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
      
            }
        }

    }
}
