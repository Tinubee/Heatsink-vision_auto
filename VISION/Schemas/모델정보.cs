using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public enum 모델구분
    {
        [Bindable(false)]
        None = 0,
        [Description("0023")]
        HeatSink0023 = 1,
        [Description("0024")]
        HeatSink0024 = 2,
        [Description("0026")]
        HeatSink0026 = 3,
        [Description("0028")]
        HeatSink0028 = 4,
        [Description("0033")]
        HeatSink0033 = 5,
    }

    public class 모델정보
    {

    }
}
