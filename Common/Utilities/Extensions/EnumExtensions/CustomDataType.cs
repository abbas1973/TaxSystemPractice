﻿using System.ComponentModel;

namespace Application.Utilities
{
    /// <summary>
    /// نوع داده 
    /// </summary>
    public enum CustomDataType
    {
        [Description("عددی")]
        Number = 0,


        [Description("متنی")]
        String = 1,


        [Description("بولین")]
        Bool = 2,


        [Description("دراپدون")]
        DropDown = 3
    }
}
