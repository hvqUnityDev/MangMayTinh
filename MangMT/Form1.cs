using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangMT
{
    public partial class Form1 : Form
    {

        IPAddress _ipaddress;
        IPEndPoint _ipendpoint;

        Socket _socketserver;
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
                int _recv = _socketserver.ReceiveFrom(_data, ref _endpoint);
                string _s = Encoding.Unicode.GetString(_data, 0, _recv);
                if (_s.ToUpper().Equals("QUIT"))
                {
                    break;
                }

                label1.Text += $"\nClient: {_s}";
                //_data = new byte[1024];
                //_data = Encoding.Unicode.GetBytes(_s);
                //_socketserver.SendTo(_data, 0, _data.Length, SocketFlags.None, _endpoint);
            }
        }

        void SetUp()
        {
            _ipaddress = IPAddress.Any;
            _ipendpoint = new IPEndPoint(_ipaddress, 2022);

            _socketserver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketserver.Bind(_ipendpoint);

            // Ở Udp socket ta cần tạo một Remote IPEndPoint từ xa để 	   	   nhận dữ liệu về
            IPEndPoint _remoteipendpoint = new IPEndPoint(IPAddress.Any, 0);
            _endpoint = (EndPoint)_remoteipendpoint;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _s = textBox1.Text;
            byte[] _data = new byte[1024];
            _data = Encoding.Unicode.GetBytes(_s);
            _socketserver.SendTo(_data, _endpoint);
            label1.Text += $"\nServer: {_s}";
            textBox1.Text = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _socketserver.Close();
             _ipaddress = null;
            _ipendpoint = null;

            _socketserver = null;
            _endpoint = null;
        }
    }
}
