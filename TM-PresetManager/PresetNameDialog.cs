using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TM_PresetManager
{
    public partial class PresetNameDialog : Form
    {
        public string presetName;
        public PresetNameDialog()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (presetNameBox.Text == string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Specify the name");
                return;
            }
            this.DialogResult = DialogResult.OK;
            presetName = presetNameBox.Text;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
