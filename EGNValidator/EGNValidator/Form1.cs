using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace EGNValidator
{
    public partial class EGNValidatorForm : Form
    {
        private bool parsed;

        public EGNValidatorForm()
        {
            InitializeComponent();
            parsed = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!parsed)
            {
                string inputText = textBox1.Text;
                StringBuilder sb = new StringBuilder();
                for (Match m = Regex.Match(inputText, @"\w+"); m.Success; m = m.NextMatch())
                {
                    sb.Append(m.Value);
                    sb.Append("\t");
                    int code = EGNValidator.Validate(m.Value);
                    switch (code)
                    {
                        case -2:
                            sb.Append("невалидно ЕГН");
                            break;
                        case -1:
                            sb.Append("невалиден брой символи в ЕГН-то");
                            break;
                        case 0:
                            sb.Append("валидно ЕГН");
                            break;
                        default:
                            sb.Append("невалиден символ на позиция " + (code + 1));
                            break;
                    }
                    sb.Append("\r\n");
                }
                textBox1.Text = sb.ToString();
                parsed = true;
            }
        }

        private void сприПрограматаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                string line = string.Empty;
                StringBuilder result = new StringBuilder();
                try
                {
                    reader = new StreamReader(openFileDialog1.FileName);
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.Append(line);
                        result.Append("\r\n");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Нещо се обърка при отваряне на файла \"" + openFileDialog1.FileName + "\".");
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                textBox1.Text = result.ToString();
                parsed = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool openFile = true;
            if (textBox1.Text.Length != 0)
            {
                if (MessageBox.Show("Текстовото поле не е празно. Сигурни ли сте, че искате да презапишете съдържанието?",
                    "Презаписване", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    openFile = false;
                }
            }
            if (openFile)
            {
                OpenFile();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.CheckFileExists = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(saveFileDialog1.FileName, false);
                    writer.Write(textBox1.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Нещо се обърка при запазване на файла \"" + saveFileDialog1.FileName + "\".");
                }
                finally
                {
                    if (writer != null)
                    {
                        try
                        {
                            writer.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}