class Storage {
    // Библиотека из заданий 4 & 5 обрезаная по функционалу под данный проект

    constructor() {

        this.map = new Map();

        let allNotes = document.getElementsByClassName('body-element');
    
        for (let node of allNotes) {
            let text = document.getElementById(`${node.id}-title`).textContent;
            let date = document.getElementById(`${node.id}-date`);

            if (date) {
                date = this.getCorrectDateString(date.textContent);
            }

            this.map.set(node.id, [text, date]);
        }
    }


    

    add(arr) {
        // Метод добавляющий элементы в библиотеку. Принимает на вход массивы, и проводит проверку, является ли аргумент массивом

        if (arr instanceof Array) {

            let id = 0;

            for (let key of this.map.keys()) {
                if (parseInt(key) > id) {
                    id = parseInt(key);
                }
            }

            id += 1;

            if (arr[1]) {
                arr[1] = this.getCorrectDateString(arr[1]);
            }

            this.map.set(`${id}`, arr);
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
        // Регистронезависимый метод производящий поиск элементов с заданой фразой в библиотеке:

        let temp = new Map();
        data = data.toLowerCase();

        if (data == '') {
            return this.map;
        }

        for (let key of this.map.keys()) {

            let item = this.map.get(key)[0].toLowerCase();

            if (item.includes(data)) {

                temp.set(key, this.map.get(key));
            }
        }
        return temp;
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

    deleteById(id) {
        // Метод удаляющий элемент с указанным ID. Произвводит проверку наличия указанного ID в базе:

        if (this.map.has(`${id}`)) {

            this.map.delete(`${id}`);
        }
        else {
            console.log('-Элемент с таким ID отсутствует в библиотеке или ID задан неверно!');
        }
    }

    getCorrectDateString(date) {

        date = date.split(' ');

        let monthWords = ['январ', 'феврал', 'март', 'апрел', 'ма', 'июн', 'июл', 'август', 'сентябр', 'октябр', 'ноябр', 'декабр'];

        for (let word of monthWords) {
            if (date[1].includes(word)) {
                date[1] = monthWords.indexOf(word) + 1;
                break;
            }
        }

        if (date[1] < 10) {
            date[1] = `0${date[1]}`;
        }

        if (date[0].length < 2) {
            date[0] = `0${date[0]}`;
        }

        return `${date[2]}-${date[1]}-${date[0]}`;
    }
}
