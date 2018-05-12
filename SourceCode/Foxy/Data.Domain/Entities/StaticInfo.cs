using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities
{
    public static class StaticInfo
    {
        public static List<int> minutesForLevel = new List<int>()
        {
            0, //           lev0
            30, //5 min     lev11
            60, //10 min    lev12
            8640, //6z      lev21
            20160, //2w     lev22
            43200, //1month lev3
            0 // last level lev4
        };
    }
}
