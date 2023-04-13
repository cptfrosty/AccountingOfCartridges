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

            CreateTable(); //Создание таблицы отображения

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
                if (Database.Catridges[i].Data.BarcodeNumber.Contains(barcode))
                {
                    CatridgeData catridge = Database.Catridges[i].Data;
                    dataGridView1.Rows.Add(catridge.BarcodeNumber, catridge.Model, catridge.Office, catridge.StatusText());
                }
            }
        }

        private void SelectRowInTable(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string barcode = row.Cells[0].Value.ToString();
                UpdateInfoSelectRowCartridge(barcode);
            }
        }

        private void ShowInfoSelectRow(string barcode)
        {

        }

        private void UpdateInfoSelectRowCartridge(string barcode)
        {
            CatridgeData cartridge = Database.Catridges.Where(c => c.Data.BarcodeNumber == barcode).First<CatridgeData>();
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

        /// <summary>
        /// Создание таблицы для отображения данных
        /// </summary>
        private void CreateTable()
        {
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

            for (int i = 0; i < Database.Catridges.Count; i++)
            {
                CatridgeData catridge = Database.Catridges[i];

                dataGridView1.Rows.Add(catridge.BarcodeNumber, catridge.Model, catridge.Office, catridge.StatusText());
            }
        }
    }
}
