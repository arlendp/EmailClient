﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace MyEmail
{
    public partial class draft : Form
    {
        public draft()
        {
            InitializeComponent();
        }
        string strCon;
        OleDbConnection sqlCon;
        private void DBConnect()
        {
            strCon = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + Application.StartupPath + @"\MyEmail.mdb";
            sqlCon = new OleDbConnection(strCon);
        }
        private void draft_Load(object sender, EventArgs e)
        {
            DBConnect();
            OleDbDataAdapter da = new OleDbDataAdapter("select 收件人,主题,时间 from draft where 发件人='" + login.User + "'", sqlCon);//("select username as 用户名," + "password as 密码,realname as 真实姓名 from emailuser", sqlCon);
            DataSet ds = new DataSet();
            da.Fill(ds, "tablename");
            dataGridView1.DataSource = ds.Tables["tablename"]; //将数据库中草稿箱数据填充至datagridview
        }
           
       
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//双击某一行显示该草稿的详细信息
        {
            try
            {
                string body;
                sendmail senddraft = new sendmail();
                senddraft.Text = "草稿信息";
                senddraft.txtTo.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                senddraft.txtSubject.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                DBConnect();
                OleDbDataAdapter da = new OleDbDataAdapter("select 内容 from draft where 时间='" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", sqlCon);//("select username as 用户名," + "password as 密码,realname as 真实姓名 from emailuser", sqlCon);
                DataSet ds = new DataSet();
                da.Fill(ds, "tablename");
                body = ds.Tables["tablename"].Rows[0]["内容"].ToString();
                senddraft.webbody.Document.Write(body);//显示草稿的内容
                senddraft.Show();
            }
            catch
            {
                
            }
            
        }
        public static int index;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)//获取鼠标点击的单元格的行号
        {
            index = e.RowIndex;
        }

        private void deletedraft_Click(object sender, EventArgs e)//删除选中的草稿箱邮件
        {
            try
            {
                if (MessageBox.Show("确定删除邮件吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    DBConnect();
                    sqlCon.Open();
                    OleDbCommand cmd = new OleDbCommand("delete from draft where 发件人='" + login.User + "'and 收件人='" + dataGridView1.Rows[index].Cells[0].Value.ToString() + "' and 时间='" + dataGridView1.Rows[index].Cells[2].Value.ToString() + "'", sqlCon);
                    cmd.ExecuteNonQuery ();
                    sqlCon.Close ();
                    MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    draft_Load(sender, e);
                }
            }
            catch
            {
                MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }               
    }
}
