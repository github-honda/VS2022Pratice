using FormBase.DMarshal;
using System.Runtime.InteropServices;

namespace FormBase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void btnMarshal_Click(object sender, EventArgs e)
        {
            MyMarshalTest();
        }
        void MyMarshalTest()
        {
            MarshalSample2 class1 = new()
            {
                age = 18,
                name = $"¤¤¤å´ú¸Õ, {DateTime.Now}"
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
