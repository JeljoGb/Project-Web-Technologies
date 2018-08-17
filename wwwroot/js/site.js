$(document).ready(function () {

    // Wire up the Add button to send the new item to the server
    $('#add-boardGame-button').on('click', addBoardGame);
    $('#remove-boardGame-button').on('click', removeBoardGame);
});


function addBoardGame() {
    $('#add-boardGame-error').hide();
    var newTitle = $('#add-boardGame-title').val();
    $.post('/BoardGame/AddBoardGame', { title: newTitle }, function () {
        window.location = '/BoardGame';
    })
        .fail(function (data) {
            if (data && data.responseJSON) {
                var firstError = data.responseJSON[Object.keys(data.responseJSON)[0]];
                $('#add-boardGame-error').text(firstError);
                $('#add-boardGame-error').show();
            }
        });
}

function removeBoardGame() {

}