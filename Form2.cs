
using NPOI.HSSF.UserModel; // 用于.xls
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;  // 对应 .xlsx
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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
            //Form waitForm = new Form()
            //{
            //    Text = "查询",
            //    Size = new Size(400, 200),
            //    StartPosition = FormStartPosition.CenterScreen,
            //    ControlBox = false,
            //    FormBorderStyle = FormBorderStyle.FixedDialog
            //};
            //Label label = new Label()
            //{
            //    Text = "正在连接医保网查询中，请稍候...",
            //    Dock = DockStyle.Fill,
            //    TextAlign = ContentAlignment.MiddleCenter
            //};
            //waitForm.Controls.Add(label);
            //// 显示等待框（非模态，防止阻塞 UI）
            //waitForm.Show();
            //waitForm.Refresh();
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

            //开始解析
            if (!string.IsNullOrEmpty(Appmsg))
            {
                //  waitForm.Close();
                Form3qtjk.CloseWaitForm();
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
                        Form3qtjk.CloseWaitForm();

                    }
                    else
                    {
                        //  waitForm.Close();
                        Form3qtjk.CloseWaitForm();
                        MessageBox.Show("### 银海提示：该患者没有住院信息!!! ###");
                     

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
    new XElement("prm_outputfile", "D:/123nursecode.txt")

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
                //     MessageBox.Show("该患者的结算信息已经存放在  " + "D:/123nursecode.txt" + "  ”请到C盘核实！",
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
                Dictionary<int, string> headerMap = GetHeaderMap();
                //定义字典

                Dictionary<string, string> payTypeMap = new Dictionary<string, string>
{
  { "108", "辅助生殖门诊" },
    { "11", "普通门诊" },
    { "110102", "个人账户代支" },
    { "1102", "新冠门诊" },
    { "12", "药店购药" },
    { "18", "特殊门诊" },
    { "31", "普通住院" },
    { "32", "重大疾病住院" },
    { "36", "转外住院" },
    { "37", "转院住院" },
    { "990105", "发热门诊新冠病毒检测" },
    { "G11", "(工伤)普通门诊" },
    { "G31", "(工伤)普通住院" },
    { "M11", "门诊产前检查" },
    { "M31", "生育住院" },
    { "M32", "计划生育手术住院" },
    { "M33", "分娩住院" }
    // 按需补充
};

                Dictionary<string, string> clearTypeMap = new Dictionary<string, string>
{
   { "99970", "职工医疗费用" },
    { "99971", "城乡居民费用" },
    { "99972", "离休医疗费用" },
    { "99982", "职工生育费用" },
    { "YD01", "异地医疗费用" }
    // 按需补充
};
                Dictionary<string, string> areaMap = new Dictionary<string, string>
{
  { "520000", "贵州省" },
    { "520100", "贵阳市" },
    { "520102", "南明区" },
    { "520103", "云岩区" },
    { "520111", "花溪区" },
    { "520112", "乌当区" },
    { "520113", "白云区" },
    { "520114", "小河区" },
    { "520115", "观山湖区" },
    { "520121", "开阳县" },
    { "520122", "息烽县" },
    { "520123", "修文县" },
    { "520170", "贵安新区" },
    { "520181", "清镇市" },
    { "520199", "贵阳市市本级" },
    { "520200", "六盘水市" },
    { "520201", "钟山区" },
    { "520203", "六枝特区" },
    { "520221", "水城县" },
    { "520222", "盘县" },
    { "520240", "钟山经济开发区" },
    { "520281", "盘州市" },
    { "520299", "六盘水市市本级" },
    { "520300", "遵义市" },
    { "520302", "红花岗区" },
    { "520303", "汇川区" },
    { "520304", "播州区" },
    { "520321", "遵义县" },
    { "520322", "桐梓县" },
    { "520323", "绥阳县" },
    { "520324", "正安县" },
    { "520325", "道真仡佬族苗族自治县" },
    { "520326", "务川仡佬族苗族自治县" },
    { "520327", "凤冈县" },
    { "520328", "湄潭县" },
    { "520329", "余庆县" },
    { "520330", "习水县" },
    { "520340", "新蒲新区" },
    { "520381", "赤水市" },
    { "520382", "仁怀市" },
    { "520383", "新蒲新区" },
    { "520399", "遵义市市本级" },
    { "520400", "安顺市" },
    { "520402", "西秀区" },
    { "520403", "平坝区" },
    { "520421", "平坝县" },
    { "520422", "普定县" },
    { "520423", "镇宁布依族苗族自治县" },
    { "520424", "关岭布依族苗族自治县" },
    { "520425", "紫云苗族布依族自治县" },
    { "520440", "经济技术开发区" },
    { "520441", "黄果树旅游区" },
    { "520499", "安顺市市本级" },
    { "520500", "毕节市" },
    { "520502", "七星关区" },
    { "520521", "大方县" },
    { "520522", "黔西县" },
    { "520523", "金沙县" },
    { "520524", "织金县" },
    { "520525", "纳雍县" },
    { "520526", "威宁彝族回族苗族自治县" },
    { "520527", "赫章县" },
    { "520528", "百里杜鹃管理区" },
    { "520529", "金海湖新区" },
    { "520540", "百里杜鹃风景名胜区" },
    { "520541", "毕节经济开发区" },
    { "520581", "黔西市" },
    { "520599", "毕节市市本级" },
    { "520600", "铜仁市" },
    { "520602", "碧江区" },
    { "520603", "万山区" },
    { "520621", "江口县" },
    { "520622", "玉屏侗族自治县" },
    { "520623", "石阡县" },
    { "520624", "思南县" },
    { "520625", "印江土家族苗族自治县" },
    { "520626", "德江县" },
    { "520627", "沿河土家族自治县" },
    { "520628", "松桃苗族自治县" },
    { "520640", "大龙开发区" },
    { "520699", "铜仁市市本级" },
    { "522300", "黔西南布依族苗族自治州" },
    { "522301", "兴义市" },
    { "522302", "兴仁市" },
    { "522322", "兴仁县" },
    { "522323", "普安县" },
    { "522324", "晴隆县" },
    { "522325", "贞丰县" },
    { "522326", "望谟县" },
    { "522327", "册亨县" },
    { "522328", "安龙县" },
    { "522340", "义龙新区" },
    { "522399", "黔西南布依族苗族自治州州本级" },
    { "522400", "毕节地区" },
    { "522401", "毕节市" },
    { "522422", "大方县" },
    { "522423", "黔西县" },
    { "522424", "金沙县" },
    { "522425", "织金县" },
    { "522426", "纳雍县" },
    { "522427", "威宁彝族回族苗族自治县" },
    { "522428", "赫章县" },
    { "522440", "百里杜鹃管理区" },
    { "522499", "毕节地区本级" },
    { "522600", "黔东南苗族侗族自治州" },
    { "522601", "凯里市" },
    { "522622", "黄平县" },
    { "522623", "施秉县" },
    { "522624", "三穗县" },
    { "522625", "镇远县" },
    { "522626", "岑巩县" },
    { "522627", "天柱县" },
    { "522628", "锦屏县" },
    { "522629", "剑河县" },
    { "522630", "台江县" },
    { "522631", "黎平县" },
    { "522632", "榕江县" },
    { "522633", "从江县" },
    { "522634", "雷山县" },
    { "522635", "麻江县" },
    { "522636", "丹寨县" },
    { "522640", "凯里经济开发区" },
    { "522699", "黔东南苗族侗族自治州州本级" },
    { "522700", "黔南布依族苗族自治州" },
    { "522701", "都匀市" },
    { "522702", "福泉市" },
    { "522722", "荔波县" },
    { "522723", "贵定县" },
    { "522725", "瓮安县" },
    { "522726", "独山县" },
    { "522727", "平塘县" },
    { "522728", "罗甸县" },
    { "522729", "长顺县" },
    { "522730", "龙里县" },
    { "522731", "惠水县" },
    { "522732", "三都县" },
    { "522740", "都匀经济开发区" },
    { "522799", "黔南布依族苗族自治州州本级" },
    { "527000", "贵安新区" },
    { "527099", "贵安新区本级" },
    { "529900", "贵州省省本级" }
    // 按需补充
};


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

                    //特殊字段处理

                    for (int i = 0; i < parts.Length; i++)
                    {
                        string value = parts[i];

                        // 如果是支付类别列
                        if (headerMap.ContainsKey(i) && headerMap[i] == "支付类别")
                        {
                            if (payTypeMap.ContainsKey(value))
                                value = payTypeMap[value];
                        }

                        // 如果是清算类别列
                        if (headerMap.ContainsKey(i) && headerMap[i] == "清算类别")
                        {
                            if (clearTypeMap.ContainsKey(value))
                                value = clearTypeMap[value];
                        }
                        // 如果是参保区划类别列
                        if (headerMap.ContainsKey(i) && headerMap[i] == "参保区划")
                        {
                            if (areaMap.ContainsKey(value))
                                value = areaMap[value];
                        }

                        dr[i] = value;
                    }
                    dt.Rows.Add(dr);
                }

                // 3. 绑定 DataGridView
                drsjk.DataSource = dt;
                waitForm.Close();

            }
        }

        private static Dictionary<int, string> GetHeaderMap()
        {
            return new Dictionary<int, string>
{
    { 0,  "人员编号" },              // aac001 个人编号
{ 1,  "个人帐户支付金额" },      // yka065 个人帐户支付金额
{ 2,  "消费总金额" },            // yka055 医疗费总额
{ 3,  "全自费金额" },            // yka056 全自费金额
{ 4,  "挂钩自付金额" },          // yka057 挂钩自付金额
{ 5,  "符合范围金额" },          // yka111 符合范围金额
{ 6,  "进入起付线金额" },        // yka058 进入起付线金额
{ 7,  "统筹报销金额" },          // yka248 基本医疗统筹支付金额
{ 8,  "大额医疗支付金额" },      // yka062 大额医疗支付金额
{ 9,  "公务员补助报销金额" },    // yke030 公务员补助报销金额
{ 10, "个人账户支付后余额" },    // ykc177 个人账户支付后余额
{ 11, "清算分中心" },            // ykb037 清算分中心
{ 12, "清算类别" },              // yka316 清算类别
{ 13, "清算方式" },              // yka054 清算方式
{ 14, "清算期号" },              // yae366 清算期号
{ 15, "医疗人员类别" },          // akc021 医疗人员类别
{ 16, "医疗类别" },              // ykc121 就诊结算方式
{ 17, "居保人员类别" },          // ykc280 居保人员类别
{ 18, "居保人员身份" },          // ykc281 居保人员身份
{ 19, "结算时间" },              // aae036 经办时间
{ 20, "就诊编号" },              // akc190 门诊住院流水号
{ 21, "姓名" },                  // aac003 姓名
{ 22, "结算编号" },              // yka103 结算编号
{ 23, "社会保险办法" },          // ykb065 执行社会保险办法
{ 24, "支付类别" },              // aka130 支付类别
{ 25, "参保区划" },              // yab003 行政区划
{ 26, "操作员工号" },            // aae011 经办人编码
{ 27, "操作员" },                // ykc141 经办人姓名
{ 28, "医疗救助" },              // yka469 医疗救助
{ 29, "卫计补偿" },              // yka471 卫计补偿
{ 30, "优抚补偿" },              // ake183 优抚补偿
{ 31, "其它基金" },              // ake173 其它基金
{ 32, "就诊凭证类型" },          // mdtrtCertType 就诊凭证类型
{ 33, "病种编码" },              // diseNo 病种编码
{ 34, "病种名称" },              // diseName 病种名称
{ 35, "统筹区划" },              // yab139 参保分中心
{ 36, "身份证号" },              // aac002 身份证号
{ 37, "特殊人员类型" },          // spPsnType 特殊人员类型
{ 38, "原发送报文ID" }        // medins_setlId 结算报文 id

};
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

            // 使用 SXSSFWorkbook（流式写入，参数100表示内存中只保留100行1）
            SXSSFWorkbook workbook = new SXSSFWorkbook(100);
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

            // 释放临时文件资源
            workbook.Dispose();

            MessageBox.Show("导出成功！");
        }

        private void qtjk_Click(object sender, EventArgs e)
        {
            Form3qtjk form3qtjk = new Form3qtjk(); // 创建 Form2 实例

            // form3qtjk.Show(); // 方式1：非模态显示（两个窗体都能操作）
            form3qtjk.ShowDialog(); // 方式2：模态显示（必须先关掉 Form2 才能回到 Form1）
        }

        private void ybjk04_Click(object sender, EventArgs e)
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
                string BusinessID = "04";
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






                    XmlNodeList nodes021 = doc.SelectNodes("//output/prm_akb021");
                    XmlNodeList nodes020 = doc.SelectNodes("//output/prm_akb020");

                    StringBuilder sb = new StringBuilder();

                    if (nodes021 != null && nodes021.Count > 0)
                    {
                        sb.AppendLine("医院名称：");
                        foreach (XmlNode node in nodes021)
                        {
                            sb.AppendLine(node.InnerText);
                        }
                    }
                    else
                    {
                        sb.AppendLine("未找到 prm_akb021 节点");
                    }

                    sb.AppendLine(); // 空行

                    if (nodes020 != null && nodes020.Count > 0)
                    {
                        sb.AppendLine("医疗机构编码：");
                        foreach (XmlNode node in nodes020)
                        {
                            sb.AppendLine(node.InnerText);
                        }
                    }
                    else
                    {
                        sb.AppendLine("未找到 prm_akb020 节点");
                    }
                    waitForm.Close();
                    MessageBox.Show(sb.ToString(), "医院名称");





                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("调用失败：" + ex.Message);
            }
        }
        private DataTable originalTable;   // 保存完整数据
        private DataView currentView;      // 当前视图

        public void Form1_Load()
        {
            // 模拟一些数据
            originalTable = new DataTable();




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
            Dictionary<int, string> headerMap = GetHeaderMap();
            //定义字典

            Dictionary<string, string> payTypeMap = new Dictionary<string, string>
{
  { "108", "辅助生殖门诊" },
    { "11", "普通门诊" },
    { "110102", "个人账户代支" },
    { "1102", "新冠门诊" },
    { "12", "药店购药" },
    { "18", "特殊门诊" },
    { "31", "普通住院" },
    { "32", "重大疾病住院" },
    { "36", "转外住院" },
    { "37", "转院住院" },
    { "990105", "发热门诊新冠病毒检测" },
    { "G11", "(工伤)普通门诊" },
    { "G31", "(工伤)普通住院" },
    { "M11", "门诊产前检查" },
    { "M31", "生育住院" },
    { "M32", "计划生育手术住院" },
    { "M33", "分娩住院" }
    // 按需补充
};

            Dictionary<string, string> clearTypeMap = new Dictionary<string, string>
{
   { "99970", "职工医疗费用" },
    { "99971", "城乡居民费用" },
    { "99972", "离休医疗费用" },
    { "99982", "职工生育费用" },
    { "YD01", "异地医疗费用" }
    // 按需补充
};
            Dictionary<string, string> areaMap = new Dictionary<string, string>
{
  { "520000", "贵州省" },
    { "520100", "贵阳市" },
    { "520102", "南明区" },
    { "520103", "云岩区" },
    { "520111", "花溪区" },
    { "520112", "乌当区" },
    { "520113", "白云区" },
    { "520114", "小河区" },
    { "520115", "观山湖区" },
    { "520121", "开阳县" },
    { "520122", "息烽县" },
    { "520123", "修文县" },
    { "520170", "贵安新区" },
    { "520181", "清镇市" },
    { "520199", "贵阳市市本级" },
    { "520200", "六盘水市" },
    { "520201", "钟山区" },
    { "520203", "六枝特区" },
    { "520221", "水城县" },
    { "520222", "盘县" },
    { "520240", "钟山经济开发区" },
    { "520281", "盘州市" },
    { "520299", "六盘水市市本级" },
    { "520300", "遵义市" },
    { "520302", "红花岗区" },
    { "520303", "汇川区" },
    { "520304", "播州区" },
    { "520321", "遵义县" },
    { "520322", "桐梓县" },
    { "520323", "绥阳县" },
    { "520324", "正安县" },
    { "520325", "道真仡佬族苗族自治县" },
    { "520326", "务川仡佬族苗族自治县" },
    { "520327", "凤冈县" },
    { "520328", "湄潭县" },
    { "520329", "余庆县" },
    { "520330", "习水县" },
    { "520340", "新蒲新区" },
    { "520381", "赤水市" },
    { "520382", "仁怀市" },
    { "520383", "新蒲新区" },
    { "520399", "遵义市市本级" },
    { "520400", "安顺市" },
    { "520402", "西秀区" },
    { "520403", "平坝区" },
    { "520421", "平坝县" },
    { "520422", "普定县" },
    { "520423", "镇宁布依族苗族自治县" },
    { "520424", "关岭布依族苗族自治县" },
    { "520425", "紫云苗族布依族自治县" },
    { "520440", "经济技术开发区" },
    { "520441", "黄果树旅游区" },
    { "520499", "安顺市市本级" },
    { "520500", "毕节市" },
    { "520502", "七星关区" },
    { "520521", "大方县" },
    { "520522", "黔西县" },
    { "520523", "金沙县" },
    { "520524", "织金县" },
    { "520525", "纳雍县" },
    { "520526", "威宁彝族回族苗族自治县" },
    { "520527", "赫章县" },
    { "520528", "百里杜鹃管理区" },
    { "520529", "金海湖新区" },
    { "520540", "百里杜鹃风景名胜区" },
    { "520541", "毕节经济开发区" },
    { "520581", "黔西市" },
    { "520599", "毕节市市本级" },
    { "520600", "铜仁市" },
    { "520602", "碧江区" },
    { "520603", "万山区" },
    { "520621", "江口县" },
    { "520622", "玉屏侗族自治县" },
    { "520623", "石阡县" },
    { "520624", "思南县" },
    { "520625", "印江土家族苗族自治县" },
    { "520626", "德江县" },
    { "520627", "沿河土家族自治县" },
    { "520628", "松桃苗族自治县" },
    { "520640", "大龙开发区" },
    { "520699", "铜仁市市本级" },
    { "522300", "黔西南布依族苗族自治州" },
    { "522301", "兴义市" },
    { "522302", "兴仁市" },
    { "522322", "兴仁县" },
    { "522323", "普安县" },
    { "522324", "晴隆县" },
    { "522325", "贞丰县" },
    { "522326", "望谟县" },
    { "522327", "册亨县" },
    { "522328", "安龙县" },
    { "522340", "义龙新区" },
    { "522399", "黔西南布依族苗族自治州州本级" },
    { "522400", "毕节地区" },
    { "522401", "毕节市" },
    { "522422", "大方县" },
    { "522423", "黔西县" },
    { "522424", "金沙县" },
    { "522425", "织金县" },
    { "522426", "纳雍县" },
    { "522427", "威宁彝族回族苗族自治县" },
    { "522428", "赫章县" },
    { "522440", "百里杜鹃管理区" },
    { "522499", "毕节地区本级" },
    { "522600", "黔东南苗族侗族自治州" },
    { "522601", "凯里市" },
    { "522622", "黄平县" },
    { "522623", "施秉县" },
    { "522624", "三穗县" },
    { "522625", "镇远县" },
    { "522626", "岑巩县" },
    { "522627", "天柱县" },
    { "522628", "锦屏县" },
    { "522629", "剑河县" },
    { "522630", "台江县" },
    { "522631", "黎平县" },
    { "522632", "榕江县" },
    { "522633", "从江县" },
    { "522634", "雷山县" },
    { "522635", "麻江县" },
    { "522636", "丹寨县" },
    { "522640", "凯里经济开发区" },
    { "522699", "黔东南苗族侗族自治州州本级" },
    { "522700", "黔南布依族苗族自治州" },
    { "522701", "都匀市" },
    { "522702", "福泉市" },
    { "522722", "荔波县" },
    { "522723", "贵定县" },
    { "522725", "瓮安县" },
    { "522726", "独山县" },
    { "522727", "平塘县" },
    { "522728", "罗甸县" },
    { "522729", "长顺县" },
    { "522730", "龙里县" },
    { "522731", "惠水县" },
    { "522732", "三都县" },
    { "522740", "都匀经济开发区" },
    { "522799", "黔南布依族苗族自治州州本级" },
    { "527000", "贵安新区" },
    { "527099", "贵安新区本级" },
    { "529900", "贵州省省本级" }
    // 按需补充
};


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

                //特殊字段处理

                for (int i = 0; i < parts.Length; i++)
                {
                    string value = parts[i];

                    // 如果是支付类别列
                    if (headerMap.ContainsKey(i) && headerMap[i] == "支付类别")
                    {
                        if (payTypeMap.ContainsKey(value))
                            value = payTypeMap[value];
                    }

                    // 如果是清算类别列
                    if (headerMap.ContainsKey(i) && headerMap[i] == "清算类别")
                    {
                        if (clearTypeMap.ContainsKey(value))
                            value = clearTypeMap[value];
                    }
                    // 如果是参保区划类别列
                    if (headerMap.ContainsKey(i) && headerMap[i] == "参保区划")
                    {
                        if (areaMap.ContainsKey(value))
                            value = areaMap[value];
                    }

                    dr[i] = value;
                }
                dt.Rows.Add(dr);
            }

            // 3. 绑定 DataGridView
            // 3. 保存和绑定 DataGridView
            originalTable = dt;           // 保存原始表
            drsjk.DataSource = originalTable;




        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load();


            if (originalTable == null)
            {
                MessageBox.Show("数据还未加载，请先导入数据！");
                return;
            }

            string keyword = ssnr.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("请输入关键字！");
                return;
            }

            DataView dv = new DataView(originalTable);

            List<string> conditions = new List<string>();
            foreach (DataColumn col in originalTable.Columns)
            {
                conditions.Add($"{col.ColumnName} LIKE '%{keyword.Replace("'", "''")}%'");
            }

            dv.RowFilter = string.Join(" OR ", conditions);

            if (dv.Count == 0)
            {
                MessageBox.Show("未找到匹配内容！");
                return;
            }

            drsjk.DataSource = dv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 恢复原始数据
            drsjk.DataSource = originalTable;

            ssnr.Clear();
        }

        private void ybsfsbff_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(); // 创建 Form2 实例

            // form3qtjk.Show(); // 方式1：非模态显示（两个窗体都能操作）
            form1.ShowDialog(); // 方式2：模态显示（必须先关掉 Form2 才能回到 Form1）
        }
    }
}
