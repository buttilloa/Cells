using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {
        ServerHandle server = new ServerHandle();
        Commands cmdManager;
        public ServerForm()
        {
            InitializeComponent();
            this.Text = "CELLS SERVER";
            CommandBox.Text = "";
            cmdManager = new Commands(server, LogBox);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
           }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!server.isRunning)
            {
                LogBox.Text = "";
                server.initiate(this, LogBox);
                StartButton.Text = "Stop";
                IPAddressLabel.Text = server.getIP();
                timer.Enabled = true;
            }
            else
            {
                server.shutdown();
                StartButton.Text = "Start";
                server = new ServerHandle();
                LogBox.Text += Time.Text+" STOPPED";
                ConnectionsBox.Text = "";
                IPAddressLabel.Text = "";
                timer.Enabled = false;
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            LogBox.SelectionStart = ServerHandle.log.Length;
            LogBox.ScrollToCaret();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.shutdown();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                if (CommandBox.Text != "")
                    HandleCommand();

        }
        public void HandleCommand()
        {
            cmdManager.ParseCmd(CommandBox.Text);
            CommandBox.Text = "";
        }
        public void removeConnection(int id)
        {
            addConnection("" + id, 3);
        }
        public void addConnection(int id, string IP)
        {
            addConnection("{" + id + "}-" + IP + " |\n", 0);
        }
        delegate void addConnectionCallback(string text, int call);
        void addConnection(string text, int call)
        {
            if (ConnectionsBox != null)
                if (ConnectionsBox.InvokeRequired)
                {
                    addConnectionCallback d = new addConnectionCallback(addConnection);
                    this.Invoke(d, new object[] { text, call });
                }
                else
                    if (call == 3)
                    {
                        string connections = ConnectionsBox.Text;
                        int index = connections.IndexOf("{" + Convert.ToInt32(text) + "}");
                        int end = connections.IndexOf("|", index);
                        ConnectionsBox.Text = ConnectionsBox.Text.Remove(index, end - index + 1);
                    }
                    else ConnectionsBox.Text += text;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (CommandBox.Text != "") HandleCommand();
        }
        private void Time_Click(object sender, EventArgs e)
        {
       }
        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Time.Text = "(" + now.Hour + ":" + now.Minute + ":" + now.Second.ToString().PadLeft(2, '0') + ")";
        }
    }
}
