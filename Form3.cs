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
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;

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

                    // 加载 XML 字1符串
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Outputxml);  // Outputxml 是你的 XML 字符串






                    XmlNodeList rowNodes = doc.SelectNodes("//sqldata/row/prm_sysdate");

                    if (rowNodes != null && rowNodes.Count > 0)
                    {
                        // 假设你只关心第一个 <row> 节点
                        XmlNode firstRow = rowNodes[0];
                        string prm_sysdate = firstRow.InnerText;
                        // 显示在文本框中
                        waitForm.Close();
                        MessageBox.Show("当前医保中心时间为：" + prm_sysdate, "解析结果");
                    }
                    else
                    {
                        MessageBox.Show("未找到 prm_sysdate 节点。");
                    }




                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("调用失败：" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string businessId = "04";
            string dataXml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";

            string appMsg, outputXml;
            bool success = CallInterface(businessId, dataXml, out appMsg, out outputXml);

            if (!string.IsNullOrEmpty(appMsg))
            {
                MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //取出这个的值。
            if (!string.IsNullOrEmpty(outputXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputXml);
                XmlNodeList nodes020 = doc.SelectNodes("//output/prm_akb020");
                //取出机构编码。
                string jgbm = string.Empty;
                if (nodes020 != null && nodes020.Count > 0)
                {
                    jgbm = nodes020[0].InnerText; // 取第一个
                }

                XElement xElement = new XElement("input",
                    new XElement("prm_akb020", jgbm),
                    new XElement("prm_outputfile", "D:/123nursecode.txt"),
                    new XElement("page_num", "1"),   // 开始日期
                    new XElement("page_size", "100") // 结束日期
                );
                string businessIdgb002 = "GB002";
                string dataXmlgb002 = xElement.ToString();
                string appMsggb002, outputXmlgb002;
                ShowWaitForm();
                CallInterface(businessIdgb002, dataXmlgb002, out appMsggb002, out outputXmlgb002);

                if (!string.IsNullOrEmpty(appMsggb002))
                {
                    CloseWaitForm();
                    MessageBox.Show($"【医保接口提示】\n{appMsggb002}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if (!string.IsNullOrEmpty(outputXmlgb002))
                //{
                //    CloseWaitForm();
                //    MessageBox.Show("成功生成文件D:/123nursecode.txt");
                //}
                if (!string.IsNullOrEmpty(outputXmlgb002))
                {

                    // {
                    //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
                    //"医保返回",
                    //MessageBoxButtons.OK,
                    //MessageBoxIcon.Information);
                    //     waitForm.Close();

                    // }
                    string filePath = "D:/123nursecode.txt";

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("文件不存在！");
                        return;
                    }

                    // 1. 读取所有行
                    string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("GBK"));

                    // 2. 创建 DataTable
                    DataTable dt = new DataTable();
                    // 定义固定表头映射（列索引 -> 列名）
                    Dictionary<int, string> headerMap = new Dictionary<int, string>
{
    { 0, "医疗机构编码" },
    { 1, "医疗机构名称" },
      { 2, "医保护士代码" },
    { 3, "姓名" },

    { 4, "性别" },
     { 5, "身份证件类型" },
    { 6, "身份证件号码" },
     { 7, "人员状态" },
    { 8, "合同起始时间" },
     { 9, "合同截止时间" },
      { 10, "护士执业证书编码" },
    { 11, "执业医疗机构名称" },
     { 12, "执业类别" },
     { 13, "专业技术职务" }

}
                ;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // 按制表符分割
                        string[] parts = line.Split('\t');

                        // 如果表头还没创建，就动态添加列
                        // 动态创建表头
                        if (dt.Columns.Count < parts.Length)
                        {
                            for (int i = dt.Columns.Count; i < parts.Length; i++)
                            {
                                if (headerMap.ContainsKey(i))
                                    dt.Columns.Add(headerMap[i]);   // 固定列名
                                else
                                    dt.Columns.Add("字段" + (i + 1)); // 默认列名
                            }
                        }

                        // 加入一行数据
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < parts.Length; i++)
                        {
                            dr[i] = parts[i];
                        }
                        dt.Rows.Add(dr);
                    }

                    // 3. 绑定 DataGridView
                    xzxxsjk.DataSource = dt;
                    CloseWaitForm();

                }






            }






        }


        public static bool CallInterface(string businessId, string dataXml, out string appMsg, out string outputXml)
        {
            appMsg = string.Empty;
            outputXml = string.Empty;

            // 创建 COM 对象
            Type comType = Type.GetTypeFromProgID("yinhai.TAIYUAN.interface");
            if (comType == null)
            {
                appMsg = "请检查医保组件是否安装有问题！";
                return false;
            }

            dynamic yinhaiobject = Activator.CreateInstance(comType);

            string businessSequence = string.Empty;
            string businessValidate = string.Empty;
            long appCode = 0;

            // 初始化
            yinhaiobject.yh_interface_init("10086", "10010");

            // 开始调用
            yinhaiobject.yh_interface_call(
                businessId,
                dataXml,
                ref businessSequence,
                ref businessValidate,
                ref outputXml,
                ref appCode,
                ref appMsg
            );

            return appCode == 0; // 约定：0 表示成功
        }

        private static Form waitForm;

        /// <summary>
        /// 显示等待框
        /// </summary>
        public static void ShowWaitForm()
        {
            // 防止重复创建
            if (waitForm != null && !waitForm.IsDisposed)
                return;

            waitForm = new Form()
            {
                Text = "查询",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen,
                ControlBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                TopMost = true
            };

            Label label = new Label()
            {
                Text = "正在连接医保网查询中，请稍候...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("微软雅黑", 12, FontStyle.Regular)
            };

            waitForm.Controls.Add(label);

            // 非模态显示，不阻塞 UI
            waitForm.Show();
            waitForm.Refresh();
        }

        /// <summary>
        /// 关闭等待框
        /// </summary>
        public static void CloseWaitForm()
        {
            if (waitForm != null && !waitForm.IsDisposed)
            {
                waitForm.Close();
                waitForm.Dispose();
                waitForm = null;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (xzxxsjk.Rows.Count == 0)
            {
                MessageBox.Show("没有可导出的数据！");
                return;
            }

            // 选择保存路径
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel文件|*.xlsx";
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.FileName = "医保联网下载数据.xlsx";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            // 使用 SXSSFWorkbook（流式写入，参数100表示内存中只保留100行1）
            SXSSFWorkbook workbook = new SXSSFWorkbook(100);
            ISheet sheet = workbook.CreateSheet("数据");

            // 写入表头
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < xzxxsjk.Columns.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(xzxxsjk.Columns[i].HeaderText);
            }

            // 写入内容
            for (int i = 0; i < xzxxsjk.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < xzxxsjk.Columns.Count; j++)
                {
                    object value = xzxxsjk.Rows[i].Cells[j].Value;
                    row.CreateCell(j).SetCellValue(value == null ? "" : value.ToString());
                }
            }

            // 保存到文件
            using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            // 释放临时文件资源
            workbook.Dispose();

            MessageBox.Show("导出成功！");
        }

        private void ysxxxz_Click(object sender, EventArgs e)
        {

            string businessId = "04";
            string dataXml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";

            string appMsg, outputXml;
            bool success = CallInterface(businessId, dataXml, out appMsg, out outputXml);

            if (!string.IsNullOrEmpty(appMsg))
            {
                MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //取出这个的值。
            if (!string.IsNullOrEmpty(outputXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputXml);
                XmlNodeList nodes020 = doc.SelectNodes("//output/prm_akb020");
                //取出机构编码。
                string jgbm = string.Empty;
                if (nodes020 != null && nodes020.Count > 0)
                {
                    jgbm = nodes020[0].InnerText; // 取第一个
                }

                XElement xElement = new XElement("input",
                    new XElement("prm_akb020", jgbm),
                    new XElement("prm_outputfile", "D:/123nursecode.txt")

                );
                string businessIdgb002 = "GB001";
                string dataXmlgb002 = xElement.ToString();
                string appMsggb002, outputXmlgb002;
                ShowWaitForm();
                CallInterface(businessIdgb002, dataXmlgb002, out appMsggb002, out outputXmlgb002);

                if (!string.IsNullOrEmpty(appMsggb002))
                {
                    CloseWaitForm();
                    MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if (!string.IsNullOrEmpty(outputXmlgb002))
                //{
                //    CloseWaitForm();
                //    MessageBox.Show("成功生成文件D:/123nursecode.txt");
                //}
                if (!string.IsNullOrEmpty(outputXmlgb002))
                {

                    // {
                    //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
                    //"医保返回",
                    //MessageBoxButtons.OK,
                    //MessageBoxIcon.Information);
                    //     waitForm.Close();

                    // }
                    string filePath = "D:/123nursecode.txt";

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("文件不存在！");
                        return;
                    }

                    // 1. 读取所有行
                    string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("GBK"));

                    // 2. 创建 DataTable
                    DataTable dt = new DataTable();
                    // 定义固定表头映射（列索引 -> 列名）
                    Dictionary<int, string> headerMap = new Dictionary<int, string>
{
    { 0, "医疗机构编码" },
    { 1, "医疗机构名称" },
      { 2, "医保医生代码" },
    { 3, "姓名" },

    { 4, "性别" },
     { 5, "身份证件类型" },
    { 6, "身份证件号码" },
     { 7, "人员状态" },
    { 8, "合同起始时间" },
     { 9, "合同截止时间" },
      { 10, "医生执业证书编码" },
    { 11, "执业医疗机构名称" },
     { 12, "执业类别" },
     { 13, "执业范围" },
     { 14, "执业级别" },
     { 15, "专业技术职务" }


}
                ;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // 按制表符分割
                        string[] parts = line.Split('\t');

                        // 如果表头还没创建，就动态添加列
                        // 动态创建表头
                        if (dt.Columns.Count < parts.Length)
                        {
                            for (int i = dt.Columns.Count; i < parts.Length; i++)
                            {
                                if (headerMap.ContainsKey(i))
                                    dt.Columns.Add(headerMap[i]);   // 固定列名
                                else
                                    dt.Columns.Add("字段" + (i + 1)); // 默认列名
                            }
                        }

                        // 加入一行数据
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < parts.Length; i++)
                        {
                            dr[i] = parts[i];
                        }
                        dt.Rows.Add(dr);
                    }

                    // 3. 绑定 DataGridView
                    xzxxsjk.DataSource = dt;
                    CloseWaitForm();

                }






            }



        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void yjryxxxz_Click(object sender, EventArgs e)
        {

            string businessId = "04";
            string dataXml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";

            string appMsg, outputXml;
            bool success = CallInterface(businessId, dataXml, out appMsg, out outputXml);

            if (!string.IsNullOrEmpty(appMsg))
            {
                MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //取出这个的值。
            if (!string.IsNullOrEmpty(outputXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputXml);
                XmlNodeList nodes020 = doc.SelectNodes("//output/prm_akb020");
                //取出机构编码。
                string jgbm = string.Empty;
                if (nodes020 != null && nodes020.Count > 0)
                {
                    jgbm = nodes020[0].InnerText; // 取第一个
                }

                XElement xElement = new XElement("input",
                    new XElement("prm_akb020", jgbm),
                    new XElement("prm_outputfile", "D:/123nursecode.txt")

                );
                string businessIdgb002 = "GB004";
                string dataXmlgb002 = xElement.ToString();
                string appMsggb002, outputXmlgb002;
                ShowWaitForm();
                CallInterface(businessIdgb002, dataXmlgb002, out appMsggb002, out outputXmlgb002);

                if (!string.IsNullOrEmpty(appMsggb002))
                {
                    CloseWaitForm();
                    MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if (!string.IsNullOrEmpty(outputXmlgb002))
                //{
                //    CloseWaitForm();
                //    MessageBox.Show("成功生成文件D:/123nursecode.txt");
                //}
                if (!string.IsNullOrEmpty(outputXmlgb002))
                {

                    // {
                    //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
                    //"医保返回",
                    //MessageBoxButtons.OK,
                    //MessageBoxIcon.Information);
                    //     waitForm.Close();

                    // }
                    string filePath = "D:/123nursecode.txt";

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("文件不存在！");
                        return;
                    }

                    // 1. 读取所有行
                    string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("GBK"));

                    // 2. 创建 DataTable
                    DataTable dt = new DataTable();
                    // 定义固定表头映射（列索引 -> 列名）
                    Dictionary<int, string> headerMap = new Dictionary<int, string>
{
    { 0, "医疗机构编码" },
    { 1, "医疗机构名称" },
      { 2, "医疗技术人员代码" },
    { 3, "姓名" },

    { 4, "性别" },
     { 5, "身份证件类型" },
    { 6, "身份证件号码" },
     { 7, "人员状态" },
    { 8, "合同起始时间" },
     { 9, "合同截止时间" },
      { 10, "执业范围" },
    { 11, "专业技术职务" },
     { 12, "执业类别" }



}
                ;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // 按制表符分割
                        string[] parts = line.Split('\t');

                        // 如果表头还没创建，就动态添加列
                        // 动态创建表头
                        if (dt.Columns.Count < parts.Length)
                        {
                            for (int i = dt.Columns.Count; i < parts.Length; i++)
                            {
                                if (headerMap.ContainsKey(i))
                                    dt.Columns.Add(headerMap[i]);   // 固定列名
                                else
                                    dt.Columns.Add("字段" + (i + 1)); // 默认列名
                            }
                        }

                        // 加入一行数据
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < parts.Length; i++)
                        {
                            dr[i] = parts[i];
                        }
                        dt.Rows.Add(dr);
                    }

                    // 3. 绑定 DataGridView
                    xzxxsjk.DataSource = dt;
                    CloseWaitForm();

                }






            }

        }

        private void yjyfryxxxz_Click(object sender, EventArgs e)
        {
            string businessId = "04";
            string dataXml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";

            string appMsg, outputXml;
            bool success = CallInterface(businessId, dataXml, out appMsg, out outputXml);

            if (!string.IsNullOrEmpty(appMsg))
            {
                MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //取出这个的值。
            if (!string.IsNullOrEmpty(outputXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputXml);
                XmlNodeList nodes020 = doc.SelectNodes("//output/prm_akb020");
                //取出机构编码。
                string jgbm = string.Empty;
                if (nodes020 != null && nodes020.Count > 0)
                {
                    jgbm = nodes020[0].InnerText; // 取第一个
                }

                XElement xElement = new XElement("input",
                    new XElement("prm_akb020", jgbm),
                    new XElement("prm_outputfile", "D:/123nursecode.txt")

                );
                string businessIdgb002 = "GB005";
                string dataXmlgb002 = xElement.ToString();
                string appMsggb002, outputXmlgb002;
                ShowWaitForm();
                CallInterface(businessIdgb002, dataXmlgb002, out appMsggb002, out outputXmlgb002);

                if (!string.IsNullOrEmpty(appMsggb002))
                {
                    CloseWaitForm();
                    MessageBox.Show($"【医保接口提示】\n{appMsg}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if (!string.IsNullOrEmpty(outputXmlgb002))
                //{
                //    CloseWaitForm();
                //    MessageBox.Show("成功生成文件D:/123nursecode.txt");
                //}
                if (!string.IsNullOrEmpty(outputXmlgb002))
                {

                    // {
                    //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
                    //"医保返回",
                    //MessageBoxButtons.OK,
                    //MessageBoxIcon.Information);
                    //     waitForm.Close();

                    // }
                    string filePath = "D:/123nursecode.txt";

                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("文件不存在！");
                        return;
                    }

                    // 1. 读取所有行
                    string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("GBK"));

                    // 2. 创建 DataTable
                    DataTable dt = new DataTable();
                    // 定义固定表头映射（列索引 -> 列名）
                    Dictionary<int, string> headerMap = new Dictionary<int, string>
{
    { 0, "医疗机构编码" },
    { 1, "医疗机构名称" },
      { 2, "医疗技术人员代码" },
    { 3, "姓名" },

    { 4, "性别" },
     { 5, "身份证件类型" },
    { 6, "身份证件号码" },
     { 7, "人员状态" },
    { 8, "合同起始时间" },
     { 9, "合同截止时间" },
      { 10, "执业范围" },
    { 11, "专业技术职务" },
     { 12, "执业类别" }



}
                ;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // 按制表符分割
                        string[] parts = line.Split('\t');

                        // 如果表头还没创建，就动态添加列
                        // 动态创建表头
                        if (dt.Columns.Count < parts.Length)
                        {
                            for (int i = dt.Columns.Count; i < parts.Length; i++)
                            {
                                if (headerMap.ContainsKey(i))
                                    dt.Columns.Add(headerMap[i]);   // 固定列名
                                else
                                    dt.Columns.Add("字段" + (i + 1)); // 默认列名
                            }
                        }

                        // 加入一行数据
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < parts.Length; i++)
                        {
                            dr[i] = parts[i];
                        }
                        dt.Rows.Add(dr);
                    }

                    // 3. 绑定 DataGridView
                    xzxxsjk.DataSource = dt;
                    CloseWaitForm();

                }


            }
        }

        private void ybmlxj_Click(object sender, EventArgs e)
        {


            string startSerial = "A5200000000000000000";  // 起始流水号
            string batchFile = "D:/123nursecode.txt";    // 每次接口默认输出文件
            string finalFile = "D:/all_data.txt";        // 累积保存的文件
            int batchSize = 1000;

            // 如果最终文件已存在，先清空
            if (File.Exists(finalFile))
                File.Delete(finalFile);

            string currentSerial = startSerial;
            int totalCount = 0;
            MessageBox.Show("即将开始下载医保目录数据，下载需要数小时，请耐心等待！可查看\"D:/all_data.txt\"文件大小查询进度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            while (true)
            {


                // 构造 XML
                XElement xElement = new XElement("input",
                    new XElement("prm_aaalsh", currentSerial),
                    new XElement("prm_outputfile", batchFile)
                );

                string businessIdgb002 = "91ANew";
                string dataXmlgb002 = xElement.ToString();
                string appMsggb002, outputXmlgb002;



                // 调用接口
                CallInterface(businessIdgb002, dataXmlgb002, out appMsggb002, out outputXmlgb002);
                if (!string.IsNullOrEmpty(appMsggb002))
                {
                    CloseWaitForm();
                    MessageBox.Show($"【医保接口提示】\n{appMsggb002}\n", "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 读取接口生成的文件
                string[] batchData = File.Exists(batchFile)
    ? File.ReadAllLines(batchFile, System.Text.Encoding.GetEncoding("GBK"))
    : new string[0];


                if (batchData.Length == 0)
                {
                    Console.WriteLine("没有更多数据，结束循环");
                    break;
                }


                // 累积保存到最终文件
                using (var writer = new StreamWriter(finalFile, true, System.Text.Encoding.UTF8))
                {
                    foreach (var line in batchData)
                        writer.WriteLine(line);
                }
                totalCount += batchData.Length;

                Console.WriteLine($"本次获取 {batchData.Length} 条数据，总计已保存到 {finalFile}");

                // 判断是否是最后一批
                if (batchData.Length < batchSize)
                {
                    Console.WriteLine("数据小于批量大小，说明最后一批，结束");
                    break;
                }

                // 计算下一批流水号
                currentSerial = IncrementSerial(currentSerial, batchSize);
            }

            Console.WriteLine("✅ 数据全部获取完毕");
            waitForm.Close();


            //if (!string.IsNullOrEmpty(outputXmlgb002))
            //{
            //    CloseWaitForm();
            //    MessageBox.Show("成功生成文件D:/123nursecode.txt");
            //}


            // {
            //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
            //"医保返回",
            //MessageBoxButtons.OK,
            //MessageBoxIcon.Information);
            //     waitForm.Close();

            // }
            string filePath = "D:/all_data.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在！");
                return;
            }

            // 1. 读取所有行
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("GBK"));

            // 2. 创建 DataTable
            DataTable dt = new DataTable();
            // 定义固定表头映射（列索引 -> 列名）
            Dictionary<int, string> headerMap = new Dictionary<int, string>
{
   { 0, "流水号" },
{ 1, "医保目录编码" },
{ 2, "医保目录名称" },
{ 3, "目录更新时间" },
{ 4, "大类编码" },
{ 5, "统计类型" },
{ 6, "拼音助记码" },
{ 7, "五笔助记码" },
{ 8, "规 格" },
{ 9, "剂型名称" },
{ 10, "备注（贵州其它说明）" },
{ 11, "生产企业代码" },
{ 12, "生产厂家" },
{ 13, "生产地" },
{ 14, "商品名" },
{ 15, "批准文号" },
{ 16, "项目内涵" },
{ 17, "除外内容" },
{ 18, "限制使用说明（国家）" },
{ 19, "生育项目标志" },
{ 20, "创建时间" },
{ 21, "目录启用时间" },
{ 22, "目录停用时间" },
{ 23, "国家目录类别" },
{ 24, "注册规格" },
{ 25, "最小包装数量" },
{ 26, "最小包装单位" },
{ 27, "通用名编号" },
{ 28, "目录剂型" },
{ 29, "本位码" },
{ 30, "是否民族药" },
{ 31, "最小制剂单位" }




}
        ;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 按制表符分割
                string[] parts = line.Split('\t');

                // 如果表头还没创建，就动态添加列
                // 动态创建表头
                if (dt.Columns.Count < parts.Length)
                {
                    for (int i = dt.Columns.Count; i < parts.Length; i++)
                    {
                        if (headerMap.ContainsKey(i))
                            dt.Columns.Add(headerMap[i]);   // 固定列名
                        else
                            dt.Columns.Add("字段" + (i + 1)); // 默认列名
                    }
                }

                // 加入一行数据
                DataRow dr = dt.NewRow();
                for (int i = 0; i < parts.Length; i++)
                {
                    dr[i] = parts[i];
                }
                dt.Rows.Add(dr);
            }

            // 3. 绑定 DataGridView
            xzxxsjk.DataSource = dt;
            CloseWaitForm();







        }

        // 流水号自增
        static string IncrementSerial(string serial, int step)
        {
            string prefix = serial.Substring(0, 4); // 'A520'
            long number = long.Parse(serial.Substring(4));
            number += step;
            return prefix + number.ToString("D16"); // 保持16位数字
        }

        private void button2_Click(object sender, EventArgs e)
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
            string BusinessID = "02";
            string Dataxml = "<input> <prm_aae011>ml</prm_aae011> <prm_ykc141>马力</prm_ykc141> <prm_yabtch></prm_yabtch> </input>";               // 构造你的参数
            string Businesssequence = "";
            string Businessvalidate = "";
            string Outputxml = "";
            long Appcode = 0;
            string Appmsg = " ";

            // 调用方法
            yinhaiobject.yh_interface_init("10086", "10010");

            // 创建等待框
            Form3qtjk.ShowWaitForm();



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
                Form3qtjk.CloseWaitForm();
                string message = $"【医保接口提示】\n{Appmsg}\n";
                MessageBox.Show(message, "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (!string.IsNullOrEmpty(Outputxml))
            {
                Form3qtjk.CloseWaitForm();
                MessageBox.Show("密码修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ybmlxzcx3_Click(object sender, EventArgs e)
        {
            string filePath = "D:/all_data.txt";


            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在！");
                return;
            }

            // 创建 DataTable（只保存当前页数据）
            DataTable dt = new DataTable();
            Dictionary<int, string> headerMap = new Dictionary<int, string>
{
    {0,"流水号"},{1,"医保目录编码"},{2,"医保目录名称"},{3,"目录更新时间"},
    {4,"大类编码"},{5,"统计类型"},{6,"拼音助记码"},{7,"五笔助记码"},
    {8,"规 格"},{9,"剂型名称"},{10,"备注（贵州其它说明）"},{11,"生产企业代码"},
    {12,"生产厂家"},{13,"生产地"},{14,"商品名"},{15,"批准文号"},
    {16,"项目内涵"},{17,"除外内容"},{18,"限制使用说明（国家）"},{19,"生育项目标志"},
    {20,"创建时间"},{21,"目录启用时间"},{22,"目录停用时间"},{23,"国家目录类别"},
    {24,"注册规格"},{25,"最小包装数量"},{26,"最小包装单位"},{27,"通用名编号"},
    {28,"目录剂型"},{29,"本位码"},{30,"是否民族药"},{31,"最小制剂单位"}
};

            // 分页参数
            int pageSize = 5000; // 每页读取 5000 行
            int pageIndex = 0;   // 当前页索引
            int totalLines = 0;    // 文件总行数
            // 初始化列
            for (int i = 0; i < headerMap.Count; i++)
                dt.Columns.Add(headerMap[i]);

            // 读取指定页
            void LoadPage(int page)
            {
                dt.Rows.Clear();
                int startLine = page * pageSize;
                int endLine = startLine + pageSize;
                int currentLine = 0;

                using (var sr = new StreamReader(filePath, Encoding.GetEncoding("GBK")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        if (currentLine >= startLine && currentLine < endLine)
                        {
                            string[] parts = line.Split('\t');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < parts.Length && i < dt.Columns.Count; i++)
                                dr[i] = parts[i];
                            dt.Rows.Add(dr);
                        }

                        currentLine++;
                        if (currentLine >= endLine) break;
                    }
                }
            }

            // 绑定 DataGridView
            LoadPage(pageIndex);
            xzxxsjk.DataSource = dt;


            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV 文件|*.csv",
                FileName = "导出数据.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var sr = new StreamReader(filePath, Encoding.GetEncoding("GBK")))
                    using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        // 写表头
                        string header = string.Join(",", headerMap.Values);
                        sw.WriteLine(header);

                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            string[] parts = line.Split('\t');

                            // 转成 CSV 格式，注意转义引号和逗号
                            for (int i = 0; i < parts.Length; i++)
                            {
                                string value = parts[i].Replace("\"", "\"\""); // 转义引号
                                if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                                    value = $"\"{value}\"";
                                parts[i] = value;
                            }

                            sw.WriteLine(string.Join(",", parts));
                        }
                    }

                    MessageBox.Show("导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }



    }

}
