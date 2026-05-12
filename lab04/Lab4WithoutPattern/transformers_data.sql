-- Создание таблицы
CREATE TABLE IF NOT EXISTS characters (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    fraction TEXT NOT NULL,
    position TEXT,
    altform TEXT,
    weapon TEXT
);

-- Вставка данных
INSERT INTO characters (name, fraction, position, altform, weapon) VALUES
('Optimus Prime', 'Autobot', 'Leader', 'Freightliner Coronado Truck', 'ion cannons, Energy blade, Star Sword'),
('Ratchet', 'Autobot', 'Medic', 'Ford E-350 ambulance', 'Built-in blades, built-in soldering iron'),
('Bumblebee', 'Autobot', 'Scout', 'yellow and black pony car brand "Urbana 500"', 'Dual Laser Blasters'),
('Arcee', 'Autobot', 'Scout', 'Kawasaki Ninja 250R Racing Sport Bike', 'Two blasters and sharp blades'),
('Bulkhead', 'Autobot', 'Warrior, builder', 'Hummer-HX SUV', 'Two ion blasters and two maces'),
('Megatron', 'Decepticon', 'Leader', 'The Cybertronian spaceship', 'Thermonuclear Cannon, Built-in Blade, Dark Star Sword'),
('Starscream', 'Decepticon', 'Air Commander', 'The F-16 Fighting Falcon fighter', 'Rocket launchers, a stunner spear, blasters'),
('Soundwave', 'Decepticon', 'Scout, signalman', 'MQ-9 "Reaper" unmanned aircraft', 'System probes, resonant blaster, laser guns'),
('Shockwave', 'Decepticon', 'Scientist', 'Cybertronian tank', 'Plasma cannon'),
('Knock Out', 'Decepticon', 'Medic', 'Aston Martin DBS V12 sports car', 'Circular Saw, Drill, Energy Spear');