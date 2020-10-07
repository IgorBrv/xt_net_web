// Функции относящиеся к окну редактора профиля пользователя

const editor = document.getElementById('editor');
const pe = document.querySelector('.password-editor-window');


function PageSettingsButtonClick() {
    // Нажатие кнопки "настройка страницы"
    editor.classList.remove('unified-form-container-hidden');
}

function EditorCloseButtonClick() {
    // Нажатие кнопки закрытия окна редактора
    editor.classList.add('unified-form-container-hidden');
}

function EditorFormClick() {
    // Заглушка для формы редактора (для реализации закрытия по щелчку фона редактора)
    event.stopImmediatePropagation();
}

function PasswordEditorButtonClick() {
    // Нажатие кнопки редактора пароля

    if (pe && pe.classList.contains('password-editor-window-hidden')) {
        pe.classList.remove('password-editor-window-hidden');   
    }
}

function PasswordEditorCloseButtonClick() {
    // Нажатие кнопки редактора пароля

    if (pe && !pe.classList.contains('password-editor-window-hidden')) {
        pe.classList.add('password-editor-window-hidden');
    }
}

function RemoveEmblemButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при удалении аватара

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Вы действительно хотите удалить аватар?', 'RemoveEmblemButtonClick(temp)')
}

function EditorSaveClick(button) {
    // Нажатие на кнопку "сохранить" окна редактора

    event.stopImmediatePropagation();

    let nameField = editor.querySelector('.unified-form-content-header-title');
    let name = editor.querySelector('.unified-form-content-header-title').value;
    let statement = editor.querySelector('.unified-form-content-input-text').value;
    let birth = editor.querySelector('.unified-form-content-input').value;

    if (name.replace(/ /g, "") == "") {

        let curName = document.getElementById('username').textContent;
        nameField.value = curName;
        name = curName;
    }

    let data = new FormData();
    data.append('type', 'UserUpdateRequest');
    data.append('id', button.id);
    data.append('name', name);
    data.append('statement', statement);
    data.append('birth', birth);


    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Обновление профиля пользователя не удалось!\nПроверьте правильность вводимых данных");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            let nameLabel = document.getElementById('username');
            let statusLabel = document.querySelector('.user-card-content-status');
            let birthLabel = document.getElementById('userbirth');
            let ageLabel = document.getElementById('userage');

            if (nameLabel && statusLabel && birthLabel && ageLabel) {
                nameLabel.textContent = name;
                statusLabel.textContent = statement;
                birthLabel.textContent = data[1];
                ageLabel.textContent = `${data[2]} лет`;
            }

            EditorCloseButtonClick();
        }
    }).catch((error) => ErrorReport(error));
}


function EmblemButtonClick(input) {
    // Обработка загрузки файла эмблемы

    let data = new FormData();
    data.append('type', 'EmblemUploadRequest');
    data.append('id', input.name);
    data.append('attachfile', input.files[0]);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Сохранение эмблемы не удалось!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            mainEmblem = document.querySelector('.user-card-image');
            mainEmblemContainer = document.querySelector('.user-card-image-container');
            editorEmblem = editor.querySelector('.unified-form-avatar-image');
            editorEmblemContainer = editor.querySelector('.unified-form-avatar');

            mainEmblem.classList.add("hidden");
            editorEmblem.classList.add("hidden");
            mainEmblemContainer.style.backgroundImage = data[1];
            editorEmblemContainer.style.backgroundImage = data[1];

            if (!editorEmblemContainer.classList.contains('unified-form-avatar-opacity')) {

                editorEmblemContainer.classList.add("unified-form-avatar-opacity");
            }
        }
    }).catch((error) => ErrorReport(error));
}


function RemoveEmblemButtonClick(button) {
    // Нажатие на кнопку удаления аватара пользователя

    HideAlertWindow()

    let data = new FormData();
    data.append('type', 'EmblemRemoveRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Удаление эмблемы не удалось!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            mainEmblem = document.querySelector('.user-card-image');
            mainEmblemContainer = document.querySelector('.user-card-image-container');
            editorEmblem = editor.querySelector('.unified-form-avatar-image');
            editorEmblemContainer = editor.querySelector('.unified-form-avatar');

            mainEmblem.classList.remove("hidden");
            editorEmblem.classList.remove("hidden");
            mainEmblemContainer.style.backgroundImage = "";
            editorEmblemContainer.style.backgroundImage = "";

            if (editorEmblemContainer.classList.contains('unified-form-avatar-opacity')) {

                editorEmblemContainer.classList.remove("unified-form-avatar-opacity");
            }
        }
    }).catch((error) => ErrorReport(error));
}

function PasswordEditorSaveClick(button) {
    // Нажатие на кнопку "сохранить" окна редактора пароля

    event.stopImmediatePropagation();
    let oldpass = document.getElementById('oldpas');
    let newpass = document.getElementById('newpas');
    let prepeat = document.getElementById('newpasreply');

    if (oldpass.value.replace(/ /g, "") == "" || newpass.value.replace(/ /g, "") == "" || prepeat.value.replace(/ /g, "") == "" || newpass.value != prepeat.value) {
        oldpass.value = "";
        newpass.value = "";
        prepeat.value = "";

        ShowAlertWindow('Ошибка', "Изменение пароля не удалось!\nПроверьте правильность ввода!");
    }
    else {
        let data = new FormData();
        data.append('type', 'PasswordUpdateRequest');
        data.append('oldpas', oldpass.value);
        data.append('newpas', newpass.value);

        fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

            if (response.status !== 200) {
                return Promise.reject();
            }
            return response.json();

        }).then(function (data) {
            if (data[0] == "false") {
                ShowAlertWindow('Ошибка', "Изменение пароля не удалось!\nПроверьте правильность ввода!");
            }
            else if (data[0] == "error") {
                window.location.replace('/Pages/errorPage.cshtml');
            }
            else {
                window.location.replace('/Models/logoutModel.cshtml');
            }
        }).catch((error) => ErrorReport(error));
    }
}