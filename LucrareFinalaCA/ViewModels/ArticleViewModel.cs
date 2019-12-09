using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; } //change to upload image to create a theme contained website
        public string Author { get; set; }
        public string ArticleText { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string[] Categories { get; set; }
        public List<RatingViewModel> Ratings { get; set; }
    }
}
