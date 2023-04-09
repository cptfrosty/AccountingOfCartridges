using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfCartridges.Data
{
    public class CatridgeData
    {
        public long ID;
        public string Model;
        public string Office;
        public int Status; //Отдан -> Принят на склад -> Передан на заправку/ремонт -> Принят на склад для передачи
        public string FCs; //ФИО
        public string BarcodeNumber;
        public string Printer;
        public string Comment;

        private int _stage = 0;

        public string StatusText()
        {
            // 0 - статус, когда катридж находиться у владельца и ниразу не был передан на заправку
            // 1 - катридж находиться на складе для отправки на ремонт/заправку
            // 2 - катридж передан на ремонт/заправку
            // 3 - катридж полученен с заправки/ремонта и находиться на складе
            // 4 - катридж передан в кабинет

            string res = "";
            switch (Status)
            {
                case 0:
                    res = "";
                    break;
                case 1:
                    res = "Ожидает отправки";
                    break;
                case 2:
                    res = "Передан на ремонт/заправку";
                    break;
                case 3:
                    res = "Ожидает передачи в кабинет";
                    break;
                case 4:
                    res = "Передан в кабинет";
                    break;
            }

            return res;
        }

        public string StatusText(int i)
        {
            // 0 - статус, когда катридж находиться у владельца и ниразу не был передан на заправку
            // 1 - катридж находиться на складе для отправки на ремонт/заправку
            // 2 - катридж передан на ремонт/заправку
            // 3 - катридж полученен с заправки/ремонта и находиться на складе
            // 4 - катридж передан в кабинет

            string res = "";
            switch (i)
            {
                case 0:
                    res = "";
                    break;
                case 1:
                    res = "Ожидает отправки";
                    break;
                case 2:
                    res = "Передан на ремонт/заправку";
                    break;
                case 3:
                    res = "Ожидает передачи в кабинет";
                    break;
                case 4:
                    res = "Передан в кабинет";
                    break;
            }

            return res;
        }

        public int GetStage()
        {
            return _stage;
        }

        public void NextStage()
        {

        }

        public void PrevStage()
        {

        }

        public void SetStage()
        {

        }
    }
}
