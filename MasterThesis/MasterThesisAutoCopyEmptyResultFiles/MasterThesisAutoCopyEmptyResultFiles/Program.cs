using System;
using System.IO;

namespace MasterThesisAutoCopyEmptyResultFiles
{
    /// <summary>
    /// This program copies an example file for the current directory and copies it to all subdirectories.
    /// It changes the name according up to the year the acquisition occured. It creates both the resolved and unresolved file.
    /// If the file already exists (because of relevant data) then that file will be skipped.
    /// </summary>
    /// <remarks>
    /// The program has to be run from the directory that contains all subdirectories where the files have to be copied.
    /// An example file that has to be copied needs to be in the same directory with the program.
    /// This file has to be named "EMPTY_EXAMPLE.csv".
    /// Every subdirectory needs a text file called "AcquisitionInYear.txt" containing the year up to the program should create files.
    /// </remarks>
    /// <author>Jurjen Lelifeld</author>
    /// <date>07-04-2016</date>
    public class Program
    {
        private const int _startYear = 1996;
        private const string _resolved = "RES.csv";
        private const string _unresolved = "UNRES.csv";
        private const string _inputFileName = "EMPTY_EXAMPLE.csv";

        enum ExitCode : byte
        {
            Success = 0
        }

        /// <summary>
        /// Main method. Executes everything.
        /// </summary>
        static int Main()
        {
            Console.WriteLine("Make sure the program is in the folder where the company subfolders are");
            Console.WriteLine("Make sure the empty file to copy is called EMPTY_EXAMPLE.csv and is in the same folder as this program");
            Console.WriteLine("Press any key to start if above is complied");
            Console.ReadLine();
            
            // Retrieve the current dir the program is run from
            string sourceDir = Directory.GetCurrentDirectory();

            // Retrieve the list of subdirectories in the current dir
            var subdirectoryList = Directory.GetDirectories(sourceDir);           

            // Retrieve the file and store it in a variable          
            FileInfo file = new FileInfo(Path.Combine(sourceDir, _inputFileName));

            foreach (string subdirectory in subdirectoryList)
            {
                int yearForFileName = _startYear;

                // Extract the ending year of this acquisition to calculate number of files needed
                int endingYear = int.Parse(File.ReadAllText(Path.Combine(subdirectory, "AcquisitionInYear.txt")));
                int numberOfYearsBeforeAcquisition = endingYear - _startYear;

                // Extract the company name from the folder name
                var splittedSubdirectoryName = subdirectory.Split('\\');
                string companyName = splittedSubdirectoryName[splittedSubdirectoryName.Length - 1];
                companyName = companyName.Substring(3, companyName.Length - 3);

                // Loop through the number of years files are needed
                for (int i = 0; i <= numberOfYearsBeforeAcquisition; i++)
                {
                    // Build namestring 
                    string fileNameYear = yearForFileName.ToString();
                    string fileName = fileNameYear + "_" + companyName + "_";

                    // Create the RES file
                    string resolvedFileName = fileName + _resolved;
                    copyFile(file, subdirectory, resolvedFileName); 

                    // Create the UNRES file
                    string unResolvedFileName = fileName + _unresolved;
                    copyFile(file, subdirectory, unResolvedFileName);

                    // Increase year for next files
                    yearForFileName++;
                }                
            }

            Console.WriteLine("Done! All files copied. Press any key to exit.");
            Console.ReadLine();

            return (int)ExitCode.Success;
        }

        private static void copyFile(FileInfo file, string subdirectory, string fileName)
        {
            // Create final path and name to save the file
            string filePathAndName = Path.Combine(subdirectory, fileName);

            // If the file does not yet exist, copy the file. Else, skip.
            if (!File.Exists(filePathAndName))
            {
                file.CopyTo(filePathAndName);
            }
        }
    }
}
