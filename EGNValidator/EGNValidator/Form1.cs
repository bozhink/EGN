using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EGNValidator
{
    public partial class EGNValidatorForm : Form
    {
        private bool parsed;

        public EGNValidatorForm()
        {
            this.InitializeComponent();
            this.parsed = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!this.parsed)
            {
                string inputText = this.textBox1.Text;
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

                    sb.AppendLine();
                }

                this.textBox1.Text = sb.ToString();
                this.parsed = true;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenFile()
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                string line = string.Empty;
                StringBuilder result = new StringBuilder();
                try
                {
                    reader = new StreamReader(this.openFileDialog1.FileName);
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.AppendLine(line);
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

                this.textBox1.Text = result.ToString();
                this.parsed = false;
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool openFile = true;
            if (this.textBox1.Text.Length != 0)
            {
                if (MessageBox.Show("Текстовото поле не е празно. Сигурни ли сте, че искате да презапишете съдържанието?",
                    "Презаписване", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    openFile = false;
                }
            }
            if (openFile)
            {
                this.OpenFile();
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.CheckFileExists = true;
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(this.saveFileDialog1.FileName, false);
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
