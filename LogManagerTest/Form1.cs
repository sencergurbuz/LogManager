using LogManager;
using NetworkManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogManagerTest
{
    public partial class Form1 : Form
    {
        cLogManager logManager;

        cNetworkManager networkManager;

        public Form1()
        {
            InitializeComponent();

            logManager = new cLogManager();
            logManager.networkManager = new cNetworkManager();
            logManager.networkManager.Adress = "127.0.0.1";
            logManager.networkManager.Port = 1000;
            logManager.FolderPath = "Logs";
            logManager.isGetFromNetwork = true;           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logManager.Add(LogLevel.Warning, "Deneme");
        }

        ucLogStack uc;

        private void button2_Click(object sender, EventArgs e)
        {
            uc = new ucLogStack();
            uc.Dock = DockStyle.Fill;
            uc.loggManager = logManager;
            panel2.Controls.Add(uc);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint iPEndPoint;

                string ad = textBox1.Text.Split(':')[0];
                int pr =Convert.ToInt32( textBox1.Text.Split(':')[1]);
                iPEndPoint = new IPEndPoint(IPAddress.Parse(ad), pr);

                cLogItem item = new cLogItem();
                item.enLogLevel = LogLevel.Info;
                item.Summary = "UDP Log";
                item.Description = "This log send from udp";

                byte[] b = item.Serialize();
                new UdpClient().Send(b, b.Length, iPEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata alındı : "+ ex.Message);
            }
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            logManager.isSaveToFile = checkBox1.Checked;
        }
    }
}
