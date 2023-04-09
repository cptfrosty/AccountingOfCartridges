using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfCartridges.Models
{
    class CartridgeModel
    {
        public int ID;
        public string Model;
        public string Office;
        public int Status; //Отдан -> Принят на склад -> Передан на заправку/ремонт -> Принят на склад для передачи
        public string FCs; //ФИО
        public string BarcodeNumber;
        public string Printer;
        public string Comment;
    }
}
