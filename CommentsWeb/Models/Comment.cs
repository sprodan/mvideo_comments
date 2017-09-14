using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Models
{
    public class Comment
    {
        public double Score { get; set; }
        public int Sku { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string PositiveReaction { get; set; }
        public string NegativeReaction { get; set; }
        public string Body { get; set; }

    }
}
