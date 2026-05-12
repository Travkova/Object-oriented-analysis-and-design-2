import javax.swing.*;
import java.awt.*;
import java.sql.*;
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;

public class DirectAccessApp extends JFrame {
    private JTextField inputField;
    private JTextArea outputArea;
    private static final String DB_URL = "jdbc:sqlite:transformers.db"; //url для подключения к БД

    public DirectAccessApp() {
        setTitle("Transformers (БЕЗ паттерна)");
        setSize(580, 460);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout(10, 10));

        // Панель управления
        JPanel topPanel = new JPanel(new FlowLayout());
        inputField = new JTextField(22);
        JButton btnSearch = new JButton("Найти по имени");
        JButton btnListAll = new JButton("Список всех");
        topPanel.add(new JLabel("Имя персонажа:"));
        topPanel.add(inputField);
        topPanel.add(btnSearch);
        topPanel.add(btnListAll);
        add(topPanel, BorderLayout.NORTH);

        // Область вывода
        outputArea = new JTextArea();
        outputArea.setEditable(false);
        outputArea.setFont(new Font("Consolas", Font.PLAIN, 14));
        outputArea.setMargin(new Insets(10, 10, 10, 10));
        add(new JScrollPane(outputArea), BorderLayout.CENTER);

        //  Инициализация БД из SQL-файла: читает файл sql и выполняет sql-команды
        initDatabaseFromSQL();

        // Обработчик кнопки ПОИСК
        btnSearch.addActionListener(e -> {
            String name = inputField.getText().trim(); //получаем текст из поля + убираем пробелы
            if (name.isEmpty()) return;
            outputArea.setText("Поиск...\n");
            try (Connection conn = DriverManager.getConnection(DB_URL)) {
                //прямой доступ к БД (нарушение изоляции)
                String sql = "SELECT * FROM characters WHERE LOWER(name) = LOWER(?)";
                PreparedStatement ps = conn.prepareStatement(sql);
                ps.setString(1, name);  // Подставляем имя в запро
                ResultSet rs = ps.executeQuery(); // Выполняем запрос
                 // Если нашли — форматируем, иначе — сообщение
                outputArea.setText(rs.next() ? formatRow(rs) : "Персонаж не найден в архиве.");
            } catch (SQLException ex) {
                outputArea.setText("Ошибка БД: " + ex.getMessage());
                ex.printStackTrace(); //вывод ошибки в консоль
            }
        });

        // Обработчик кнопки СПИСОК
        btnListAll.addActionListener(e -> {
            outputArea.setText(" Загрузка списка...\n");
            StringBuilder sb = new StringBuilder("Все трансформеры:\n" + "─".repeat(50) + "\n");
            try (Connection conn = DriverManager.getConnection(DB_URL);
                 Statement stmt = conn.createStatement();
                 ResultSet rs = stmt.executeQuery("SELECT * FROM characters")) {
                // Перебираем все строки результата    
                while (rs.next()) sb.append(formatRow(rs)).append("\n"); //добавляем всех персонажей
                outputArea.setText(sb.toString()); //вывод списка
            } catch (SQLException ex) {
                outputArea.setText("Ошибка БД: " + ex.getMessage());
            }
        });
    }

    // Безопасная инициализация БД из внешнего SQL-файла
    private void initDatabaseFromSQL() {
        outputArea.append("Инициализация базы данных...\n");
        try (Connection conn = DriverManager.getConnection(DB_URL)) {
            // 1. Проверяем, создана ли уже таблица
            ResultSet meta = conn.getMetaData().getTables(null, null, "characters", null);
            if (meta.next()) {
                outputArea.append("Таблица characters уже существует. Пропускаю создание.\n");
                return;
            }

            // 2. Ищем SQL-файл (сначала в classpath, потом в текущей папке)
            InputStream is = getClass().getResourceAsStream("/transformers_data.sql");
            if (is == null) {
                File f = new File("transformers_data.sql");
                if (!f.exists()) {
                    outputArea.append("Файл transformers_data.sql не найден!\n");
                    outputArea.append(" Положите его в корневую папку проекта рядом с .class файлами.\n");
                    return;
                }
                is = new FileInputStream(f); //открываем файл
            }

            // 3. Читаем и выполняем SQL-скрипт
            String sqlScript = new String(is.readAllBytes(), StandardCharsets.UTF_8);
            String[] statements = sqlScript.split(";"); //разбиваем по ;
            
            try (Statement stmt = conn.createStatement()) {
                for (String sql : statements) {
                    String clean = sql.replaceAll("--.*", "").trim(); // Убираем комментарии
                    if (!clean.isEmpty()) {
                        stmt.execute(clean); //выполняем команду
                    }
                }
            }
            outputArea.append("База данных успешно создана и заполнена из transformers_data.sql\n");
            
        } catch (Exception e) {
            outputArea.append("Ошибка инициализации БД: " + e.getMessage() + "\n");
            e.printStackTrace();
        }
    }

    // Метод для форматирования строк
    private String formatRow(ResultSet rs) throws SQLException {
        return String.format(" %s\n   Фракция: %s | Должность: %s\n   Альт-форма: %s | Оружие: %s\n",
                rs.getString("name"), rs.getString("fraction"), rs.getString("position"),
                rs.getString("altform"), rs.getString("weapon"));
    }

    public static void main(String[] args) {
        // Явная регистрация драйвера SQLite (на случай проблем с автозагрузкой)
        try { Class.forName("org.sqlite.JDBC"); } catch (ClassNotFoundException ignored) {}
        
        // Запуск GUI
        SwingUtilities.invokeLater(() -> new DirectAccessApp().setVisible(true));
    }
}