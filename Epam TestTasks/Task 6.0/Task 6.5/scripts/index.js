
// Глобальные переменные:
let storage = new Storage();
let editor = document.querySelector('.editor-window-container');
let mainPageBody = document.querySelector('.body-container');
let saveButton = document.querySelector('.editor-window-savebutton');
let addButton = document.querySelector('.add-button');
let editorTitleField = document.querySelector('.editor-window-header-text');
let editorTextField = document.querySelector('.editor-window-textarea');
let curEditеdNote = null;

// Вспомогательные переменные для функции поиска:
let hiddenNotes = [];
let removeInWork = [];
let restoreInWork = [];

// Добавление стандартных записей в библиотеку и на страницу:
storage.add(['Привет!', 'Я - \n твоя записная книжка! Ты можешь \nдобавить в меня заметку, или найти свои заметки при помощи поиска!\n :)'])
storage.add(['Кстати...', 'Ты можешь удалять ненужные заметки нажав на крестик в углу заметки или редактировать старые заметки при помощи двойного клика ;)'])
storage.add(['тест', 'gh'])
storage.add(['Привет!', 'Я - твоя записная книжка! Ты можешь добавить в меня заметку, или найти свои заметки при помощи поиска! :)'])
storage.add(['Кстати...', 'Ты можешь удалять ненужные заметки нажав на крестик в углу заметки или редактировать старые заметки при помощи двойного клика ;)'])
storage.add(['тест', 'gh'])


InjectAllNotesToPage();

// Функции:

function InjectAllNotesToPage() {
    // Функция перемещающая все записи из библиотеки на страницу

    let allNotes = storage.getAll();

    for (let key of allNotes.keys()) {

        AddNote(key, allNotes.get(key)[0], allNotes.get(key)[1]);
    }
}

// Функции обработки взаимодействий с элементами формы:

function NoteClick(id) {
    // Функция обработки клика по записке

    curEditеdNote = id
    let selectedNote = storage.getById(id);
    editorTextField.value = selectedNote[1];
    editorTitleField.value = selectedNote[0];
    saveButton.textContent = 'Сохранить';
    editor.classList.add('editor-window-container-to-top');
}


function AddButtonClick() {
    // Функция обработки клика по кнопке добавления заметки

    editorTextField.value = '';
    editorTitleField.value = '';
    editor.classList.add('editor-window-container-to-top');
    saveButton.textContent = 'Создать';
}


function RemoveClick(event) {
    // Функция обработки клика по кнопке удаления заметки

    event.stopImmediatePropagation();

    if (confirm('Вы действительно хотите удалить заметку?')) {

        storage.deleteById(event.target.id);
        SmoothRemove(document.getElementById(event.target.id));
    }
}


function EditorSaveClick() {
    // Функция обработки кнопки "сохранить/создать" окна редактора:

    if (curEditеdNote != null){

        storage.updateById(curEditеdNote, [editorTitleField.value, editorTextField.value])

        let curNoteTitle = document.getElementById(`${curEditеdNote}-title`);
        let curNoteText = document.getElementById(`${curEditеdNote}-text`);

        curNoteTitle.textContent = editorTitleField.value;
        curNoteText.innerHTML = editorTextField.value.replaceAll('\n', '<br>');

        curEditеdNote = null;

    } else {

        let id = storage.add([editorTitleField.value, editorTextField.value]);
        let createdItem = AddNote(id, editorTitleField.value, editorTextField.value, true);

        SmoothRestore(createdItem, false);
    }

    EditorCloseButtonClick();
}


function EditorCloseButtonClick() {
    // Функция обработки клика по кнопке закрытия редактора

    editor.classList.remove('editor-window-container-to-top');
}


