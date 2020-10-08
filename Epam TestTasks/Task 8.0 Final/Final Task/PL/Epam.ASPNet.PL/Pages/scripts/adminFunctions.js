//Скрипты подключаемые пользователям с правами модератора


function BlockUserButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при удалении аватара

    temp = button;
    event.stopImmediatePropagation();
    ShowAlertWindow('Подтверждение', 'Вы действительно хотите заблокировать пользователя?', 'BlockUserButtonClick(temp)')
}

function BlockUserButtonClick(button) {
    // Нажатие на кнопку "Заблокировать пользователя"

    HideAlertWindow();

    let data = new FormData();
    data.append('type', 'UserBlockRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Блокировка пользователя не удалась!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            button.textContent = 'Разблокировать';
            button.setAttribute('onclick', 'UnblockUserButtonClick(this)');
        }
    }).catch((error) => ErrorReport(error));
}

function UnblockUserButtonClick(button) {
    // Нажатие на кнопку "Разблокировать пользователя"

    event.stopImmediatePropagation();

    let data = new FormData();
    data.append('type', 'UserUnblockRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Разблокировка пользователя не удалась!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            button.textContent = 'Заблокировать';
            button.setAttribute('onclick', 'BlockUserButtonClickAlert(this)');
        }
    }).catch((error) => ErrorReport(error));
}