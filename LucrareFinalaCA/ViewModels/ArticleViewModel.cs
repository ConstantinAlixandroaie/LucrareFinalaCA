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
        [Required]
        public string Title { get; set; }
        public byte[] Image { get; set; } 
        public string Author { get; set; }
        [Required]
        public string ArticleText { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string[] Categories { get; set; }
        public bool ApprovedStatus { get; set; }

    }
}
