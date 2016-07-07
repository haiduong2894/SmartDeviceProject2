using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SmartDeviceProject2
{
    class WebServer
    {
        public TextBox txtShowData;
        
        /// <summary>
        /// Thread gui du lieu len web server
        /// </summary>
        public Thread threadSendToWeb = null;

        /// <summary>
        /// Thread nhan du lieu tu web server
        /// </summary>
        public Thread threadReceiveFromWeb = null;

        public string DataReceiveFromWeb = null;

        public void DisplayData(string msg, TextBox listBox1)
        {
            listBox1.Invoke(new EventHandler(delegate
            {
                listBox1.Text += msg + "\r\n";
                listBox1.SelectionStart = listBox1.Text.Length;
                listBox1.ScrollToCaret();
            }));
        }
        /// <summary>
        /// Ham gui du lieu len WEB
        /// Phuong thuc gui: GET
        /// </summary>
        /// <param name="datasend"></param>
        public void sendDataToWeb(string datasend)
        {
            try
            {
                string[] url1 = Connections.Confix();
                string url = url1[0];
                if (datasend[0] == '#')
                    datasend = datasend.Substring(1, datasend.Length - 1);

                url = url + "?data=" + datasend;

                // Create web request

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Set value for request headers
                request.Method = "GET";
                request.ProtocolVersion = HttpVersion.Version11;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                request.Headers.Add("Accept-Language", "en-us");
                request.KeepAlive = true;
                StreamReader responseStream = null;
                HttpWebResponse webResponse = null;
                string webResponseStream = string.Empty;

                // Get response for http web request

                webResponse = (HttpWebResponse)request.GetResponse();
                responseStream = new StreamReader(webResponse.GetResponseStream());

                // Read web response into string
                webResponseStream = responseStream.ReadToEnd();

                //close webresponse
                webResponse.Close();
                responseStream.Close();
                // show data receive
                //DisplayData(webResponseStream, txtShowData);
            }
            catch
            {
                // MessageBox.Show("Giông dở rồi , có đéo mạng đâu mà đòi chạy , ngu lắm mày bật mạng lên");
                //   DisplayData("Giông dở rồi,ngu lắm mày bật mạng lên", txtShowData);
            }
        }

        /// <summary>
        /// Ham nhan du lieu tu WEB server
        /// Phuong thuc nhan: GET
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string receiveDataFromWeb(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Set value for request headers
                request.Method = "GET";
                request.ProtocolVersion = HttpVersion.Version11;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                request.Headers.Add("Accept-Language", "en-us");
                request.KeepAlive = true;
                StreamReader responseStream = null;
                HttpWebResponse webResponse = null;
                string webResponseStream = string.Empty;

                // Get response for http web request
                webResponse = (HttpWebResponse)request.GetResponse();
                responseStream = new StreamReader(webResponse.GetResponseStream());

                // Read web response into string
                webResponseStream = responseStream.ReadToEnd();

                //DisplayData("Nguyen Hai Duong", txtShowData);
                //DisplayData(webResponseStream, txtShowData);/////////////////////////////////////////////
                //close WebResponse
                webResponse.Close();
                return webResponseStream;
            }
            catch
            {
                // MessageBox.Show("khong doc duoc gi tu web");
                return "";
            }
        }
    }
}
