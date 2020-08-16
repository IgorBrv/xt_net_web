'use strict';


// Запросим ввод строки с клавиатуры и передадим его в Program:
let string = require('readline')
    .createInterface(process.stdin, process.stdout)
    .question("\n Введите выражение для обработки или нажмите ENTER для использования стандартного выражения: ", Program);


function Program(string) {

    // Проверим ввод, если он пуст или имеет внутри что-то кроме доопустимых символов, то подставим строку приведённую в задании:
    if (!CheckString(string)) {
        string = '3.5 +4 * 10 - 5.3 / 5 =';
    }

    let result;
    let action;
    let exit = false;
    let num = new Array();
    let endOfTheString = false

    // Обработаем выражение посимволно
    for (let i in string) {

        // Если достигнут конец строки - поставим пометку (с целью обеспепчить выполнение выражений без =)
        if (i == string.length - 1) {

            endOfTheString = true;

            if (IsANum(string[i])) {

                num.push(string[i])
            }
        }

        // Если был обработан знак =, устанавливается метка выхода
        if (exit) {
            break;
        }
        // Если символ строки цифра - добавляем её к числу
        else if (IsANum(string[i]) && !endOfTheString) {

            num.push(string[i])
        }
        // Если символ строки обозначает операцию - обрабатываем число
        else if (IsAnAction(string[i]) || endOfTheString) {

            if (num.length != 0) {

                if (result == undefined) {

                    result = parseFloat(num.join(''));
                    action = string[i];
                    num = new Array();
                }
                else {
                    switch (action) {
                        case "*":
                            result *= parseFloat(num.join(''));
                            break;
                        case "+":
                            result += parseFloat(num.join(''));
                            break;
                        case "-":
                            result -= parseFloat(num.join(''));
                            break;
                        case "/":
                            result /= parseFloat(num.join(''));
                            break;
                    }

                    if (string[i] == '=') {
                        exit = true;
                    }
                    else {
                        action = string[i];
                        num = new Array(0);
                    }

                }
            }
            else if (parseInt(i) + 1 < string.length && IsANum(string[parseInt(i) + 1])) {
                num.push(string[i]);
            }
            else {
                console.log("\n Обнаружен неверный синтаксис!")
                result = undefined;
                break;
            }
        }
        else if (string[i].charCodeAt(0) != 32) {
            console.log(`\n  Обнаружен недопустимый символ! '${string[i]}', код: ${string[i].charCodeAt(0)}`)
            result = undefined;
            break;
        }
    }

    // Выведем результат если он доступен:
    if (result != undefined) {
        console.log(`\n Выражение: ${string}`);
        console.log(`\n Результат вычислений: ${result.toFixed(2)}`);
    }

    require('readline')
        .createInterface(process.stdin, process.stdout)
        .question("\n Нажмите [Enter] для выхода...", function () {
            process.exit();
        });
}


function IsANum(char) {
    // Вспомогательная функция, определяющая является ли символ цифрой:

    if (char.match(/\d/) || char == '.') {

        return true;
    }

    return false;
}

function IsAnAction(char) {
    // Вспомогательная функция, определяющая является ли символ знаком допустимой операции:

    let chars = ['+', '-', '*', '/', '=']

    if (chars.includes(char)) {

        return true;
    }

    return false;
}

function CheckString(string) {
    // Вспомогательная функция подменяющая все знаки препинания в заданой строке на заданые символы и опционально приводящая строку к нижнему регистру:

    for (let char of string) {

        if (!IsANum(char) && !IsAnAction(char) && char.charCodeAt(0) != 32) {

            return false;
        }

        return true;
    }
}

