using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Data
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
        public string ArticleText { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool ApprovedStatus { get; set; }
    }
}
