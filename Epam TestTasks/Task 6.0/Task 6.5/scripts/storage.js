class Storage {
    // вспомогательная библиотека для хранения данных в памяти приложения

    constructor() {

        this.map = new Map();
        this.freeIDs = new Array();

    }

    add(arr) {
        // Метод добавляющий элементы в библиотеку. Принимает на вход массивы, и проводит проверку, является ли аргумент массивом

        if (arr instanceof Array) {

            let id;

            if (this.freeIDs.length > 0) {

                id = this.freeIDs.shift()
                this.map.set(id, arr);
            }
            else {
                id = `${this.map.size + 1}`
                this.map.set(id, arr);
            }

            return id;    // Вернём присвоенный объекту ID для предоставления возможности поиска объекта в библиотеке по присвоенному ID 
        }
        else {
            console.log('-Аргумент пуст, или не является массивом (Библиотека принимает только МАССИВЫ!)');

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
        // Метод производящий поиск элементов с заданой фразой в библиотеке:

        let temp = new Map();
        data = data.toLowerCase()

        for (let key of this.map.keys()) {

            for (let item of this.map.get(key)) {

                item = item.toLowerCase();

                if (item.includes(data)) {

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

    updateById(id, arr) {
        // Метод производящий обновление полей записи с указанным ID. Проверяет наличие указанного ID в базе,
        // сверяет поля найденного массива с предложеным, и в случае различия подменяет их:

        if (this.map.has(`${id}`)) {

            if (arr instanceof Array) {

                for (let key in arr) {

                    if (this.map.get(`${id}`)[key] != arr[key]) {

                        this.map.get(`${id}`)[key] = arr[key];
                    }
                }
            }
            else {
                console.log('-Библиотека принимает только МАССИВЫ!');
            }
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }

    replaceById(id, arr) {
        // Метод производящий замену записи с указанным ID. Проверяет наличие указанного ID в базе.

        if (this.map.has(`${id}`)) {

            if (arr instanceof Array) {

                this.map.set(`${id}`, arr);
            }
            else {
                console.log('-Библиотека принимает только МАССИВЫ!');
            }
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }
}
