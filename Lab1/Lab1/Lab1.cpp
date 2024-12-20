#include <iostream>
#include <string>

using namespace std;

class GruzCompany { //фирма грузоперевозок
private:
    double PayTon;
    double Mass;
    string companyName;

public:
    GruzCompany() : PayTon(0), Mass(0), companyName("") {}

    //сеттеры\методы

    void setPayTon(double value) { //1
        if (value >= 0)
            PayTon = value;
        else
            cerr << "Ошибка: Оплата за тонну не может быть отрицательной." << endl;
    }

    void setMass(double value) { //2
        if (value >= 0)
            Mass = value;
        else
            cerr << "Ошибка: Масса перевезённых грузов не может быть отрицательной." << endl;
    }

    void setCompanyName(const string& name) { //3
        companyName = name;
    }

    double calculateTotal() {  //4
        return PayTon * Mass;
    }

    void printCompanyInfo() { //5
        cout << "Название компании: " << companyName << endl;
        cout << "Общая выручка: " << calculateTotal() << " рублей." << endl;
    }
};

//

int main() {

    setlocale(LC_ALL, "Russian");

    GruzCompany company; //обьект*

    //значения полей через сеттеры

    company.setPayTon(100);
    company.setMass(500);
    company.setCompanyName("Петрович.");

    //вывод инфы о компании

    company.printCompanyInfo();

    return 0;
}