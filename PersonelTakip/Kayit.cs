using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PersonelTakip
{
    //Data Source=LAPTOP-VUJS54LS\SQLEXPRESS;Initial Catalog=PersonelTekip;Integrated Security=True
    public partial class Kayit : Form
    {

        SqlConnection con;
        SqlCommand kmt;
        SqlDataReader dr;
        SqlCommand kmt1;
        public static string SqlCon=@"Data Source=LAPTOP-VUJS54LS\SQLEXPRESS;Initial Catalog=PersonelTekip;Integrated Security=True";
        
        
        public Kayit()
        {
            InitializeComponent();
        }


        public void eklemeidkontrol() {

            string sql = "select * from tablo where kid=@kid";
            //kmt.Parameters.AddWithValue("@kid", label6.Text );

            con = new SqlConnection(SqlCon);
            kmt = new SqlCommand(sql , con);
            kmt.Parameters.AddWithValue("@kid", label6.Text);
            con.Open();
            dr = kmt.ExecuteReader();

            if (dr.Read())
            {
                MessageBox.Show("bu kart zaten kayıtlı");
            }
            else {

                try
                {

                    con = new SqlConnection(SqlCon);
                    string sql1 = "INSERT INTO dbo.tablo (kid, isim, sinif, bolum, resim) VALUES ('" + label6.Text + "', '" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "', '" + textBox2.Text + "')";

                    kmt1 = new SqlCommand(sql1 , con);
                    con.Open();
                    kmt1.Connection = con;
                    
                    kmt1.ExecuteNonQuery();

                    label8.Text = "Kayıt Yapıldı";
                    label8.ForeColor = Color.Green;


                    con.Close();
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("HATA");
                }




            }




        }

        public void silme1() {

            con = new SqlConnection(SqlCon);
            kmt = new SqlCommand();

            
            

                con.Open();
                kmt.Connection = con;
                kmt.CommandText = "delete from tablo";
                MessageBox.Show("silme işlemi gerçekleşti");
                kmt.ExecuteNonQuery();
                con.Close();
            
         }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string sonuc;
            sonuc = serialPort1.ReadExisting();

            if (sonuc != "")
            {
                label6.Text = sonuc;
            }
        }

        private void Kayit_Load(object sender, EventArgs e)
        {

            serialPort1.PortName = Form1.portismi;
            serialPort1.BaudRate = Convert.ToInt16(Form1.banthizi);

            if (serialPort1.IsOpen == false)
            {
                try
                {
                    serialPort1.Open();
                    label7.Text = "Bağlantı Sağlandı";
                    label7.ForeColor = Color.Green;
                }
                catch
                {
                    label7.Text = "Bağlantı Sağlanamadı";
                }
            }
            else
            {
                label7.Text = "Bağlantı Sağlanamadı";
                label7.ForeColor = Color.Red;
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label6.Text = "__________";
            textBox1.Text = "";
            comboBox1.Text = "Seçiniz...";
            comboBox2.Text = "Seçiniz...";
            textBox2.Text = "";
            label8.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* con = new SqlConnection(SqlCon);
             kmt = new SqlCommand(SqlCon);
             if (label6.Text == "__________" || textBox1.Text == "" || comboBox1.Text == "Seçiniz..." || comboBox2.Text == "Seçiniz..." || textBox2.Text == "")
             {
                 label8.Text = "Eksik Bilgi Var!";
                 label8.ForeColor = Color.Red;
             }
             else
             {

                 try
                 {

                     con.Open();
                     kmt.Connection = con;
                     kmt.CommandText = "INSERT INTO dbo.tablo (kid, isim, sinif, bolum, resim) VALUES ('" + label6.Text + "', '" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "', '" + textBox2.Text + "')";
                     kmt.ExecuteNonQuery();

                         label8.Text = "Kayıt Yapıldı";
                         label8.ForeColor = Color.Green;


                     con.Close();
                 }
                 catch
                 {
                     con.Close();
                     MessageBox.Show("Bu kart zaten kayıtli!");
                 }

             }*/
            eklemeidkontrol();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları (jpg) |*.jgp|Tüm Dosyalar |*.*";
            openFileDialog1.InitialDirectory=Application.StartupPath+"\\foto";
            dosya.RestoreDirectory = true;

            if(dosya.ShowDialog()== DialogResult.OK)
            {
                string di = dosya.SafeFileName;
                textBox2.Text = di;
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            silme1();
        }

        private void Kayit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();

            timer1.Stop();
            serialPort1.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var durum = MessageBox.Show("Çıkış yapmak istiyor musunuz?", "Çıkış Yapılıyor..", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (durum == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();

        }
    }
}
