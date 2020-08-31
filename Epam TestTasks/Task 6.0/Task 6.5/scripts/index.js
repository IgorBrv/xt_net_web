// Глобальные переменные и константы:
const storage = new Storage();
const body = document.querySelector('.body');
const editor = document.querySelector('.editor-window-container');
const mainPageBody = document.querySelector('.body-container');
const saveButton = document.querySelector('.editor-window-savebutton');
const addButton = document.querySelector('.add-button');
const editorTitleField = document.querySelector('.editor-window-title');
const editorTextField = document.querySelector('.editor-window-textarea');
const effectsSwitcherBox = document.getElementById('switcher-body-1');
const effectsSwitcher = document.getElementById('switcher-1');
const hightlightSwitcherBox = document.getElementById('switcher-body-2');
const hightlightSwitcher = document.getElementById('switcher-2');
let hightLightsLastStatus;
let curEditеdNote = null;
let desktop;


// Отключение всех визуальных эффектов для мобильных устройст:
if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {

    let head = document.querySelector('.header-container');
    head.removeChild(head.querySelector('.switchers-container'));
    head.removeChild(head.querySelector('.filler'));
    head.style.justifyContent = 'center';
    head.style
    desktop = false;

  } else {
    desktop = true;
}

// Добавление стандартных записей в библиотеку и на страницу:
storage.add(['Привет!', 'Я - твоя записная книжка! Ты можешь добавить a в меня заметку, или найти свои заметки при помощи поиска! :)'])
storage.add(['Кстати...', 'Ты можешь удалять ненужные заметки нажав на крестик в углу заметки или редактировать старые заметки кликнув на них ;)q'])
storage.add(['К сведению', 'Мои\nполя\nподдерживвают\nпереносы\nстрок\n!!!q'])
storage.add(['И ещё', 'Я добавлю пару заметок-филлеров чтобы было проще протестировать. s ;)q'])
storage.add(['', '**ещё заметка-филлер** g :)q'])
storage.add(['', '**ещё заметка-филлер** s :)q'])
storage.add(['Ах да...', 'По многим предсохранённым заметкам разбросаны латинские буквы. Это сделано, опять же, для удобства тестирования. Попробуй ввести в строку поиска букув "g"!']);

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

function NoteClick(note) {
    // Функция обработки клика по заметке

    curEditеdNote = note
    let selectedNote = storage.getById(note.id);
    editorTextField.value = selectedNote[1];
    editorTitleField.value = selectedNote[0];
    saveButton.textContent = 'Сохранить';
    editor.classList.add('editor-window-container-to-top');
    ChangeOpacity(editor, 1, 20);
}


function AddButtonClick() {
    // Функция обработки клика по кнопке добавления заметки

    editorTextField.value = '';
    editorTitleField.value = '';
    editor.classList.add('editor-window-container-to-top');
    saveButton.textContent = 'Создать';
    ChangeOpacity(editor, 1, 20);
}


function RemoveClick(event) {
    // Функция обработки клика по кнопке удаления заметки

    event.stopImmediatePropagation();

    if (confirm('Вы действительно хотите удалить заметку?')) {

        let toRemove = new Map();
        toRemove.set(event.target.id, 'rm');
        ContentManager(toRemove);
    }
}


function EditorSaveClick() {
    // Функция обработки кнопки "сохранить/создать" окна редактора:

    if (curEditеdNote != null){

        storage.updateById(curEditеdNote.id, [editorTitleField.value, editorTextField.value])

        let curNoteTitle = curEditеdNote.querySelector('.body-element-header');
        let curNoteText = curEditеdNote.querySelector('.body-element-text');

        curNoteTitle.textContent = editorTitleField.value;
        curNoteText.textContent = editorTextField.value;

        curEditеdNote = null;

    } else {

        let id = storage.add([editorTitleField.value, editorTextField.value]);
        let createdItem = new Map();
        createdItem.set(AddNote(id, editorTitleField.value, editorTextField.value, true), 'cr');
        ContentManager(createdItem);
    }

    EditorCloseButtonClick();
}


function EditorCloseButtonClick() {
    // Функция обработки клика по кнопке закрытия редактора
    
    ChangeOpacity(editor, 0, 20, function() {editor.classList.remove('editor-window-container-to-top')} );
    curEditеdNote = null;
}

function EditorFormClick() {
    // Заглушка для предотвращения сквозного щелчка по форме до задника
    
    event.stopImmediatePropagation();
}

function SwitchEffectsClick() {
    // Функция обработки клика по переключателю эффектоов

    if (desktop) {  // В данный момент использует флаг "desktop" 

        SearchInput('');
        document.querySelector('.search-input').value = '';
        effectsSwitcher.classList.add('switcher-disabled');
        effectsSwitcherBox.classList.add('hightlights-switcher-disabled');

        if (hightLights) {

            SwitchHighlightsClick()
            hightlightSwitcherBox.classList.remove('hightlights-switcher-disabled');
            hightLightsLastStatus = true;

        }
        else{
            hightLightsLastStatus = false;
        }

        desktop = false;
        hightlightSwitcher.classList.add('switcher-blocked');
        hightlightSwitcherBox.classList.add('hightlights-switcher-blocked');
    }
    else {

        SearchInput('');
        document.querySelector('.search-input').value = '';
        effectsSwitcher.classList.remove('switcher-disabled');
        effectsSwitcherBox.classList.remove('hightlights-switcher-disabled');
        
        if (hightLightsLastStatus) {

            SwitchHighlightsClick()
        }

        desktop = true;
        hightlightSwitcher.classList.remove('switcher-blocked');
        hightlightSwitcherBox.classList.remove('hightlights-switcher-blocked');
    }
}


function SwitchHighlightsClick() {
    // Функция обработки кликов по переключателю подсветки поиска

    if (hightLights) {

        hightLights = false;
        HightlightSearch(true);
        hightlightSwitcher.classList.add('switcher-disabled');
        hightlightSwitcherBox.classList.add('hightlights-switcher-disabled');
    }
    else {
        hightLights = true;
        HightlightSearch();
        hightlightSwitcher.classList.remove('switcher-disabled');
        hightlightSwitcherBox.classList.remove('hightlights-switcher-disabled');
    }
}


// Остальная логика:


function AddNote(id, title, text, opacity = false) {
    // Функция добавляющая запись на страницу и в библиотеку

    // Форма заметки:
    let bodyElement = document.createElement('div');
    bodyElement.className = 'body-element';
    bodyElement.id = id;
    bodyElement.style.opacity = 1;
    bodyElement.setAttribute('onclick', 'NoteClick(this)');
    if (opacity) {
        bodyElement.style.opacity = 0;
        bodyElement.style.position = 'fixed';
    }

    // Заголовок заметки:
    let bodyElementHeader = document.createElement('p');
    bodyElementHeader.className = 'body-element-header';
    bodyElementHeader.id = `${id}-title`;
    bodyElementHeader.appendChild(document.createTextNode(`${title}`));

    // Текст заметки:
    let bodyElementText = document.createElement('p');
    bodyElementText.className = 'body-element-text';
    bodyElementText.id = `${id}-text`;
    bodyElementText.appendChild(document.createTextNode(`${text}`));


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

    if (opacity) {

        return bodyElement;
    }
    else {

        mainPageBody.appendChild(bodyElement);
    }
}