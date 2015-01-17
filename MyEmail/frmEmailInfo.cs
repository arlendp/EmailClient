using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyEmail
{
    public partial class frmEmailInfo : Form
    {
        public frmEmailInfo()
        {
            InitializeComponent();
            webBrowser.Navigate("about:blank");//初始化webbrowser控件
        }

        private void frmEmailInfo_Load(object sender, EventArgs e)//将邮件的各信息填充到对应的控件中
        {
            txtFrom.Text = frmMain.strFrom;
            txtSubject.Text = frmMain.strSubject;
            txtAttachment.Text = frmMain.strAttachment;
            txtDate.Text = frmMain.strDate;
            if (frmMain.mailMessage.HTMLBody == null)
            {
                webBrowser.Document.Write(frmMain.mailMessage.Body);//将邮件内容写入webbrowser控件
            }
            else
            {
                webBrowser.Document.Write(frmMain.mailMessage.HTMLBody);
            }            
        }
  
        private void remailbutton_Click(object sender, EventArgs e)//点击回复按钮，自动填充回复的文本格式并调用发送窗体
        {
            sendmail remail = new sendmail();
            remail.Text = "Re:" + txtSubject.Text + " - 写邮件";
            remail.txtTo.Text = txtFrom.Text;
            remail.txtSubject.Text = "Re:" + txtSubject.Text;
            if (frmMain.mailMessage.HTMLBody == null)
            {
                remail.webbody.Document.Write("<br><br><br>---------原始邮件---------<br>发件人：" + txtFrom.Text + "<br>发送时间：" + txtDate.Text + "<br>收件人：" + login.User + "<br>主题：" + txtSubject.Text + "<br>--------------------------" + "<br>" + frmMain.mailMessage.Body);
            }
            else
            {
                remail.webbody.Document.Write("<br><br><br>---------原始邮件---------<br>发件人：" + txtFrom.Text + "<br>发送时间：" + txtDate.Text + "<br>收件人：" + login.User + "<br>主题：" + txtSubject.Text + "<br>--------------------------" + frmMain.mailMessage.HTMLBody);
            }
            remail.Show();            
        }

        private void resendbutton_Click(object sender, EventArgs e)//点击转发按钮，自动填充转发的文本格式并调用发送窗体
        {
            sendmail resend = new sendmail();
            resend.Text = "Fw:" + txtSubject.Text + " - 写邮件";
            resend.txtSubject.Text = "Fw:" + txtSubject.Text;
            if (frmMain.mailMessage.HTMLBody == null)
            {
                resend.webbody.Document.Write("<br><br><br>---------原始邮件---------<br>发件人：" + txtFrom.Text + "<br>发送时间：" + txtDate.Text + "<br>收件人：" + login.User + "<br>主题：" + txtSubject.Text + "<br>--------------------------" + "<br>" + frmMain.mailMessage.Body);
            }
            else
            {
                resend.webbody.Document.Write("<br><br><br>---------原始邮件---------<br>发件人：" + txtFrom.Text + "<br>发送时间：" + txtDate.Text + "<br>收件人：" + login.User + "<br>主题：" + txtSubject.Text + "<br>--------------------------" + frmMain.mailMessage.HTMLBody);
            }
            resend.Show();       
        }
    }
}
