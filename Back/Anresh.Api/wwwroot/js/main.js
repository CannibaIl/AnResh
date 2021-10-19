function displayError(msg) {
    $('#error').text(msg).slideUp();
    setTimeout(function () {
        $('#error').slideUp();
    }, 1000);
}

function displaySuccess(msg) {
    $('#success').text(msg).slideDown();
    setTimeout(function () {
        $('#success').slideUp();
    }, 5000);
}