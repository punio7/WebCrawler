$(function () {
    var hub = $.connection.ChatHub;

    hub.client.addMessage = addMessage;
    hub.client.addSystemMessage = addSystemMessage;

    $('#consoleInput').focus();

    $.connection.hub.start().done(function () {
        hub.server.clientJoinSession(sessionId);
        $('#executeButton').click(function () {
            sendCommand(hub);
        });
        $("#consoleInput").keyup(function (event) {
            if (event.keyCode === 13) {
                sendCommand(hub);
            }
        });
        $(window).on('unload', function () {

        });
    });
});

function sendCommand(hub) {
    var command = $('#consoleInput').val();
    $('#consoleInput').val('');
    addMessage(command);

    var args = { SessionId: sessionId, Command: command };
    hub.server.clientSendCommand(args).fail(hubError);
}

var lineFinished = true;

function addMessage(message, isNewLine = true) {
    if (message === "") message = " ";
    var consoleOutput = $('#consoleOutput');

    let element = null;
    if (lineFinished === true) {
        element = $('<pre>');
        element.html(message);
        consoleOutput.append(element);
    }
    else {
        element = $('#consoleOutput pre:last-child');
        element.html(element.html() + message);
    }
    lineFinished = isNewLine;
    element[0].scrollIntoView({
        behavior: 'smooth'
    });
}

function addSystemMessage(message) {
    addMessage(message);
}

function hubError(e) {
    if (e.source === 'HubException') {
        addSystemMessage('Wystąpił błąd podczas wysyłania komendy do serwera: ' + e.message);
    }
}