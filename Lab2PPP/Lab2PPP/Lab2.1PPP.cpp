#include <iostream>
#include <vector>
#include <string>


using namespace std;

class Employee {
public:
    Employee(const string& name, int age, const string& position)
        : name(name), age(age), position(position) {}

    void displayInfo() const {
        cout << "Name: " << name << ", Age: " << age << ", Position: " << position << endl;
    }

private:
    string name;
    int age;
    string position;
};

class HRDepartment {
public:
    // singl � ���� ���������
    static HRDepartment& getInstance() {
        static HRDepartment instance; // ��� ���
        return instance;
    }

    void addEmployee(const Employee& employee) {
        employees.push_back(employee);
    }

    void displayEmployees() const {
        for (const auto& employee : employees) {
            employee.displayInfo();
        }
    }

private:
    HRDepartment() {}
    HRDepartment(const HRDepartment&) = delete; // ������ ����������
    HRDepartment& operator=(const HRDepartment&) = delete; // ������ ���������������

    vector<Employee> employees;
};

int main() {
    Employee emp1("Tom Cruz", 30, "Manager");
    Employee emp2("Will Smith", 25, "Engineer");
    Employee emp3("Jack Black", 28, "Designer");

    HRDepartment& hr = HRDepartment::getInstance();

    hr.addEmployee(emp1);
    hr.addEmployee(emp2);
    hr.addEmployee(emp3);

    cout << "List of Employees:\n";
    hr.displayEmployees();

    return 0;
}
