using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace SmartDeviceProject2
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public static Queue dataSendToWeb = new Queue();
        public bool FWeb = false;

        public string data_dequeue = null;
        /// <summary>
        /// Du lieu nhan tu WEB
        /// </summary>
        private static string dataReceiveFromWeb = null;
        public string DataReceiveFromWeb
        {
            set { dataReceiveFromWeb = value; }
            get { return dataReceiveFromWeb; }
        }

        WebServer web = new WebServer();
        public void ShowStr(string str)
        {
            tbReceive.Text += str + "\r\n";
        }
        public Form1()
        {
            InitializeComponent();
            web.txtShowData = tbReceive;
        }

        public void ThreadSendToWeb()
        {
            web.threadSendToWeb = new Thread(new ThreadStart(send));
            web.threadSendToWeb.IsBackground = true;
            web.threadSendToWeb.Start();
            web.DisplayData("ThreadSendToWeb is opened!", tbReceive);
        }

        
        public void send()
        {
            while (true)
            {
                try
                {
                    if (dataSendToWeb.Count > 0)
                    {
                        data_dequeue = dataSendToWeb.Dequeue().ToString();
                        web.DisplayData(data_dequeue, tbReceive);
                        web.sendDataToWeb(data_dequeue);
                    }
                    Thread.Sleep(200);
                }
                catch { }
            }
        }
        public void ThreadReceiveFromWeb()
        {
            web.threadReceiveFromWeb = new Thread(new ThreadStart(Request));
            web.threadReceiveFromWeb.IsBackground = true;
            web.threadReceiveFromWeb.Start();
        }
        public void Request()
        {
            while (FWeb)
            {
                try
                {
                    //  DisplayData("Request len web :", txtShowData);
                    string[] url = Connections.Confix();
                    string uriCom = url[2];
                    while (true)
                    {
                        web.DataReceiveFromWeb = web.receiveDataFromWeb(uriCom);

                        if (web.DataReceiveFromWeb.Length > 0)
                        {
                            web.DisplayData(web.DataReceiveFromWeb, tbReceive);
                            web.DataReceiveFromWeb = null;
                        }
                        Thread.Sleep(100);
                    }
                }
                catch
                {
                }
            }
        }
        private void btSend_Click(object sender, EventArgs e)
        {
            dataSendToWeb.Enqueue(tbSend.Text);
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (btConnect.Text == "Connect")
            {
                btConnect.Text = "DisConnect";
                FWeb = true;
                ThreadSendToWeb();
                ThreadReceiveFromWeb();                
            }
            else
            {
                btConnect.Text = "Connect";
                FWeb = false;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            //int temp = int.Parse(tbSend.Text, System.Globalization.NumberStyles.HexNumber);
            //tbReceive.Text += temp.ToString()+"\r\n";
            string clgt = tbSend.Text;
            string mac = clgt.Substring(0, 2);
            ShowStr(clgt[2].ToString());
            
            int mac_int = int.Parse(mac, System.Globalization.NumberStyles.HexNumber);
            if ((mac_int > 48) && (mac_int < 96)) ShowStr("sensor bao chay");
            else if ((mac_int > 0) && (mac_int < 48)) ShowStr("snsor vuon lan");
            else ShowStr("dong vat");
            //else ShowStr("nho hon 1a");
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            FWeb = false;
        }
    }
}