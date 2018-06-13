using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities
{
    public static class StaticInfo
    {
        public static List<int> minutesForLevel = new List<int>()
        {
            //0, //           lev0
            //5, //5 min     lev11
            //60, //10 min    lev12
            //8640, //6z      lev21
            //20160, //2w     lev22
            //43200, //1month lev3
            //0 // last level lev4
            0, //           lev0
            3, //3 min     lev11
            5, //5 min    lev12
            10, //10min     lev21
            15, //15min     lev22
            20, //20min lev3
            0 // last level lev4
        };

        public static int TotalLevelNumber = 0;
    }
}
