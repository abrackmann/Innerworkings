using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobReader
{
    class RunJobs
    {
        //private static List<Job> Jobs; //Move to db layer

        public static List<Job> CreateJobs(string input)
        {
            List<Job> jobsOut = new List<Job>();
            var docLines = input.Split(new string[] { "\r\n\r\n" }, System.StringSplitOptions.None);
            foreach (var job in docLines)
            {
                int jobId;
                var currentJob = new Job();
                var lines = new StringReader(job);
                while (true)
                {
                    var line = lines.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    else if (line.Contains("Job"))
                    {
                        var res = Regex.Match(line, @"\d+").Value;
                        int.TryParse(res, out jobId);
                        currentJob.Id = jobId;
                        currentJob.LineItems = new List<LineItem>();
                    }
                    else if (line.Contains("extra-margin"))
                    {
                        currentJob.ExtraMargin = true;
                    }
                    else if (!string.IsNullOrEmpty(line)) //everything else should be a line item
                    {
                        var item = line.Split(' ');
                        var lineItem = new LineItem();
                        if (item.Length > 1 && item.Length < 4)
                        {
                            //items should be in string value exempt format
                            foreach (var part in item)
                            {
                                decimal val;
                                var isDecimal = decimal.TryParse(part, out val);
                                if (isDecimal)
                                    lineItem.Cost = val;
                                else if (part.ToLower() == "exempt") 
                                    lineItem.Exempt = true;
                                else
                                    lineItem.Name = part;
                            }
                            calculateLineItemTotal(lineItem);
                            currentJob.LineItems.Add(lineItem);
                        }
                    }                    
                }
                calculateJobTotal(currentJob);
                jobsOut.Add(currentJob);
            }
            //Jobs = jobsOut;
            return jobsOut;
        }

        public static string PrintJob (Job job)
        {
            var lineOut = new StringBuilder();
            foreach(var item in job.LineItems)
            {
                lineOut.AppendLine(string.Format("{0}: {1}", item.Name, item.TotalCost));
            }
            lineOut.AppendLine(string.Format("Total: {0}", job.Total));
            lineOut.AppendLine();
            return lineOut.ToString();
        }

        //additional methods
        //get job
        //update job

        private static void calculateLineItemTotal(LineItem lineItem)
        {
            lineItem.TotalCost = lineItem.Cost;
            if (!lineItem.Exempt)
                lineItem.TotalCost = lineItem.TotalCost * .07m + lineItem.TotalCost;
            lineItem.TotalCost = Math.Round(lineItem.TotalCost, 2);
        }

        private static void calculateJobTotal(Job job)
        {
            job.Total = job.LineItems.Sum(l => l.TotalCost);
            var allCost = job.LineItems.Sum(l => l.Cost);
            if (job.ExtraMargin)
                job.Total = job.Total + allCost * .16m;
            else
                job.Total = job.Total + allCost * .11m;
            job.Total = (0.02m / 1.00m) * Math.Round(job.Total * (1.00m/0.02m));
        }
    }
}
