using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace sitechecker
{
    public partial class Form1 : Form
    {
        
        bool autocheck = false;
        Site current_site;

        struct Site
        {
            public string url;
            public string name;
            public bool active;
            public List<bool> series ;
            public string desc;
            public string updateTime;
        }
        internal static Form1 main;

        List<Site> sitelist = new List<Site>();

        public Form1()
        {
            InitializeComponent();
            main = this;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.List;
            addSite("https://www.youtube.com", "youtube", "news");
            addSite("http://www.break.com/", "break", "news");
            addSite("https://thepiratebay.org/", "piratebay", "download");
            addSite("https://www.google.se/", "google", "search");
            addSite("https://www.facebook.com/", "facebook", "news");
            addSite("https://us.battle.net/forums/en/d3/", "diablo3", "game");
            addSite("https://forhonor.ubisoft.com/game/en-us/home/", "forhonor", "game");

            // select item
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Select();
            }
        }

        private void addSite(string url, string name, string desc)
        {
            Site s = new Site();
            s.url = url;
            s.name = name;
            s.desc = desc;
            s.series = new List<bool>();
           // s.series.Add(false);

            var l = listView1.Items.Add(name);
            s = siteActive(s);
            if (s.active) {
      
                l.BackColor = Color.LightGreen;
            } else
            {
                l.BackColor = Color.LightPink;
            }
            sitelist.Add(s);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            addSite(textBox1.Text, textBox2.Text, textBox4.Text);
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private Site siteActive(Site s)
        {
            s.updateTime = DateTime.Now.ToString("t");
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(s.url);  
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";
           
                try
                {
                    request.GetResponse();

                    s.active = true;
                    s.series.Add(s.active);
                    request.Abort();
                    return s;
                }
                catch (WebException wex)
                {
                    s.active = false;
                    s.series.Add(s.active);
                    request.Abort();
                    return s;
                }
            }
            catch
            {
                s.active = false;
                s.series.Add(s.active);
                return s;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Console.WriteLine(listView1.SelectedItems[1].Index);
            if (listView1.SelectedItems.Count > 0)
            {
                Site site = sitelist[listView1.SelectedItems[0].Index];
                current_site = site;
                setSiteInfo(site);
            }
                } 

        private void setSiteInfo(Site s)
        {
            label15.Text = s.updateTime;
            label16.Text = s.active.ToString();
            label17.Text = s.desc;
            label18.Text = s.url;
            label19.Text = s.name;

            updateChart(s);
            
        }

        delegate void updateChartDelegate(Site s);
        private void updateChart(Site s)
        {

            if (chart1.InvokeRequired)
            {
                chart1.Invoke(new updateChartDelegate(this.updateChart), new object[] { s });
            }
            else
            {
                chart1.Series.Clear();
                chart1.Series.Add(s.name);
                chart1.Series[s.name].ChartType = SeriesChartType.SplineArea;
                chart1.Series[s.name].XValueType = ChartValueType.DateTime;
                chart1.Series[s.name].YValueType = ChartValueType.Auto;
                chart1.Series[s.name].YAxisType = AxisType.Secondary;
                foreach (bool b in s.series)
                {
                    if (b)
                    {
                        chart1.Series[s.name].Points.AddXY(s.updateTime, 1);
                    }
                    else
                    {
                        chart1.Series[s.name].Points.AddXY(s.updateTime, 0);
                    }
                }
                chart1.Series[s.name].ChartArea = "ChartArea1";
            }
        }
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label7.Text =  trackBar1.Value.ToString();
           
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckAll();
        }

        delegate void changeColorDelegate(Color c, int i);
        public void changeColor(Color c, int i)
        {
            if (listView1.InvokeRequired)
            {
              
                listView1.Invoke(new changeColorDelegate(this.changeColor), new object[] { c, i });
            }
            else
            {
               
                listView1.Items[i].BackColor = c;
            }
        }

       public void CheckAll()
        {
            List<Site> templist = new List<Site>();

          
                for (int i = 0; i < sitelist.Count; i++)
                {
                    sitelist[i] = siteActive(sitelist[i]);

                    if (sitelist[i].active)
                    {
                   
                        changeColor(Color.LightGreen, i);
                    }
                    else
                    {
                        changeColor(Color.LightPink, i);
                    }
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[0];
            if (listView1.SelectedItems.Count > 0)
            {
                var i = item.Index;
                sitelist[i] = siteActive(sitelist[i]);

                if (sitelist[i].active)
                {
                    item.BackColor = Color.LightGreen;
                }
                else
                {
                    item.BackColor = Color.LightPink;
                }
                
            }       
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (autocheck)
            {
                timer1.Stop();
                autocheck = false;
                button3.BackColor = Color.Red;
             
            } else
            {
                label24.Text = 0.ToString();
                label8.Text = label7.Text;
                timer1.Start();
                autocheck = true;
                button3.BackColor = Color.Green;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = Int32.Parse(label8.Text);
            label8.Text = (i - 1).ToString();

            if (Int32.Parse(label8.Text) <= 0)
            {
              
                  if (!backgroundWorker1.IsBusy)
                                 {
                    
                                     int j = Int32.Parse(label24.Text);
                                     label24.Text = (j + 1).ToString();
                                     backgroundWorker1.RunWorkerAsync();
                                 }        
                label8.Text = label7.Text;
            }    
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            changeText("An allCheck has been started \n at :" + DateTime.Now.ToString("t"));
            CheckAll();
            changeText2("An allCheck has been completed \n at : " + DateTime.Now.ToString("t"));

            updateChart(current_site);

        }

        delegate void changeTextDelegate(string s);
        public void changeText(string s)
        {
            if (label21.InvokeRequired)
            {
                label21.Invoke(new changeTextDelegate(this.changeText), new object[] { s });
            }
            else
            {
                label21.Text = s;
            }
        }
        delegate void changeText2Delegate(string s);
        public void changeText2(string s)
        {
            if (label22.InvokeRequired)
            {
                label22.Invoke(new changeText2Delegate(this.changeText2), new object[] { s });
            }
            else
            {
                label22.Text = s;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Site site = sitelist[listView1.SelectedItems[0].Index];
                setSiteInfo(site);
            }
        }
    }
}
