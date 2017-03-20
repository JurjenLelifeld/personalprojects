using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MasterThesisCreateCitingPatentsList
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + @"\";
            long totalNumberOfCitingDocuments = 0;

            Console.WriteLine("Enter company name:");
            string companyName = Console.ReadLine();

            try
            {
                Console.WriteLine("Reading the files from the folder to process");
                string[] files = Directory.GetFiles(path, "*.txt");

                int year = 2001;

                foreach (var file in files)
                {
                    Console.WriteLine("Processing file of year {0}", year);

                    List<string> citingDocumentNumbers = new List<string>();                    
                    int numberOfDocuments = 1;
                    int numberOfFilesCreated = 0;
                    long totalNumberOfCitingDocumentsPerYear = 0;

                    var lines = File.ReadAllLines(file).Where(item => !string.IsNullOrWhiteSpace(item));

                    Console.WriteLine("Processing document numbers retrieved from file");
                    foreach (var line in lines)
                    {
                        string[] documentNumbers = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                        foreach (var documentNumber in documentNumbers)
                        {
                            citingDocumentNumbers.Add(documentNumber);
                            numberOfDocuments++;
                            totalNumberOfCitingDocuments++;
                            totalNumberOfCitingDocumentsPerYear++;

                            if (numberOfDocuments == 1000)
                            {
                                CreateNewFile(citingDocumentNumbers, path, year, companyName, ref numberOfFilesCreated);
                                numberOfDocuments = 1;
                                citingDocumentNumbers.Clear();
                            }
                        }
                    }

                    if(citingDocumentNumbers.Any())
                        CreateNewFile(citingDocumentNumbers, path, year, companyName, ref numberOfFilesCreated);
                    
                    Console.WriteLine("Processed {0} citing document numbers for year {1}", totalNumberOfCitingDocumentsPerYear, year);
                    year++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: {0}", e.ToString());
            }

            Console.WriteLine("Processed {0} citing document numbers", totalNumberOfCitingDocuments);
            Console.WriteLine("Finished processing! Press any key to exit.");
            Console.ReadLine();
        }

        private static void CreateNewFile(List<string> citingDocumentNumbers, string path, int year, string companyName, ref int numberOfFilesCreated )
        {
            numberOfFilesCreated++;

            string nameForNewFile = companyName + "_" + year.ToString() + "_CITINGNUMBERS_" + numberOfFilesCreated.ToString("D2") + ".txt";
            string pathForNewFile = path + nameForNewFile;

            if (!File.Exists(pathForNewFile))
            {
                Console.WriteLine("Creating new file {0}", nameForNewFile);
                File.WriteAllLines(pathForNewFile, citingDocumentNumbers, Encoding.UTF8);
            }
            else
            {
                Console.WriteLine("Creating new file {0} failed", nameForNewFile);
            }
        }
    }
}
