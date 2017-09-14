using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parse.Logic;
using Parse.Models;
namespace CommentsWeb.Pages
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string TempMessage { get; set; }

        private List<ScoredWorld> positiveWorlds1;
        private List<ScoredWorld> negativeWorlds1;
        private List<ScoredWorld> commonWorlds1;

        private bool _parsingDone = false;
        public void OnGet()
        {
            
            var ai = new AI();
            ai.Progress += i => Console.WriteLine($"{i}0%");

            //if (!_parsingDone)
            //{
            //    TempMessage = "Update your page";
            //    var positiveWorlds = ProcessData(ai, 4);
            //    var negativeWorlds = ProcessData(ai, 5);
            //    var commonWorlds = ProcessData(ai, 6);

            //    Parser.WriteToFile(positiveWorlds, "positiveWorlds.json");
            //    Parser.WriteToFile(negativeWorlds, "negativeWorlds.json");
            //    Parser.WriteToFile(commonWorlds, "commonWorlds.json");
            //    Console.WriteLine("Parsing done");
            //}
            TempMessage = "Parsed";
            _parsingDone = true;
            if(positiveWorlds1 == null) positiveWorlds1 = Parser.GetDataFromFile("positiveWorlds.json");

            if (negativeWorlds1 == null) negativeWorlds1 = Parser.GetDataFromFile("negativeWorlds.json");
            if (commonWorlds1 == null) commonWorlds1 = Parser.GetDataFromFile("commonWorlds.json");
        }

        private double GetResultIndex(string positiveComment, string negativeComment, string comment)
        {
            var positiveIndex = AI.AnalizeComment(positiveComment, positiveWorlds1);
            var negativeIndex = AI.AnalizeComment(negativeComment, negativeWorlds1);
            var commonIndex = AI.AnalizeComment(comment, commonWorlds1);
            var valuableindex = 0;
            double sum = 0;
            if (positiveIndex != -1)
            {
                valuableindex++;
                sum += positiveIndex;
            }
            if (negativeIndex != -1)
            {
                valuableindex++;
                sum += negativeIndex;
            }
            if (commonIndex != -1)
            {
                valuableindex++;
                sum += commonIndex;
            }
            if (valuableindex == 0) return -1;
            else
            {
                var resultIndex = sum / valuableindex;
                return resultIndex;
            }
        }


        static List<ScoredWorld> ProcessData(AI ai, int index)
        {
            var data = new Parser().GetComments();
            Console.WriteLine($"_______________{index} worlds_________________");
            var processedData = ai.GetStructuredWorldCount(data, index);
            Console.WriteLine("done");
            return processedData;
        }
    }
}