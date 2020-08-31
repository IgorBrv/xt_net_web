// Данный файл содержит функции для работы с отображением элементовв в теле сайта (размещение, перемещение, анимации)

// Глобальные переменные
let queue = [];
let filteredNotes;
let lastInput = '';
let hiddenNotes = [];
let removedNotes = [];
let hightLights = true;
let movingMap = new Map();
let hightLightedNotes = [];
let сontentManagerDown = true;
let processingList = new Map();



function SearchInput(input) {
    // Функция обрабатывающая ввод в строку поиска:

    let step = new Map();
    let allNotes = storage.getAll();
    lastInput = input.toLowerCase();
    filteredNotes = storage.getByData(input);    // Функцию поиска осуществляет библиотека

    for (let key of allNotes.keys()) {
        // Ищем элементы которые не были выделены библиотекой в результатах поиска:

        if (!filteredNotes.has(key)) {

            step.set(key, 'hd');    // Удаление
        }
        else {
            if (hiddenNotes.filter(function(a) {return a.id == key}).length > 0) {

                step.set(key, 'rs');    // Восстановление
            }
        }
    }

    if (step) {    // Если найдены элементы на уделиен или восстановление, в очередь добавляется шаг
        ContentManager(step);    // Запускаем обработчик:
    }
}


