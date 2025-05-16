using FormBase.DMarshal;
using System.Runtime.InteropServices;

namespace FormBase
{
    public partial class Form1 : Form
    {
        #region properties

        Color _OriginalColor = Color.Black;
        bool _Shown = false;
        CProject _Project = CProject._Me!;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Shown += Form1_Shown;

            Text = _Project._Name;

            menuStrip1.Items.Clear();
            menuStrip1.Items.Add(new ToolStripMenuItem("1 �\��", null));
            menuStrip1.Items.Add(new ToolStripMenuItem("2 �\��", null));
            menuStrip1.Items.Add(new ToolStripMenuItem("3 �\��", null));
            menuStrip1.Items.Add(new ToolStripMenuItem("&H Help", null));
            ToolStripMenuItem? Menu1;
            Menu1 = menuStrip1.Items[0] as ToolStripMenuItem; 
            Menu1?.DropDown.Items.Add(new ToolStripMenuItem("&A �\��", null, OnClickMenu, "010010"));
            Menu1?.DropDown.Items.Add(new ToolStripMenuItem("&B �\��", null, OnClickMenu, "010020"));
            Menu1?.DropDown.Items.Add(new ToolStripMenuItem("&C �\��", null, OnClickMenu, "010030"));
            Menu1 = menuStrip1.Items[3] as ToolStripMenuItem;
            Menu1?.DropDown.Items.Add(new ToolStripMenuItem("About", null, OnClickMenu, "0H00A0"));
            Menu1?.DropDown.Items.Add(new ToolStripMenuItem("Help", null, OnClickMenu, "0H00H0"));

            ToolStripStatusLabel Label1 = new ToolStripStatusLabel();
            ToolStripStatusLabel Label2 = new ToolStripStatusLabel();
            statusStrip1.Items.Clear();
            statusStrip1.Items.Add(Label1);
            statusStrip1.Items.Add(Label2);
            Label1.Spring = true; // �� Label1 �۰ʽվ�j�p, �����Ѿl�Ŷ�
            Label1.TextAlign = ContentAlignment.MiddleLeft;
            Label2.Width = 160;
            Label2.TextAlign = ContentAlignment.MiddleRight;

            _OriginalColor = Label1.ForeColor; // �x�s��l�C��

        }
        private void Form1_Shown(object? sender, EventArgs e)
        {
            _Shown = true;
            Task.Run(_Project.Run);
        }

        void OnClickMenu(object? sender, EventArgs e)
        {
            string? sMenuName = null;
            if (sender is ToolStripMenuItem menu1)
                sMenuName = menu1.Name;

            switch (sMenuName)
            {
                case "010010":
                    break;
                case "0H00A0":
                    AboutBox1 F1 = new AboutBox1();
                    F1.Show();
                    break;
                case "0H00H0":
                    break;
                default:
                    //CProject.gMe.PopupError($"Wrong sMenuName={sMenuName}.");
                    break;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string s1 = "1234";
            string s2 = s1.Substring(3, 0);
            if (s2 == null)
                textBox1.Text = "s2==null";
            if (s2 == string.Empty)
                textBox1.Text = "s2==Empty";
            //s2 = s1.Substring(7, 2);
            //textBox1.Text = s2;
        }

        public void MyShowMsgError(string PMsg)
        {
            if (statusStrip1.Items.Count < 1)
                return;

            // �ഫ���O����ؤ覡:
            ToolStripStatusLabel? Label1Sample = (ToolStripStatusLabel)statusStrip1.Items[0];
            ToolStripStatusLabel? Label1 = statusStrip1.Items[0] as ToolStripStatusLabel;
            if (Label1 == null)
                return;
            Label1.Text = PMsg;
            Label1.ForeColor = Color.Red;
        }
        public void MyShowMsg(string PMsg)
        {
            if (statusStrip1.Items.Count < 1)
                return;

            // �ഫ���O����ؤ覡:
            ToolStripStatusLabel? Label1Sample = (ToolStripStatusLabel)statusStrip1.Items[0];
            ToolStripStatusLabel? Label1 = statusStrip1.Items[0] as ToolStripStatusLabel;
            if (Label1 == null)
                return;
            Label1.Text = PMsg;
            Label1.ForeColor = _OriginalColor;
        }

        private void btnMarshal_Click(object sender, EventArgs e)
        {
            MyMarshalTest();
        }
        void MyMarshalTest()
        {
            MarshalSample2 class1 = new()
            {
                age = 18,
                name = $"�������, {DateTime.Now}"
            };

            MarshalSample3 sample3 = new();

            sample3.ObjectToBytes();
            sample3.BytesToObject();

            if (sample3.TryObjectToBytes(class1, out byte[]? Bytes1, out Exception? Exception1))
                if (sample3.TryBytesToObject(Bytes1, out MarshalSample2? class2, out Exception? Exception2))
                    textBox1.Text = class2.name;

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
