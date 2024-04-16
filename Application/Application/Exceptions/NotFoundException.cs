﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
