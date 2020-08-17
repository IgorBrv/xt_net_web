'use strict';

const Service = require('./class');

var storage = new Service();

// Добавим элементы в библиотеку:
storage.add({ "1": "Vasya" });
storage.add({ "2": "Petya" });
storage.add({ "3": "Fedya" });
storage.add({ "4": "Pavel" });

// Отобразим содержимое библиотеки:
console.log('\nОтобразим содержимое библиотеки:\n')
console.log(storage.getAll());

// Удалим 2 элемента с ID 2 и 3:
storage.deleteById(2);
storage.deleteById(3);

// Добавим новые, и посмотрим на присвоеные ID:
console.log('\nУдалим 2 элемента с ID 2 и 3, добавим новые, и посмотрим на присвоеные ID:\n')
storage.add({ "5": "Senya" });
storage.add({ "6": "Semen" });
console.log(storage.getAll());

// Обновим значение элемена с ID = 4:
console.log('\nОбновим значение элемена с ID = 4:\n')
storage.updateById(4, { "4": "Afony", "5": "Tanya" });
console.log(storage.getAll()); 

// Заменим значение элемена с ID = 4:
console.log('\nЗаменим значение элемена с ID = 4:\n');
storage.replaceById(4, { "5": "Kolya" });
console.log(storage.getAll()); 

// Получим значение элемента с ID = 4:
console.log('\nПолучим значение элемента с ID = 4:\n');
console.log(storage.getById("4"));

// Попробуем добавить в библиотеку НЕ объект:
console.log('\nПопробуем добавить в библиотеку НЕ объект:\n');
storage.add("Vasya");

// Попробуем получить значение элемента с ID не в базе:
console.log('\nПопробуем получить значение элемента с ID не в базе:\n');
console.log(`-результат: ${storage.getById("10")}`);

require('readline')
    .createInterface(process.stdin, process.stdout)
    .question("\n Нажмите [Enter] для выхода...", function () {
        process.exit();
    });
