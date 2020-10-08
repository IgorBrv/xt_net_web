// Функции относящиеся к окну сообщений (чат с пользователем)


let messagesBody = document.querySelector(".messages-container");
messagesBody.scrollTop = messagesBody.scrollHeight;


function ChatInputKeyPressed(event) {
    // Отслеживание нажатия Enter в строке ввода в чате
    if (event.keyCode == 13) {
        let button = document.querySelector('.chat-input-send-button');
        ChatSendButtonClick(button);
    }
}


function MessageRemoveButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при удалении чата

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Cообщение удалится для всех!\nВы действительно хотите удалить сообщенние?', 'MessageRemoveButtonClick(temp)')
}


function ChatSendButtonClick(button) {
    // Нажатие кнопки "отправить" чата

    let inputField = document.querySelector('.chat-input-textarea');

    if (inputField.value != '' && inputField.value != ' ') {
        let data = new FormData();
        data.append('type', 'MessageSendRequest');
        data.append('idChat', inputField.id);
        data.append('text', inputField.value);

        fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

            if (response.status !== 200) {
                return Promise.reject();
            }
            return response.json();

        }).then(function (data) {
            if (data[0] == "false") {
                ShowAlertWindow('Ошибка', "Не удалось отправить сообщение!")
            }
            else if (data[0] == "error") {
                window.location.replace('/Pages/errorPage.cshtml');
            }
            else {
                inputField.value = null;
                DrawMessage(data[1], data[2], data[3], true, data[4]);
            }
        }).catch((error) => ErrorReport(error));
    }
}


function MessageRemoveButtonClick(button) {
    // Нажатие на кнопку удаления сообщения в чате

    HideAlertWindow();

    let messagesBody = document.querySelector(".messages-container");
    let message = document.getElementById(button.id);

    let data = new FormData();
    data.append('type', 'MessageRemoveRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Удаление сообщения не удалось!")
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            messagesBody.removeChild(message);

            if (messagesBody.getElementsByClassName("body-element").length == 0) {
                window.location.replace('/Pages/chatsPage.cshtml');
            }
        }
    }).catch((error) => ErrorReport(error));
}


function DrawMessage(id, name, text, removable = false, emblem = null, position = null) {
    // Функция добавляющая сообщение в чат

    // Форма пузыря сообщения:
    let bodyElement = document.createElement('div');
    bodyElement.className = `body-element`;
    if (position) {
        bodyElement.className = `body-element ${position}`;
    }
    bodyElement.id = id;

    let bodyElementMain = document.createElement('form');
    bodyElementMain.className = `body-element-main`;
    bodyElementMain.action = `/Pages/userPage.cshtml`;
    bodyElementMain.method = `post`;

    let bodyElementAvatar = document.createElement('button');
    bodyElementAvatar.className = `body-element-avatar`;

    let bodyElementAvatarImage = document.createElement('img');
    bodyElementAvatarImage.className = "body-element-avatar-image";

    if (emblem != null) {

        bodyElementAvatarImage.src = emblem;
    }
    else {
        bodyElementAvatarImage.src = '/Pages/images/user.svg';
    }

    let bodyElementContent = document.createElement('div');
    bodyElementContent.className = `body-element-content`;

    // Имя отправителя:
    let bodyElementHeader = document.createElement('p');
    bodyElementHeader.className = 'body-element-header';
    bodyElementHeader.appendChild(document.createTextNode(`${name}`));

    // Текст сообщения:
    let bodyElementText = document.createElement('p');
    bodyElementText.className = 'body-element-text';
    bodyElementText.appendChild(document.createTextNode(`${text}`));

    let bodyElementFooter = document.createElement('div');
    bodyElementFooter.className = `body-element-footer`;

    if (removable) {
        // Кнопка удаления сообщения:
        let bodyRemoveButton = document.createElement('div');
        bodyRemoveButton.className = 'body-element-button';
        bodyRemoveButton.id = id;
        bodyRemoveButton.setAttribute('onclick', 'MessageRemoveButtonClickAlert(this)');

        // Изображение в кнопке удаления сообщения:
        let bodyRemoveButtonImg = document.createElement('img');
        bodyRemoveButtonImg.src = "/Pages/images/close.svg";
        bodyRemoveButtonImg.alt = "message remove icon";
        bodyRemoveButtonImg.className = "body-element-remove-button-img";

        bodyRemoveButton.appendChild(bodyRemoveButtonImg);
        bodyElementFooter.appendChild(bodyRemoveButton);
    }
    else {
        let bodyElementFooterFiller = document.createElement('div');
        bodyElementFooterFiller.className = `body-remove-button-filler`;
        bodyElementFooter.appendChild(bodyElementFooterFiller);
    }

    bodyElement.appendChild(bodyElementMain);
    bodyElementAvatar.appendChild(bodyElementAvatarImage);
    bodyElementContent.appendChild(bodyElementHeader);
    bodyElementContent.appendChild(bodyElementText);
    bodyElementMain.appendChild(bodyElementAvatar);
    bodyElementMain.appendChild(bodyElementContent);
    bodyElement.appendChild(bodyElementFooter);

    let messagesBody = document.querySelector(".messages-container");
    messagesBody.appendChild(bodyElement);
    messagesBody.scrollTop = messagesBody.scrollHeight;
}