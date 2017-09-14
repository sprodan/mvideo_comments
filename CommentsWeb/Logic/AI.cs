﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Parse.Models;

namespace Parse.Logic
{
    public class ScoredWorld
    {
        public string World { get; set; }
        public int Count { get; set; }
        public double Score { get; set; }
    }

    public class AI
    {
        public Action<int> Progress;

        private static List<Tuple<object, double>> GetLines(List<Comment> data, int index)
        {
            var result = new List<Tuple<object, double>>();
            foreach (var comment in data)
            {
                switch (index)
                {
                    case 0:
                        result.Add(new Tuple<object, double>(comment.Score, comment.Score));
                        break;
                    case 1:
                        result.Add(new Tuple<object, double>(comment.Sku, comment.Score));
                        break;
                    case 2:
                        result.Add(new Tuple<object, double>(comment.Name, comment.Score));
                        break;
                    case 3:
                        result.Add(new Tuple<object, double>(comment.Date, comment.Score));
                        break;
                    case 4:
                        result.Add(new Tuple<object, double>(comment.PositiveReaction, comment.Score));
                        break;
                    case 5:
                        result.Add(new Tuple<object, double>(comment.NegativeReaction, comment.Score));
                        break;
                    case 6:
                        result.Add(new Tuple<object, double>(comment.Body, comment.Score));
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public List<ScoredWorld> GetStructuredWorldCount(List<Comment> data, int index)
        {
            var result = new List<ScoredWorld>();
            var dict = new Dictionary<string, int>();
            var lines = GetLines(data, index);
            var count = data.Count;
            int completion = 0;
            var factor = count / 10;
            foreach (var line in lines)
            {
                completion++;
                if(completion % factor == 0) Progress?.Invoke(completion / factor );
                if (line?.Item1 == null) continue;
                var worlds = line.Item1.ToString().Split(' ');
                foreach (var world in worlds)
                {
                    var newworld = Parser.FixWorld(world);
                    if(string.IsNullOrEmpty(newworld) || string.IsNullOrWhiteSpace(newworld) || newworld.Length < 4) continue;
                    if (dict.ContainsKey(newworld))
                    {
                        var scoredWorld = result.FirstOrDefault(x => x.World == newworld);
                        scoredWorld.Count++;
                        scoredWorld.Score += line.Item2;
                    }
                    else
                    {
                        result.Add(new ScoredWorld(){Count = 1, Score = line.Item2, World = newworld});
                        dict.Add(newworld, 1);
                    }

                }
            }
            result.ForEach(x => x.Score = x.Score / x.Count);
            return result;
        }

        public static IOrderedEnumerable<ScoredWorld> SortStructuredCount(List<ScoredWorld> data)
        {
            return data.OrderByDescending(x => x.Count);
        }

        public static double AnalizeComment(string comment, List<ScoredWorld> data)
        {
            var fixedComment = Parser.FixLine(comment);
            var worlds = fixedComment.Split(' ');
            var valuableWorldsCount = 0;
            double index = 0;
            foreach (var world in worlds)
            {
                var normalWorld = Parser.FixWorld(world).Replace(",", "");
                if(normalWorld.Length < 4) continue;
                var worldData = data.FirstOrDefault(x => x.World == normalWorld);
                if(worldData == null) continue;
                index += worldData.Score * worldData.Count;
                valuableWorldsCount += worldData.Count;
            }
            return valuableWorldsCount == 0 ? -1 : index / valuableWorldsCount;
        }
    }
}