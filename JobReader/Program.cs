using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JobReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "";
            try
            {
                //Change file path to your input file path
                input = File.ReadAllText("C:\\Users\\abrackmann\\Desktop\\testinput.txt");
            }
            catch (SystemException ex)
            {
                throw new Exception(ex.ToString());
            }
            var jobs = RunJobs.CreateJobs(input);
            var output = "";
            foreach(var job in jobs)
            {
                var jobOutput = RunJobs.PrintJob(job);
                Console.WriteLine(jobOutput);
                output = output + jobOutput;
            }
            Console.WriteLine("Press Any Key to write file to your desktop");
            try
            {
                //Change file path to your desired output file path
                File.WriteAllText("C:\\Users\\abrackmann\\Desktop\\testoutput.txt", output);
            }
            catch (SystemException ex)
            {
                throw new Exception(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
