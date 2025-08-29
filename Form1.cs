using System.Data;
using System.Xml;

namespace Ybsfsb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                string BusinessID = "03";
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

                    // 获取根节点 <output>
                    XmlNode root = doc.SelectSingleNode("output");

                    // 安全地取出某些字段值，并赋值到控件
                    if (root != null)
                    {
                        xm.Text = root.SelectSingleNode("prm_aac003")?.InnerText ?? "";
                        //  xb.Text = root.SelectSingleNode("prm_aac004")?.InnerText ?? "";
                        var xbValue = root.SelectSingleNode("prm_aac004")?.InnerText ?? "";

                        if (xbValue == "1")
                        {
                            xb.Text = "男";
                        }
                        else if (xbValue == "2")
                        {
                            xb.Text = "女";
                        }
                        else
                        {
                            xb.Text = "其它"; // 其他值显示空
                        }

                        nl.Text = root.SelectSingleNode("prm_akc023")?.InnerText ?? "";
                        sfzh.Text = root.SelectSingleNode("prm_aac002")?.InnerText ?? "";
                        dz.Text = root.SelectSingleNode("prm_aac006")?.InnerText ?? "";
                        dwmc.Text = root.SelectSingleNode("prm_aab004")?.InnerText ?? "";
                        grzhye.Text = root.SelectSingleNode("prm_akc087")?.InnerText ?? "";
                        fzxbh.Text = root.SelectSingleNode("prm_yab003")?.InnerText ?? "";
                        zhgjrxm.Text = root.SelectSingleNode("prm_auther_name")?.InnerText ?? "";
                        zhgjrsfzh.Text = root.SelectSingleNode("prm_auther_certno")?.InnerText ?? "";
                        gjrcbqh.Text = root.SelectSingleNode("prm_auther_insu_admdvs")?.InnerText ?? "";
                        mzljbxje.Text = root.SelectSingleNode("prm_yka128")?.InnerText ?? "";


                    }
                    // 自定义参数中文名
                    Dictionary<string, string> customHeaders = new Dictionary<string, string>
{
    { "akb020", "机构编码" },
    { "yab003", "统筹区域编码" },
    { "yka026", "慢病编码" },
    { "yka027", "慢病名称" },
    { "aae030", "开始日期" },
    { "aae031", "结束日期" },
    { "yka260", "备用" },
    { "ykd112", "备用1" },
    { "yka068", "备用2" },
    { "ykd111", "备用3" }

};
                    // 自定义参数中文名
                    Dictionary<string, string> renyuanbiaoti = new Dictionary<string, string>
{
    { "ykc296", "人员类别" },
    { "ykc297", "人员类别名称" },
    { "aae030", "开始时间" },
    { "aae031", "结束时间" }


};

                    XmlNodeList rowNodes = doc.SelectNodes("//prm_ykc010/row");

                    if (rowNodes.Count == 0) return;

                    DataTable dt = new DataTable();

                    // 获取所有列名（从第一个 row 节点提取）
                    foreach (XmlNode child in rowNodes[0].ChildNodes)
                    {
                        string colName = child.Name;
                        if (!dt.Columns.Contains(colName))
                            dt.Columns.Add(colName);
                    }

                    // 解析每行数据
                    foreach (XmlNode row in rowNodes)
                    {
                        DataRow dr = dt.NewRow();
                        bool skipRow = false; // 是否跳过本行
                        foreach (XmlNode child in row.ChildNodes)
                        {
                            if (child.Name == "yka027" && child.InnerText == "特药病种")
                            {
                                skipRow = true;
                                break; // 提前退出内层循环
                            }
                            dr[child.Name] = child.InnerText;

                        }
                        if (!skipRow)
                        {
                            dt.Rows.Add(dr);

                        }
                    }

                    // 显示在 DataGridView 上  慢病数据框
                    mbsjk.DataSource = dt;
                    waitForm.Close();



                    // 设置中文列名
                    foreach (DataGridViewColumn col in mbsjk.Columns)
                    {
                        if (customHeaders.ContainsKey(col.Name))
                            col.HeaderText = customHeaders[col.Name];
                    }
                    //下面是人员类别数据框




                    XmlNodeList ryrowNodes = doc.SelectNodes("//prm_ykc296/row");

                    // 如果没有数据行，先清空 DataGridView
                    if (ryrowNodes.Count == 0)
                    {
                        rysjk.DataSource = null;   // 清空数据源
                        rysjk.Rows.Clear();        // 清空所有行
                        rysjk.Refresh();           // 刷新界面
                        waitForm.Close();

                        return;
                    }


                    DataTable dtry = new DataTable();

                    // 获取所有列名（从第一个 row 节点提取）
                    foreach (XmlNode child in ryrowNodes[0].ChildNodes)
                    {
                        string colName = child.Name;
                        if (!dtry.Columns.Contains(colName))
                            dtry.Columns.Add(colName);
                    }

                    // 解析每行数据
                    foreach (XmlNode row in ryrowNodes)
                    {
                        DataRow dr = dtry.NewRow();
                        foreach (XmlNode child in row.ChildNodes)
                        {
                            dr[child.Name] = child.InnerText;
                        }
                        dtry.Rows.Add(dr);
                    }

                    // 显示在 DataGridView 上
                    rysjk.DataSource = dtry;


                    // 设置中文列名
                    foreach (DataGridViewColumn col in rysjk.Columns)
                    {
                        if (renyuanbiaoti.ContainsKey(col.Name))
                            col.HeaderText = renyuanbiaoti[col.Name];
                    }

                }
                waitForm.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("调用失败：" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); // 创建 Form2 实例

            form2.Show(); // 方式1：非模态显示（两个窗体都能操作）
                          // form2.ShowDialog(); // 方式2：模态显示（必须先关掉 Form2 才能回到 Form1）
        }
    }
}
