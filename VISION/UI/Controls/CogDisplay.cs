using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION.UI.Controls
{
    public partial class CogDisplay : XtraUserControl
    {
        public static Color 배경색상 = DevExpress.LookAndFeel.DXSkinColors.IconColors.Black;
        public CogDisplay() => InitializeComponent();

        public void Init(Boolean showScrollBar = true)
        {
            this.cdyDisplay.AutoFit = true;
            this.cdyDisplay.BackColor = 배경색상;
            this.cdyDisplay.MouseMode = CogDisplayMouseModeConstants.Pan;
        }

        public void SetImage(CogImage8Grey image)
        {
            this.cdyDisplay.Image = image;
            this.cdyDisplay.Fit(true);
        }
    }
}
