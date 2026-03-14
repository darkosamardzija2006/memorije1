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
using System.Media;


namespace memorije1
{
    public partial class Memorije : Form
    {
        int Stotinke = 0;
        string imekoris;
        int[] vreme_ostalih = new int[100];
        SoundPlayer player = new SoundPlayer("happy.wav");
        private void PrikaziVreme()
        {
            int t = Stotinke;
            int stotinke = t % 100;
            t /= 100;
            int sekunde = t % 60;
            t /= 60;
            int minuti = t;
            label17.Text = string.Format("{0:00}:{1:00}:{2:00}", minuti, sekunde, stotinke);
        }

        Random R = new Random();
        List<string> slike = new List<string>()
        {
            "C","C","E","E","e","e","I","I","J","J","P","P","S","S","T","T"
        };

        Label prvik, drugik;

        public Memorije()
        {
            InitializeComponent();
            dodeljivanje();
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (prvik != null && drugik != null)
                return;

            Label kliklabel = sender as Label;
            if (kliklabel == null)
                return;

            if (kliklabel.ForeColor == Color.Black)
                return;

            if (prvik == null)
            {
                prvik = kliklabel;
                prvik.ForeColor = Color.Black;
                return;
            }

            drugik = kliklabel;
            drugik.ForeColor = Color.Black;
            proverapobednika();

            if (prvik.Text == drugik.Text)
            {
                prvik = null;
                drugik = null;
            }
            else
                timer1.Start();
        }

        private void proverapobednika()
        {
            Label label;

            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                label = tableLayoutPanel1.Controls[i] as Label;
                if (label != null && label.ForeColor == label.BackColor)
                    return;
            }

            stoperica.Stop();
            MessageBox.Show("Čestitam pronašao si sve parove");
            pravljenjeniza();
            pravljenjerangliste();
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            prvik.ForeColor = prvik.BackColor;
            drugik.ForeColor = drugik.BackColor;
            prvik = null;
            drugik = null;
        }

        private void pravljenjeniza()
        {
            if (!File.Exists("ranglista.txt"))
                return;

            using (StreamReader sr = new StreamReader("ranglista.txt"))
            {
                string s;
                int i = 0;
                while ((s = sr.ReadLine()) != null)
                {
                    int minuti = int.Parse(s.Substring(0, 2));
                    int sekunde = int.Parse(s.Substring(3, 2));
                    int stotinke = int.Parse(s.Substring(6, 2));
                    vreme_ostalih[i] = minuti * 60000 + sekunde * 1000 + stotinke * 10;
                    i++;
                }
            }
        }

        private void pravljenjerangliste()
        {
            int trenutni_igrac = Stotinke;
            bool dodaj_u_rang_listu = false;

            for (int j = 0; j < vreme_ostalih.Length; j++)
            {
                if (vreme_ostalih[j] > trenutni_igrac)
                {
                    dodaj_u_rang_listu = true;
                    break;
                }
            }

            if (dodaj_u_rang_listu)
            {
                using (StreamWriter sw = new StreamWriter("ranglista.txt", true))
                {
                    //sw.Write("\n");
                    int stotinke = trenutni_igrac % 100;
                    trenutni_igrac /= 100;
                    int sekunde = trenutni_igrac % 60;
                    trenutni_igrac /= 60;
                    int minuti = trenutni_igrac;
                    if (minuti == 0)
                        sw.WriteLine(minuti + "0:" + sekunde + ":" + stotinke + " " + imekoris);
                    else
                        sw.WriteLine(minuti + ":" + sekunde + ":" + stotinke + " " + imekoris);
                }
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            
        }

        private void stoperica_Tick(object sender, EventArgs e)
        {
            Stotinke++;
            PrikaziVreme();
        }

        private void start_Click(object sender, EventArgs e)
        {
            stoperica.Enabled = !stoperica.Enabled;
            start.Text = "STOP";
            start.BackColor = Color.Red;
        }

        private void ranglista_Click(object sender, EventArgs e)
        {
            if (!File.Exists("ranglista.txt"))
            {
                MessageBox.Show("Trenutno nema rang liste.");
                return;
            }

            using (StreamReader sr = new StreamReader("ranglista.txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    MessageBox.Show(s);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            imekoris = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (player.IsLoadCompleted && player.Stream != null)
            {
                if (player.Stream.Length > 0)
                {
                    player.Stop();
                }
                else
                {
                    player.Play();
                }
            }
            else
            {
                player.Load();
                player.Play();
            }
        }

        private void dodeljivanje()
        {
            Label label;
            int rb;
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label)
                    label = (Label)tableLayoutPanel1.Controls[i];
                else
                    continue;

                rb = R.Next(0, slike.Count);
                label.Text = slike[rb];
                slike.RemoveAt(rb);
            }
        }
    }
}
