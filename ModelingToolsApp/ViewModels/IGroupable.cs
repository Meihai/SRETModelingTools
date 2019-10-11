﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelingToolsApp.ViewModels
{
    interface IGroupable
    {
        Guid ID {get;}

        Guid ParentID{get;set;}
        bool IsGroup{get;set;}
    
    }
}
