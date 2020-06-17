using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinDownForm
{
    public static class ControlHelper
    {
        public static void InvokeEx(this Control control, Action action) {
            if (control.InvokeRequired) {
                control.Invoke(action);
            } else {
                action();
            }
        }
    }
}
