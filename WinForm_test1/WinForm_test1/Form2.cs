using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using RestSharp;
using System.Net.Http; //API 取資料 
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_test1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        //前一頁
        private void Button8_Click(object sender, EventArgs e)
        {
            
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }
        //========================================================================
        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false; // 暫時停用按鈕，避免重複點擊
            textBox1.Text = "下載中...";

            string content = await DownloadContentAsync("https://jsonplaceholder.typicode.com/todos/");


            if (content.Length > 200)
                textBox1.Text = content.Substring(0, 200);
            else
                textBox1.Text = content; // 直接顯示全部

            btnDownload.Enabled = true;
        }

        public async Task<string> DownloadContentAsync(string url)
        {
            // 建立 RestClient
            var client = new RestClient(url);

            // 建立 GET 請求 //可更改目標5
            var request = new RestRequest("5", Method.Get);

            // 執行請求
            var response = await client.ExecuteAsync(request);

            // 回傳內容
            return response.Content;
        }

        /*
        private async Task<string> DownloadContentAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(url);
                return content;
            }
        }*/
        //========================================================================
        private void Button1_Click(object sender, EventArgs e)
        {
            // 建立門
            Door<string> frontDoor = new Door<string>();

            // 訂閱事件，把訊息寫到 textBox2
            frontDoor.OnKnock += (msg) => textBox2.AppendText("收到通知: " + msg + Environment.NewLine);

            // 敲門
            frontDoor.Knock("請進來!");
        }

        // 泛型委派
        public delegate void Notify<T>(T message);

        public class Door<T>
        {
            public event Notify<T> OnKnock;

            public void Knock(T message)
            {
                // 觸發事件
                OnKnock?.Invoke(message);
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
