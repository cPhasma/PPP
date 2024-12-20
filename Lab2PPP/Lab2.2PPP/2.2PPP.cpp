#include <iostream>
#include <string>
#include <vector>
#include <limits>

using namespace std;



enum class TariffType {
    ECONOMY,
    BUSINESS,
    PREMIUM
};

class Tariff {
    
public:
    Tariff(string name, double price, TariffType type)
        : name(name), price(price), type(type) {}

    string getName() const {
        return name;
    }

    double getPrice() const {
        return price;
    }

    void displayTariff() const {
        cout << "�����: " << name << ", ���� �� �������: " << price << endl;
    }

private:
    string name;
    double price;
    TariffType type;
};

class Client {
public:
    Client(string name) : name(name) {}

    string getName() const {
        return name;
    }

private:
    string name;
};

class Order {
public:
    Order(Client* client, Tariff* tariff, double volume)
        : client(client), tariff(tariff), volume(volume) {}

    double calculateTotal() const {
        return volume * tariff->getPrice();
    }

    void displayOrder() const {
        cout << "������: " << client->getName() << " ������� "
            << volume << " ������ �����. ����� ���������: "
            << calculateTotal() << endl;
    }

    Client* getClient() const {
        return client;
    }

private:
    Client* client;
    Tariff* tariff;
    double volume;
};

class TransportCompany {
public:
    void addTariff() {
        string name;
        double price;
        int typeInt;
        TariffType type;

        cout << "������� �������� ������: ";
        cin >> name;

        cout << "������� ���� �� �������: ";
        while (!(cin >> price) || price <= 0) {
            cout << "������! ������� ���������� ���� (����� ������ ����): ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

        cout << "�������� ��� ������ (0 - Economy, 1 - Business, 2 - Premium): ";
        while (!(cin >> typeInt) || typeInt < 0 || typeInt > 2) {
            cout << "������! ������� ���������� �������� (0, 1 ��� 2): ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }
        type = static_cast<TariffType>(typeInt);

        tariffs.push_back(new Tariff(name, price, type));
    }

    void registerClient() {
        string name;
        cout << "������� ��� �������: ";
        cin >> name;
        clients.push_back(new Client(name));
    }

    void makeOrder() {
        string clientName, tariffName;
        double volume;

        cout << "������� ��� �������: ";
        cin >> clientName;

        Client* client = findClientByName(clientName);
        if (!client) {
            cout << "������ �� ������!" << endl;
            return;
        }

        cout << "������� �������� ������: ";
        cin >> tariffName;

        Tariff* tariff = findTariffByName(tariffName);
        if (!tariff) {
            cout << "����� �� ������!" << endl;
            return;
        }

        cout << "������� ����� �����: ";
        while (!(cin >> volume) || volume <= 0) {
            cout << "������! ������� ���������� ����� (����� ������ ����): ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

        orders.push_back(new Order(client, tariff, volume));
    }

    void displayTotalForClient(const string& clientName) const {
        Client* client = findClientByName(clientName);
        if (!client) {
            cout << "������ �� ������!" << endl;
            return;
        }

        double total = 0;
        for (const auto& order : orders) {
            if (order->getClient() == client) {
                total += order->calculateTotal();
            }
        }

        cout << "����� ��������� ��� ������� " << clientName << ": " << total << endl;
    }

    void displayTotalForAllOrders() const {
        double total = 0;
        for (const auto& order : orders) {
            total += order->calculateTotal();
        }
        cout << "����� ��������� ���� �������: " << total << endl;
    }

    ~TransportCompany() {
        for (auto t : tariffs) delete t;
        for (auto c : clients) delete c;
        for (auto o : orders) delete o;
    }

private:
    vector<Tariff*> tariffs;
    vector<Client*> clients;
    vector<Order*> orders;

    Client* findClientByName(const string& name) const {
        for (const auto& client : clients) {
            if (client->getName() == name) {
                return client;
            }
        }
        return nullptr;
    }

    Tariff* findTariffByName(const string& name) const {
        for (const auto& tariff : tariffs) {
            if (tariff->getName() == name) {
                return tariff;
            }
        }
        return nullptr;
    }
};

void displayMenu() {
    cout << "����:" << endl;
    cout << "1. �������� �����" << endl;
    cout << "2. ���������������� �������" << endl;
    cout << "3. �������� �����" << endl;
    cout << "4. ������� ����� ����� ��� �������" << endl;
    cout << "5. ������� ����� ����� ���� �������" << endl;
    cout << "6. �����" << endl;
}

int main() {
    setlocale(LC_ALL, "RU");
    TransportCompany company;
    int choice;

    do {
        displayMenu();
        cout << "������� ��� �����: ";
        while (!(cin >> choice) || choice < 1 || choice > 6) {
            cout << "������! ������� ���������� ����� (�� 1 �� 6): ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }

        switch (choice) {
        case 1:
            company.addTariff();
            break;
        case 2:
            company.registerClient();
            break;
        case 3:
            company.makeOrder();
            break;
        case 4: {
            string clientName;
            cout << "������� ��� �������: ";
            cin >> clientName;
            company.displayTotalForClient(clientName);
            break;
        }
        case 5:
            company.displayTotalForAllOrders();
            break;
        case 6:
            cout << "����� �� ���������..." << endl;
            break;
        }
    } while (choice != 6);

    return 0;
}
