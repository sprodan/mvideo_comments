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

        [BindProperty]
        public Comment Comment { get; set; }

        public void OnGet()
        {
            Comment = new Comment();
            TempMessage = "Parsed";
        }

        public void OnPost()
        {
            if (!ModelState.IsValid) return;
            var index = AI.Instance.GetResultIndex(Comment.PositiveReaction, Comment.NegativeReaction, Comment.Body);
            TempMessage = $"your index: {index}";
        }





    }
}