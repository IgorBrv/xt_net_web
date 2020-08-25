class Storage {
    // вспомогательная библиотека для хранения данных в памяти приложения

    constructor() {

        this.map = new Map();
        this.freeIDs = new Array();

    }

    add(object) {
        // Метод добавляющий элементы в библиотеку. Принимает на вход объекты, и проводит проверку, является ли аргумент объектом

        if (typeof (object) == 'object') {

            let id;

            if (this.freeIDs.length > 0) {

                id = this.freeIDs.shift()
                this.map.set(id, object);
            }
            else {
                id = `${this.map.size + 1}`
                this.map.set(id, object);
            }

            return id;    // Вернём присвоенный объекту ID для предоставления возможности поиска объекта в библиотеке по присвоенному ID 
        }
        else {
            console.log('-Аргумент пуст, или не является объектом (Библиотека принимает только ОБЪЕКТЫ!)');

            return null;
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
        // Метод возвращающий все элементы библиотеки.

        return this.map;
    }

    getByData(data) {
        
        let temp = new Map();

        for (let key of this.map.keys()) {

            for (let objKey of Object.keys(this.map.get(key))) {

                if (objKey.includes(data) || this.map.get(key)[objKey].includes(data)) {

                    temp.set(key, this.map.get(key));
                }
            }
        }
        return temp;
    }

    deleteById(id) {
        // Метод удаляющий элемент с указанным ID. Произвводит проверку наличия указанного ID в базе:

        if (this.map.has(`${id}`)) {

            this.map.delete(`${id}`);
            this.freeIDs.push(`${id}`);
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
