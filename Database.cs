using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using AccountingOfCartridges.Data;
using System.Data;

namespace AccountingOfCartridges
{
    class Database
    {
        static string pathDB = @"AccountingOfCartridges";
        static SQLiteConnection connection;
        static SQLiteCommand command;

        public static List<CatridgeData> Catridges;

        public delegate void Loaded();
        public static event Loaded LoadedCartridge;

        public static bool Connect()
        {
            Catridges = new List<CatridgeData>();

            try
            {
                connection = new SQLiteConnection("Data Source=" + pathDB + ";Version=3; FailIfMissing=False");
                connection.Open();
                LoadCartridges();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }

        //----COMMANDS---------------------//
        public static bool LoadCartridges()
        {
            Catridges.Clear();

            command = new SQLiteCommand(connection);

            command.CommandText = $"SELECT * FROM Cartridges";
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);

            if (data.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    CatridgeData catridge = new CatridgeData();
                    DataRow row = data.Rows[i];
                    catridge.ID = row.Field<long>("ID");
                    catridge.Model = row.Field<string>("Model");
                    catridge.Office = row.Field<string>("Cabinet");
                    catridge.FCs = row.Field<string>("FCs");
                    catridge.BarcodeNumber = row.Field<string>("NumBarcode");
                    catridge.Printer = row.Field<string>("Printer");
                    catridge.Status = row.Field<int>("IDStatus");

                    Catridges.Add(catridge);
                }

                LoadedCartridge?.Invoke();
                return true;
            }
        }

        public static void AddLog(long ID, string log, DateTime date)
        {
            command = new SQLiteCommand(connection);
            command.CommandText = $"INSERT INTO Logs (IDCartridge, Date, Log) VALUES ('{ID}', '{date}', '{log}')";

            command.ExecuteNonQueryAsync();

            UpdateCartrige(Form1.Cartridge);

            LoadCartridges();
        }

        public static void UpdateCartrige(CatridgeData cartridge)
        {
            CatridgeData c = cartridge;
            command = new SQLiteCommand(connection);
            command.CommandText = $"Update Cartridges SET " +
                $"IDStatus = {c.Status}" +
                $" WHERE ID = {c.ID})";
            command.ExecuteNonQueryAsync();
        }

        public static List<string> LoadLogs(long id)
        {
            List<string> result = new List<string>();

            command = new SQLiteCommand(connection);

            command.CommandText = $"SELECT * FROM Logs WHERE Logs.IDCartridge = {id}";
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                string date = row.Field<string>("Date");
                string log = row.Field<string>("Log");

                string resultLog = $"[{date}] Изменён статус на \"{log}\"";

                result.Add(resultLog);
            }

            return result;
        }
    }
}
