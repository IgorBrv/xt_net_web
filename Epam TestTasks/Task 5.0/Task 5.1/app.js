'use strict';

// Запросим ввод строки с клавиатуры и передадим его в Program:
let string = require('readline')
    .createInterface(process.stdin, process.stdout)
    .question("\n Введите строку для обработки или нажмите ENTER для использования стандартной строки: ", Program);


function Program(string) {

    // Проверим ввод, если он пустой или не имеет букв, то подставим строку приведённую в задании:
    if (PrepareString(string, '') == '') {
        string = 'У Попа,была собака!!!';
    }

    // Приведём строку к нижнему индексу и удалим все знаки пунктуации:
    let temp = PrepareString(string, ' ').toLowerCase();
    let lettersToRemove = new Array;

    // Разобьём временную строку в массив слов:
    temp = temp.split(' ')

    for (let word of temp) {

        for (let char of word) {    // Пройдёмся по символам в каждом из слов, , и если индекс первого вхождения НЕ равен индексу
            // последнего вхождения сделаем вывод, что буква повторяется. Добавим её в список на удаление

            if (!lettersToRemove.includes(char) && word.indexOf(char) != word.lastIndexOf(char)) {

                lettersToRemove.splice(0, 0, char);
            }
        }
    }

    console.log(`\n Фраза на входе: ${string}`);

    let result = new Array;

    // Заменим все буквы из списка на удаление пустыми символами при помощи replace и регулярного выражения:
    for (let letter of string) {

        if (!lettersToRemove.includes(letter.toLowerCase())) {
            result.push(letter)
        }
    }

    console.log(`\n Фраза на выходе: ${result.join('')}\n`);

    require('readline')
        .createInterface(process.stdin, process.stdout)
        .question(" Нажмите [Enter] для выхода...", function () {
            process.exit();
        });
}

function PrepareString(string, spacer) {
    // Вспомогательная функция подменяющая все знаки препинания в заданой строке на заданые символы и опционально приводящая строку к нижнему регистру:

    let symbolsToRemove = new Array;

    for (let i in string) {

        if (string.charCodeAt(i) != 32 && !IsALetter(string[i]) && !symbolsToRemove.includes(string[i])) {

            symbolsToRemove.splice(0, 0, string[i])
        }
    }

    let temp = []
    let lastChar;

    for (let i in string) {

        if (symbolsToRemove.includes(string[i])) {

            if (lastChar != spacer && i != string.length - 1) {

                temp.push(spacer)
                lastChar = spacer;
            }
        }
        else {

            temp.push(string[i])
            lastChar = string[i];
        }
    }

    return temp.join('');
}

function IsALetter(i) {
    // Вспомогательная функция определяющая является ли символ бувкой русского или английского алфавитов:

    i = i.charCodeAt(0);

    if (i >= 65 && i <= 90 || i >= 97 && i <= 122 || i >= 1040 && i <= 1103 || i == 1025 || i == 1105) {

        return true;
    }

    return false;
}

