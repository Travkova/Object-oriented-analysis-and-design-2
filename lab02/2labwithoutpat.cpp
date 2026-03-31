#include <iostream>
#include <string>
#include <cstring>
#include <cstdlib>
using namespace std;

//кредитная карта через сбер
class CreditCard_Sber {
private:
    string cardNumber, cardHolder, expiryDate, cvv;
public:
    CreditCard_Sber(string number, string holder, string expiry, string cvvCode)
        : cardNumber(number), cardHolder(holder), expiryDate(expiry), cvv(cvvCode) {}
    void pay(double amount) {
        cout << "Кредитная карта через Сбер" << endl;
        cout << "   Карта: " << cardNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Сбера..." << endl;
        cout << "   > Проверка карты через Сбер..." << endl;
        cout << "   > Списание средств (Сбер)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }

    void refund(string transactionId, double amount) {
        cout << "   > Возврат через Сбер: " << transactionId << endl;
        cout << "   > Сумма: " << amount << " руб." << endl;
    }
};
//кредитная карта через тбанк
class CreditCard_Tbank {
private:
    string cardNumber, cardHolder, expiryDate, cvv;
public:
    CreditCard_Tbank(string number, string holder, string expiry, string cvvCode)
        : cardNumber(number), cardHolder(holder), expiryDate(expiry), cvv(cvvCode) {}
    void pay(double amount) {
        cout << "Кредитная карта через Т-Банк" << endl;
        cout << "   Карта: " << cardNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Т-Банка..." << endl;
        cout << "   > Проверка карты через Тбанк..." << endl;
        cout << "   > Списание средств (Тбанк)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат через Тбанк: " << transactionId << endl;
        cout << "   > Сумма: " << amount << " руб." << endl;
    }
};
//кредитная карта через альфабанк
class CreditCard_Alfa {
private:
    string cardNumber, cardHolder, expiryDate, cvv;
public:
    CreditCard_Alfa(string number, string holder, string expiry, string cvvCode)
        : cardNumber(number), cardHolder(holder), expiryDate(expiry), cvv(cvvCode) {}
    void pay(double amount) {
        cout << "Кредитная карта через Альфа-Банк" << endl;
        cout << "   Карта: " << cardNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Альфа-Банка..." << endl;
        cout << "   > Проверка карты через Альфа..." << endl;
        cout << "   > Списание средств (Альфа)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат через Альфа: " << transactionId << endl;
        cout << "   > Сумма: " << amount << " руб." << endl;
    }
};

// SberPay через Сбер
class SberPay_Sber {
private:
    string phoneNumber, deviceId;

public:
    SberPay_Sber(string phone, string device) : phoneNumber(phone), deviceId(device) {}

    void pay(double amount) {
        cout << "\n SberPay через Сбер" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Сбера..." << endl;
        cout << "   > Проверка устройства SberPay..." << endl;
        cout << "   > Списание через SberPay" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат SberPay через Сбер: " << transactionId << endl;
    }
};

// SberPay через Т-Банк (кросс-банковская)
class SberPay_Tbank {
private:
    string phoneNumber, deviceId;

public:
    SberPay_Tbank(string phone, string device) : phoneNumber(phone), deviceId(device) {}

    void pay(double amount) {
        cout << "\nSberPay через Т-Банк" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Т-Банка..." << endl;
        cout << "   > Перенаправление на шлюз SberPay..." << endl;
        cout << "   > Списание средств" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат SberPay через Тбанк: " << transactionId << endl;
    }
};

// Tpay через Т-Банк
class Tpay_Tbank {
private:
    string phoneNumber, deviceId;

public:
    Tpay_Tbank(string phone, string device) : phoneNumber(phone), deviceId(device) {}

    void pay(double amount) {
        cout << "\n Tpay через Т-Банк" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Т-Банка..." << endl;
        cout << "   > Проверка Tpay..." << endl;
        cout << "   > Списание через Tpay" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) { 
        cout << "   > Возврат T-Pay через Тбанк: " << transactionId << endl;
    }
};

// СБП через Сбер
class SBP_Sber {
private:
    string phoneNumber, bankId;

public:
    SBP_Sber(string phone, string bank) : phoneNumber(phone), bankId(bank) {}

    void pay(double amount) {
        cout << "\n СБП через Сбер" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Сбера..." << endl;
        cout << "   > Отправка запроса в СБП через Сбер..." << endl;
        cout << "   > Мгновенное зачисление" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат СБП через Сбер: " << transactionId << endl;
    }
};

// СБП через Альфа-Банк
class SBP_Alfa {
private:
    string phoneNumber, bankId;

public:
    SBP_Alfa(string phone, string bank) : phoneNumber(phone), bankId(bank) {}

    void pay(double amount) {
        cout << "\nСБП через Альфа-Банк" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        cout << "   Сумма: " << amount << " руб." << endl;
        cout << "   > Подключение к API Альфа-Банка..." << endl;
        cout << "   > Отправка запроса в СБП через Альфа..." << endl;
        cout << "   > Мгновенное зачисление" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(string transactionId, double amount) {
        cout << "   > Возврат СБП через Альфа: " << transactionId << endl;
    }
};

int main(int argc, char* argv[]) {
    setlocale(LC_ALL, "Russian");
    
    if (argc >= 4) {
        string bank = argv[2];
        double amount = stod(argv[3]); //преобразование строки в число
        string card = (argc > 4) ? argv[4] : "1234567890123456";
        string holder = (argc > 5) ? argv[5] : "Иванов И.";
        string expiry = (argc > 6) ? argv[6] : "12/25";
        string cvv = (argc > 7) ? argv[7] : "123";
        
        cout << "***** ПЛАТЁЖ (БЕЗ ПАТТЕРНА) *****" << endl;
        cout << "Сумма: " << amount << " руб." << endl;
        cout << "----------------------------" << endl;
        
        if (bank == "sber") {
            CreditCard_Sber payment(card, holder, expiry, cvv);
            payment.pay(amount);
        }
        else if (bank == "tbank") {
            CreditCard_Tbank payment(card, holder, expiry, cvv);
            payment.pay(amount);
        }
        else if (bank == "alfa") {
            CreditCard_Alfa payment(card, holder, expiry, cvv);
            payment.pay(amount);
        }
        
        cout << "RESULT:SUCCESS" << endl;
        return 0;
    }
    
    cout << "Payment Backend (Without Pattern) Ready" << endl;
    return 0;
}