using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Parse.Models;

namespace Parse.Logic
{
    public class Parser
    {
        private List<Comment> _comments;

        public static string FixLine(string line)
        {
            var newLine = "";
            var flag = false;
            foreach (var c in line)
            {
                var tmpc = c;
                if (tmpc == '\"')
                {
                    flag = !flag;
                }
                if (flag)
                {
                    if (tmpc == ',') tmpc = '.';
                }
                newLine += tmpc;
            }
            return newLine;
        }

        public static string FixWorld(string world)
        {
            return world.ToLower()
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(")", "")
                .Replace("!", "")
                .Replace("#", "")
                .Replace("(", "")
                .Replace("?", "")
                .Replace("+", "")
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("*", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("|", "");
        }

        public List<Comment> GetComments()
        {
            if (_comments != null) return _comments;

            var dataList = new List<Comment>();
            var lines = File.ReadAllLines(@"feedback.csv");
            foreach (var line in lines)
            {
                var fixedline = FixLine(line);
                var data = fixedline.Split(',');
                if (data[0].Contains('.')) data[0] = data[0].Replace('.', ',');
                if (data.Length == 7)
                {
                    dataList.Add(new Comment
                    {
                        Score = double.Parse(data[0]),
                        Sku = int.Parse(data[1]),
                        Name = data[2],
                        Date = data[3],
                        PositiveReaction = data[4],
                        NegativeReaction = data[5],
                        Body = data[6]

                    });
                }
                if (data.Length == 5)
                {
                    dataList.Add(new Comment
                    {
                        Score = double.Parse(data[0]),
                        Sku = int.Parse(data[1]),
                        Name = data[2],
                        Date = data[3],
                        Body = data[4]

                    });
                }
            }
            _comments = dataList;
            return _comments;
        }

        public static void WriteToFile(List<ScoredWorld> data, string fileName)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, serializedData);
        }

        public static List<ScoredWorld> GetDataFromFile(string fileName)
        {
            var data = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<ScoredWorld>>(data);
        }
    }
}
