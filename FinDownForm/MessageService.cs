using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.MessageBoxIcon;

namespace FinDownForm {
    class MessageService {
        public void ShowMessage(string message) {
            MessageBox.Show(message, @"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowExclamation(string exclamation) {
            MessageBox.Show(exclamation, @"Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void ShowError(string error) {
            MessageBox.Show(error, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}