function ContentManager(step) {
    // Единый СИНХРОННЫЙ обработчик местоположения заметок на экране. Работает шаг-за-шагом до опустоошения очереди

    queue.push(step);   // Добавляем шаг изменений в очередь

    if (сontentManagerDown) {   // Запускаем инстанцию контент менеджера, если нет запущеной инстанции

        сontentManagerDown = false; 

        let temp = document.getElementsByClassName('body-element');
        let notesOnPage = [];
    
        for (let note of temp) {    // Для дальнейшей обработки элементам прописываются фиксированые координаты и position = 'fixed'
    
            notesOnPage.push(note);    // Элементы перемещаются в отдельный список, т.к. temp не поддерживает splice

            if (desktop) {
                note.style.left = `${(note.getBoundingClientRect()['x'])}px`;
                note.style.top = `${note.getBoundingClientRect()['y']-10}px`;
            }
        }
        if (desktop) {

            for (let note of temp) {

                note.style.position = 'fixed';
            }
        }
    
        while (queue.length > 0) {    // Цикл обработки очереди

            let curStep = queue.shift();

            // Цикл располагающий на форме новосозданные элементы~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            for (let item of curStep.keys()) { 
                
                if (curStep.get(item) == 'cr') {


                    let tmp = document.getElementsByClassName('body-element');
                    let left = tmp[tmp.length-1].getBoundingClientRect()['x'];
                    let top = tmp[tmp.length-1].getBoundingClientRect()['y'];

                    item.style.top = `${top}px`;
                    item.style.left = `${left}px`;
                    notesOnPage.push(item);

                    if (storage.getByData(lastInput).has(item.id)) {    // Размещаем элемент в фформу если в данный момент не включен поиск под который элемент не прохоодит

                        if (desktop) {

                            ChangeOpacity(item, 1);
                        }
                        else {
                            item.style.opacity = '1';
                        }

                        mainPageBody.appendChild(item);

                    }
                    else {
                        hiddenNotes.push(item);
                    }
                }
            }

            // Цикл обработки элементов предназначеных для удаления~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            for (let id of curStep.keys()) {    

                if (curStep.get(id) == 'rm' && document.getElementById(id)) {

                    storage.deleteById(id);
                    let item = document.getElementById(id);
                    notesOnPage.splice(notesOnPage.indexOf(item), 1);

                    if (desktop) {

                        let commonHeight = 0;

                        for (let elem of notesOnPage) { // Для того, чтобы скролл-бар не начинал появляться и исчезать при удалении сбивая разметку и позиционировавние
    
                            commonHeight += elem.getBoundingClientRect()['height'] + 10;
                            
                            if (commonHeight >= window.innerHeight - 65) {
    
                                body.style.overflowY = 'scroll';
                                break;
                            }
                        }
    
                        removedNotes.push(item);
    
                        ChangeOpacity(item, 0);
                    }
                    else {
                        mainPageBody.removeChild(item);
                    }
                }
            }

            // Цикл обработки элементов предназначеных для сокрытия~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            for (let id of curStep.keys()) {    

                if (curStep.get(id) == 'hd' && document.getElementById(id)) {
                    
                    let item = document.getElementById(id);

                    if (desktop) {

                        let commonHeight = 0;

                        for (let elem of notesOnPage) { // Для того, чтобы скролл-бар не начинал появляться и исчезать при поиске сбивая разметку и позиционировавние
    
                            commonHeight += elem.getBoundingClientRect()['height'] + 10;
                            
                            if (commonHeight >= window.innerHeight - 65) {
    
                                body.style.overflowY = 'scroll';
                                break;
                            }
                        }

                        ChangeOpacity(item, 0);    // Передача события в функцию изменения прозрачности
                    }
                    else {
                        mainPageBody.removeChild(item);
                    }
    
                    if (!hiddenNotes.includes(item)) {    // Добавление сокрытого элемента в список сокрытых элементов

                        hiddenNotes.push(item);
                    }
    
                }
            }

            // Цикл обработки элементов предназначеных для восстановления из сокрытого вида~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            for (let id of curStep.keys()) {    

                if (curStep.get(id) == 'rs') {
    
                    let note;
    
                    for (let item of hiddenNotes) {    // Извлечение элемента с нужным id из списка сокрытых элементов

                        if (item.id == id) {

                            note = item;
                            hiddenNotes.splice(hiddenNotes.indexOf(item), 1);
                            break;
                        }
                    }
    
                    if (processingList.has(note)) {    // Передача в функцию изменения прозрачности если элемент ещё не был удалён с формы
          
                        processingList.set(note, 1);
                    }
                    else {

                        itemPlaced = false;
    
                        for (let item of notesOnPage) {

                            if (parseInt(item.id) >= parseInt(id)) {    // Размещение восстаналиваемого элемента на форме с учётом порядка (по времени добавления)
    
                                mainPageBody.insertBefore(note, item);
                                itemPlaced = true;
                                break;
                            }
                        }
                        if (!itemPlaced) {

                            mainPageBody.appendChild(note);
                        }

                        if (desktop) {

                            ChangeOpacity(note, 1);    // Передача в функцию изменения прозрачности восстановленного элемента
                        }
                        else {
                            note.style.opacity = '1';
                        }
                    }
                }
            }

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            if (desktop) {

                MoveItems();    // Запуск функции перераспределяющей элементы по форме

                if (hightLights) {   // Запуск функции подсвечивающей результаты поиска
    
                    HightlightSearch();
                }
            }
        }
        сontentManagerDown = true;
    }
} 


function MoveItems() {
    // Вспомогательная функция изучающая состояние формы и перемещающая элементы при создании и удалении

    let notesOnPage = document.getElementsByClassName('body-element');
    let top = 55;
    let upperOffset = -10;
    let bottomOffset = 55;

    for (let note of notesOnPage) {

        if (!hiddenNotes.includes(note) && !removedNotes.includes(note)) {

            if (top < window.innerHeight || note.getBoundingClientRect()['top']-10 < window.innerHeight || movingMap.has(note) ) {    // Для оптимизации работы, плавно перемещаются ТОЛЬКО элементы расположеные в ввидимой зоне

                if (note.getBoundingClientRect()['top'] > window.innerHeight) {

                    note.style.top = `${window.innerHeight + upperOffset}px`;
                    upperOffset += (note.getBoundingClientRect()['height'] + 10);
                }

                if (note.getBoundingClientRect()['top'] < 55-note.getBoundingClientRect()['height']) {

                    note.style.top = `${bottomOffset-note.getBoundingClientRect()['height']}px`;
                    bottomOffset -= (note.getBoundingClientRect()['height'] + 10);

                }


                MoveItem(note, top);    // Передача элемента в функцию плавноого перемещения для конкретного элемента
            }
            else {                              // Не плавное изменение координат для элементов не в зоне видимоости
                note.style.top = `${top}px`;
            } 
           
            top = top + note.getBoundingClientRect()['height'] + 10;
        }
    }
}


