using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    public class Class_Common
    {
        public void Err(string message)
        {

        }
        public bool info(string message)
        {
            bool result = false;
            Infomation infoform = new Infomation();
            infoform.msg = message;
            if (infoform.ShowDialog() == DialogResult.OK)
                result = true;

            return result;
        }
    }

}
