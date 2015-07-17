﻿using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace Blog.Models
{

    public class Comment
    {
        

        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId {get; set;}
        public string Body {get; set;}
        public System.DateTimeOffset Created {get; set;}
        public Nullable<System.DateTimeOffset> Updated {get; set;}
        public string UpdateReason {get;set;}

        public virtual ApplicationUser Author { get; set; }
        public virtual BlogPost Post { get; set; }

    }
}