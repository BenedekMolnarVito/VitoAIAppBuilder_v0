using Python.Runtime;
using System.Text.Json;

namespace AIAppBuilder_v0
{
    public partial class Form1 : Form
    {
        private string embeddedContext = "";
        private string rootDir = @"C:\Users\User\source\repos\AIAppBuilder_v0\CodeGenerationOutput\";

        public Form1()
        {
            InitializeComponent();
            InitializePython();
        }

        private void InitializePython()
        {
            try
            {
                PythonHelper.SetPythonEnvironment();
                PythonEngine.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Python: {ex.Message}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PythonEngine.Shutdown();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Documents|*.docx;*.xlsx;*.xlsm;*.pdf;*.zip";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    EmbedFile(filePath);
                }
            }
        }

        private void EmbedFile(string filePath)
        {
            try
            {
                using (Py.GIL())
                {
                    dynamic backend = Py.Import("backend"); // Ensure backend.py is in PYTHONPATH
                    PyObject pyDocumentText = backend.load_document(filePath);
                    List<string> documentText = [];

                    foreach (PyObject item in pyDocumentText)
                    {
                        documentText.Add(item.As<string>());
                    }
                    foreach (var doc in documentText)
                    {
                        dynamic embedding = backend.embed_text(doc);
                        embeddedContext += doc;
                    }
                    rtbOutput.AppendText($"Document {Path.GetFileName(filePath)} loaded and embedded successfully.\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading document {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(prompt))
            {
                MessageBox.Show("Please enter a prompt.");
                return;
            }

            if (string.IsNullOrEmpty(embeddedContext))
            {
                MessageBox.Show("Please load and embed a document first.");
                return;
            }

            try
            {
                using (Py.GIL())
                {
                    dynamic backend = Py.Import("backend");
                    string gptResponse = backend.generate_code(prompt, embeddedContext);
                    var generatedCode = JsonSerializer.Deserialize<GeneratedCode>(gptResponse);
                    foreach (var file in generatedCode.GeneratedFiles)
                    {
                        //string path = file.FileDirectory;
                        var filePath = file.FileDirectory + file.FileName;
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        File.WriteAllText(filePath, file.Code);
                        rtbOutput.AppendText($"{file.FileName} --- Code generated successfully ---" + "\n\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating code: {ex.Message}");
            }
        }

        private void bntTestCode_Click(object sender, EventArgs e)
        {
            GeneratedForm testForm = new GeneratedForm();
            testForm.ShowDialog();
        }
    }
}
