using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LogManager
{
    public partial class ucLogStack : UserControl
    {
        private bool display = false;

        private cLogManager _logManager = new cLogManager() { };

        public cLogManager loggManager
        {
            get
            {
                return _logManager;
            }
            set
            {
                if (value == null)
                {
                    value = new cLogManager();
                }

                _logManager = value;
            }
        }

        public ucLogStack()
        {
            InitializeComponent();
        }

        int lastId = 0;
        
        private void bwDisplayLogs_DoWork(object sender, DoWorkEventArgs e)
        {
            while (display)
            {
                if (_logManager.LogStack.Count > lastId)
                {
                    string[] cells = new string[4]
{
                    _logManager.LogStack[lastId].Id.ToString(),
                    _logManager.LogStack[lastId].dateTime.ToString(),
                    _logManager.LogStack[lastId].enLogLevel.ToString(),
                    _logManager.LogStack[lastId].Summary
};
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke((MethodInvoker)(() => dataGridView1.Rows.Insert(0, cells)));
                    }
                    else 
                    {
                        dataGridView1.Rows.Insert(0, cells);
                    }
                    lastId++;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (display)
            {
                button1.BackColor = Color.PaleVioletRed;
                display = false;
            }
            else
            {
                display = true;
                button1.BackColor = Color.PaleGreen;
                bwDisplayLogs.RunWorkerAsync();
            }
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 )
            {
                return;
            }

            int index = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            cLogItem logItem = _logManager.LogStack.Find(x => x.Id == index);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Description");
            sb.AppendLine(logItem.Description);

            textBoxLogDetail.Text = sb.ToString();
        }
    }
}
