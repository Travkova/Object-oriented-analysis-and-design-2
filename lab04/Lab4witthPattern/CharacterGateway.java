import java.io.*;
import java.nio.charset.StandardCharsets;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

/**
 * Шлюз (Gateway):
 * "Инкапсулирует доступ к другой системе или источнику данных"
 * Клиент НЕ знает о JDBC, SQL-скриптах, структуре таблиц или драйверах.
 */
public class CharacterGateway {
    private static final String DB_URL = "jdbc:sqlite:transformers.db"; 

    public CharacterGateway() {
        initDatabase(); //автоматич инициализация при создании шлюза
    }

    // Инициализация БД из внешнего SQL-файла 
    private void initDatabase() {
        try (Connection conn = DriverManager.getConnection(DB_URL)) {
            // Проверяем, создана ли таблица
            ResultSet meta = conn.getMetaData().getTables(null, null, "characters", null);
            if (meta.next()) return; // Таблица уже есть, скрипт не запускаем

            // Ищем SQL-файл
            InputStream is = getClass().getResourceAsStream("/transformers_data.sql");
            if (is == null) {
                File f = new File("transformers_data.sql");
                if (!f.exists()) throw new FileNotFoundException("Файл transformers_data.sql не найден в папке запуска!");
                is = new FileInputStream(f);
            }

            // Читаем скрипт
            String script = new String(is.readAllBytes(), StandardCharsets.UTF_8);
            String[] statements = script.split(";");

            try (Statement stmt = conn.createStatement()) {
                for (String sql : statements) {
                    String clean = sql.replaceAll("--.*", "").trim(); // Убираем комментарии
                    if (!clean.isEmpty()) stmt.execute(clean);
                }
            }
            System.out.println("БД создана и заполнена из transformers_data.sql");
        } catch (Exception e) {
            throw new RuntimeException("Ошибка инициализации шлюза: " + e.getMessage(), e);
        }
    }

    /**  Единый метод поиска для клиента */
    public Character findByName(String name) {
        String sql = "SELECT * FROM characters WHERE LOWER(name) = LOWER(?)";
        try (Connection conn = DriverManager.getConnection(DB_URL);
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setString(1, name); //безопасная подстановка параметра
            ResultSet rs = ps.executeQuery();
            if (rs.next()) return mapRow(rs); //преобразование строки бд в объект Character
        } catch (SQLException e) {
            throw new RuntimeException("Шлюз: ошибка поиска", e);
        }
        return null; 
    }

    /** Единый метод получения списка для клиента */
    public List<Character> findAll() {
        List<Character> list = new ArrayList<>();
        String sql = "SELECT * FROM characters";
        try (Connection conn = DriverManager.getConnection(DB_URL);
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) list.add(mapRow(rs)); //маппинг каждой строки
        } catch (SQLException e) {
            throw new RuntimeException("Шлюз: ошибка выборки", e);
        }
        return list;
    }

    /** Внутренний маппинг ResultSet → Объект (скрыт от клиента) */
    private Character mapRow(ResultSet rs) throws SQLException {
        return new Character(
            rs.getString("name"), rs.getString("fraction"),
            rs.getString("position"), rs.getString("altform"),
            rs.getString("weapon")
        );
    }
}