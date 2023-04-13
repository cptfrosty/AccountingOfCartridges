using AccountingOfCartridges.Data;
using System;

namespace AccountingOfCartridges.Models
{
    class PrinterModel: ITechnic
    {
        public PrinterData Data;

        public void ChangeStatus(int num)
        {
            throw new NotImplementedException();
        }
    }
}
