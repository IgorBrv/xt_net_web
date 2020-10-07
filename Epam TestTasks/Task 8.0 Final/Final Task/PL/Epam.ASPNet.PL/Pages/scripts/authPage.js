// Функции относящиеся к форме аутентификации/регистрации

function BackToAuthButtonClick() {
    // Нажатие кнопки "назад" окна регистрации
    window.location.replace('/Pages/authPage.cshtml')
}

function RegistrationButtonClick() {
    // Нажатие кнопки "регистрация" окна аутентификации
    window.location.replace('/Pages/regPage.cshtml')
}