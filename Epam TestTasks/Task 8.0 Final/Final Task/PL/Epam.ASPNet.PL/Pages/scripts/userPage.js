// Функции относящиеся к окну пользователя

let lastButton = null;

function UserClick(card) {
    // Нажатие по карточке друга на странице пользователя

    card.submit();
}


function PersonRemoveButtonClickAlert(button) {
    // Прослойка для обображения всплывающего окна с вопросом при удалении пользователя
    temp = button;
    event.stopImmediatePropagation();

    if (button.classList.contains('user-card-button') && button.textContent.includes('заявку')) {

        ShowAlertWindow('Подтверждение', 'Вы действительно хотите отменить заявку дружбы?', 'PersonRemoveButtonClick(temp)')
    }
    else {
        ShowAlertWindow('Подтверждение', 'Вы действительно хотите удалить пользователя из друзей?', 'PersonRemoveButtonClick(temp)')
    }
}


function PersonAddButtonClick(button) {
    // Нажатие на кнопку "добавить в друзья"

    event.stopImmediatePropagation();

    let personElement = document.getElementById(button.id);


    let data = new FormData();
    data.append('type', 'PersonAddButtonClick');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Добавление пользователя в друзья не удалось!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            if (button.classList.contains('user-card-button')) {

                button.textContent = "Отменить заявку";
                button.setAttribute('onclick', 'PersonRemoveButtonClickAlert(this)');


                let usercard = null;
                let friendsContainer = document.querySelector('.user-card-friend-box');

                for (let item of friendsContainer.getElementsByClassName('body-element')) {
                    if (item.id == button.name) {
                        usercard = item;
                        break;
                    }
                }

                if (usercard) {

                    button.textContent = "Отменить дружбу";
                    usercard.classList.remove('hidden-box');
                    usercard.name = "friend";

                    let label = document.querySelector('.user-card-friend-box-label');

                    if (label && label.classList.contains('hidden')) {
                        label.classList.remove('hidden');
                    }
                }
            }
            else {
                if (personElement.name == 'request') {

                    let container = document.querySelector('.user-card-content-container');
                    let friendsContainer = document.querySelector('.user-card-friend-box');
                    container.removeChild(personElement);

                    if (document.getElementsByName('request').length == 0) {
                        let label = document.getElementById('requestsLabel');
                        container.removeChild(label);
                    }

                    personElement.name = 'friend';
                    friendsContainer.appendChild(personElement);
                    let friendsContainerLabel = document.querySelector('.user-card-friend-box-label');

                    if (friendsContainerLabel.classList.contains('hidden')) {
                        friendsContainerLabel.classList.remove('hidden')
                    }
                }
                let footer = personElement.querySelector('.body-element-footer')
                footer.removeChild(button);
            }
        }
    }).catch((error) => ErrorReport(error));
}


function PersonRemoveButtonClick(button) {
    // Нажатие на кнопку удаления из друзей

    HideAlertWindow();
    let data = new FormData();
    data.append('type', 'PersonRemoveRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Удаление пользователя из друзей не удалось!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            let container = document.querySelector('.user-card-content-container');

            if (button.classList.contains('user-card-button') && container != null) {
                // Сценарий если нажата кнопка удаления под аватаром со страницы другого пользователя

                button.textContent = "Добавить в друзья";
                button.setAttribute('onclick', 'PersonAddButtonClick(this)');

                let curUserCard = null;
                let friendsContainer = document.querySelector('.user-card-friend-box');

                for (let item of friendsContainer.getElementsByClassName('body-element')) {
                    // Определяем, находимся ли текущий пользователь в друзьях у пользователя открытой страницы:

                    if (item.id == button.name) {
                        curUserCard = item;
                        break;
                    }
                }

                console.log(curUserCard);
                console.log(friendsContainer);

                if (curUserCard) {
                    // Если находится - удаляем пользователя, пересчитываем колличество друзей и в случае необходимости удаляем лейбл:
                    friendsContainer.removeChild(curUserCard);

                    if (document.getElementsByName('friend').length == 0 && !document.querySelector('.user-card-friend-box-label').classList.contains('hidden')) {
                        let friendsContainerLabel = document.querySelector('.user-card-friend-box-label');
                        friendsContainerLabel.classList.add('hidden');
                    }
                }
            }
            else if (button.classList.contains('user-card-button') && container == null) {
                // Сценарий, если кнопка удаления нажата с заблокированой страницы:

                container = document.querySelector('.user-card');
                container.removeChild(button);
            }
            else {
                // Сценарий, если кнопка удаления нажата с карточки пользоввателя:

                let personElement = null;
                let friendsContainer = document.querySelector('.user-card-friend-box');

                for (let item of container.getElementsByClassName('body-element')) {
                    // Находим карточку пользователя с которой была нажата кнопка удаления:
                    if (item.id == button.id) {
                        personElement = item;
                        break;
                    }
                }

                // Удаляем карточку пользователя и лейблы в случае необходимости:
                if (personElement.name == "friend") {
                    friendsContainer.removeChild(personElement);
                }
                else {
                    container.removeChild(personElement);
                }

                if (document.getElementsByName('request').length == 0 && document.getElementById('requestsLabel') != null) {
                    let label = document.getElementById('requestsLabel');
                    container.removeChild(label);
                }

                if (document.getElementsByName('inventation').length == 0 && document.getElementById('inventationLabel') != null) {
                    let label = document.getElementById('inventationLabel');
                    container.removeChild(label);
                }

                if (document.getElementsByName('friend').length == 0 && !document.querySelector('.user-card-friend-box-label').classList.contains('hidden')) {
                    let friendsContainerLabel = document.querySelector('.user-card-friend-box-label');
                    friendsContainerLabel.classList.add('hidden');
                }
            }
        }
    }).catch((error) => ErrorReport(error));
}


function PersonMessageButtonClick(button) {
    // Нажатие на кнопку отправки сообщения

    event.stopImmediatePropagation();

    let data = new FormData();
    data.append('type', 'PersonChatRequest');
    data.append('id', button.id);

    fetch('/Models/ajaxHub.cshtml', { method: "POST", body: data }).then(response => {

        if (response.status !== 200) {
            return Promise.reject();
        }
        return response.json();

    }).then(function (data) {
        if (data[0] == "false") {
            ShowAlertWindow('Ошибка', "Удаление сообщения не удалось!");
        }
        else if (data[0] == "error") {
            window.location.replace('/Pages/errorPage.cshtml');
        }
        else {
            console.log(data[1]);
            window.location.replace('/Pages/messagesPage.cshtml');
        }
    }).catch((error) => ErrorReport(error));
}



