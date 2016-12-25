using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGenGam
{
    public partial class frmGetComments : Form
    {
        Form firstformref;
        public frmGetComments(ref Screen ScreenHandle)
        {
            firstformref = new Screen();
             firstformref = ScreenHandle;
          filetype = ScreenHandle.filetypeList;
            InitializeComponent();
        }

        IList<Filetype> filetype ;

        private void btnSaveComments_Click(object sender, EventArgs e)
        {
            //firstformref .file

        }
    }
}
