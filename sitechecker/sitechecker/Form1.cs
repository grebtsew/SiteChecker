using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sitechecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        struct Site
        {
           public string url;
           public string name;
        }

        List<Site> sitelist = new List<Site>();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Site newItem = new Site();
            newItem.url = textBox1.Text;
            newItem.name = textBox2.Text;
            sitelist.Add(newItem);
            listView1.Items.Add(textBox2.Text);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
