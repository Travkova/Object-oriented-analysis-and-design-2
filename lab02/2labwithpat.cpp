#include <iostream>
#include <string>
#include <vector>
#include <cstring>
#include <cstdlib>
using namespace std;

//интерфейс платёжного шлюза
class PaymentGateway {
public:
    virtual void processPayment(const string& paymentDetails, double amount) = 0;
    virtual void refund(const string& transactionId, double amount) = 0;
    virtual string getGatewayName() = 0;
    virtual ~PaymentGateway() {}
};

class SberGateway : public PaymentGateway {
public:
    void processPayment(const string& paymentDetails, double amount) override {
        cout << "   БАНК: СБЕР" << endl;
        cout << "   > Подключение к API Сбера..." << endl;
        cout << "   > Проверка данных: " << paymentDetails << endl;
        cout << "   > Списание " << amount << " руб. (Сбер)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(const string& transactionId, double amount) override {
        cout << "   > Возврат через Сбер: " << transactionId << endl;
        cout << "   > Сумма возврата: " << amount << " руб." << endl;
    }
    string getGatewayName() override { return "Сбер"; }
};

class TbankGateway : public PaymentGateway {
public:
    void processPayment(const string& paymentDetails, double amount) override {
        cout << "   БАНК: Т-БАНК" << endl;
        cout << "   > Подключение к API Т-Банка..." << endl;
        cout << "   > Проверка данных: " << paymentDetails << endl;
        cout << "   > Списание " << amount << " руб. (Т-Банк)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(const string& transactionId, double amount) override {
        cout << "   > Возврат через Т-Банк: " << transactionId << endl;
        cout << "   > Сумма возврата: " << amount << " руб." << endl;
    }
    string getGatewayName() override { return "Т-Банк"; }
};

class AlfaGateway : public PaymentGateway {
public:
    void processPayment(const string& paymentDetails, double amount) override {
        cout << "   БАНК: АЛЬФА-БАНК" << endl;
        cout << "   > Подключение к API Альфа-Банка..." << endl;
        cout << "   > Проверка данных: " << paymentDetails << endl;
        cout << "   > Списание " << amount << " руб. (Альфа)" << endl;
        cout << "   STATUS:SUCCESS" << endl;
    }
    void refund(const string& transactionId, double amount) override {
        cout << "   > Возврат через Альфа: " << transactionId << endl;
        cout << "   > Сумма возврата: " << amount << " руб." << endl;
    }
    string getGatewayName() override { return "Альфа-Банк"; }
};

// абстрактный класс способа оплаты
class PaymentMethod {
protected:
    PaymentGateway* gateway;
public:
    PaymentMethod(PaymentGateway* gw) : gateway(gw) {}
    virtual void pay(double amount) = 0;
    virtual void refund(const string& transactionId, double amount) = 0;
    virtual string getMethodName() = 0;
    string getGatewayInfo() {
        return " (через " + gateway->getGatewayName() + ")";
    }
    virtual ~PaymentMethod() {}
};
//конкретный способ - кредитная карта
class CreditCardPayment : public PaymentMethod {
private:
    string cardNumber, cardHolder, expiryDate, cvv;
public:
    CreditCardPayment(PaymentGateway* gw, string number, string holder,
                     string expiry, string cvvCode)
        : PaymentMethod(gw), cardNumber(number), cardHolder(holder),
          expiryDate(expiry), cvv(cvvCode) {}
    
    void pay(double amount) override {
        cout << "КРЕДИТНАЯ КАРТА" << endl;
        cout << "   Карта: ****-****-****-" << cardNumber.substr(cardNumber.length()-4) << endl;
        cout << "   Владелец: " << cardHolder << endl;
        string paymentDetails = "Card:" + cardNumber + ",Holder:" + cardHolder;
        gateway->processPayment(paymentDetails, amount);
    }
    void refund(const string& transactionId, double amount) override {
        cout << "\nВОЗВРАТ по кредитной карте" << endl;
        gateway->refund(transactionId, amount);
    }
    string getMethodName() override { return "Кредитная карта"; }
};
//конкретный способ - бесконтактная оплата (SberPay/Tpay/AlfaPay)
class ContactlessPayment : public PaymentMethod {
private:
    string appName, deviceId, token;
public:
    ContactlessPayment(PaymentGateway* gw, string app, string device, string authToken)
        : PaymentMethod(gw), appName(app), deviceId(device), token(authToken) {}
    
