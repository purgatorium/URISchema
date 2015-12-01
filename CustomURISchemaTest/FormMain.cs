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

            foreach (string arg in arguments)
            {
                // 한글 사용시 뷁 같은거 한글자 무조건 넣으면 utf8인지 949인지 euckr인지 확인 가능
                string src = arg.Replace("mytro.recplayer:", "");

                // 65001: UTF8 코드페이지
                string decode = HttpUtility.UrlDecode(src, Encoding.GetEncoding(65001));
                if (false == decode.Substring(0, 1).Equals("뷁"))
                {
                    // 949: 윈도우 한글 코드페이지
                    decode = HttpUtility.UrlDecode(src, Encoding.GetEncoding(949));
                    if (false == decode.Substring(0, 1).Equals("뷁"))
                    {
                        // 51949: 리눅스한글 코드페이지 euc-kr
                        decode = HttpUtility.UrlDecode(src, Encoding.GetEncoding(51949));
                        if (false == decode.Substring(0, 1).Equals("뷁"))
                        {
                            // 뭔가 다른 인코딩이거나 첫글자에 "뷁"을 사용하지 않았다.
                            continue;
                        }
                    }
                }

                // 실제 사용할 argument를 출력한다.
                textBox1.AppendText(decode.Replace("뷁", "") + Environment.NewLine);
            }
        }
    }
}
