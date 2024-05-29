using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.a11ConnectionString);
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == "" || textBox3.Text == "") throw new Exception();
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cm = new SqlCommand("insert into Autor values(@p1,@p2,@p3,@p4)", con);
                //tražimo posledji uneti indeks, uvećamo ga za 1, i unesemo u textBox1
                int ind = 1 + int.Parse(listBox1.Items[listBox1.Items.Count - 1].ToString().Substring(0, 5).Trim());
                //dodajemo parametre
                cm.Parameters.AddWithValue("p1", ind);
                cm.Parameters.AddWithValue("p2", textBox2.Text);
                cm.Parameters.AddWithValue("p3", textBox3.Text);
                cm.Parameters.AddWithValue("p4", dateTimePicker1.Value.ToShortDateString());
                //unos
                cm.ExecuteNonQuery();
                //prikaz
                string p1 = Poravnaj(ind.ToString(), 5);
                string p2 = Poravnaj(textBox2.Text, 20);
                string p3 = Poravnaj(textBox3.Text, 20);
                string p4 = Poravnaj(dateTimePicker1.Text, 19);
                listBox1.Items.Add(p1 + " " + p2 + " " + p3 + " " + p4);
                //obaveštenje
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                MessageBox.Show("Unos je uspeo");
            }
            catch
            {
                textBox2.Text = "";
                textBox3.Text = "";
                dateTimePicker1.ResetText();
                labelEP.Visible = labelED.Visible = labelET.Visible = true;
                MessageBox.Show("Unseite obavezna polja!");
            }
            if (con.State == ConnectionState.Open) con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            reset();
            textBox1.Enabled = true;
            labelEP.Visible = false;
            labelED.Visible = false;
            labelET.Visible = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelEP.Visible = false;
            labelED.Visible = false;
            labelET.Visible = false;
            button2.Enabled = false;
            button3.Enabled = false;
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select * from Autor", con);
                SqlDataReader r = cm.ExecuteReader();
                while (r.Read())
                {
                    DateTime dat = DateTime.Parse(r[3].ToString());
                    string p1 = Poravnaj(r[0].ToString(), 5);
                    string p2 = Poravnaj(r[1].ToString(), 20);
                    string p3 = Poravnaj(r[2].ToString(), 20);
                    string p4 = Poravnaj(dat.ToString("dd.MM.yyyy."), 19);
                    listBox1.Items.Add(p1 + " " + p2 + " " + p3 + " " + p4);
                }
                r.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            con.Close();
            listBox1.SelectedIndex = 0;
        }

        private string Poravnaj(string t, int d)
        {
            string r = t;
            while (r.Length < d) r = " " + r;
            return r;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reset();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form3 f3=new Form3();
            f3.Show();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ceo_red = listBox1.SelectedItems[0].ToString();
                textBox1.Text = ceo_red.Substring(0, 5).Trim();
                textBox2.Text = ceo_red.Substring(6, 20).Trim();
                textBox3.Text = ceo_red.Substring(27, 20).Trim();
                string dat = ceo_red.Substring(48, 19).Trim();
                DateTime dt = DateTime.ParseExact(dat, "dd.MM.yyyy.", null);
                dateTimePicker1.Value = dt;
            }
            catch
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string id = textBox1.Text;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    string ceo_red = listBox1.Items[i].ToString();
                    string br = ceo_red.Substring(0, 5).Trim();
                    if (br == id)
                    {
                        listBox1.SelectedIndex = i;
                        return;
                    }
                }
            }
            catch
            {
            }
            reset();
        }
        private void reset()
        {
            labelEP.Visible = labelED.Visible = labelET.Visible = false;
            textBox1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker1.ResetText();
        }
    }
}
