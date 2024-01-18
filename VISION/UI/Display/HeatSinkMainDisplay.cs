using Cognex.VisionPro;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.Schemas;

namespace VISION.UI
{
    public partial class HeatSinkMainDisplay : UserControl
    {
        public HeatSinkMainDisplay()
        {
            InitializeComponent();
        }
      
        public void InputImage(CameraType 구분, ICogImage 이미지, int 번호)
        {
            if(구분 == CameraType.Cam04) //2개
            {
                if (번호 == 1) this.cdyDisplay5.Image = 이미지;
                if (번호 == 2) this.cdyDisplay5_1.Image = 이미지;
            }
            if (구분 == CameraType.Cam05) //3개
            {
                if (번호 == 1) this.cdyDisplay4.Image = 이미지;
                else if (번호 == 2) this.cdyDisplay4_2.Image = 이미지;
                else if (번호 == 3) this.cdyDisplay4_3.Image = 이미지;
            }
        }
    }
}
