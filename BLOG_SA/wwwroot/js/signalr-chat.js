var nickName;

$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").configureLogging(signalR.LogLevel.Information).build();

    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";

    const broadcastMessageToGroupClient = "BroadcastMessageToGroupClient";
    const receiveMessageForGroupClients = "ReceiveMessageForGroupClients";

    const groupName = "BackendDevelopers";


    //init
    async function start() {
        try {
            await connection.start().then(() => {
                console.log("ChatHub connected");

                if (getCookie("nickName").length >= 3) {
                    nickName = getCookie("nickName");
                    $("#nick-input").val(getCookie("nickName"));
                    $(".nick-container").hide();
                    $(".chat-container").show();

                    connection.invoke("AddGroup", groupName, nickName).then(() => {
                        console.log("ChatHub group added");
                    });
                }
            });
        } catch (e) {
            setTimeout(() => start(), 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    })

    start();

    $("#btn-open-chat").click(function () {
        if ($("#nick-input").val().length < 3) {
            alert("Lütfen geçerli bir nick name giriniz.");
            return;
        }
        nickName = $("#nick-input").val();
        setCookie("nickName", nickName, 7);
        $(".nick-container").hide();
        $(".chat-container").show();

        connection.invoke("AddGroup", groupName, nickName).then(() => {
            console.log("ChatHub group added");
        });
    });

    //init end

    //group

    $("#send-message").click(function () {
        const message = $("#message-input").val();

        if (!message) return;

        connection.invoke(broadcastMessageToGroupClient, groupName, message, nickName).catch(err => console.error("error:", err));
    });

    $("#message-input").keypress(function (e) {

        var key = e.which;
        if (key == 13)
        {
            const message = $("#message-input").val();

            if (!message) return false;

            connection.invoke(broadcastMessageToGroupClient, groupName, message, nickName).catch(err => console.error("error:", err));
            return false;
        }
    });

    connection.on(receiveMessageForGroupClients, (message, nickName) => {
        if (!message) return;

        $("#chat-messages").append(`${nickName} : ${message} <br />`);
        $("#chat-messages").scrollTop($('#chat-messages')[0].scrollHeight);
        $("#message-input").val("");
        $("#message-input").focus();

    });
    //group end

    //client count
    connection.on(receiveConnectedClientCountAllClient, (count) => {
        $("#span-connected-count").html(count);
        console.log("Connected client count:", count);
    });
    //client count end
});

