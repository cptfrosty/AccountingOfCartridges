using AccountingOfCartridges.Data;
using System;

namespace AccountingOfCartridges.Models
{
    class CartridgeModel : ITechnic
    {
        public CatridgeData Data;

        public void ChangeStatus(int num)
        {
            throw new NotImplementedException();
        }
    }
}
