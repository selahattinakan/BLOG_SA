﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IService
    {
        public int GetActiveUserId();
        public string GetActiveUserName();
    }
}
