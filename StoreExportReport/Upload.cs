using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace StoreExportReport
{
    public partial class Upload : Form
    {

        FileValidateService fileValidateService = new FileValidateService();
        public Upload()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            string headerLine = File.ReadLines(filePath).First();
            string lastLine = File.ReadLines(filePath).Last();
            StartGoProcess go = new StartGoProcess();
            StartPythonProcess python = new StartPythonProcess();
            int goExitCode;
            int pythonExitCode;

            if (!fileValidateService.validateFirstLine(headerLine))
            {
                Console.WriteLine($"Ambiente: {Constants.env}");
                MessageBox.Show("Prima riga non valida!");
                return;
            }

            if (!fileValidateService.validateLastLine(lastLine))
            {
                MessageBox.Show("Ultima riga non valida!");
                return;
            }


            //Prima e ultima riga valide, posso procedere a validare le altre
            //Sono sicuro che l'ultima riga è un numero
            int productsRowNumber = Int32.Parse(lastLine);
            string jsonCompany = File.ReadAllText("companies.json");
            //serve installare la libreria Newtonsoft.Json
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(jsonCompany);
            Company companyToReport = new Company();
            Console.WriteLine(jsonCompany);
            Boolean validCompany = false;
            foreach (Company company in companies)
            {
                if (company.CompanyName.Equals(headerLine.Replace(" ", "")))
                {
                    validCompany = true;
                    companyToReport = company;
                    Console.WriteLine("Società trovata");
                }
            }
            if (!validCompany)
            {
                MessageBox.Show("Società per il quale si sta chiedendo il report non trovata");
                return;
            } 

            using (var reader = new StreamReader(filePath))
            {
                int counter = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Console.WriteLine($"Line: {line}");
                    if (!line.Equals(headerLine) && !line.Equals(lastLine) && !fileValidateService.validateGenericLine(line, counter, companyToReport))
                    {
                        MessageBox.Show("Riga " + counter + " non valida!");
                        return;
                    }
                    //var values = line.Split(';');                   
                    counter++;
                }
                if (counter - 2 != productsRowNumber)
                {
                    Console.WriteLine($"Righe indicate: {productsRowNumber}");
                    Console.WriteLine($"Righe nel file: {counter - 2}");
                    MessageBox.Show("Il numero di prodotti non corrisponde a quello indicato nell'ultima riga");
                }
                else
                {
                    string tempFilePath = Constants.filePath + Constants.tempFileName;
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    } 
                    File.Copy(filePath, tempFilePath);
                    goExitCode = go.generateJsonReport();
                    if (goExitCode == 0)
                    {
                        pythonExitCode = python.generateExcellReport();
                        if (pythonExitCode == 0)
                        {
                            MessageBox.Show("Export generato correttamente\nIl file è disponibile al path: " + Constants.filePath + "\nI report precedenti sono disponibili al seguente path: " + Constants.filePath + Constants.historyDirectory + headerLine);
                        }
                        else
                        {
                            MessageBox.Show("Non è stato possibile generare il report excell");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Non è stato possibile generare il report json");
                    }
                }
            }
            /*String fileName = Path.GetFileName(openFileDialog1.FileName);
            string path = @"C:\Users\Carlo\Upload\";
            File.Copy(openFileDialog1.FileName, path + fileName);
            MessageBox.Show("File Caricato");*/

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                String filePath = openFileDialog1.FileName;
                String fileExtension = Path.GetExtension(filePath);
                if (fileValidateService.validateFileExtension(fileExtension))
                {
                    textBox1.Text = filePath;
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    MessageBox.Show("L'estensione del file deve essere csv e non " + fileExtension);
                }
            }
        }
    }
}
