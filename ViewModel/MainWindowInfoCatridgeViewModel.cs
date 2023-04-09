using AccountingOfCartridges.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingOfCartridges.ViewModel
{
    class MainWindowInfoCatridgeViewModel
    {
        private CatridgeData catridge;

        public Label Id;
        public Label NameCatridge;
        public Label Cabinet;
        public Label FCs; //ФИО
        public Label Printer;
        public Label Barcode;
        public Label LastUpdate;
        public Label Logs;

        

        public void SetModel(CatridgeData catridge)
        {
            this.catridge = catridge;
        }

        public void SetStage()
        {
            
        }

        public void DeleteCatridge()
        {

        }
    }
}
