// Функции относящиеся к окну чатов пользователя

const container = document.querySelector('.body-element-chats-container');
const leavedContainer = document.querySelector('.body-element-leaved-container');


function ChatClick(chat) {
    // Нажатие на чат из страницы чатов
    chat.submit();
}


function RemoveChatButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при удалении чата

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Вы действительно хотите удалить чат?', 'RemoveChatButtonClick(temp)')
}


function RemoveChatButtonClick(button) {
    // Нажатие кнопки "Удалить чат"

    HideAlertWindow();

    let data = new FormData();
    data.append('type', 'ChatRemoveRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Удаление чата не удалось!")
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            let chat = document.getElementById(button.id);
            container.removeChild(chat);

            if (data[2] == '0') {
                let msgBox = document.getElementById('msgbox');
                let msgLed = document.querySelector('.messageLed');

                if (msgLed && msgBox) {
                    msgBox.removeChild(msgLed);
                }
            }
        }
    }).catch((error) => ErrorReport(error));
}


function LeaveChatButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при покидании чата

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Вы действительно хотите покинуть чат?', 'LeaveChatButtonClick(temp)')
}


function LeaveChatButtonClick(button) {
    // Нажатие кнопки "Покинуть чат"

    HideAlertWindow();

    let data = new FormData();
    data.append('type', 'ChatLeaveRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Покинуть чат не удалось!")
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            let chat = document.getElementById(button.id);
            let exitEmblem = button.querySelector('.exitbutton');
            let returnEmblem = button.querySelector('.returnbutton');
            let leavedChats = leavedContainer.getElementsByClassName('body-element');

            container.removeChild(chat);

            if (chat.style.border == "2px solid darkorange") {
                chat.style.border = "1px solid #828282"
            }

            if (leavedChats.length > 0) {
                leavedContainer.insertBefore(chat, leavedChats[0]);
            }
            else {
                leavedContainer.appendChild(chat);
            }

            button.setAttribute('onclick', 'ReturnToChatButtonClickAlert(this)')

            if (leavedContainer.classList.contains('hidden')) {
                leavedContainer.classList.remove('hidden');
            }

            if (returnEmblem.classList.contains('hidden')) {
                returnEmblem.classList.remove('hidden')
            }

            if (!exitEmblem.classList.contains('hidden')) {
                exitEmblem.classList.add('hidden')
            }

            if (data[2] == '0') {
                let msgBox = document.getElementById('msgbox');
                let msgLed = document.querySelector('.messageLed');

                if (msgLed && msgBox) {
                    msgBox.removeChild(msgLed);
                }
            }
        }
    }).catch((error) => ErrorReport(error));
}

function ReturnToChatButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при возвращении в чат

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Вы действительно хотите вернуться в чат?', 'ReturnToChatButtonClick(temp)')
}


function ReturnToChatButtonClick(button) {
    // Нажатие кнопки "Вернуться в чат"

    HideAlertWindow();

    let data = new FormData();
    data.append('type', 'ReturnToChatRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Вернуться в чат не удалось!")
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            let chat = document.getElementById(button.id);
            let exitEmblem = button.querySelector('.exitbutton');
            let returnEmblem = button.querySelector('.returnbutton');
            let chats = container.getElementsByClassName('body-element');

            leavedContainer.removeChild(chat);

            if (chats.length > 0) {
                container.insertBefore(chat, chats[0]);
            }
            else {
                container.appendChild(chat);
            }

            if (!leavedContainer.classList.contains('hidden') && leavedContainer.getElementsByClassName('body-element').length == 0) {
                leavedContainer.classList.add('hidden');
            }

            if (!returnEmblem.classList.contains('hidden')) {
                returnEmblem.classList.add('hidden')
            }

            if (exitEmblem.classList.contains('hidden')) {
                exitEmblem.classList.remove('hidden')
            }

            button.setAttribute('onclick', 'LeaveChatButtonClickAlert(this)')

            if (data[2] == '0') {
                let msgBox = document.getElementById('msgbox');
                let msgLed = document.querySelector('.messageLed');

                if (msgLed && msgBox) {
                    msgBox.removeChild(msgLed);
                }
            }
        }
    }).catch((error) => ErrorReport(error));
}