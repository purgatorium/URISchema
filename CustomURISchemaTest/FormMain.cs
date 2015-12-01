// reference: https://msdn.microsoft.com/en-us/library/aa767914(v=vs.85).aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;


namespace CustomURISchemaTest
{
    public partial class FormMain : Form
    {
        string[] arguments;

        public FormMain(string[] args)
        {
            InitializeComponent();
            arguments = args;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            textBox1.Text = Environment.CommandLine + Environment.NewLine + Environment.NewLine;

            string arguments_only = Environment.CommandLine.Substring(Environment.CommandLine.LastIndexOf("mytro.recplayer:\"") + "mytro.recplayer:\"".Length);
            arguments_only = arguments_only.Substring(0, arguments_only.Length - 1);

            Encoding encoding = null;

            ////////////////////////////////////////
            // 첫번째 글자가 "뷁" 인지 확인한다.
            ////////////////////////////////////////

            // 65001: UTF8 코드페이지
            if (HttpUtility.UrlDecode(arguments_only, Encoding.GetEncoding(65001)).Substring(0, 1).Equals("뷁"))
            {
                encoding = Encoding.GetEncoding(65001);
                textBox1.AppendText("UTF8 인코딩 사용" + Environment.NewLine + Environment.NewLine);
            }
            // 949: ks_c_5601-1987 윈도우 한글 코드페이지
            else if (HttpUtility.UrlDecode(arguments_only, Encoding.GetEncoding(949)).Substring(0, 1).Equals("뷁"))
            {
                encoding = Encoding.GetEncoding(949);
                textBox1.AppendText("ks_c_5601 인코딩 사용" + Environment.NewLine + Environment.NewLine);
            }
            // 51949: euc-kr 리눅스한글 코드페이지 euc-kr
            else if (HttpUtility.UrlDecode(arguments_only, Encoding.GetEncoding(51949)).Substring(0, 1).Equals("뷁"))
            {
                encoding = Encoding.GetEncoding(51949);
                textBox1.AppendText("euc-kr 인코딩 사용" + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                // 뭔가 다른 인코딩이거나 첫글자에 "뷁"을 사용하지 않았다.
                textBox1.AppendText("인식불가" + Environment.NewLine + Environment.NewLine);
                return;
            }


            foreach (string arg in arguments)
            {
                string decode = HttpUtility.UrlDecode(arg, encoding);

                // 첫번째 argument에는 항상 URI스키마가 같이 붙어서 온다.
                string real_arg = decode.Replace("mytro.recplayer:뷁", "");


                // 실제 사용할 argument를 출력한다.
                textBox1.AppendText(string.Format("[{0}] -> [{1}]" + Environment.NewLine, arg, real_arg));
            }
        }
    }
}
