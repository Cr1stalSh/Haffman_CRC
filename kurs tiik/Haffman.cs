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
    public partial class Haffman : Form
    {
        public Haffman()
        {
            InitializeComponent();
        }

        private void Haffman_Load(object sender, EventArgs e)
        {
            codes = new Dictionary<char, string>();
        }

        private void Haffman_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show(); 
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = richTextBoxMes.Text;
            Dictionary<char, int> frequency = new Dictionary<char, int>();
            foreach (char c in message)
            {
                if (frequency.ContainsKey(c))
                    frequency[c]++;
                else
                    frequency[c] = 1;
            }
            Node root = BuildHuffTr(frequency);
            codes.Clear(); 
            GenCodes(root, "");
            var sortedCodes = codes.OrderBy(kv => kv.Value.Length);
            StringBuilder dictionary = new StringBuilder();
            foreach (var entry in sortedCodes)
            {
                dictionary.Append($"  {entry.Key}:{entry.Value};\n");
            }
            richTextBox1.Text = dictionary.ToString().TrimEnd();

            int totalCharacters = message.Length;
            Dictionary<char, double> characterProb = frequency.ToDictionary(pair => pair.Key, pair => (double)pair.Value / totalCharacters);
            var sortedCharacterProb = characterProb.OrderByDescending(kv => kv.Value);
            StringBuilder dictionaryprob = new StringBuilder();
            foreach (var entry in sortedCharacterProb)
            {
                dictionaryprob.Append($"  {entry.Key}:{entry.Value.ToString("F2")};\n");
            }
            richTextBox2.Text = dictionaryprob.ToString().TrimEnd();
            double averageCodeLength = sortedCodes.Sum(entry => entry.Value.Length * characterProb[entry.Key]);
            double entropy = -characterProb.Sum(pair => pair.Value * Math.Log(pair.Value, 2));
            textBox3.Text = averageCodeLength.ToString("F4");
            textBox2.Text = entropy.ToString("F4");
            richTextBox3.Text = CodeMes(message);
            richTextBox4.Clear();
            PrintTree(root);
        }
        private void PrintTree(Node node, string indent = "")
        {
            
            if (node != null)
            {
                
                 richTextBox4.Text += ($"{indent}Symbol: {node.Symbol}, Freq: {node.Freq}\n");

                if (node.Left != null || node.Right != null)
                {
                    PrintTree(node.Left, indent + "  ");
                    PrintTree(node.Right, indent + "  ");
                }
            }
        }
        private class Node
        {
            public char Symbol { get; set; }
            public int Freq { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }
        private string CodeMes(string message)
        {
            StringBuilder codeMes = new StringBuilder();
            foreach (char c in message)
            {
                codeMes.Append(codes[c]);
            }
            return codeMes.ToString();
        }
        private Dictionary<char, string> codes;
        private Node BuildHuffTr(Dictionary<char, int> frequency)
        {
            List<Node> nodes = frequency.Select(pair => new Node { Symbol = pair.Key, Freq = pair.Value }).ToList();

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(n => n.Freq).ToList();
                Node parent = new Node { Symbol = '*', Freq = nodes[0].Freq + nodes[1].Freq, Left = nodes[0], Right = nodes[1] };
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Add(parent);
            }

            return nodes.FirstOrDefault();
        }
        private void GenCodes(Node node, string code)
        {
            if (node == null) return;
            if (node.Left == null && node.Right == null)
            {
                codes[node.Symbol] = code;
            }
            GenCodes(node.Left, code + "0");
            GenCodes(node.Right, code + "1");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = DecodeHuffman(codes, richTextBoxMes.Text);
        }
        private string DecodeHuffman(Dictionary<char, string> huffmanCodes, string encodedString)
        {
            string decodedString = "";
            string currentCode = "";

            foreach (char bit in encodedString)
            {
                currentCode += bit; 
                foreach (var kvp in huffmanCodes)
                {
                    if (kvp.Value == currentCode) 
                    {
                        decodedString += kvp.Key; 
                        currentCode = ""; 
                        break;
                    }
                }
            }

            return decodedString;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
