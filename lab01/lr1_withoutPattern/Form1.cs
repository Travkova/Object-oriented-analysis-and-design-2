using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr1_withoutPattern
{
    public partial class Form1 : Form
    {
        // Менеджеры печати
        private PrinterManager manager1;
        private PrinterManager manager2;

        // Таймер для автоматического обновления интерфейса
        private Timer updateTimer;

        // Конструктор формы
        public Form1()
        {
            InitializeComponent();

            // Инициализация менеджеров (БЕЗ паттерна Singleton)
            manager1 = PrinterManager.Instance();      // Создает НОВЫЙ экземпляр
            manager2 = new PrinterManager(true);       // Создает НОВЫЙ экземпляр

            // Настройка выпадающего списка принтеров
            SetupComboBox();

            // Настройка и запуск таймера
            SetupTimer();

            // Первоначальное обновление интерфейса
            UpdateUI();
        }

        /// Настройка выпадающего списка принтеров
        private void SetupComboBox()
        {
            cmbPrinter.Items.Clear();
            cmbPrinter.Items.AddRange(new[] { "HP LaserJet", "Canon", "Samsung" });
            cmbPrinter.SelectedIndex = 0;
        }

        /// Настройка таймера для автоматического обновления
        private void SetupTimer()
        {
            updateTimer = new Timer();
            updateTimer.Interval = 500; // Обновление каждые 500 мс
            updateTimer.Tick += Timer_Tick;
            updateTimer.Start();
        }

        /// Обработчик тика таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        /// Обновление всего интерфейса
        private void UpdateUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUI));
                return;
            }

            try
            {
                // Обновление информации о Менеджере 1
                lblManager1Id.Text = $"ID: {manager1.InstanceId}";
                lblManager1Queue.Text = $"Очередь: {manager1.GetDocuments().Count}";

                // Обновление информации о Менеджере 2
                lblManager2Id.Text = $"ID: {manager2.InstanceId}";
                lblManager2Queue.Text = $"Очередь: {manager2.GetDocuments().Count}";

                // Обновление списков документов
                UpdateDocumentsList(lvManager1Docs, manager1);
                UpdateDocumentsList(lvManager2Docs, manager2);

                // Обновление списков принтеров
                UpdatePrintersList(lvManager1Printers, manager1);
                UpdatePrintersList(lvManager2Printers, manager2);
            }
            catch { /* Игнорируем ошибки обновления */ }
        }

        /// Обновление списка документов
        private void UpdateDocumentsList(ListView listView, PrinterManager manager)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            foreach (var doc in manager.GetDocuments())
            {
                ListViewItem item = new ListViewItem(doc.Doc_name);           // Колонка "Документ"
                item.SubItems.Add(doc.CurrentStatus);                          // Колонка "СтатусДокумента"
                item.SubItems.Add(doc.Printer_name);                           // Колонка "НаПринтер"

                // Цветовая индикация статуса
                if (doc.CurrentStatus == "Завершен")
                    item.BackColor = Color.LightGreen;
                else if (doc.CurrentStatus == "Ошибка")
                    item.BackColor = Color.LightCoral;
                else if (doc.CurrentStatus == "Печатается")
                    item.BackColor = Color.LightYellow;

                listView.Items.Add(item);
            }

            listView.EndUpdate();
        }

        /// Обновление списка принтеров
        private void UpdatePrintersList(ListView listView, PrinterManager manager)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            foreach (var printer in manager.GetPrinters())
            {
                ListViewItem item = new ListViewItem(printer.Printer_name);    // Колонка "Принтер"
                item.SubItems.Add(printer.CurrentStatus);                      // Колонка "СтатусПринтера"

                // Цветовая индикация статуса принтера
                if (printer.CurrentStatus == "Свободен")
                    item.ForeColor = Color.Green;
                else if (printer.CurrentStatus == "Занят")
                {
                    item.ForeColor = Color.Orange;
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
                else if (printer.CurrentStatus == "Ошибка")
                    item.ForeColor = Color.Red;
                else if (printer.CurrentStatus == "Оффлайн")
                    item.ForeColor = Color.Gray;

                listView.Items.Add(item);
            }

            listView.EndUpdate();
        }

        /// Добавление документа в менеджер
        private void AddDocumentToManager(PrinterManager manager, string managerName)
        {
            // Проверка на пустое название
            if (string.IsNullOrWhiteSpace(txtDocName.Text))
            {
                MessageBox.Show("Введите название документа", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создание нового документа
            Document doc = new Document(
                txtDocName.Text,
                cmbPrinter.SelectedItem.ToString(),
                txtDocText.Text
            );

            // Добавление в менеджер
            manager.AddDocument(doc);

            // Логирование
            AddLogMessage($" {managerName}: добавлен '{doc.Doc_name}' (ID: {manager.InstanceId})");

            // Очистка полей ввода
            txtDocName.Clear();
            txtDocText.Clear();
        }

        /// Добавление сообщения в лог
        private void AddLogMessage(string message)
        {
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke(new Action<string>(AddLogMessage), message);
                return;
            }

            lbLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");

            // Ограничиваем количество сообщений
            while (lbLog.Items.Count > 50)
                lbLog.Items.RemoveAt(lbLog.Items.Count - 1);
        }

        // Обработчики событий

        /// Кнопка "+в менеджер 1"
        private void btnAddToManager1_Click(object sender, EventArgs e)
        {
            AddDocumentToManager(manager1, "Менеджер 1");
        }

        /// Кнопка "+в менеджер 2"
        private void btnAddToManager2_Click(object sender, EventArgs e)
        {
            AddDocumentToManager(manager2, "Менеджер 2");
        }

        /// Кнопка "Обработать" для Менеджера 1
        private async void btnProcessManager1_Click(object sender, EventArgs e)
        {
            btnProcessManager1.Enabled = false;
            btnProcessManager1.Text = " Обработка...";

            var progress = new Progress<string>(msg => AddLogMessage(msg));
            await manager1.ProcessQueueAsync(progress);

            btnProcessManager1.Text = "Обработать";
            btnProcessManager1.Enabled = true;
        }

        /// Кнопка "Очистить" для Менеджера 1
        private void btnClearManager1_Click(object sender, EventArgs e)
        {
            manager1.ClearQueue();
            AddLogMessage($" Менеджер 1: очередь очищена");
        }

        /// Кнопка "Обработать" для Менеджера 2
        private async void btnProcessManager2_Click(object sender, EventArgs e)
        {
            btnProcessManager2.Enabled = false;
            btnProcessManager2.Text = " Обработка...";

            var progress = new Progress<string>(msg => AddLogMessage(msg));
            await manager2.ProcessQueueAsync(progress);

            btnProcessManager2.Text = "Обработать";
            btnProcessManager2.Enabled = true;
        }

        /// Кнопка "Очистить" для Менеджера 2
        private void btnClearManager2_Click(object sender, EventArgs e)
        {
            manager2.ClearQueue();
            AddLogMessage($" Менеджер 2: очередь очищена");
        }

        /// Кнопка "Показать проблему"
        private void btnShowProblem_Click(object sender, EventArgs e)
        {
            string message =
                "* У каждого менеджера СВОЯ очередь!\n" +
                "* Документы не видны друг другу!\n" +
                "* Принтеры ПРОДУБЛИРОВАНЫ!\n" +
                "* Instance() НЕ гарантирует единственность!\n\n";

            MessageBox.Show(message, "Проблема без Singleton",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// Кнопка "Очистить лог"
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
            AddLogMessage(" Лог очищен");
        }

        /// Освобождение ресурсов при закрытии формы
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (updateTimer != null)
                {
                    updateTimer.Stop();
                    updateTimer.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
    public class Document
    {
        public string[] Doc_id;
        public string[] Doc_status;
        public string Doc_name;
        public string Printer_name;
        public string Content { get; set; }

        public Document(string docName, string printerName, string content)
        {
            Doc_id = new string[1];
            Doc_id[0] = GenerateDocId();

            Doc_status = new string[4];
            Doc_status[0] = "Создан";      // Текущий статус
            Doc_status[1] = "В очереди";    // Статус ожидания
            Doc_status[2] = "Печатается";   // Статус печати
            Doc_status[3] = "Завершен";     // Финальный статус

            Doc_name = docName;
            Printer_name = printerName;
            Content = content;
        }
        private string GenerateDocId()
        {
            return $"DOC-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}";
        }
        public string CurrentStatus
        {
            get { return Doc_status[0]; }
        }
        public void SetStatus(string status)
        {
            if (Doc_status != null && Doc_status.Length > 0)
            {
                Doc_status[0] = status;
            }
        }
    }
    public class Printer
    {
        public string Printer_id;
        public string Printer_name;
        public string[] Printer_status;

        public Printer(String printerName)
        {
            Printer_id = GeneratePrinterId();
            Printer_name = printerName;
            Printer_status = new string[4];
            Printer_status[0] = "Свободен";    // Текущий статус
            Printer_status[1] = "Занят";        // Статус печати
            Printer_status[2] = "Ошибка";       // Статус ошибки
            Printer_status[3] = "Оффлайн";      // Принтер отключен
        }
        private string GeneratePrinterId()
        {
            return $"PRN-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}";
        }
        public string CurrentStatus
        {
            get { return Printer_status[0]; }
        }
        public void SetStatus(string status)
        {
            if (Printer_status != null && Printer_status.Length > 0)
            {
                Printer_status[0] = status;
            }
        }
        public bool IsAvailable()
        {
            return Printer_status != null &&
                Printer_status.Length > 0 &&
                Printer_status[0] == "Свободен";
        }
    }
    public class PrinterManager
    {
        private static PrinterManager UniqueInstance;
        private List<Document> documents;
        private List<Printer> printers;
        public string InstanceId { get; private set; }
        private PrinterManager()
        {
            documents = new List<Document>();
            printers = new List<Printer>();
            InstanceId = Guid.NewGuid().ToString().Substring(0, 8);
            InitializePrinters();
        }
        public PrinterManager(bool createNew)
        {
            InstanceId = Guid.NewGuid().ToString().Substring(0, 8);
            documents = new List<Document>();
            printers = new List<Printer>();
            InitializePrinters();

            // Логируем создание для демонстрации
            Console.WriteLine($"[PrinterManager] Создан НОВЫЙ экземпляр через public конструктор. ID: {InstanceId}");
        }
        public static PrinterManager Instance()
        {
            UniqueInstance = new PrinterManager();
            Console.WriteLine($"[PrinterManager] Instance() создал НОВЫЙ экземпляр. ID: {UniqueInstance.InstanceId}");
            return UniqueInstance;
        }
        private void InitializePrinters()
        {
            printers.Add(new Printer("HP LaserJet"));
            printers.Add(new Printer("Canon"));
            printers.Add(new Printer("Samsung"));
        }
        public string Driver(Document doc)
        {
            return $"%!PS-Adobe-3.0\n" +
                $"%%Title: {doc.Doc_name}\n" +
                $"%%Creator: PrinerManager (ID: {InstanceId})\n" +
                $"%%CreationDate: {DateTime.Now}\n" +
                $"\n" +
                $"({doc.Content}) show\n" +
                $"showpage\n" +
                $"%%EOF";
        }

        public void AddDocument(Document doc)
        {
            documents.Add(doc);
            doc.SetStatus("В очереди");
        }
        public async Task ProcessQueueAsync(IProgress<string> progress)
        {
            var toProcess = new List<Document>(documents);
            foreach (var doc in toProcess)
            {
                var printer = printers.Find(p =>
                p.Printer_name == doc.Printer_name &&
                p.IsAvailable());

                if (printer != null)
                {
                    documents.Remove(doc);
                    doc.SetStatus("Печатается");
                    printer.SetStatus("Занят");
                    progress?.Report($"[{InstanceId}] Печать: {doc.Doc_name} на {printer.Printer_name}");
                    string pclData = Driver(doc);
                    await Task.Delay(1500);
                    bool success = new Random().Next(0, 10) < 8;
                    if (success)
                    {
                        doc.SetStatus("Завершен");
                        progress?.Report($"[{InstanceId}] Документ '{doc.Doc_name}' успешно напечатан");
                    }
                    else
                    {
                        doc.SetStatus("Ошибка");
                        progress?.Report($"[{InstanceId}] Ошибка печати документа '{doc.Doc_name}'");
                    }
                    printer.SetStatus("Свободен");
                }
                else
                {
                    progress?.Report($"[{InstanceId}] Принтер '{doc.Printer_name}'недоступен для документа '{doc.Doc_name}'");
                }
            }
        }
        public void ClearQueue()
        {
            documents.Clear();
        }
        public List<Document> GetDocuments()
        {
            return documents;
        }
        public List<Printer> GetPrinters()
        {
            return printers;
        }
        public string GetInfo()
        {
            return $"ID: {InstanceId} | Документов: {documents.Count}";
        }
    }
}