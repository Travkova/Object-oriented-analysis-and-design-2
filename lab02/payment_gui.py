import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
import subprocess
import sys
import os
from datetime import datetime

class PaymentGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("ИринаМаркет - Платёжная система")
        self.root.geometry("900x750")
        self.root.configure(bg='#f0f0f0')
        
        # Пути к исполняемым файлам
        if sys.platform == "win32":
            self.backend_with_pattern = "payment_backend_withpat.exe"
            self.backend_without_pattern = "payment_backend_withoutpat.exe"
        else:
            self.backend_with_pattern = "./payment_backend_withpat"
            self.backend_without_pattern = "./payment_backend_withoutpat"
        
        # Переменные заказа
        self.order_id = "#789456"
        self.amount = 12490.0
        
        # Вызов настройки интерфейса
        self.setup_ui()
    
    def setup_ui(self):
        # Заголовок
        title_frame = tk.Frame(self.root, bg='#2c3e50', height=70)
        title_frame.pack(fill=tk.X)
        title_frame.pack_propagate(False)
        
        tk.Label(title_frame, text=" ИринаМаркет", 
                font=("Arial", 24, "bold"), fg='white', 
                bg='#2c3e50').pack(pady=15)
        
        # Информация о заказе
        info_frame = tk.Frame(self.root, bg='#f0f0f0')
        info_frame.pack(fill=tk.X, padx=30, pady=15)
        
        tk.Label(info_frame, text=f"Номер заказа: {self.order_id}",
                font=("Arial", 12), bg='#f0f0f0').pack(side=tk.LEFT)
        tk.Label(info_frame, text=f"Сумма: {self.amount:,.2f} ₽",
                font=("Arial", 12, "bold"), fg='#27ae60', 
                bg='#f0f0f0').pack(side=tk.RIGHT)
        
        # Основная форма
        form_frame = tk.LabelFrame(self.root, text="Параметры платежа", 
                                  font=("Arial", 11, "bold"), 
                                  bg='white', padx=20, pady=15)
        form_frame.pack(fill=tk.X, padx=30, pady=10)
        
        # Выбор версии (с паттерном или без)
        tk.Label(form_frame, text="Версия:", 
                bg='white', font=("Arial", 10)).grid(row=0, column=0, sticky=tk.W, pady=5)
        self.version_var = tk.StringVar(value="С паттерном Мост")
        version_combo = ttk.Combobox(form_frame, textvariable=self.version_var,
                                    values=["С паттерном Мост", "Без паттерна"],
                                    state="readonly", width=25)
        version_combo.current(0)
        version_combo.grid(row=0, column=1, pady=5, padx=10)
        
        # Способ оплаты
        tk.Label(form_frame, text="Способ:", 
                bg='white', font=("Arial", 10)).grid(row=1, column=0, sticky=tk.W, pady=5)
        self.method_var = tk.StringVar(value="Кредитная карта")
        method_combo = ttk.Combobox(form_frame, textvariable=self.method_var,
                                   values=["Кредитная карта", "Бесконтактная", "СБП"],
                                   state="readonly", width=25)
        method_combo.current(0)
        method_combo.grid(row=1, column=1, pady=5, padx=10)
        
        # Банк
        tk.Label(form_frame, text="Банк:", 
                bg='white', font=("Arial", 10)).grid(row=2, column=0, sticky=tk.W, pady=5)
        self.bank_var = tk.StringVar(value="СберБанк")
        bank_combo = ttk.Combobox(form_frame, textvariable=self.bank_var,
                                 values=["СберБанк", "Т-Банк", "Альфа-Банк"],
                                 state="readonly", width=25)
        bank_combo.current(0)
        bank_combo.grid(row=2, column=1, pady=5, padx=10)
        
        # Номер карты
        tk.Label(form_frame, text="Номер карты:", 
                bg='white', font=("Arial", 10)).grid(row=3, column=0, sticky=tk.W, pady=5)
        self.card_entry = tk.Entry(form_frame, font=("Arial", 10), width=28)
        self.card_entry.grid(row=3, column=1, pady=5, padx=10)
        self.card_entry.insert(0, "1234-5678-9012-3456")
        
        # Владелец
        tk.Label(form_frame, text="Владелец:", 
                bg='white', font=("Arial", 10)).grid(row=4, column=0, sticky=tk.W, pady=5)
        self.holder_entry = tk.Entry(form_frame, font=("Arial", 10), width=28)
        self.holder_entry.grid(row=4, column=1, pady=5, padx=10)
        self.holder_entry.insert(0, "Иванов И.")
        
        # Срок действия
        tk.Label(form_frame, text="Срок (MM/YY):", 
                bg='white', font=("Arial", 10)).grid(row=5, column=0, sticky=tk.W, pady=5)
        self.expiry_entry = tk.Entry(form_frame, font=("Arial", 10), width=28)
        self.expiry_entry.grid(row=5, column=1, pady=5, padx=10)
        self.expiry_entry.insert(0, "12/25")
        
        # CVV
        tk.Label(form_frame, text="CVV:", 
                bg='white', font=("Arial", 10)).grid(row=6, column=0, sticky=tk.W, pady=5)
        self.cvv_entry = tk.Entry(form_frame, font=("Arial", 10), width=28, show="*")
        self.cvv_entry.grid(row=6, column=1, pady=5, padx=10)
        self.cvv_entry.insert(0, "123")
        
        # Кнопка оплаты
        pay_btn = tk.Button(self.root, text=" ОПЛАТИТЬ", 
                           command=self.on_pay_click,
                           font=("Arial", 14, "bold"),
                           bg='#27ae60', fg='white',
                           activebackground='#229954',
                           activeforeground='white',
                           padx=50, pady=12,
                           relief=tk.RAISED, bd=3)
        pay_btn.pack(pady=15)
        
        # Лог операций
        log_frame = tk.LabelFrame(self.root, text="Лог операций (вывод C++)", 
                                 font=("Arial", 11, "bold"), 
                                 bg='white', padx=15, pady=10)
        log_frame.pack(fill=tk.BOTH, expand=True, padx=30, pady=10)
        
        self.log_text = scrolledtext.ScrolledText(log_frame, height=12, 
                                                  font=("Courier New", 10),
                                                  bg='#ecf0f1', fg='#2c3e50')
        self.log_text.pack(fill=tk.BOTH, expand=True)
        
        # Кнопка очистки лога
        clear_btn = tk.Button(log_frame, text="Очистить лог", 
                             command=self.clear_log,
                             font=("Arial", 9))
        clear_btn.pack(anchor=tk.E, pady=5)
        
        # Статус
        self.status_label = tk.Label(self.root, text="", 
                                    font=("Arial", 10), bg='#f0f0f0', fg='#e74c3c')
        self.status_label.pack(pady=5)
    
    def call_cpp_backend(self, method, bank, amount, card, holder, expiry, cvv):
        """Вызов C++ бэкенда через subprocess"""
        try:
            # Выбор версии
            if self.version_var.get() == "С паттерном Мост":
                backend = self.backend_with_pattern
                version_name = "С ПАТТЕРНОМ МОСТ"
            else:
                backend = self.backend_without_pattern
                version_name = "БЕЗ ПАТТЕРНА"
            
            # Маппинг для банка
            bank_map = {
                "СберБанк": "sber",
                "Т-Банк": "tbank",
                "Альфа-Банк": "alfa"
            }
            internal_bank = bank_map.get(bank, "sber")
            
            # Маппинг для метода
            method_map = {
                "Кредитная карта": "card",
                "Бесконтактная": "contactless",
                "СБП": "sbp"
            }
            internal_method = method_map.get(method, "card")
            
            # Проверяем существование файла
            if not os.path.exists(backend):
                return None, "", f"Ошибка: Файл {backend} не найден!\nСкомпилируйте C++ код сначала.", -1
            
            # Формируем команду
            cmd = [backend, internal_method, internal_bank, str(amount), card, holder, expiry, cvv]
            
            # Запускаем процесс
            result = subprocess.run(
                cmd, 
                capture_output=True, 
                text=True, 
                timeout=10,
                encoding='utf-8',
                errors='replace'
            )
            
            return version_name, result.stdout, result.stderr, result.returncode
            
        except FileNotFoundError:
            return None, "", f"Ошибка: Файл {backend} не найден!", -1
        except subprocess.TimeoutExpired:
            return None, "", "Ошибка: Превышено время ожидания ответа от бэкенда", -1
        except Exception as e:
            return None, "", f"Ошибка: {str(e)}", -1
    
    def on_pay_click(self):
        """Обработка кнопки оплаты"""
        # Сбор данных из формы
        method = self.method_var.get()
        bank = self.bank_var.get()
        card = self.card_entry.get()
        holder = self.holder_entry.get()
        expiry = self.expiry_entry.get()
        cvv = self.cvv_entry.get()
        
        # Валидация
        if len(card.replace(" ", "").replace("-", "")) < 16:
            messagebox.showwarning("Ошибка", "Введите корректный номер карты (16 цифр)")
            return
        if len(cvv) != 3:
            messagebox.showwarning("Ошибка", "CVV должен содержать 3 цифры")
            return
        
        # Блокируем кнопку на время обработки
        self.status_label.config(text=" Обработка платежа...", fg='#f39c12')
        self.root.update()
        
        # Логирование начала
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        self.log_text.insert(tk.END, f"\n{'='*60}\n")
        self.log_text.insert(tk.END, f"ПЛАТЁЖ {self.order_id} [{timestamp}]\n")
        self.log_text.insert(tk.END, f"Сумма: {self.amount:,.2f} ₽\n")
        self.log_text.insert(tk.END, f"{'='*60}\n")
        self.log_text.see(tk.END)
        self.root.update()
        
        # Вызов C++ бэкенда
        version_name, stdout, stderr, returncode = self.call_cpp_backend(
            method, bank, self.amount, card, holder, expiry, cvv
        )
        
        # Вывод результата
        if version_name:
            self.log_text.insert(tk.END, f"Версия: {version_name}\n")
            self.log_text.insert(tk.END, f"Метод: {method}, Банк: {bank}\n")
            self.log_text.insert(tk.END, f"{'-'*60}\n")
            self.log_text.insert(tk.END, stdout)
            
            if stderr:
                self.log_text.insert(tk.END, f"\nОШИБКИ:\n{stderr}\n")
            
            self.log_text.insert(tk.END, f"{'-'*60}\n")
            
            if returncode == 0 and "RESULT:SUCCESS" in stdout:
                self.log_text.insert(tk.END, " Платёж успешно проведён!\n")
                self.status_label.config(text=" Успешно!", fg='#27ae60')
                messagebox.showinfo("Успех", 
                                   f"Платёж на сумму {self.amount:,.2f} ₽ успешен!\n"
                                   f"Заказ {self.order_id} оплачен.")
            else:
                self.log_text.insert(tk.END, " Ошибка платежа!\n")
                self.status_label.config(text=" Ошибка!", fg='#e74c3c')
                messagebox.showerror("Ошибка", "Не удалось провести платёж")
        else:
            self.log_text.insert(tk.END, f" {stdout}\n")
            self.status_label.config(text=" Ошибка!", fg='#e74c3c')
            messagebox.showerror("Ошибка", stdout or "Неизвестная ошибка")
        
        self.log_text.see(tk.END)
    
    def clear_log(self):
        """Очистка лога"""
        self.log_text.delete(1.0, tk.END)
        self.status_label.config(text="")

# Запуск
if __name__ == "__main__":
    root = tk.Tk()
    app = PaymentGUI(root)
    root.mainloop()
