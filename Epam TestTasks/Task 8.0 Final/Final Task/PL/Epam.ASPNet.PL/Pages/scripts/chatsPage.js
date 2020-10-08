// Функции относящиеся к окну чатов пользователя

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
            let container = document.querySelector('.body-container');
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