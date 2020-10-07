// Общие функции для всех страниц

const fatalErrorGate = document.getElementById('fatalErrorForm');
const fatalErrorDetails = document.getElementById('fatalErrorDetails');

function ErrorReport(text) {
    // Функция репорта ошибки

    fatalErrorDetails.value = text;
    fatalErrorGate.submit();
}