function ChangeOpacity(item, targetOpacity, speed = 35, func = null) {
    // Вспомогательная СИНХРОННАЯ функция изменения прозрачности элемента:

    if (processingList.has(item)) {   // Обрабатываемый элемент добавляется в MAP для синхронизации

        processingList.set(item, targetOpacity);
    }
    else {
        processingList.set(item, targetOpacity);

        let opacity = parseFloat(item.style.opacity);

        if (!opacity) {opacity = 0};

        let changingProcess = setInterval(function() {    // Цикл изменения прозрачности с интервалом между шагами. Цель прозрачности указана в карте processingList и изменяется асинхронно
    
            if ((processingList.get(item) > 0 && opacity > processingList.get(item)) || (processingList.get(item) == 0 && opacity < processingList.get(item))) {
              
                clearInterval(changingProcess)
    
                if (opacity.toFixed(0) == 0) {    // TODO

                    if (func) {
                        func();
                    }
                    else {
                        mainPageBody.removeChild(item);

                        if (removedNotes.includes(item) ) {
                            removedNotes.splice(removedNotes.indexOf(item), 1);
                        }
                    }
                }
    
                processingList.delete(item);
                
                if (movingMap.size == 0 && processingList.size == 0) {
                    // При обрабоотке прозрачности последнего элемента, при отсутстии перемещаемых элементов, позиционирование всех элементоов выставляется в static

                    for (let item of document.getElementsByClassName('body-element')) {

                        item.style.position = 'static';
                    }

                    if (hiddenNotes.length == 0 && body.style.overflowY == 'scroll') {
                        body.style.overflowY = '';
                    }
                }
    
                return;
            }
            
            if (item.style.opacity > processingList.get(item)) {

                opacity -= 0.1;
                item.style.opacity = opacity;
            }
            else {
                opacity += 0.1;
                item.style.opacity = opacity;
            }
    
        }, speed);
    }
}


function MoveItem(note, target) {
    // СИНХРОННАЯ функция перемещающая конкретную заметку по полю

    if (movingMap.has(note)) { 

        movingMap.set(note, target);

    }
    else {

        movingMap.set(note, target);

        let step = 5;   // step и initialStep следует изменять одновременно!
        let initialStep = 5;
        let topOffset = 0;
        let topPosition = note.getBoundingClientRect()['y']-10;

        if (Math.abs(note.getBoundingClientRect()['y']-10 - movingMap.get(note)) < step && step == initialStep) {    // Утоньшение шага при близости целевой позиции для более точного позиционирования

            step = 1;
        }
    
        let mooving = setInterval(function() {    // Цикл изменения положения элемента с интервалом между шагами. Цель положения указана в карте movingMap и изменяется асинхронно
    
            if ((note.getBoundingClientRect()['y']-10 > window.innerHeight && topOffset > step) || (note.getBoundingClientRect()['y']-10 < 0 && topOffset < 0-step)) {
                // Оптимизация. Элемент за границами экрана прекращает движение и распологается в целевой точке

                note.style.top = `${movingMap.get(note)}px`;
            }

            if ((note.getBoundingClientRect()['y']-10 < movingMap.get(note) + step && note.getBoundingClientRect()['y']-10 > movingMap.get(note) - step)) {
                
                clearInterval(mooving);
                movingMap.delete(note);
    
                if (movingMap.size == 0 && processingList.size == 0) {
                    // При обрабоотке положения последнего элемента, при отсутстии элементов изменяющих прозрачность, позиционирование всех элементоов выставляется в static

                    for (let item of document.getElementsByClassName('body-element')) {

                        item.style.position = 'static';
                    }
                    
                    if (hiddenNotes.length == 0 && body.style.overflowY == 'scroll') {
                        body.style.overflowY = '';
                    }
                }
    
                return;
            }
    
            note.style.top = `${topPosition+topOffset}px`

            if (Math.abs(note.getBoundingClientRect()['y']-10 - movingMap.get(note)) < step && step == initialStep) {    // Утоньшение шага при близости целевой позиции для более точного позиционирования

                step = 1;
            }

            if (Math.abs(note.getBoundingClientRect()['y']-10 - movingMap.get(note)) > step * 5 && step != initialStep) {    // Утолщение шага при перекладках перед достижением целевой позиции

                step = initialStep;
            }
    
            if (note.getBoundingClientRect()['y']-10 > movingMap.get(note)) {
                
                topOffset -= step;
            }
            else {
                topOffset += step;
            }
    
        }, 5);
    }
}


