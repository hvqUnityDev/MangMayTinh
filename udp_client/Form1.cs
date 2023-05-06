using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace udp_client
{
    public partial class Form1 : Form
    {
        IPAddress _ipaddress;// = IPAddress.Parse("127.0.0.1");
        IPEndPoint _ipendpoint;// = new IPEndPoint(_ipaddress, 2022);
        Socket _socketclient;
        EndPoint _endpoint;
        public Form1()
        {
            InitializeComponent();
            SetUp();
            Thread thread = new Thread(new ThreadStart(Update)); 
            thread.Start();
        }

        void Update()
        {
            while (true)
            {
                byte[] _data = new byte[1024];
                int _recv = _socketclient.ReceiveFrom(_data, ref _endpoint);
                string _s = Encoding.Unicode.GetString(_data, 0, _recv);
                if (_s.ToUpper().Equals("QUIT"))
                {
                    break;
                }

                label1.Text += $"\nServer: {_s}";
            }
        }

        void SetUp()
        {
            _ipaddress = IPAddress.Parse("127.0.0.1");
            _ipendpoint = new IPEndPoint(_ipaddress, 2022);
            _socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string _s = "Hello Server!...";
            byte[] _data = new byte[1024];
            _data = Encoding.Unicode.GetBytes(_s);
            _socketclient.SendTo(_data, _ipendpoint);
            //tạo endpoint nhận dữ liệu về từ server

            _endpoint = (EndPoint)_ipendpoint;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _s = textBox1.Text;
            byte[] _data = new byte[1024];
            _data = Encoding.Unicode.GetBytes(_s);
            _socketclient.SendTo(_data, _endpoint);
            label1.Text += $"\nClient: {_s}";
            textBox1.Text = "";
        }

        private void fClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            _socketclient.Close();
        }

       
    }
}
