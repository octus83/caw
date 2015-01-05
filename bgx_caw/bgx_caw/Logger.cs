using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    /// <summary>
    /// Logger klasse die sowohl consolen log anbietet sowie eine Textdatei
    /// </summary>
    class Logger
    {
        Boolean writeFile = true;

        public Logger()
        {

        }
        /// <summary>
        /// Erzeugt einen Log String mit Zeit Stempel und schreibt ihn in eine Textdatei und gibt
        /// ihn auch in der Console aus
        /// </summary>
        /// <param name="lines">String was geloggt wird</param>
        /// <param name="partialClass">Partialer Klassen name oder Klassen name</param>
        public void log(String lines, String partialClass)
        {
            DateTime time = DateTime.Now;
            String format = "MMM ddd d HH:mm yyyy";    // Use this format
            // Console.WriteLine(time.ToString(format))
            writeToFile(time.ToString(format) + " - " + partialClass+" : "+lines);
        }
        /// <summary>
        /// Schreibt den Log String zur log.txt
        /// </summary>
        /// <param name="lines"></param>
        private void writeToFile(String lines)
        {
            writeToConsole(lines);
            if (writeFile)
            {
                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt", true);
                    file.WriteLine(lines);
                    file.Close();
                }
                catch (Exception exc)
                {
                    writeToFile("Error Write Log to File");
                    writeToConsole(exc.Message + exc.StackTrace);
                    writeFile = false;
                }
            }
        }
        /// <summary>
        /// Gibt den log String auch in der Konsole aus
        /// </summary>
        /// <param name="lines"></param>
        private void writeToConsole(String lines)
        {
            Console.WriteLine("Console -> " + lines);
        }
    }
}
