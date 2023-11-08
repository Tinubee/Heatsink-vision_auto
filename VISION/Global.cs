using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VISION.Schemas;

namespace VISION
{
    public static class Global
    {
        public static 그랩제어 그랩제어;
        public static 조명제어 조명제어;
        public static 환경설정 환경설정;


        public static Boolean Init()
        {
            그랩제어 = new 그랩제어();
            조명제어 = new 조명제어();
            환경설정 = new 환경설정();

            return true;
        }
    }

   
}
