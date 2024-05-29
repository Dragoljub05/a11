using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection(Properties.Settings.Default.a11ConnectionString);
        Random R = new Random();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select DispMem from chListBox1", con);
                SqlDataReader r = cm.ExecuteReader();
                while (r.Read())
                {
                    checkedListBox1.Items.Add(r[0].ToString());

                }
                r.Close();
            }
            catch
            {

            }
            con.Close();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                chart1.Series[0].Points.Clear();
                SqlCommand cm = new SqlCommand(@"
                                                select a.Ime+' '+a.Prezime, count(*)
                                                from AUTOR a join AUTOR_IZDANJE i
                                                on a.AutorID=i.AutorID
                                                where a.Ime+' '+a.Prezime in " + Obelezeni() + @"
                                                group by a.Ime+' '+a.Prezime
                                                    ", con);
                SqlDataReader r = cm.ExecuteReader();
                int i = 0;
                while (r.Read())
                {
                    chart1.Series[0].Points.Add(int.Parse(r[1].ToString()));
                    chart1.Series[0].Points[i].AxisLabel = r[0].ToString();
                    chart1.Series[0].Points[i].Color = SlucajnaBoja();
                    i++;

                }
                r.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private Color SlucajnaBoja()
        {

            int slBroj = R.Next(1, 7);
            if (slBroj == 1) return Color.Yellow;
            else if (slBroj == 2) return Color.DarkOrchid;
            else if (slBroj == 3) return Color.DarkOrange;
            else if (slBroj == 4) return Color.LimeGreen;
            else if (slBroj == 5) return Color.DarkRed;
            else return Color.SkyBlue;
        }

        private string Obelezeni()
        {
            string s = "";
            foreach (string imp in checkedListBox1.CheckedItems)
            {
                s += "'" + imp + "',";
            }
            return "(" + s.Substring(0, s.Length - 1) + ")";
        }
    }
}
