// Глобальные переменные:
let storage = new Storage();
let editor = document.querySelector('.editor-window-container');
let mainPageBody = document.querySelector('.body-container');
let saveButton = document.querySelector('.editor-window-savebutton');
let addButton = document.querySelector('.add-button');
let editorTitleField = document.querySelector('.editor-window-header-text');
let editorTextField = document.querySelector('.editor-window-textarea');
let curEditеdNote = null;
let hiddenNotes = [];
let removeInWork = [];
let restoreInWork = [];

// Добавление стандартных записей в библиотеку:
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

function SearchInput(input) {

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
        if (document.getElementById(id) && !removeInWork.includes(id))
        {
            removeInWork.push(id);
            hiddenNotes.push(id);

            let itemToRemove = document.getElementById(id)

            let opacity = 1;

            let opacityDeacreasing = setInterval(function() {
                if (opacity <= 0) {
                    clearInterval(opacityDeacreasing)
                    mainPageBody.removeChild(itemToRemove);
                    removeInWork.splice(removeInWork.indexOf(id), 1);
                    toRemove.splice(toRemove.indexOf(id), 1);
                    return;
                }

                itemToRemove.style.opacity = opacity;
                opacity -= 0.1;


            }, 35);
        }
    }

    for (let id of toRestore) {
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

            if (highterItem) {

                itemToRestore = AddNote(id, allNotes.get(id)[0], allNotes.get(id)[1], true, highterItem);
            }
            else {
                console.log('!');
                itemToRestore = AddNote(id, allNotes.get(id)[0], allNotes.get(id)[1], true);
            }

            let opacity = 0;

            let opacityIncreasing = setInterval(function() {

                if (opacity > 1) {
                    clearInterval(opacityIncreasing)

                    toRestore.splice(toRestore.indexOf(id), 1);
                    restoreInWork.splice(restoreInWork.indexOf(id), 1);
                    hiddenNotes.splice(hiddenNotes.indexOf(id), 1);

                    return;
                }

                itemToRestore.style.opacity = opacity;
                opacity += 0.1;

            }, 35);
        }
    }
}

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
    storage.deleteById(event.target.id);
    mainPageBody.removeChild(document.getElementById(event.target.id));
}

function EditorSaveClick() {
    if (curEditеdNote != null){

        storage.updateById(curEditеdNote, [editorTitleField.value, editorTextField.value])

        let curNoteTitle = document.getElementById(`${curEditеdNote}-title`);
        let curNoteText = document.getElementById(`${curEditеdNote}-text`);

        curNoteTitle.textContent = editorTitleField.value;
        curNoteText.textContent = editorTextField.value;
        curEditеdNote = null;

    } else {

        let id = storage.add([editorTitleField.value, editorTextField.value]);
        AddNote(id, editorTitleField.value, editorTextField.value);
    }
    EditorCloseButtonClick();
}

function EditorCloseButtonClick() {
    // Функция обработки клика по кнопке закрытия редактора

    editor.classList.remove('editor-window-container-to-top');
}

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
    bodyElementText.appendChild(document.createTextNode(text));

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
