window.onload = function () {
    Init();

    $('#executeButton').click(() => {
        let message = $('#consoleInput').val();
        $('#consoleInput').val('');
        Engine.Output(message);
        Engine.Input = message;
        Execute(Engine.Input);
    });

    $("#consoleInput").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#executeButton").click();
        }
    });

    $("#consoleInput").focus();
}

class EngineClass {
    constructor() {
        this.Input;
        this.EndLine = '<br/>';
        this.NonBreakingSpace = "&nbsp;";
        //this.fileSystem = require('fs');
        //this.loadScript = require('load-script');
        this.lineFinished = true;
        this.currentTextClass = "W";
        this.defaultColorCode = "W";
    }

    get DefaultColor() {
        return "|" + this.defaultColorCode;
    }

    Output(message, isNewLine = true) {
        let element = null;

        this.analyzeAndWrite(message);

        if (isNewLine === true) {
            $('#consoleOutput').append(this.EndLine);
        }

        var consoleContainer = $('.consoleContainer');
        consoleContainer[0].scrollTop = consoleContainer[0].scrollHeight;
    }

    analyzeAndWrite(message) {
        var messageToWrite = "";
        this.currentTextClass = this.defaultColorCode;
        for (var i = 0; i < message.length; i++) {
            if (message[i] !== '|') {
                messageToWrite += message[i];
            }
            else {
                this.write(messageToWrite);
                messageToWrite = "";
                i++;
                this.changeTextClass(message[i]);
            }
        }

        this.write(messageToWrite);
    }

    changeTextClass(code) {
        this.currentTextClass = code;
    }

    write(message) {
        if (message === "") {
            return;
        }

        var element = $('<span>');
        element.addClass(this.currentTextClass);
        element.html(message);
        $('#consoleOutput').append(element);
    }

    async OutputPrinter(message, isNewLine = true, delay = 60) {
        this.Output(message, isNewLine);
    }

    LoadScript(location) {
        location = "/Content/apps/DungeonCrawler2/" + location;
        console.log('Loading script file ' + location);
        var s = document.createElement('script');
        s.src = location;
        s.type = "text/javascript";
        s.async = false;
        document.getElementsByTagName('head')[0].appendChild(s);
    }

    LoadData(location) {
        location = "/Content/apps/DungeonCrawler2/" + location;
        console.log('Loading data file ' + location);

        var data = "";

        jQuery.ajax({
            url: location,
            success: function (response) {
                data = response;
            },
            async: false
        });

        return data.replace(/^\uFEFF/, '');
    }

    Reload() {
        location.reload();
    }

    Exit() {

    }
}

var Engine = new EngineClass();

//$(function () {
//    var hub = $.connection.ChatHub;

//    hub.client.addMessage = addMessage;
//    hub.client.addSystemMessage = addSystemMessage;

//    $('#consoleInput').focus();

//    $.connection.hub.start().done(function () {
//        hub.server.clientJoinSession(sessionId);
//        $('#executeButton').click(function () {
//            sendCommand(hub);
//        });
//        $("#consoleInput").keyup(function (event) {
//            if (event.keyCode === 13) {
//                sendCommand(hub);
//            }
//        });
//        $(window).on('unload', function () {

//        });
//    });
//});

//function sendCommand(hub) {
//    var command = $('#consoleInput').val();
//    $('#consoleInput').val('');
//    addMessage(command);

//    var args = { SessionId: sessionId, Command: command };
//    hub.server.clientSendCommand(args).fail(hubError);
//}

//var lineFinished = true;

//function addMessage(message, isNewLine = true) {
//    if (message === "") message = " ";
//    var consoleOutput = $('#consoleOutput');

//    let element = null;
//    if (lineFinished === true) {
//        element = $('<pre>');
//        element.html(message);
//        consoleOutput.append(element);
//    }
//    else {
//        element = $('#consoleOutput pre:last-child');
//        element.html(element.html() + message);
//    }
//    lineFinished = isNewLine;
//    element[0].scrollIntoView({
//        behavior: 'smooth'
//    });
//}

//function addSystemMessage(message) {
//    addMessage(message);
//}

//function hubError(e) {
//    if (e.source === 'HubException') {
//        addSystemMessage('Wystąpił błąd podczas wysyłania komendy do serwera: ' + e.message);
//    }
//}