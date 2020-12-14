using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneForm
{
    public partial class Form1 : Form
    {
        int[] pin = { 0000, 0147, 0258, 0369 };
        Form2 f2;
        Button b;
        Color tecla = Color.LightGray;
        Color selecTecla = Color.Crimson;
        Color focoTecla = Color.LightSlateGray;
        SaveFileDialog sv;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            
            int intentos = 3;
            f2 = new Form2();
            getText();
            bool error = true;

            while (error)
            {
                if (intentos > 0)
                {
                    DialogResult res = f2.ShowDialog();
                    int num;

                    switch (res)
                    {
                        case DialogResult.OK:
                            if(f2.textBox1.Text.Length == 4)
                            {
                                if (int.TryParse(f2.textBox1.Text, out num))
                                {
                                    if (!pin.Contains(num))
                                    {
                                        f2.lblPin.Text = Strings.pinNoValido + Strings.intentos + (--intentos);
                                    }
                                    else
                                    {
                                        error = false;
                                    }
                                }
                                else
                                {
                                    f2.lblPin.Text = Strings.numPin + Strings.intentos + (--intentos);
                                }
                            }
                            else
                            {
                                f2.lblPin.Text = Strings.pinNoValido + Strings.longPin + Strings.intentos + (--intentos);
                            }

                            break;

                        case DialogResult.Cancel:
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            if (!error)
            {
                int x = 50;
                int y = 150;
                for (int i = 1; i <= 12; i++)
                {
                    b = new Button();                    
                    b.Location = new System.Drawing.Point(x, y);
                    b.Size = new System.Drawing.Size(50, 50);
                    b.BackColor = tecla;
                    b.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Click);
                    b.MouseEnter += new System.EventHandler(this.btn_Enter);
                    b.MouseLeave += new System.EventHandler(this.btn_Leave);

                    if (i == 10)
                    {
                        b.Tag = '*';
                        b.Text += b.Tag;
                    }
                    else if (i == 11)
                    {
                        b.Tag = 0;
                        b.Text += b.Tag.ToString();
                    }
                    else if(i == 12)
                    {
                        b.Tag = '#';
                        b.Text += b.Tag;
                    }                    
                    else
                    {
                        b.Tag = i;
                        b.Text = b.Tag.ToString();
                    }

                    this.Controls.Add(b);

                    if (i % 3 == 0)
                    {
                        y += 50;
                        x = 50;
                    }
                    else
                    {
                        x += 50;
                    }
                }
            }              
        }

        private void getText()
        {
           // Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            this.Text = Strings.tituloForm;
            btnReset.Text = Strings.borrar;
            archivoToolStripMenuItem.Text = Strings.archivo;
            resetToolStripMenuItem.Text = Strings.borrar;
            salirToolStripMenuItem.Text = Strings.salir;
            acercaDeToolStripMenuItem.Text = Strings.acercaDe;
            grabarNúmeroToolStripMenuItem.Text = Strings.guardar;
            f2.Text = Strings.introPIN;
            f2.button1.Text = Strings.aceptar;
        }

        private void btn_Click(Object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = selecTecla;
            this.textBox1.Text += b.Tag;
        }

        private void btn_Enter(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.BackColor != selecTecla)
            {
                b.BackColor = focoTecla;
            }
        }

        private void btn_Leave(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if(b.BackColor != selecTecla)
            {
                b.BackColor = tecla;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    c.BackColor = tecla;
                }
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '*' || e.KeyChar == '#')
            {
                foreach (Control c in this.Controls)            //HASHTABLE con btn clave 1,2,3...
                {
                    if (c is Button)
                    {
                        if(c.Tag.ToString() == e.KeyChar.ToString())
                        {
                            textBox1.Text += c.Tag.ToString();
                            c.BackColor = selecTecla;
                        }
                    }                    
                }
            }
        }

        private void grabarNumero(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                sv = new SaveFileDialog();
                this.sv.Title = Strings.guardar;
                this.sv.InitialDirectory = Environment.CurrentDirectory;
                this.sv.Filter = ".txt |*.txt|"+Strings.todosArchivos+"|*.*";
                this.sv.ValidateNames = true;
                sv.OverwritePrompt = false; //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog.overwriteprompt?view=net-5.0

                DialogResult res = sv.ShowDialog();

                switch (res)
                {
                    case DialogResult.OK:

                        StreamWriter sw;                            
                        using (sw = new StreamWriter(this.sv.FileName, true))
                        {
                            sw.WriteLine(textBox1.Text);
                                
                        }                        
                        break;
                }
            }
            else
            {
               MessageBox.Show(Strings.introduceNum, Strings.errorGuardar, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show(Strings.confSalir, Strings.conf, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                 == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Strings.infoApp, Strings.info, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
