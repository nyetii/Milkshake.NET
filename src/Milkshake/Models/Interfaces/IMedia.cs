﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface IMedia : IMilkshake
    {
        public DateTime CreationDateTime { get; set; }
        public string Path { get; set; }
    }
}
