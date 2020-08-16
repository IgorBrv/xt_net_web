'use strict';

class Service {
    // вспомогательная библиотека для хранения данных в памяти приложения

    constructor() {

        this.map = new Map();
        this.freeIDs = new Array();

    }

    add(object) {
        // Метод добавляющий элементы в библиотеку. Принимает на вход объекты, и проводит проверку, является ли аргумент объектом

        if (typeof (object) == 'object') {

            if (this.freeIDs.length > 0) {

                this.map.set(`${this.freeIDs[0]}`, object);
                this.freeIDs.splice(0, 1);
            }
            else {
                this.map.set(`${this.map.size + 1}`, object);
            }
        }
        else {
            console.log('-Аргумент пуст, или не является объектом (Библиотека принимает только ОБЪЕКТЫ!)');
        }
    }

    getById(id) {
        // Метод возввращающий элемент по ID. Производит проверку наличия запрашиваемого ID в библиотеке

        if (this.map.has(`${id}`)) {

            return this.map.get(`${id}`);
        }
        else {
            return null;
        }
    }

    getAll() {
        // Метод возвращающий все элементы библиотеки. Подшивает всё в удобовваримую строку для отображения в демонстрации:

        let temp = new Array();

        this.map.forEach(function (value1, value2) {

            let text = new Array();

            for (let key in value1) {

                text.push(`'${key}': '${value1[key]}'`)
            }

            temp.push(` ${value2}: { ${text.join(', ')} } `);
        });

        return temp;
    }

    deleteById(id) {
        // Метод удаляющий элемент с указанным ID. Произвводит проверку наличия указанного ID в базе:

        if (this.map.has(`${id}`)) {

            this.map.delete(id);
            this.freeIDs.push(id);
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }

    updateById(id, object) {
        // Метод производящий обновление полей объекта с указанным ID. Проверяет наличие указанного ID в базе,
        // и в случае если имеющийся объект имеет все поля входящего объекта обновляет эти поля. Иначе заменяет объект:

        if (this.map.has(`${id}`)) {

            if (typeof (object) == 'object') {

                for (let key in object) {

                    this.map.get(`${id}`)[key] = object[key];
                }
            }
            else {
                console.log('-Библиотека принимает только ОБЪЕКТЫ!');
            }
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }

    replaceById(id, object) {
        // Метод производящий замену объекта с указанным ID. Проверяет наличие указанного ID в базе.

        if (this.map.has(`${id}`)) {

            if (typeof (object) == 'object') {

                this.map.set(`${id}`, object);
            }
            else {
                console.log('-Библиотека принимает только ОБЪЕКТЫ!');
            }
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }
}

module.exports = Service;