    void pay(double amount) override {
        cout << "БЕСКОНТАКТНАЯ ОПЛАТА (" << appName << ")" << endl;
        cout << "   Устройство: " << deviceId << endl;
        string paymentDetails = "App:" + appName + ",Device:" + deviceId;
        gateway->processPayment(paymentDetails, amount);
    }
    void refund(const string& transactionId, double amount) override {
        cout << "\nВОЗВРАТ по бесконтактной оплате" << endl;
        gateway->refund(transactionId, amount);
    }
    string getMethodName() override { return appName; }
};
//конкретный способ - СБП
class SBPPayment : public PaymentMethod {
private:
    string phoneNumber, bankId;
public:
    SBPPayment(PaymentGateway* gw, string phone, string bank)
        : PaymentMethod(gw), phoneNumber(phone), bankId(bank) {}
    
    void pay(double amount) override {
        cout << "СБП (СИСТЕМА БЫСТРЫХ ПЛАТЕЖЕЙ)" << endl;
        cout << "   Телефон: " << phoneNumber << endl;
        string paymentDetails = "SBP:Phone:" + phoneNumber + ",Bank:" + bankId;
        gateway->processPayment(paymentDetails, amount);
    }
    void refund(const string& transactionId, double amount) override {
        cout << "\nВОЗВРАТ по СБП" << endl;
        gateway->refund(transactionId, amount);
    }
    string getMethodName() override { return "СБП"; }
};

int main(int argc, char* argv[]) {
    setlocale(LC_ALL, "Russian");
    
    // Режим GUI: payment_backend.exe <method> <bank> <amount> <card/phone> <holder> <expiry> <cvv>
    if (argc >= 4) {
        string method = argv[1];      // card, contactless, sbp
        string bank = argv[2];        // sber, tbank, alfa
        double amount = stod(argv[3]);
        
        PaymentGateway* gateway = nullptr;
        if (bank == "sber") gateway = new SberGateway();
        else if (bank == "tbank") gateway = new TbankGateway();
        else if (bank == "alfa") gateway = new AlfaGateway();
        
        if (!gateway) {
            cout << "ERROR:Invalid bank" << endl;
            return 1;
        }
        
        cout << "***** ПЛАТЁЖ *****" << endl;
        cout << "Сумма: " << amount << " руб." << endl;
        cout << "----------------------------" << endl;
        
        PaymentMethod* payment = nullptr;
        
        if (method == "card") {
            string card = (argc > 4) ? argv[4] : "1234567890123456";
            string holder = (argc > 5) ? argv[5] : "Иванов И.";
            string expiry = (argc > 6) ? argv[6] : "12/25";
            string cvv = (argc > 7) ? argv[7] : "123";
            payment = new CreditCardPayment(gateway, card, holder, expiry, cvv);
        }
        else if (method == "contactless") {
            string phone = (argc > 4) ? argv[4] : "+79161234567";
            string device = (argc > 5) ? argv[5] : "IMEI-123456";
            string app = (argc > 6) ? argv[6] : "SberPay";
            payment = new ContactlessPayment(gateway, app, device, "token123");
        }
        else if (method == "sbp") {
            string phone = (argc > 4) ? argv[4] : "+79161234567";
            payment = new SBPPayment(gateway, phone, "044525225");
        }
        
        if (payment) {
            cout << "Метод: " << payment->getMethodName() << payment->getGatewayInfo() << endl;
            cout << "----------------------------" << endl;
            payment->pay(amount);
            cout << "----------------------------" << endl;
            cout << "RESULT:SUCCESS" << endl;
            delete payment;
        }
        
        delete gateway;//освобождение памяти
        return 0;
    }
    
    // Режим демонстрации (без аргументов)
    cout << "Payment Backend Ready" << endl;
    cout << "Usage: payment_backend.exe <method> <bank> <amount> [params...]" << endl;
    cout << "Methods: card, contactless, sbp" << endl;
    cout << "Banks: sber, tbank, alfa" << endl;
    
    return 0;
}