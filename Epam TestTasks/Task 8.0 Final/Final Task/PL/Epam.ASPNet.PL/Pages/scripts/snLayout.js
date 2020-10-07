// Функции относящиеся к общей форме соцсети
let temp = null;

const alert = document.getElementById('alertWindow');

function ExitButtonClick() {
    // Нажатие кнопки выход левой панели сайта (разлогин)
    window.location.replace('/Models/logoutModel.cshtml');
}

function MessagesButtonClick() {
    // Нажатие кнопки сообщений левой панели сайта
    window.location.replace('/Pages/chatsPage.cshtml')
}

function HomeButtonClick() {
    // Нажатие кнопки "домой"  левой панели сайта
    window.location.replace('/Pages/userPage.cshtml')
}

function SearchInputKeyPressed(event) {
    // Отслеживание нажатия Enter в строке поиска людей
    if (event.keyCode == 13) {
        event.submit;
    }
}

function HideAlertWindow() {
    // Функция сокрытия окна уведомления

    if (alert && !alert.classList.contains('unified-form-container-hidden')) {
        alert.classList.add('unified-form-container-hidden');
    }
}

function AlertFormClick() {
    // Заглушка для формы месаджбокса (для реализации закрытия по щелчку фона месаджбокса)
    event.stopImmediatePropagation();
}

function ShowAlertWindow(title, text, func) {
    // Функция отображения окна уведомления

    let awTitle = alert.querySelector('.unified-form-content-header-title');
    let awText = alert.querySelector('.unified-form-content-p');
    let awButton = alert.querySelector('.unified-form-footer-button');

    awTitle.textContent = title;
    awText.textContent = text;

    if (func != null) {
        awButton.setAttribute('onclick', func);
        awButton.hidden = false;
    }
    else {
        awButton.hidden = true;
    }
    if (alert && alert.classList.contains('unified-form-container-hidden')) {
        alert.classList.remove('unified-form-container-hidden');   
    }
}
