using Core1.Persistance;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core1.Services
{
    public class ClassExportCSV : ParentClass
    {
        public void ExportCSV ()
        {
            var Dataslist = connexion.Datas.ToList();
            //var Dataslist2 = (from d in connexion.Datas select d).ToList();  //identique à la ligne du dessus mais au format Linq

            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("id, date, temperature, taux d'humidite");

            foreach (Datas data in Dataslist)
            {
                
                csvcontent.AppendLine(data.id.ToString() + "," +
                    data.date.ToString() + "," +
                    data.temperature.ToString().Replace(",", ".") + "," +
                    data.humidite.ToString() + "%");
            }

            Console.ReadLine();
            try
            {
                string pathCsv = @"C:\Users\JL DUBS\Dropbox\C# Project - Groupe 3\2_Developpement\Export CSV\source_donnees.csv";
                File.AppendAllText(pathCsv, csvcontent.ToString());

                Console.WriteLine("fichier CSV créé");
                MessageBox.Show("votre fichier .csv a bien été créé");
            }
            catch
            {
                MessageBox.Show("attention le fichier n'a pu être créé");
            }
        }
    }
}
