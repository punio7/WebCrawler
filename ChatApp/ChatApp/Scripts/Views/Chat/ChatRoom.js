$(function () {
    var hub = $.connection.ChatHub;
    hub.client.addMessage = addMessage;

    hub.client.addSystemMessage = addSystemMessage;

    $('#message').focus();

    $.connection.hub.start().done(function () {
        hub.server.clientJoinSession(sessionId);
        $('#sendmessage').click(function () {
            sendCommand(hub);
        });
        $("#message").keyup(function (event) {
            if (event.keyCode == 13) {
                sendCommand(hub);
            }
        });
        $(window).on('unload', function () {

        })
    });
});

function sendCommand(hub) {
    var message = $('#message').val();
    var chatlog = $('#ta_chatlog');
    chatlog.append(message + '\n');
    $('#message').val('').focus();

    var args = { SessionId: sessionId, Command: message };
    hub.server.clientSendCommand(args).fail(hubError);
}

function addMessage(message) {
    var chatlog = $('#ta_chatlog');
    chatlog.append(message);
    chatlog[0].scrollTop = chatlog[0].scrollHeight;
}

function addSystemMessage(message) {
    addMessage(message);
}

function hubError(e) {
    if (e.source === 'HubException') {
        addSystemMessage('Wystąpił błąd podczas wysyłania komendy do serwera: ' + e.message);
    }
}