function SearchInput(input) {
    // Функция обрабатывающая ввод в строку поиска:
    
    let allNotes = storage.getAll();
    let filteredNotes = storage.getByData(input);
    let toRemove = [];
    let toRestore = [];

    for (let key of allNotes.keys()){
        if (!filteredNotes.has(key)) {
            toRemove.push(key);
        }
        else {
            
            if (hiddenNotes.includes(key)) {

                toRestore.push(key);
            }
        }
    }

    for (let id of toRemove) {
        // Цикл отвечающий за сокрытие элементовв не отвечающих критериям поиска:

        if (document.getElementById(id) && !removeInWork.includes(id))
        {
            removeInWork.push(id);
            hiddenNotes.push(id);
            let itemToRemove = document.getElementById(id)

            // плавное удаление элемента:
            SmoothRemove(itemToRemove);
        }
    }

    for (let id of toRestore) {
        // Цикл отвечающий восстаноление сокрытых элементов:

        if ((!document.getElementById(id)  || removeInWork.includes(id)) && !restoreInWork.includes(id))
        {
            restoreInWork.push(id);

            let highterItem;
            let itemToRestore;

            for (let item of document.getElementsByClassName('body-element')) {
                if (item.id >= id) {

                    highterItem = item;
                    break;
                }
            }

            // Восстановление производится с сортировкой по индексу:
            if (highterItem) {
                
                itemToRestore = AddNote(id, allNotes.get(id)[0], allNotes.get(id)[1], true, highterItem);
            }
            else {
                itemToRestore = AddNote(id, allNotes.get(id)[0], allNotes.get(id)[1], true);
            }

            // Плавное появление восстановленого элемента:
            SmoothRestore(itemToRestore)
        }
    }
}


// Остальная логика:


function AddNote(id, title, text, opacity = false, highterIndex = null) {
    // Функция добавляющая запись на страницу и в библиотеку

    // Форма заметки:
    let bodyElement = document.createElement('div');
    bodyElement.className = 'body-element';
    bodyElement.id = id;
    bodyElement.setAttribute('onclick', 'NoteClick(this.id)');
    if (opacity) {
        bodyElement.style.opacity = 0;
    }

    // Заголовок заметки:
    let bodyElementHeader = document.createElement('p');
    bodyElementHeader.className = 'body-element-header';
    bodyElementHeader.id = `${id}-title`;
    bodyElementHeader.appendChild(document.createTextNode(title));

    // Текст заметки:
    let bodyElementText = document.createElement('p');
    bodyElementText.className = 'body-element-text';
    bodyElementText.id = `${id}-text`;
    bodyElementText.innerHTML = text.replaceAll('\n', '<br>');


    // Кнопка удаления заметки:
    let bodyRemoveButton = document.createElement('div');
    bodyRemoveButton.className = 'body-remove-button';
    bodyRemoveButton.id = id;
    bodyRemoveButton.setAttribute('onclick', 'RemoveClick(event)');


    // Изображение в кнопке удаления заметки:
    let bodyRemoveButtonImg = document.createElement('img');
    bodyRemoveButtonImg.src = "./images/close.svg";
    bodyRemoveButtonImg.alt = "window closure icon";
    bodyRemoveButtonImg.id = id;
    bodyRemoveButtonImg.className = "body-remove-button-img";

    // Сборка
    bodyRemoveButton.appendChild(bodyRemoveButtonImg);
    bodyElement.appendChild(bodyElementHeader);
    bodyElement.appendChild(bodyElementText);
    bodyElement.appendChild(bodyRemoveButton);

    if (highterIndex == null){

        mainPageBody.appendChild(bodyElement);
    } else {

        mainPageBody.insertBefore(bodyElement, highterIndex);
    }

    return bodyElement;
}


function SmoothRemove(itemToRemove) {
    // Вспомогательная функция производящая затухание удаляемой заметки:
    
    let opacity = 1;

    let opacityDecreasing = setInterval(function() {
        if (opacity <= 0) {

            clearInterval(opacityDecreasing)
            mainPageBody.removeChild(itemToRemove);
            removeInWork.splice(removeInWork.indexOf(itemToRemove.id), 1);

            return;
        }

        itemToRemove.style.opacity = opacity;
        opacity -= 0.1;

    }, 35);
}


function SmoothRestore (itemToRestore, notNew = true) {
    // Вспомогательная функция производящая плавное восстановление/создание заметки:

    let opacity = 0;

    let opacityIncreasing = setInterval(function() {

        if (opacity >= 1) {

            clearInterval(opacityIncreasing)

            if (notNew) {

                restoreInWork.splice(restoreInWork.indexOf(itemToRestore.id), 1);
                hiddenNotes.splice(hiddenNotes.indexOf(itemToRestore.id), 1);
            }

            return;
        }

        itemToRestore.style.opacity = opacity;
        opacity += 0.1;

    }, 35);
}