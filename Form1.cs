using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccountingOfCartridges.Data;
using AccountingOfCartridges.UIElements;

namespace AccountingOfCartridges
{
    public partial class Form1 : Form
    {
        public static CatridgeData Cartridge;
        private StageController _stageController;

        public Form1()
        {
            InitializeComponent();
            InitRunApp();

            InputCodePoly.Focus();

            Database.Connect();

            DataGridViewTextBoxColumn colNumBarcode;
            DataGridViewTextBoxColumn colModel;
            DataGridViewTextBoxColumn colCabinet;
            DataGridViewTextBoxColumn colStatus;

            colNumBarcode = new DataGridViewTextBoxColumn();
            colNumBarcode.Name = "Баркод";

            colModel = new DataGridViewTextBoxColumn();
            colModel.Name = "Модель";

            colCabinet = new DataGridViewTextBoxColumn();
            colCabinet.Name = "Кабинет";

            colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Статус";

            dataGridView1.Columns.AddRange(colNumBarcode, colModel, colCabinet, colStatus);

            for (int i = 0; i < Database.Catridges.Count; i++) {
                CatridgeData catridge = Database.Catridges[i];

                dataGridView1.Rows.Add(catridge.BarcodeNumber, catridge.Model, catridge.Office, catridge.StatusText());
            }

            Database.LoadedCartridge += Database_LoadedCartridge;
        }

        private void Database_LoadedCartridge()
        {
            UpdateTableToBarcode(InputCodePoly.Text);
        }

        public void InitRunApp()
        {
            _stageController = new StageController();

            _stageController.AddStage(BtnStage1, TextStage1);
            _stageController.AddStage(BtnStage2, TextStage2);
            _stageController.AddStage(BtnStage3, TextStage3);
            _stageController.AddStage(BtnStage4, null);

            _stageController.ResetStages();

            //_stages = new List<StageController>();
            //_stages.Add(new StageController(BtnStage1, TextStage1));
            //_stages.Add(new StageController(BtnStage2, TextStage2));
            //_stages.Add(new StageController(BtnStage3, TextStage3));
            //_stages.Add(new StageController(BtnStage4));

            //for(int i = 0; i < _stages.Count - 1; i++)
            //{
            //    _stages[i].NextStage = _stages[i + 1];

            //    if (i > 0) _stages[i].Disable();
            //}

            //_stages[_stages.Count - 1].Disable();
        }

        private void BtnStageClick(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;

            string error = "";
            bool result = _stageController.Click(btnSender, ref error);
            if (!result) MessageBox.Show(error);

        }

        private void InputCodePoly_TextChanged(object sender, EventArgs e)
        {
            UpdateTableToBarcode(InputCodePoly.Text);
        }

        private void UpdateTableToBarcode(string barcode)
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < Database.Catridges.Count; i++)
            {
                if (Database.Catridges[i].BarcodeNumber.Contains(barcode))
                {
                    CatridgeData catridge = Database.Catridges[i];
                    dataGridView1.Rows.Add(catridge.BarcodeNumber, catridge.Model, catridge.Office, catridge.StatusText());
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string barcode = row.Cells[0].Value.ToString();
                UpdateInfoSelectRowCartridge(barcode);
            }
        }

        private void UpdateInfoSelectRowCartridge(string barcode)
        {
            CatridgeData cartridge = Database.Catridges.Where(c => c.BarcodeNumber == barcode).First<CatridgeData>();
            textLogsCartridge.Text = "";
            Cartridge = cartridge;
            List<string> logs = Database.LoadLogs(cartridge.ID);

            if (cartridge != null)
            {
                textIDCartridge.Text = $"ID: {cartridge.ID}";
                textBarcodeCartridge.Text = $"barcode: {cartridge.BarcodeNumber}";
                textNameCartridge.Text = $"{cartridge.Model}";
                textCabinetCartridge.Text = $"каб. {cartridge.Office}";
                textFCsCartridge.Text = $"{cartridge.FCs}";
                textPrinterCartridge.Text = $"Принтер: {cartridge.Printer}";

                _stageController.SetStage(cartridge.Status);
            }

            if (logs.Count > 0)
            {
                for (int i = 0; i < logs.Count; i++)
                {
                    textLogsCartridge.Text += logs[i] + "\n";
                }
            }
            else
            {
                textLogsCartridge.Text = "Логи не найдены";
            }
        }
    }
}
