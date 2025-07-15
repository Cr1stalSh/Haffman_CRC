using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs_tiik
{
    public partial class CRC : Form
    {
        public CRC()
        {
            InitializeComponent();
        }

        private void CRC_Load(object sender, EventArgs e)
        {

        }

        private void CRC_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
        public string a;
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            string inp = textBox1.Text;
            string pol = textBox2.Text;
            ulong input_code = ulong.Parse(inp, System.Globalization.NumberStyles.HexNumber);
            ulong polynomial = (ulong)Convert.ToInt32(pol, 2);
            a += "\nCRC: \n";
            ulong crc_sum = w_bit(input_code, polynomial);
            a += "\n\nCRC code is: " + Convert.ToString((long)crc_sum, 2).PadLeft((int)(number_bit_size(polynomial) - 1), '0') + "\n";
            richTextBox1.Text = a;
        }
        static ulong number_bit_size(ulong bits)
        {
            return (ulong)(Math.Log(bits, 2)) + 1;
        }
        public ulong CRCc(ulong input_code, ulong polynomial)
        {
            ulong pol_size = number_bit_size(polynomial);
            ulong input_size = number_bit_size(input_code);
            var crc = new CRC();
            ulong del = polynomial << (int)(input_size - pol_size); 
            ulong result = input_code;
            while (number_bit_size(result) >= pol_size)
            {
                del = polynomial << (int)(number_bit_size(result) - pol_size);
                a += "\nNum: " + Convert.ToString((long)result, 2);
                a += "\nPol: " + Convert.ToString((long)del, 2);
                result = result ^ del;
                if (result == 0)
                {
                    break;
                }
            }
            return result;
        }
        public ulong w_bit(ulong input_code, ulong polynomial)
        {
            input_code = input_code << (int)(number_bit_size(polynomial) - 1);
            return CRCc(input_code, polynomial);
        }
        public ulong check(ulong input_code, ulong polynomial, ulong crc)
        {
            input_code = input_code << (int)(number_bit_size(polynomial) - 1);
            input_code += crc;
            return CRCc(input_code, polynomial);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String[] ss = new String[0];
            richTextBox1.Lines = ss;
            a = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string inp = textBox1.Text;
            string pol = textBox2.Text;
            string CRCc = textBox3.Text;
            ulong input_code = ulong.Parse(inp, System.Globalization.NumberStyles.HexNumber);
            ulong polynomial = (ulong)Convert.ToInt32(pol, 2);
            ulong CRCcode = (ulong)Convert.ToInt32(CRCc, 2);
            a += "\nCheck: \n";
            ulong check_sum = check(input_code, polynomial, CRCcode);
            a += "\n\nCheck res: " + check_sum;
            richTextBox1.Text = a;
        }
    }
}
