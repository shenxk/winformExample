using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace winForm01
{
    public partial class Form1 : Form
    {
        bool textIsChanged = false;
        string textFileName = "";

        public static Form1 mainWindow = null;
        

        public Form1()
        {
            InitializeComponent();
            mainWindow = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string appCfgPath = Application.StartupPath + "\\data\\app.ini";
            if (File.Exists(appCfgPath))
            {
                StreamReader sr = new StreamReader(appCfgPath);
                int x = Convert.ToInt32(sr.ReadLine());
                int y = Convert.ToInt32(sr.ReadLine());
                int w = Convert.ToInt32(sr.ReadLine());
                int h= Convert.ToInt32(sr.ReadLine());
                int r = Convert.ToInt32(sr.ReadLine());
                int g = Convert.ToInt32(sr.ReadLine());
                int b = Convert.ToInt32(sr.ReadLine());

                this.Size = new Size(w,h);
                this.Location = new Point(x, y);
                textTest.ForeColor = Color.FromArgb(r, g, b);
                sr.Close();


            }
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textTest.Text!=""){
                //Process processes = Process.GetCurrentProcess();
                string name =  Assembly.GetExecutingAssembly().GetName().CodeBase;
                Process.Start(name);
                
            }
           
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "打开文件";
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //声明文件流对象
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                //创建读写器
                StreamReader sr = new StreamReader(fs,Encoding.Default);
                //读操作 EndofStream
                textTest.Text = sr.ReadToEnd();
                //关闭读取器
                sr.Close();
                //关闭文件流
                fs.Close();
                textFileName = ofd.FileName;
                textIsChanged = false;
            }
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveText();
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //if (textIsChanged)
            //{
            //    if (this.textTest.Text == "" && textFileName == "")
            //    {
            //        return;
            //    }
            //    DialogResult re = MessageBox.Show("文本内容已发生改变是否保存", "询问", MessageBoxButtons.YesNoCancel);
            //    if (re == DialogResult.Yes)
            //    {
            //        saveText();
            //        this.Close();
            //    }
            //    else if (re == DialogResult.No)
            //    {
            //        this.Close();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }

        private void textTest_TextChanged(object sender, EventArgs e)
        {
            textIsChanged = true;
        }
        protected void saveText()
        {
            if (textFileName == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "保存文件";
                sfd.Filter = "文本文件|*.txt|所有文件|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(textTest.Text,Encoding.Default);

                    sw.Close();
                    fs.Close();
                    textIsChanged = false;
                }
            }
            else
            {
                FileStream fs = new FileStream(textFileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(textTest.Text);

                sw.Close();
                fs.Close();
                textIsChanged = false;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textIsChanged)
            {
                if (this.textTest.Text == ""&& textFileName=="")
                {
                    return;
                }
                DialogResult re = MessageBox.Show("文本内容已发生改变是否保存", "询问", MessageBoxButtons.YesNoCancel);
                if (re == DialogResult.Yes)
                {
                    saveText();
                }
                else if (re == DialogResult.No)
                {
                }
                else
                {
                    e.Cancel = true;
                }
            }
            int x = this.Location.X;
            int y = this.Location.Y;
            int w = this.Size.Width;
            int h = this.Size.Height;
            int r = textTest.ForeColor.R;
            int g = textTest.ForeColor.G;
            int b = textTest.ForeColor.B;


            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\data\\app.ini",false);
            sw.WriteLine(x.ToString());
            sw.WriteLine(y.ToString());
            sw.WriteLine(w.ToString());
            sw.WriteLine(h.ToString());
            sw.WriteLine(r.ToString());
            sw.WriteLine(g.ToString());
            sw.WriteLine(b.ToString());

            sw.Close();
            

        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textTest.Copy();
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textTest.Cut();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textTest.Paste();
        }

        private void 查找FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currPos = textTest.SelectionStart;
            Form2 find = new Form2();
            
            find.Show();
        }
        int currPos=1;
        public void find(string data) {
            
            int findPos = textTest.Text.IndexOf(data, currPos);
            
            if (findPos != -1) {
                textTest.Select(findPos,2);
                textTest.Copy();
                currPos = findPos+1;
            }
            mainWindow.Activate();
        }

        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = textTest.Font;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textTest.Font= fd.Font;
            }
        }

        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = textTest.ForeColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                textTest.ForeColor = cd.Color;
            }
        }
    }
}
