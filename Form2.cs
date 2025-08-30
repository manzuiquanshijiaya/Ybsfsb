
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NPOI.HSSF.UserModel; // 用于.xls
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;  // 对应 .xlsx

namespace Ybsfsb
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void H28d_Click(object sender, EventArgs e)
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
            XElement dataxml = new XElement("input",
    new XElement("prm_aac001", rybh28d.Text.Trim()),
    new XElement("prm_yab139", cbssfzx28d.Text.Trim())
);
            string BusinessID = "H28d";
            string Dataxml = dataxml.ToString();               // 构造你的参数
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

            //开始解析
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
                    var fixmedinsNameNode = root.SelectSingleNode("//fixmedinsName");
                    string fixmedinsNameValue = fixmedinsNameNode?.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(fixmedinsNameValue))
                    {

                        MessageBox.Show("该患者在“  " + fixmedinsNameValue + "  ”存在在院信息，请核实！",
                   "医保返回",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                        waitForm.Close();

                    }
                    else
                    {
                        MessageBox.Show("### 该患者没有住院信息!!! ###");
                        waitForm.Close();

                    }
                }
            }
        }

        private void ybzdxz_Click(object sender, EventArgs e)
        {

            // 创建 COM 对象
            Type comType = Type.GetTypeFromProgID("yinhai.TAIYUAN.interface");
            if (comType == null)
            {
                MessageBox.Show("请检查医保组件是否安装有问题！");
                return;
            }

            dynamic yinhaiobject = Activator.CreateInstance(comType);



            string BusinessID = "57";
            string Dataxml = "<input></input>";             // 构造你的参数
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
                Text = "正在下载中，请稍候...",
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

            //开始解析
            if (!string.IsNullOrEmpty(Appmsg))
            {
                waitForm.Close();
                string message = $"【医保接口提示】\n{Appmsg}\n";
                MessageBox.Show(message, "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (!string.IsNullOrEmpty(Outputxml))
            {
                waitForm.Close();

                // 加载 XML 字符串
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Outputxml);  // Outputxml 是你的 XML 字符串

                // 获取根节点 <output>
                //  XmlNode root = doc.SelectSingleNode("output");


                XmlNodeList rowNodes = doc.SelectNodes("//sqldata/row");

                // 创建 Excel
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                int rowIndex = 0;
                if (rowNodes.Count == 0) return;


                // === 表头映射字典 ===
                var headerMap = new Dictionary<string, string>()
        {
            { "aaa100", "字典编码" },
            { "aaa101", "字典名称" },
            { "aaa102", "代码" },
            { "aaa103", "单位名称" }
        };

                // === 写表头 ===
                if (rowNodes.Count > 0)
                {
                    XmlNode firstRow = rowNodes[0];
                    IRow headerRow = sheet.CreateRow(rowIndex);
                    int colIndex = 0;
                    foreach (XmlNode child in firstRow.ChildNodes)
                    {
                        string headerName = headerMap.ContainsKey(child.Name) ? headerMap[child.Name] : child.Name;
                        headerRow.CreateCell(colIndex).SetCellValue(headerName);
                        colIndex++;
                    }
                    rowIndex++;
                }

                foreach (XmlNode row in rowNodes)
                {
                    IRow excelRow = sheet.CreateRow(rowIndex);

                    int colIndex = 0;
                    foreach (XmlNode child in row.ChildNodes)
                    {
                        excelRow.CreateCell(colIndex).SetCellValue(child.InnerText);
                        colIndex++;
                    }

                    rowIndex++;
                }

                // 保存到文件
                string filePath = @"D:\医保字典.xlsx";
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }


                // 弹窗提示
                MessageBox.Show("Excel 文件已生成: " + Path.GetFullPath(filePath),
                                "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);




            }
        }

        private void drsjcs_Click(object sender, EventArgs e)
        {
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Excel文件|*.xls;*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        DataTable dt = ReadExcel(ofd.FileName);
                        drsjk.DataSource = dt; // 显示到界面
                    }
                }
            }
        }
        private DataTable ReadExcel(string filePath)
        {
            DataTable dt = new DataTable();

            IWorkbook workbook;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    workbook = new HSSFWorkbook(fs); // 2003
                else
                    workbook = new XSSFWorkbook(fs); // 2007+
            }

            ISheet sheet = workbook.GetSheetAt(0); // 取第一个表
            if (sheet == null) return dt;

            // 读取表头
            IRow headerRow = sheet.GetRow(0);
            foreach (var cell in headerRow.Cells)
            {
                dt.Columns.Add(cell.ToString());
            }

            // 读取数据行
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                DataRow dr = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = row.GetCell(j);
                    dr[j] = cell == null ? "" : cell.ToString();
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private void jsxxcx47_Click(object sender, EventArgs e)
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
            XElement dataxml = new XElement("input",
    new XElement("prm_akb020", jgbm47.Text.Trim()),
    new XElement("prm_aac001", rybh47.Text.Trim()),
    new XElement("prm_begindate", kssj.Value.ToString("yyyy-MM-dd")), // 开始日期
    new XElement("prm_enddate", jssj.Value.ToString("yyyy-MM-dd")),   // 结束日期
    new XElement("prm_outputfile", "c:/123.txt")

);
            string BusinessID = "47";
            string Dataxml = dataxml.ToString();               // 构造你的参数
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

            //开始解析
            if (!string.IsNullOrEmpty(Appmsg))
            {
                waitForm.Close();
                string message = $"【医保接口提示】\n{Appmsg}\n";
                MessageBox.Show(message, "业务返回", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (!string.IsNullOrEmpty(Outputxml))
            {

                // {
                //     MessageBox.Show("该患者的结算信息已经存放在  " + "C:/123.txt" + "  ”请到C盘核实！",
                //"医保返回",
                //MessageBoxButtons.OK,
                //MessageBoxIcon.Information);
                //     waitForm.Close();

                // }
                string filePath = "c:/123.txt";

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
    { 0, "人员编号" },
    { 2, "消费总金额" },
      { 7, "统筹报销金额" },
    { 35, "统筹区划" },

    { 1, "性别" },
     { 20, "就诊编号" },
    { 21, "姓名" },
     { 22, "结算编号" },
    { 27, "操作员" },
    { 36, "身份证号" },
     { 24, "支付类别" },
     { 12, "清算类别" },
    { 38, "原发送报文ID" }
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
                drsjk.DataSource = dt;
                waitForm.Close();

            }
        }

        private void dc_Click(object sender, EventArgs e)
        {

            if (drsjk.Rows.Count == 0)
            {
                MessageBox.Show("没有可导出的数据！");
                return;
            }

            // 选择保存路径
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel文件|*.xlsx";
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.FileName = "结算查询数据47接口导出.xlsx";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            // 创建Excel工作簿
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("数据");

            // 写入表头
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < drsjk.Columns.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(drsjk.Columns[i].HeaderText);
            }

            // 写入内容
            for (int i = 0; i < drsjk.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < drsjk.Columns.Count; j++)
                {
                    object value = drsjk.Rows[i].Cells[j].Value;
                    row.CreateCell(j).SetCellValue(value == null ? "" : value.ToString());
                }
            }

            // 保存到文件
            using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            MessageBox.Show("导出成功！");
        }
    }
}
