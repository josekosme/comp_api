﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp_api.common
{
    public interface IDomainEventListenerAppService
    {
        void processEvent(string eventMessage);
    }
}
