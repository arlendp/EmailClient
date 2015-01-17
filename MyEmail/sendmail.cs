using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Mail;
using System.Data.OleDb;
namespace MyEmail
{
    public partial class sendmail : Form
    {
        public sendmail()
        {
            InitializeComponent();
            txtSend.Text = login.User;
            txtServer.Text = login.Smtp;
            webbody.Navigate("about:blank");
            mshtml.IHTMLDocument2 doc = this.webbody.Document.DomDocument as mshtml.IHTMLDocument2;
            if (doc != null)
            {
                doc.designMode = "on";//打开webbrowser可编辑内容的属性
            }
        }
        private void btnSend_Click(object sender, EventArgs e)//发送按钮函数
        {
            try
            {
                MailMessage message = null;
                if (txtTo.Text.IndexOf(";") != -1)//若为多收件人，分开记录各个收件人
                {
                    string[] strEmail = txtTo.Text.Split(';');
                    string sumEmail = "";
                    for (int i = 0; i < strEmail.Length; i++)
                    {
                        sumEmail = strEmail[i];
                        message = new MailMessage(new MailAddress(txtSend.Text), new MailAddress(sumEmail));//实例化mailmessage类
                        SendEmail(message);
                    }
                }
                else//只有一个收件人的情况
                {
                    message = new MailMessage(new MailAddress(txtSend.Text), new MailAddress(txtTo.Text));
                    SendEmail(message);
                }
                MessageBox.Show("发送成功！");
                try
                {
                    DBConnect();
                    sqlCon.Open();
                    OleDbCommand cmd = new OleDbCommand("insert into send(发件人,收件人,主题,内容,时间) values ('" + txtSend.Text + "','" + txtTo.Text + "','" + txtSubject.Text + "','" + webbody.DocumentText + "','" + DateTime.Now.ToLocalTime().ToString() + "')", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();//将已发送邮件存入数据库         
                }
                catch
                {
                    MessageBox.Show("保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch 
            {
                MessageBox .Show ("发送失败！");
            }
        }
        private void SendEmail(MailMessage message)//发送邮件函数
        {
            message .Subject = txtSubject .Text ;
            message.Body = webbody.DocumentText; 
            message.SubjectEncoding = System.Text.Encoding.GetEncoding("gb18030");
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Priority = System.Net.Mail.MailPriority.High;
            message.IsBodyHtml = true;//邮件头及主题、内容编码信息
            
            if (listBox1.Items.Count > 0)//记录添加的附件信息
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    AddFile(listBox1.Items[i].ToString(),message);//通过addfile函数添加附件
                }
            }
            SmtpClient client=new SmtpClient (txtServer .Text ,25);//通过25号端口连接SMTP服务器
            client .Credentials =new System.Net .NetworkCredential (txtSend .Text  ,login .Password  );//用户名和密码的认证
            client .Send (message );
        }

        private void btnSelect_Click(object sender, EventArgs e)//选择附件函数
        {        
            openFileDialog.Title = "添加附件";
            if (openFileDialog .ShowDialog ()==DialogResult .OK )//用户进行了选择，即点击了打开按钮
            {
                if (openFileDialog .FileNames !=null)//将选择的文件路径写入listbox中
                {
                    listBox1.Items.AddRange(openFileDialog .FileNames );
                }
            }
        }
        private void AddFile(string strFile,MailMessage message)//添加附件函数
        {
            Attachment myAttachment=new Attachment (strFile ,System.Net.Mime .MediaTypeNames .Application .Octet );
            myAttachment .NameEncoding =System .Text .Encoding .GetEncoding ("gb18030");//附件名编码
            System .Net .Mime .ContentDisposition disposition=myAttachment .ContentDisposition ;
            disposition .CreationDate =System .IO .File .GetCreationTime (strFile );
            disposition .ModificationDate =System .IO.File.GetLastWriteTime (strFile );
            disposition .ReadDate =System .IO .File .GetLastAccessTime (strFile );
            message.Attachments .Add (myAttachment );
        }

        private void btnDeAtt_Click(object sender, EventArgs e)//删除附件函数
        {
            for (int i = listBox1.SelectedItems.Count - 1; i > -1; i--)
            {
                listBox1.Items.Remove(listBox1.SelectedItems[i]);//从listbox中清除选中的项
            }
        }

        string strCon;
        OleDbConnection sqlCon;
        private void DBConnect()
        {
            strCon = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + Application.StartupPath + @"\MyEmail.mdb";
            sqlCon = new OleDbConnection(strCon);
        }
        private void save_Click(object sender, EventArgs e)//保存邮件到草稿箱
        {
            try
            {
                DBConnect();
                sqlCon.Open();
                OleDbCommand cmd = new OleDbCommand("insert into draft(发件人,收件人,主题,内容,时间) values ('" + txtSend.Text + "','" + txtTo.Text + "','" + txtSubject.Text + "','" + webbody.DocumentText + "','" + DateTime.Now.ToLocalTime().ToString() + "')", sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       
    }
}
