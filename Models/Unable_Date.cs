﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Unable_Date
    {
        [Key]
        public int DateId { get; set; }
        public DateTime Date { get; set; }

    }
}
