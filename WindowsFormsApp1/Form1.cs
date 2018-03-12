using Core1.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idsuppr = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
            textBox1.Text = idsuppr.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pathDb = "server=localhost; user=root; database=world_x; port=3306; password=";
            MySqlConnection conn = new MySqlConnection(pathDb);
            conn.Open();
            MySqlCommand query = new MySqlCommand("SELECT * from `meteodata`", conn);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = query;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        /**
		 * use to delete the selected line
		 * 
		 **/
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string pathDb = "server=localhost; user=root; database=world_x; port=3306; password=";
                MySqlConnection conn = new MySqlConnection(pathDb);
                conn.Open();
                string Query = "DELETE FROM `meteodata` WHERE `id`='" + int.Parse(textBox1.Text) + "';";


                MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);

                MySqlDataReader MyReader2;
                MyReader2 = MyCommand2.ExecuteReader();
                MessageBox.Show("données supprimées");
                while (MyReader2.Read())
                {
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        /**
		 * use to generate a graph
		 * 
		 **/
        private void button3_Click(object sender, EventArgs e)
        {
            string pathDb = "server=localhost; user=root; database=world_x; port=3306; password=";
            MySqlConnection conn = new MySqlConnection(pathDb);
            conn.Open();
            MySqlCommand query = new MySqlCommand("SELECT * FROM `meteodata`", conn);

            MySqlDataReader myReader;

            myReader = query.ExecuteReader();

            while (myReader.Read())
            {
                this.chart1.Series["Temperature"].Points.AddXY(myReader.GetString("id"), myReader.GetDouble("temp"));
                this.chart1.Series["Taux Humidité"].Points.AddXY(myReader.GetString("id"), myReader.GetDouble("humidity"));
            }
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress("bo76h211@gmail.com");
            email.To.Add(new MailAddress("jean-luc-dubois@orange.fr"));
            email.IsBodyHtml = true;
            email.Subject = "objet du mail";
            email.Body = " contenu du mail";
            SmtpClient smtp = new SmtpClient("127.0.0.1", 587);
            //SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("bo76h211@gmail.com", "Cesi2017");

            string file = @"C:\Users\JL DUBS\Dropbox\C# Project - Groupe 3\2_Developpement\Rapport PDF\source_donnees.pdf";
            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            // Add the file attachment to this e-mail message.
            email.Attachments.Add(data);

            try
            {
                smtp.Send(email);
                MessageBox.Show("émail envoyé");
            }
            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string pathDb = "server=localhost; user=root; database=world_x; port=3306; password=";

            MySqlConnection conn = new MySqlConnection(pathDb);
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Import des données";
            open.Filter = "Text Files(*.txt)|*.txt|All Files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader read = new StreamReader(File.OpenRead(open.FileName));

                int counter = 0;

                String[] file = File.ReadAllLines(open.FileName);         // Read the file and display it line by line.  

                foreach (String lines in file)
                {
                    string[] entries = lines.Split(' ');

                    int id = int.Parse(Regex.Replace(entries[0], "[^0-9 ]", ""));
                    DateTime date = System.DateTime.Parse(entries[1] + " " + entries[2]);
                    double temp = double.Parse(Regex.Replace(entries[3], "[^0-9., ]", "").Replace(".", ","));
                    double humidity = double.Parse(Regex.Replace(entries[4], "[^0-9., ]", "").Replace(".", ","));

                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `meteodata` (`id`, `date`, `temp`, `humidity`) VALUES(@id, @date, @temp, @humidity)", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@temp", temp);
                        cmd.Parameters.AddWithValue("@humidity", humidity);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    counter++;

                }


                Console.WriteLine("There were {0} lines.", counter);
                // Suspend the screen.  
                Console.ReadLine();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClassExportCSV test = new ClassExportCSV();
            test.ExportCSV();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string number = "11161806 20170524 11585745";
            string notes = "Test 1 semaine";
            string description = "SN: 11161806";
            string interval = " 300 secondes";
            string alarm = "-20.0 , 100.0";
            int totalRecord = 11;
            string tempStat = "31.0 / 28.1 / 29.0 / 29.0";
            string humidityStat = "56.0 / 44.0 / 49.0 %";
            DateTime startTime = new DateTime(2017, 05, 24, 12, 32, 35);
            DateTime endTime = new DateTime(2017, 05, 24, 13, 57, 35);
            string totalTime = "01:25:00";

            Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\JL DUBS\Dropbox\C# Project - Groupe 3\2_Developpement\Rapport PDF\source_donnees.pdf", FileMode.Create));
            doc.Open();

            Paragraph paragraph1 = new Paragraph("Fichier créé le : " + DateTime.Now);
            paragraph1.SpacingAfter = 10;

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("DATA REPORT"));
            cell.BackgroundColor = new BaseColor(0, 150, 0);
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1;
            cell.Padding = 10;

            table.AddCell(cell);

            table.AddCell("No:");
            table.AddCell(number);
            table.AddCell("Notes: ");
            table.AddCell(notes);
            table.AddCell("Description:");
            table.AddCell(description);
            table.AddCell("Storage Interval:");
            table.AddCell(interval);
            table.AddCell("Alarm Settings:");
            table.AddCell(alarm);
            table.AddCell("Total Records:");
            table.AddCell((totalRecord).ToString());
            table.AddCell("Temp Max/ Min / Avg / MKT:");
            table.AddCell(tempStat);
            table.AddCell("Humidity Max/ Min / Avg:");
            table.AddCell(humidityStat);
            table.AddCell(" Start Time: ");
            table.AddCell(startTime.ToString());
            table.AddCell("End Time: ");
            table.AddCell(endTime.ToString());
            table.AddCell("Total Time:");
            table.AddCell(totalTime);

            Paragraph paragraph2 = new Paragraph("Temperature and humidity data");
            paragraph2.SpacingBefore = 10;
            paragraph2.SpacingAfter = 10;

            Paragraph paragraph3 = new Paragraph("Temperature and humidity graph");
            paragraph2.SpacingBefore = 10;
            paragraph2.SpacingAfter = 10;

            /* TABLEAU 
              -----------------------------------------*/
            PdfPTable table1 = new PdfPTable(dataGridView1.Columns.Count);

            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                table1.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText));
            }

            table1.HeaderRows = 1;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView1.Columns.Count; k++)
                {
                    if (dataGridView1[k, i].Value != null)
                    {
                        table1.AddCell(new Phrase(dataGridView1[k, i].Value.ToString()));
                    }
                }
            }

            /* GRAPHIQUE
            ----------------------------------------------- */
            var chartimage = new MemoryStream();
            chart1.SaveImage(chartimage, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            iTextSharp.text.Image Chart_image = iTextSharp.text.Image.GetInstance(chartimage.GetBuffer());

            /* AJOUT AU FICHIER
             * ---------------------------------------------*/
            doc.Add(paragraph1);
            doc.Add(table);
            doc.Add(paragraph2);
            doc.Add(table1);
            doc.Add(paragraph2);
            doc.Add(Chart_image);

            doc.Close();

            Console.WriteLine("fichier PDF créé");
            MessageBox.Show("votre fichier .pdf a bien été créé");
        
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(panel1.Height == 445 & panel1.Width == 260)
            { 
            panel1.Height = 445;
            panel1.Width = 50;
            button7.Left = 3;
            label2.Visible = false;
        }
            else
            {
            panel1.Height = 445;
            panel1.Width = 260;
            button7.Left = 220;
                label2.Visible = true;
            }
        }
    }
    }
