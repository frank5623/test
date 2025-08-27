using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Windows.Forms;

namespace WinForm_test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
            
        
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ImeMode = ImeMode.Off;
            textBox3.ImeMode = ImeMode.Off;

            comboBox1.Items.Add("+");
            comboBox1.Items.Add("-");
            comboBox1.Items.Add("*");
            comboBox1.Items.Add("/");
            comboBox1.SelectedIndex = 0; //預設'+'號


            dataGridView1.DataSource = students; //一開始就顯示原始資料
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int num1 = Convert.ToInt32(textBox1.Text);
            int num2 = Convert.ToInt32(textBox3.Text);


            string op = comboBox1.Text;            // 運算符

            int result = 0;

            switch (op)
            {
                case "+": result = num1 + num2; break;
                case "-": result = num1 - num2; break;
                case "*": result = num1 * num2; break;
                case "/":
                    if (num2 == 0)
                        MessageBox.Show("不能除以 0！");
                    else
                        result = num1 / num2;
                    break;
                default:
                    MessageBox.Show("運算符號不正確！");
                    break;
            }

            label1.Text = ($"=" + result);
            list計算機記錄.Items.Add($"{num1}{op}{num2}={result}");


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            list計算機記錄.Items.Clear();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var person = new
            {
                Name = "Terry",
                Age = 30,
                Country = "Taiwan"
            };

            // 使用 Newtonsoft.Json 將物件轉成 JSON 字串
            string json = JsonConvert.SerializeObject(person, Formatting.Indented);

            // 顯示在 TextBox2
            textbox2.Text = json;


        }

        private async void BtnCallApi_Click(object sender, EventArgs e)
        {
            // 1. 建立 RestSharp Client
            var client = new RestClient("https://jsonplaceholder.typicode.com");

            // 2. 建立 Request
            var request = new RestRequest("todos/1", Method.Get);

            try
            {
                // 3. 呼叫 API
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    // 4. JSON 轉物件 (dynamic)
                    var obj = JsonConvert.DeserializeObject(response.Content);

                    // 5. 格式化 JSON 顯示
                    string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented);

                    txtResult.Text = prettyJson;
                }
                else
                {
                    txtResult.Text = $"API 呼叫失敗：{response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = $"發生錯誤：{ex.Message}";
            }
        }
        //========================================================================================
        //button4 用到的list<>
        List<Student> students = new List<Student> 
        {
            new Student { Id = 1, Name = "小明", Score = 80, ClassName = "A" },
            new Student { Id = 2, Name = "小華", Score = 60, ClassName = "A" },
            new Student { Id = 3, Name = "小美", Score = 92, ClassName = "B" },
            new Student { Id = 4, Name = "小李", Score = 70, ClassName = "B" },
            new Student { Id = 5, Name = "小強", Score = 92, ClassName = "A" }
        };

        //list<> 用到的類別
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public float Score { get; set; }
            public string ClassName { get; set; }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var highScoreStudents = from s in students
                                    where s.Score >= 80
                                    orderby s.Score descending
                                    select s;
            //var highScores = students.Where(s => s.Score >= 80);


            // 顯示結果
            dataGridView1.DataSource = highScoreStudents.ToList();

        }

        private void Button5_Click(object sender, EventArgs e)
        {
             var sorted = students
                .OrderByDescending(s => s.Score)   // 先照分數由高到低
                .ThenBy(s => s.Name);              // 分數一樣再依姓名

            dataGridView1.DataSource = sorted.ToList();
        }

        private void Button6_Click(object sender, EventArgs e)
        {

            /*
            foreach (var g in groups)
            {
                Console.WriteLine($"班級: {g.Key}, 人數: {g.Count()}");
                foreach (var s in g)
                    Console.WriteLine($"   {s.Name} - {s.Score}");
            }
            */
            var result = from s in students
                         group s by s.ClassName into g       // 先依班級分組
                         select new
                         {
                             ClassName = g.Key,
                             Count = g.Count()
                         };

             dataGridView1.DataSource= result.ToList();

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            var result = students
                .Select(s => new { s.Name, s.Score })  // 選取 Name 和 Score
                .ToList();                              // 轉成 List 才能使用或綁到 DataGridView


            dataGridView1.DataSource = result;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Form2 mainform = new Form2();
            mainform.Show();

            this.Hide();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = students;
        }

        
           
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // 結束應用程式
        }
    }


}

