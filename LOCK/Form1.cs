using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOCK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileContent;

            for (int i = 0; i < listBox3.Items.Count; i++)
            {

                //读文件
                Console.WriteLine("--{0}", "StreamReader");
                using (StreamReader sr = new StreamReader(@listBox3.Items[i].ToString()))
                {
                    fileContent = string.Empty;
                    string strLine = string.Empty;
                    while (strLine != null)
                    {
                        strLine = sr.ReadLine();
                      
                        fileContent +=strLine   + " \r\n";
                    }
                    // fileContent = AesEncrypt(fileContent,"abc");
                    // richTextBox1.Text = fileContent;
                    fileContent=Encrypt(fileContent);
                }
                //删除原文件
                if (File.Exists(@listBox3.Items[i].ToString()))
                {
                    File.Delete(@listBox3.Items[i].ToString());
                }

                //写文件
                File.WriteAllText(@listBox3.Items[i].ToString()+".knclock", fileContent);


               richTextBox1.Text += "已SD"+ i+1 + "个文件 \n";
            
                //
            }

            if (richTextBox1.Text != null) button3.Enabled = true;


        }
        public static string MD5Encrypt(string originalString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] palindata = Encoding.Default.GetBytes(originalString);
            byte[] encryptdata = md5.ComputeHash(palindata);

            return Convert.ToBase64String(encryptdata);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            String getmulu;
            getmulu = textBox1.Text;
            if(textBox1.Text == "") MessageBox.Show("错误78：请输入目录！");
            else {
            DirectoryInfo theFolder = new DirectoryInfo(@getmulu);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();

            DirectoryInfo theFolder1 = new DirectoryInfo(@getmulu);
            FileInfo[] fileInfo1 = theFolder.GetFiles();
            foreach (FileInfo NextFile in fileInfo1)
            { //遍历文件
                this.listBox2.Items.Add(NextFile.Name);

                //限定遍历的文件类型
                if (NextFile.Name.Substring(NextFile.Name.Length - 3) == "txt"

                            //  || NextFile.Name.Substring(NextFile.Name.Length - 3) == "doc" ||
                            // NextFile.Name.Substring(NextFile.Name.Length - 4) == "docx"
                            )
                        this.listBox3.Items.Add(@getmulu + "/" + NextFile.Name);
            }



            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                try
                {
                    this.listBox1.Items.Add(NextFolder.Name);
                    FileInfo[] fileInfo = NextFolder.GetFiles();
                    foreach (FileInfo NextFile in fileInfo)
                    { //遍历文件
                        this.listBox2.Items.Add(NextFile.Name);

                        //限定遍历的文件类型
                       if( NextFile.Name.Substring( NextFile.Name.Length - 3)=="txt"
                               // || NextFile.Name.Substring(NextFile.Name.Length - 3) == "doc" ||
                            //NextFile.Name.Substring(NextFile.Name.Length - 4) == "docx"
                            )
                        this.listBox3.Items.Add(@getmulu + "/" + NextFolder.Name + "/" + NextFile.Name);
                    }
                }
                catch { }

            }
            if (listBox3.Items.Count != 0)
            {
                button1.Enabled = true;
                button3.Enabled = true;

            }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789019");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789019");
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        //JS文件
        private void button3_Click(object sender, EventArgs e)
        {
            string fileContent;

            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                String JS_Mulu = @listBox3.Items[i].ToString()+".knclock";
                String JS_Mulu_1 = @listBox3.Items[i].ToString() ;
                //读文件
                Console.WriteLine("--{0}", "StreamReader");
                using (StreamReader sr = new StreamReader(JS_Mulu))
                {
                    fileContent = string.Empty;
                    string strLine = string.Empty;
                    while (strLine != null)
                    {
                        strLine = sr.ReadLine();

                        fileContent += strLine + " \r\n";
                    }
                    // fileContent = AesEncrypt(fileContent,"abc");
                    // richTextBox1.Text = fileContent;
                    fileContent = Decrypt(fileContent);
                }
                //删除原文件
                if (File.Exists(JS_Mulu))
                {
                    File.Delete(JS_Mulu);
                }

                //写文件
                File.WriteAllText(JS_Mulu_1, fileContent);


                richTextBox1.Text += "已JS" + i + 1 + "个文件 \n";
         
                //
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 关于文件锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("联系我，QQ 308346293");
        }


        ///////
    }
}
