﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }

        //public int PostId { get; set; }
        //public Post Post { get; set; }
    }
}
