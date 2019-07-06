using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace lwtool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        public static string googleTranslation(string text)
        {
            if (text == "" || text == null)
            {
                return "";
            }
            else
            {
                string result = "";
                string url = "https://translate.google.cn/translate_a/single?client=gtx&sl=en&tl=zh-CN&dt=t&q=" + text;
                string jsonData = GetInfo(url);
                //return jsonData;
                string pattern = "\"([^\"]*)\"";
                int count = Regex.Matches(jsonData, pattern).Count;
                MatchCollection matches = Regex.Matches(jsonData, pattern);
                for (int i = 0; i < count - 1; i += 2)
                {
                    //result += matches[i].Value.Trim('\"');
                    //result+="\n";
                    if (matches[i].Value.Trim('\"') == "2876b4f2653853cee22abadb2a855b9a")
                        continue;
                    else
                        result += matches[i].Value.Trim().Replace("\"", "");
                }


                return result;
            }
        }
        public static bool InChinese(string StrChineseString)
        {
            return Regex.IsMatch(StrChineseString, ".*[\\u4e00-\\u9faf].*");
        }
        public static string GetInfo(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //访问http方法  
            string strBuff = "";
            Uri httpURL = new Uri(url);
            ///HttpWebRequest类继承于WebRequest，并没有自己的构造函数，需通过WebRequest的Creat方法建立，并进行强制的类型转换     
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            ///通过HttpWebRequest的GetResponse()方法建立HttpWebResponse,强制类型转换     
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
            ///GetResponseStream()方法获取HTTP响应的数据流,并尝试取得URL中所指定的网页内容     
            ///若成功取得网页的内容，则以System.IO.Stream形式返回，若失败则产生ProtoclViolationException错误。在此正确的做法应将以下的代码放到一个try块中处理。这里简单处理     
            Stream respStream = httpResp.GetResponseStream();
            ///返回的内容是Stream形式的，所以可以利用StreamReader类获取GetResponseStream的内容，并以     
            //StreamReader类的Read方法依次读取网页源程序代码每一行的内容，直至行尾（读取的编码格式：UTF8）     
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
            strBuff = respStreamReader.ReadToEnd();
            return strBuff;
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开  
            return true;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //StringCollection lines = new StringCollection();

            string lines = "";
            //int lineCount = TextBox1.LineCount;
            ////s.Substring(0, s.Length - 1)
            //for (int line = 0; line < lineCount; line++)
            //    lines += TextBox1.GetLineText(line);
            //lines.Replace("\n", "");
            //lines.Replace("\r", "");
            lines = TextBox1.Text.Replace(@"\n", "");
            lines = lines.Replace(@"\r", "");
            lines = lines.Replace(Environment.NewLine, "");

            //TextBox1.Text = lines;
            //TextBox1.Focus();

            //string t = "";
            //HandWhiteTarans ht = new HandWhiteTarans();
            //t = ht.TranslateEnglishToChinese(lines);
            //TextBox1.Text = t;
            //TextBox1.Text = lines;
            TextBox1.Text = googleTranslation(lines);
            TextBox1.Focus();

        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Tab")
            {
                Console.WriteLine('1');
                string lines = "";
                //int lineCount = TextBox1.LineCount;
                ////s.Substring(0, s.Length - 1)
                //for (int line = 0; line < lineCount; line++)
                //    lines += TextBox1.GetLineText(line);
                //lines.Replace("\n", "");
                //lines.Replace("\r", "");
                lines = TextBox1.Text.Replace(@"\n", "");
                lines = lines.Replace(@"\r", "");
                lines = lines.Replace(Environment.NewLine, "");

                //TextBox1.Text = lines;
                //TextBox1.Focus();

                //string t = "";
                //HandWhiteTarans ht = new HandWhiteTarans();
                //t = ht.TranslateEnglishToChinese(lines);
                //TextBox1.Text = t;

                TextBox1.Text = googleTranslation(lines);
                TextBox1.Focus();
                //TextBox1.Select(5, 0);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
