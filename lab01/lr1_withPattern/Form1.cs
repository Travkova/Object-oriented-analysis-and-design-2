using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr1_withPattern
{
    public partial class Form1 : Form
    {
        private PrinterManager manager;

        // Таймер для автоматического обновления интерфейса
        private Timer updateTimer;

        // Конструктор формы
        public Form1()
        {
            InitializeComponent();

            manager = PrinterManager.Instance;

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
            updateTimer.Interval = 500;
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
                // Информация о Менеджере 1
                lblManager1Id.Text = $"ID: {manager.InstanceId}";
                lblManager1Queue.Text = $"Очередь: {manager.GetDocuments().Count}";

                // Информация о Менеджере 2 (такая же)
                lblManager2Id.Text = $"ID: {manager.InstanceId}";
                lblManager2Queue.Text = $"Очередь: {manager.GetDocuments().Count}";

                // Обновление списков документов (оба показывают одно и то же)
                UpdateDocumentsList(lvManager1Docs, manager);
                UpdateDocumentsList(lvManager2Docs, manager);

                // Обновление списков принтеров (оба показывают одно и то же)
                UpdatePrintersList(lvManager1Printers, manager);
                UpdatePrintersList(lvManager2Printers, manager);
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
                ListViewItem item = new ListViewItem(doc.Doc_name);
                item.SubItems.Add(doc.CurrentStatus);
                item.SubItems.Add(doc.Printer_name);

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
                ListViewItem item = new ListViewItem(printer.Printer_name);
                item.SubItems.Add(printer.CurrentStatus);

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
        private void AddDocumentToManager(string sourceName)
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

            // Добавление в менеджер (единственный экземпляр)
            manager.AddDocument(doc);

            // Логирование
            AddLogMessage($" {sourceName}: добавлен '{doc.Doc_name}' (общий ID: {manager.InstanceId})");

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
            AddDocumentToManager("Менеджер 1");
        }

        /// Кнопка "+в менеджер 2"
        private void btnAddToManager2_Click(object sender, EventArgs e)
        {
            AddDocumentToManager("Менеджер 2");
        }

        /// Кнопка "Обработать" для Менеджера 1
        private async void btnProcessManager1_Click(object sender, EventArgs e)
        {
            await ProcessQueueWithButton(btnProcessManager1, "Менеджер 1");
        }

        /// Кнопка "Очистить" для Менеджера 1
        private void btnClearManager1_Click(object sender, EventArgs e)
        {
            manager.ClearQueue();
            AddLogMessage($" Очередь очищена (через Менеджер 1)");
        }

        /// Кнопка "Обработать" для Менеджера 2
        private async void btnProcessManager2_Click(object sender, EventArgs e)
        {
            await ProcessQueueWithButton(btnProcessManager2, "Менеджер 2");
        }

        /// Общий метод обработки очереди с блокировкой кнопки
        private async Task ProcessQueueWithButton(Button button, string sourceName)
        {
            button.Enabled = false;
            button.Text = " Обработка...";

            var progress = new Progress<string>(msg => AddLogMessage(msg));
            await manager.ProcessQueueAsync(progress); // Тот же самый менеджер

            button.Text = "Обработать";
            button.Enabled = true;

            AddLogMessage($" Обработка через {sourceName} завершена");
        }

        /// Кнопка "Очистить" для Менеджера 2
        private void btnClearManager2_Click(object sender, EventArgs e)
        {
            manager.ClearQueue(); // Очищает ту же самую очередь
            AddLogMessage($" Очередь очищена (через Менеджер 2)");
        }


        /// Кнопка "Очистить лог"
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
            AddLogMessage(" Лог очищен");
        }

        /// Освобождение ресурсов при закрытии формы
    }

    /// Класс Document
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
            Doc_status[0] = "Создан";
            Doc_status[1] = "В очереди";
            Doc_status[2] = "Печатается";
            Doc_status[3] = "Завершен";

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

    /// Класс Printer
    public class Printer
    {
        public string Printer_id;
        public string Printer_name;
        public string[] Printer_status;

        public Printer(string printerName)
        {
            Printer_id = GeneratePrinterId();
            Printer_name = printerName;
            Printer_status = new string[4];
            Printer_status[0] = "Свободен";
            Printer_status[1] = "Занят";
            Printer_status[2] = "Ошибка";
            Printer_status[3] = "Оффлайн";
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

    /// Класс PrinterManager (правильная реализация с применением Singleton)
    public class PrinterManager
    {
        // Единственное статическое поле для хранения экземпляра
        private static PrinterManager _instance;

        // Объект для потокобезопасной блокировки
        private static readonly object _lock = new object();

        // Списки документов и принтеров
        private List<Document> documents;
        private List<Printer> printers;

        // Идентификатор экземпляра (для демонстрации)
        public string InstanceId { get; private set; }

        /// Приватный конструктор - запрещает создание экземпляров извне
        private PrinterManager()
        {
            // Фиксированный ID для Singleton
            InstanceId = "SINGLETON";

            documents = new List<Document>();
            printers = new List<Printer>();
            InitializePrinters();

            // Логируем создание (будет только один раз)
            System.Diagnostics.Debug.WriteLine($"[PrinterManager] ЕДИНСТВЕННЫЙ экземпляр создан. ID: {InstanceId}");
        }

        /// Публичное статическое свойство для доступа к единственному экземпляру
        public static PrinterManager Instance
        {
            get
            {
                // Потокобезопасная реализация
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PrinterManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// Инициализация списка принтеров
        private void InitializePrinters()
        {
            printers.Add(new Printer("HP LaserJet"));
            printers.Add(new Printer("Canon"));
            printers.Add(new Printer("Samsung"));
        }

        /// Метод Driver - преобразование в язык принтера
        public string Driver(Document doc)
        {
            return $"%!PS-Adobe-3.0\n" +
                $"%%Title: {doc.Doc_name}\n" +
                $"%%Creator: PrinterManager (SINGLETON)\n" +
                $"%%CreationDate: {DateTime.Now}\n" +
                $"\n" +
                $"({doc.Content}) show\n" +
                $"showpage\n" +
                $"%%EOF";
        }

        /// Добавление документа в очередь
        public void AddDocument(Document doc)
        {
            documents.Add(doc);
            doc.SetStatus("В очереди");
        }

        /// Асинхронная обработка очереди печати
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
                    progress?.Report($"[{InstanceId}] Принтер '{doc.Printer_name}' недоступен для документа '{doc.Doc_name}'");
                }
            }
        }

        /// Очистка очереди
        public void ClearQueue()
        {
            documents.Clear();
        }

        /// Получение списка документов
        public List<Document> GetDocuments()
        {
            return documents;
        }

        /// Получение списка принтеров
        public List<Printer> GetPrinters()
        {
            return printers;
        }

        /// Получение информации о менеджере
        public string GetInfo()
        {
            return $"ID: {InstanceId} | Документов: {documents.Count}";
        }
    }
}
