using QueryMap;
using SqlDocumentRender;
using SQLParser;
using SqlParserWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColumnMapperGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    textBox1.Text = fileContent;
                }
            }
        }

        private void tableColumnMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);

            SqlParser parser = new SqlParser(new QueryMapper(), streamWriter);
            ParsedContainer container = parser.Parser(textBox1.Text);
            QueryMapContainerBuilder builder = new QueryMapContainerBuilder();
            var output = builder.Build(container);

            string title = string.Empty;

            string tx  = new ColumnMappingMarkdownRender().CreateOutputToString(output, true);

            textBox2.Text = tx;


            streamWriter.Flush();                                   
            memStream.Seek(0, SeekOrigin.Begin);

            string debug = new StreamReader(memStream).ReadToEnd();

            consoleBox.Text = debug;

        }

        private void saveMarkdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "md files (*.md)|*.md";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                writer.WriteLine(textBox2.Text);
                writer.Dispose();
                writer.Close();
            }
        }

        private void simpleTableMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlParser parser = new SqlParser(new QueryMapper());
            ParsedContainer container = parser.Parser(textBox1.Text);
            QueryMapContainerBuilder builder = new QueryMapContainerBuilder();
            var output = builder.Build(container);

            string title = string.Empty;

            string tx = new ColumnMappingMarkdownRender().CreateOutputToString(output, false);

            textBox2.Text = tx;

        }
    }
}
