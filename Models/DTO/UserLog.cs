﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UserLog
    {
        public int UserId {  get; set; }
        public string Password { get; set; } = null!;
    }
}