function HightlightSearch(shutingDown = false) {
    // Регистронезависимая функция подсвечивающая искомый текст в заметках-результатах поиска:

    let notesOnPage = document.getElementsByClassName('body-element');

    for (let note of notesOnPage) {

        if (!['', ' '].includes(lastInput) && !shutingDown) {

            if (!hiddenNotes.includes(note) && !removedNotes.includes(note) && filteredNotes.has(note.id)) {

                let raw = storage.getById(note.id);
                let title = raw[0].toLowerCase();
                let text = raw[1].toLowerCase();

                let temp = [];
                let count = 0;
                let curIndex = 0;

                // Поиск совпадений в заголовке заметки:

                for (i of title.matchAll(new RegExp(lastInput, 'gi')) ) {
    
                    temp.push(raw[0].substring(curIndex, i.index));
                    curIndex = i.index + lastInput.length;
                    temp.push(`<span class="hightlight">${raw[0].substring(i.index, curIndex)}</span>`);
                    count++;

                }

                if (count) {

                    if (!hightLightedNotes.includes(note)) {
                        hightLightedNotes.push(note);
                    }

                    temp.push(raw[0].substring(curIndex, title.length));
                    let curNote = note.querySelector('.body-element-header');
                    curNote.innerHTML = temp.join('');
                }
                else {
                    if (hightLightedNotes.includes(note)) {

                        let curNoteTitle = note.querySelector('.body-element-header');      
                        curNoteTitle.innerHTML = raw[0];

                    }
                }

                temp = [];
                count = 0;
                curIndex = 0;
    
                // Поиск совпадений в тексте заметки:
                
                for (i of text.matchAll(new RegExp(lastInput, 'gi')) ) {

                    temp.push(raw[1].substring(curIndex, i.index));
                    curIndex = i.index + lastInput.length;
                    temp.push(`<span class='hightlight'>${raw[1].substring(i.index, curIndex)}</span>`);
                    count++;
                } 
    
                if (count) {

                    if (!hightLightedNotes.includes(note)) {
                        hightLightedNotes.push(note);
                    }
    
                    temp.push(raw[1].substring(curIndex, text.length));
                    let curNote = note.querySelector('.body-element-text');
                    curNote.innerHTML = temp.join('');

                }
                else {

                    if (hightLightedNotes.includes(note)) {

                        let curNoteText = note.querySelector('.body-element-text');
                        curNoteText.innerHTML = raw[1];
                    }
                }
            }
            else if (!filteredNotes.has(note.id) && hightLightedNotes.includes(note)) {

                let raw = storage.getById(note.id);
                let curNoteTitle = note.querySelector('.body-element-header');
                let curNoteText = note.querySelector('.body-element-text');
                curNoteTitle.textContent = `${raw[0]}`;
                curNoteText.innerHTML = raw[1];
                hightLightedNotes.splice(hightLightedNotes.indexOf(note), 1);
            }
        }
        else {
            
            for (let note of hightLightedNotes) {    // Отключение подвестки при пустой строке поиска или при сигнале отключения подсветки
                
                let raw = storage.getById(note.id);

                if (raw) {
                    let curNoteTitle = note.querySelector('.body-element-header');
                    let curNoteText = note.querySelector('.body-element-text');
                    curNoteTitle.textContent = `${raw[0]}`;
                    curNoteText.innerHTML = raw[1];
                }
            }

            hightLightedNotes = [];
        }
    }
}