﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin.BankObjects
{
    public class Account
    {
        public ulong Owner { get; set; }
        public long Balance { get; set; }
    }
}
