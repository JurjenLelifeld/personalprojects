using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MasterThesisCreateFoldersMoveCSVs
{
    /// <summary>
    /// This program processes all CSV files in the folder based on the acquired companies
    /// and focal companies where the CSV files have to be placed. It automatically create the
    /// correct folder for each focal company and moves the correct files to the folder. 
    /// It also creates the AcquisitionInYear and SDC Search Term text files.
    /// </summary>
    /// <author>
    /// Jurjen Lelifeld
    /// </author>
    /// <date>
    /// 15-04-2016
    /// </date>
    class Program
    {
        /// <summary>
        /// Main method that handles all processes
        /// </summary>
        static void Main()
        {
            // Initialize variables
            
            int focalCompanyCount = 0;

            // Create paths to retrieve data
            string rootPath = Directory.GetCurrentDirectory();
            string pathToCompaniesToProcess = rootPath + @"\CompanyToProcess.txt";
            string pathToFocalCompanies = rootPath + @"\FocalCompanies.txt";
            string pathToExampleExcelFile = rootPath + @"\EXAMPLE.xlsm";

            // Get the focal company names, used to sort the files per focal company
            var readFocalCompanies = File.ReadAllLines(pathToFocalCompanies);            

            // Get the SDC Search Term, Acquisition years and filenaming data per focal company
            var listWithToProcessPerCompany = CreateListWithCompaniesToProcess(pathToCompaniesToProcess);

            // Iterate through each focal company to process
            ProcessAllCompanies(listWithToProcessPerCompany, rootPath, readFocalCompanies, focalCompanyCount, pathToExampleExcelFile);            
        }

        private static List<List<List<string>>> CreateListWithCompaniesToProcess(string pathToCompaniesToProcess)
        {
            var listWithToProcessPerCompany = new List<List<List<string>>>();
            var listToProcess = new List<List<string>>();

            // Get the SDC Search Term, Acquisition years and filenaming data per focal company
            var readCompaniesToProcess = File.ReadAllLines(pathToCompaniesToProcess);
            foreach (var line in readCompaniesToProcess)
            {
                // If a whitespace/empty string is read, data of one focal company is processed
                if (string.IsNullOrWhiteSpace(line) || line.Equals("END"))
                {
                    // Add the data to the final list
                    listWithToProcessPerCompany.Add(listToProcess);
                    listToProcess = new List<List<string>>();
                }
                // Otherwise, just keep adding the data to the current company list
                else
                {
                    // Split the data of each company
                    var tmpList = line.Split('\t');
                    // Put the three values into their own list
                    var sublist = tmpList.ToList();
                    // Add these values to the list per company to process
                    listToProcess.Add(sublist);
                }
            }

            return listWithToProcessPerCompany;
        }

        private static void ProcessAllCompanies(List<List<List<string>>> listWithToProcessPerCompany, string rootPath, string[] readFocalCompanies,
            int focalCompanyCount, string pathToExampleExcelFile)
        {
            // Loop through all companies to process
            foreach (var companyToProcess in listWithToProcessPerCompany)
            {
                // Restart company number count for each focal company
                int companyNumber = 1;

                // Create a new directory for the focal company
                string pathForFocalCompanyDirectory = rootPath + @"\" + readFocalCompanies[focalCompanyCount];
                Directory.CreateDirectory(pathForFocalCompanyDirectory);

                // Now process all acquisition companies of that focal company
                ProcessAcquiredCompanies(rootPath, companyToProcess, pathForFocalCompanyDirectory, companyNumber, pathToExampleExcelFile);

                focalCompanyCount++;
            }
        }

        private static void ProcessAcquiredCompanies(string rootPath, List<List<string>> companyToProcess, string pathForFocalCompanyDirectory,
            int companyNumber, string pathToExampleExcelFile)
        {
            // Now process all acquisition companies of that focal company
            foreach (var company in companyToProcess)
            {
                // Create a folder for each acquired company based on the number and given name
                string pathForDirectory = pathForFocalCompanyDirectory + @"\" + companyNumber.ToString("D2") + "_" +
                                          company[1];
                Directory.CreateDirectory(pathForDirectory);

                // Create paths for all files in that directory
                string pathForFiles = pathForDirectory + @"\";
                string pathForAcquisitionYearFile = pathForFiles + "AcquisitionInYear.txt";
                string pathForSDCSearchTerm = pathForFiles + "SDC Search Term.txt";
                string pathForExampleExcelFile = pathForFiles + company[1] + "_ALLIANCES_COMBINED.xlsm";

                // Create both textfiles with the SDC search term and year
                File.WriteAllText(pathForAcquisitionYearFile, company[2]);
                File.WriteAllText(pathForSDCSearchTerm, company[0]);
                File.Copy(pathToExampleExcelFile, pathForExampleExcelFile);

                // Get the current list of files in the folder that can be moved
                var csvFiles = Directory.GetFiles(rootPath + @"\", @"*.csv");

                // Check if the files in the folder belong to the current company
                foreach (var csv in csvFiles)
                {
                    // If they belong to the current company, move the files to the folder
                    if (!csv.Contains(company[1])) continue;
                    string nameForCSVFile = csv.Split('\\').Last();
                    File.Move(csv, pathForFiles + nameForCSVFile);
                }

                companyNumber++;
            }
        }
    }
}
