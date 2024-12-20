#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <memory>


class Tarif {
protected:
    std::string name;
    double price;

public:
    Tarif(const std::string& name, double price) : name(name), price(price) {
        if (price < 0) {
            throw std::invalid_argument("Цена не может быть отрицательной");
        }
    }

    virtual ~Tarif() = default;

    virtual double getPrice() const {
        return price;
    }

    std::string getName() const {
        return name;
    }
};


class DiscountTarif : public Tarif {
private:
    double discount; 

public:
    DiscountTarif(const std::string& name, double price, double discount)
        : Tarif(name, price), discount(discount) {
        if (discount < 0 || discount > 100) {
            throw std::invalid_argument("Скидка должна быть в диапазоне от 0 до 100");
        }
    }

    double getPrice() const override {
        return price * (1 - discount / 100);
    }

    double getDiscount() const {
        return discount;
    }
};


class Company {
private:
    std::vector<std::shared_ptr<Tarif>> tarifs;

public:
    Company() = default;

    void addTarif(const std::shared_ptr<Tarif>& tarif) {
        if (!tarif) {
            throw std::invalid_argument("Тариф не может быть пустым");
        }
        tarifs.push_back(tarif);
    }

    std::shared_ptr<Tarif> findMinPriceTarif() {
        if (tarifs.empty()) {
            throw std::runtime_error("Список тарифов пуст");
        }

        auto minTarif = tarifs[0];
        for (const auto& tarif : tarifs) {
            if (tarif->getPrice() < minTarif->getPrice()) {
                minTarif = tarif;
            }
        }
        return minTarif;
    }
};


int main() {
    try {
        Company company;
        setlocale(LC_ALL, "Ru");
        
        company.addTarif(std::make_shared<Tarif>("Стандартный", 1000));
        company.addTarif(std::make_shared<DiscountTarif>("Премиум", 2000, 15));
        company.addTarif(std::make_shared<DiscountTarif>("Эконом", 800, 5));
        

        
        auto minTarif = company.findMinPriceTarif();

        std::cout << "Тариф с минимальной ценой: " << minTarif->getName()
            << " \nЦена: " << minTarif->getPrice() << " руб." << std::endl;
    }
    catch (const std::exception& ex) {
        std::cerr << "Ошибка: " << ex.what() << std::endl;
    }

    return 0;
}
