function showDialog(msg) {
    $("#dialog-msg").text(msg.msg);
    $("#dialog").dialog({
        buttons: [
          {
              text: "OK",
              click: function () {
                  $(this).dialog("close");
              }
          }
        ]
    });
}