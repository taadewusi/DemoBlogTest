using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoBlogTest.Models
{
    public class Blog
    {
        [Required]
        [Key]
        public Guid BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Publication_date { get; set; }
        public string User_Id { get; set; }

    }

    public class APICall
    {
        public List<Blog> data { get; set; }
    }
}