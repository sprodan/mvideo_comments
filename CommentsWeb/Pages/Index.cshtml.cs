using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parse.Logic;
using Parse.Models;
using CommentsWeb.Extentions;
using System.Web;
using Microsoft.Extensions.Primitives;

namespace CommentsWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string CountComments { get; set; }
        [BindProperty]
        public string CountWorlds { get; set; }
        [BindProperty]
        public string Accuracy { get; set; }

        [BindProperty]
        public Comment Comment { get; set; }
        private Parser _parser = new Parser();
        public void OnGet()
        {
            //AI.Instance.Learn();
            Comment = new Comment();
            CountComments = _parser.GetComments().Count.ToString();
            CountWorlds = AI.Instance.GetWorldsCount().ToString();
            Accuracy = AI.Instance.Accuracy(500).ToString();
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostCalculateAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var pos = HttpUtility.UrlDecode(dict["positive"]);
                var neg = HttpUtility.UrlDecode(dict["negative"]);
                var gen = HttpUtility.UrlDecode(dict["general"]);
                if (!string.IsNullOrWhiteSpace(pos) && !string.IsNullOrWhiteSpace(neg) && !string.IsNullOrWhiteSpace(gen))
                {
                    var index = AI.Instance.GetResultIndex(pos, neg, gen);
                    if (index == -1)
                    {
                        return new JsonResult(new { Status = "WARNING", Code = 400 });
                    }
                    return new JsonResult(new { Status = "OK", Code = 200, Index = index });
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
            
        }
        
    }
}