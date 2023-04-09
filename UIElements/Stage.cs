using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingOfCartridges.UIElements
{
    class Stage
    {
        public bool GetActive { get { return _isActive; } }

        public Button Button;
        public Label Feedback;
        public Stage NextStage;
        public int StageNum;

        private bool _isActive;

        private string _text;
        private string _cancelText = "Отменить";

        public Stage(Button button, Label feedback)
        {
            _text = button.Text;
            Button = button;
            Feedback = feedback;
        }

        public bool Click(ref string error)
        {
            bool res = true;

            if (!_isActive)
            {
                Active(true);
            }
            else
            {
                res = Deactivate(true);
                if (!res) error = "Отмените предыдущие этапы";
            }

            return res;
        }

        public void Interactive(bool value) 
        {
            if (value)
            {
                Button.Enabled = true;
            }
            else
            {
                Button.Text = _text;
                Button.Enabled = false;
            }
        }

        public void Reset()
        {
            if (Feedback != null) Feedback.Visible = false;
            Button.Text = _text;
            Button.Enabled = false;
            _isActive = false;
        }

        public void Active(bool writeToDB) //Активировать кнопку
        {
            if (Feedback != null) Feedback.Visible = true;
            Button.Text = _cancelText;
            _isActive = true;

            if (NextStage != null)
            {
                NextStage.Interactive(true);
                
            }

            if (writeToDB)
            {
                Form1.Cartridge.Status++;
                MessageLogAcceptDB();
            }
        }

        public bool Deactivate(bool writeToDB) //Деактивировать кнопку
        {
            if (NextStage != null) if (NextStage.GetActive) return false;

            if (NextStage != null) if (!NextStage.GetActive) NextStage.Interactive(false); 


            if (Feedback != null) Feedback.Visible = false;
            Button.Text = _text;
            _isActive = false;
            
            if (writeToDB)
            {
                Form1.Cartridge.Status--;
                MessageLogCancelDB();
            }
            return true;
        }

        private void MessageLogAcceptDB()
        {
            long id = Form1.Cartridge.ID;
            string log = $"Изменен на {Form1.Cartridge.StatusText()}";
            DateTime now = DateTime.Now;

            Database.AddLog(id, log, now);
        }

        private void MessageLogCancelDB()
        {
            long id = Form1.Cartridge.ID;
            string log = $"Изменен на {Form1.Cartridge.StatusText(StageNum-1)}";
            DateTime now = DateTime.Now;

            Database.AddLog(id, log, now);
        }
    }
}
