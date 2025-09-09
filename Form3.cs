using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Ybsfsb
{
    public partial class Form3qtjk : Form
    {
        public Form3qtjk()
        {
            InitializeComponent();
        }

        private void hqybzxsj_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建 COM 对象
                Type comType = Type.GetTypeFromProgID("yinhai.TAIYUAN.interface");
                if (comType == null)
                {
                    MessageBox.Show("请检查医保组件是否安装有问题！");
                    return;
                }

                dynamic yinhaiobject = Activator.CreateInstance(comType);

                // 准备参数
                string BusinessID = "52";
                string Dataxml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";               // 构造你的参数
                string Businesssequence = "";
                string Businessvalidate = "";
                string Outputxml = "";
                long Appcode = 0;
                string Appmsg = " ";

                // 调用方法
                yinhaiobject.yh_interface_init("10086", "10010");

                // 创建等待框
                Form waitForm = new Form()
                {
                    Text = "查询",
                    Size = new Size(400, 200),
                    StartPosition = FormStartPosition.CenterScreen,
                    ControlBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog
                };
                Label label = new Label()
                {
                    Text = "正在连接医保网查询中，请稍候...",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                waitForm.Controls.Add(label);
                // 显示等待框（非模态，防止阻塞 UI）
                waitForm.Show();
                waitForm.Refresh();



                yinhaiobject.yh_interface_call(
                    BusinessID,
                    Dataxml,
                  ref Businesssequence,
                 ref Businessvalidate,
                  ref Outputxml,
                   ref Appcode,
                  ref Appmsg
                );



                if (!string.IsNullOrEmpty(Appmsg))
                {
                    waitForm.Close();
                    string message = $"【医保接口提示】\n{Appmsg}\n";
                    MessageBox.Show(message, "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!string.IsNullOrEmpty(Outputxml))
                {

                    // 加载 XML 字符串
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Outputxml);  // Outputxml 是你的 XML 字符串

               
                    
  


                    XmlNodeList rowNodes = doc.SelectNodes("//sqldata/row/prm_sysdate");

                    if (rowNodes != null && rowNodes.Count > 0)
                    {
                        // 假设你只关心第一个 <row> 节点
                        XmlNode firstRow = rowNodes[0];
                        string prm_sysdate = firstRow.InnerText;
                        // 显示在文本框中
                        MessageBox.Show("当前医保中心时间为："+ prm_sysdate, "解析结果");
                    }
                    else
                    {
                        MessageBox.Show("未找到 prm_sysdate 节点。");
                    }




                }
                waitForm.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("调用失败：" + ex.Message);
            }
        }
    }
}
