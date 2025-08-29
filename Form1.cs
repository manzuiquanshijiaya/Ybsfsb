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
                // ���� COM ����
                Type comType = Type.GetTypeFromProgID("yinhai.TAIYUAN.interface");
                if (comType == null)
                {
                    MessageBox.Show("����ҽ������Ƿ�װ�����⣡");
                    return;
                }

                dynamic yinhaiobject = Activator.CreateInstance(comType);

                // ׼������
                string BusinessID = "03";
                string Dataxml = "<input>\r\n  <prm_payoptype>04</prm_payoptype>\r\n</input>";               // ������Ĳ���
                string Businesssequence = "";
                string Businessvalidate = "";
                string Outputxml = "";
                long Appcode = 0;
                string Appmsg = " ";

                // ���÷���
                yinhaiobject.yh_interface_init("10086", "10010");

                // �����ȴ���
                Form waitForm = new Form()
                {
                    Text = "��ѯ",
                    Size = new Size(400, 200),
                    StartPosition = FormStartPosition.CenterScreen,
                    ControlBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog
                };
                Label label = new Label()
                {
                    Text = "��������ҽ������ѯ�У����Ժ�...",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                waitForm.Controls.Add(label);
                // ��ʾ�ȴ��򣨷�ģ̬����ֹ���� UI��
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
                    string message = $"��ҽ���ӿ���ʾ��\n{Appmsg}\n";
                    MessageBox.Show(message, "ҵ�񷵻�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!string.IsNullOrEmpty(Outputxml))
                {

                    // ���� XML �ַ���
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Outputxml);  // Outputxml ����� XML �ַ���

                    // ��ȡ���ڵ� <output>
                    XmlNode root = doc.SelectSingleNode("output");

                    // ��ȫ��ȡ��ĳЩ�ֶ�ֵ������ֵ���ؼ�
                    if (root != null)
                    {
                        xm.Text = root.SelectSingleNode("prm_aac003")?.InnerText ?? "";
                        //  xb.Text = root.SelectSingleNode("prm_aac004")?.InnerText ?? "";
                        var xbValue = root.SelectSingleNode("prm_aac004")?.InnerText ?? "";

                        if (xbValue == "1")
                        {
                            xb.Text = "��";
                        }
                        else if (xbValue == "2")
                        {
                            xb.Text = "Ů";
                        }
                        else
                        {
                            xb.Text = "����"; // ����ֵ��ʾ��
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
                    // �Զ������������
                    Dictionary<string, string> customHeaders = new Dictionary<string, string>
{
    { "akb020", "��������" },
    { "yab003", "ͳ���������" },
    { "yka026", "��������" },
    { "yka027", "��������" },
    { "aae030", "��ʼ����" },
    { "aae031", "��������" },
    { "yka260", "����" },
    { "ykd112", "����1" },
    { "yka068", "����2" },
    { "ykd111", "����3" }

};
                    // �Զ������������
                    Dictionary<string, string> renyuanbiaoti = new Dictionary<string, string>
{
    { "ykc296", "��Ա���" },
    { "ykc297", "��Ա�������" },
    { "aae030", "��ʼʱ��" },
    { "aae031", "����ʱ��" }


};

                    XmlNodeList rowNodes = doc.SelectNodes("//prm_ykc010/row");

                    if (rowNodes.Count == 0) return;

                    DataTable dt = new DataTable();

                    // ��ȡ�����������ӵ�һ�� row �ڵ���ȡ��
                    foreach (XmlNode child in rowNodes[0].ChildNodes)
                    {
                        string colName = child.Name;
                        if (!dt.Columns.Contains(colName))
                            dt.Columns.Add(colName);
                    }

                    // ����ÿ������
                    foreach (XmlNode row in rowNodes)
                    {
                        DataRow dr = dt.NewRow();
                        bool skipRow = false; // �Ƿ���������
                        foreach (XmlNode child in row.ChildNodes)
                        {
                            if (child.Name == "yka027" && child.InnerText == "��ҩ����")
                            {
                                skipRow = true;
                                break; // ��ǰ�˳��ڲ�ѭ��
                            }
                            dr[child.Name] = child.InnerText;

                        }
                        if (!skipRow)
                        {
                            dt.Rows.Add(dr);

                        }
                    }

                    // ��ʾ�� DataGridView ��  �������ݿ�
                    mbsjk.DataSource = dt;
                    waitForm.Close();



                    // ������������
                    foreach (DataGridViewColumn col in mbsjk.Columns)
                    {
                        if (customHeaders.ContainsKey(col.Name))
                            col.HeaderText = customHeaders[col.Name];
                    }
                    //��������Ա������ݿ�




                    XmlNodeList ryrowNodes = doc.SelectNodes("//prm_ykc296/row");

                    // ���û�������У������ DataGridView
                    if (ryrowNodes.Count == 0)
                    {
                        rysjk.DataSource = null;   // �������Դ
                        rysjk.Rows.Clear();        // ���������
                        rysjk.Refresh();           // ˢ�½���
                        waitForm.Close();

                        return;
                    }


                    DataTable dtry = new DataTable();

                    // ��ȡ�����������ӵ�һ�� row �ڵ���ȡ��
                    foreach (XmlNode child in ryrowNodes[0].ChildNodes)
                    {
                        string colName = child.Name;
                        if (!dtry.Columns.Contains(colName))
                            dtry.Columns.Add(colName);
                    }

                    // ����ÿ������
                    foreach (XmlNode row in ryrowNodes)
                    {
                        DataRow dr = dtry.NewRow();
                        foreach (XmlNode child in row.ChildNodes)
                        {
                            dr[child.Name] = child.InnerText;
                        }
                        dtry.Rows.Add(dr);
                    }

                    // ��ʾ�� DataGridView ��
                    rysjk.DataSource = dtry;


                    // ������������
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
                MessageBox.Show("����ʧ�ܣ�" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); // ���� Form2 ʵ��

            form2.Show(); // ��ʽ1����ģ̬��ʾ���������嶼�ܲ�����
                          // form2.ShowDialog(); // ��ʽ2��ģ̬��ʾ�������ȹص� Form2 ���ܻص� Form1��
        }
    }
}
