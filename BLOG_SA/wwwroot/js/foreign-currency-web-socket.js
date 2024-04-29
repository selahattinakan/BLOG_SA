window.dataLayer = window.dataLayer || [];
function gtag() { dataLayer.push(arguments); }
gtag('js', new Date());

gtag('config', 'G-F304NNGXQJ');

var socket;

document.addEventListener("DOMContentLoaded", () => {
    ForeignCurrencySocket();
});

function ForeignCurrencySocket() {
    try {
        socket = new WebSocket('wss://s.canlidoviz.com/socket.io/?EIO=4&transport=websocket');
        socket.addEventListener('open', (event) => {
            try {
                console.log('Döviz bağlantı açıldı.');
                socket.send('40');
                console.log('40 Gönderildi.');
                socket.send('42["us",{"itemTypes":["CURRENCY"],"itemCodes":["GA","EUR/USD","BTC","XAU/USD","GAG","XAU/XAG","XBRUSD","DVZSP1","XU100","USD/RUB","GBP/USD","USD/JPY","GBP/EUR","USD/CAD","USD/SEK","AUD/USD"],"sendMessages":false}]');
                console.log('42 Gönderildi.Dinleme bekleniyor...');
            }
            catch (err) {
                $("#dolarRate").hide();
                $("#euroRate").hide();
                console.log("Döviz socket açılışında hata alındı. \n" + err.message);
            }
        });
        socket.addEventListener('message', (event) => {
            try {
                console.log("Döviz mesaj alındı");
                var testData = '42["c",["USD|0|32.0005|32.0082|||0.0935|0.0029","GA|0|2243.378|2243.542|||9.536|0.0043","GA|1|2404.35|2430.11|||3.46|0.0014","EUR|0|35.0069|35.1052|||0.0648|0.0018","XAU/USD|0|2180.23|2180.39|||2.77|0.0013","DKK|0|4.8512|4.8755|||0.1902|0.0406","NOK|0|3.173|3.1889|||0.1255|0.041","JPY|0|22.4885|22.6012|||0.0046|0.0002","ZAR|0|1.754|1.7628|||-0.0107|-0.006","NZD|0|20.4194|20.5217|||0.0019|0.0001","DVZSP1|0|33.5037|33.5568|||0.0792|0.0024","XAU/XAG|0|89.32|89.43|||-0.26|-0.0029","USD/JPY|0|147.02|147.06|||-0.03|-0.0002","GAG|0|25.0753|25.1267|||0.1878|0.0075"]]';
                RenderExchangeRate(event.data);
            }
            catch (err) {
                $("#dolarRate").hide();
                $("#euroRate").hide();
                console.log("Döviz mesaj alımında hata alındı. \n" + err.message);
            }
        });
        socket.addEventListener('close', (event) => {
            console.log('Döviz bağlantı kapandı.');
            ForeignCurrencySocket();
        });
    }
    catch (err) {
        $("#dolarRate").hide();
        $("#euroRate").hide();
        console.log("Döviz socket configurasyonunda hata alındı. \n" + err.message);
    }
}

function RenderExchangeRate(data) {
    try {
        if (data.slice(0, 2) == "42") {
            var arr = JSON.parse(data.slice(2))[1];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].slice(0, 6) == "USD|0|") {
                    var usdArr = arr[i].split("|");
                    $("#dolarRate").html("Dolar : " + usdArr[3]);
                    $("#dolarRate").show();
                }
                else if (arr[i].slice(0, 6) == "EUR|0|") {
                    var eurArr = arr[i].split("|");
                    $("#euroRate").html("Euro : " + eurArr[3]);
                    $("#euroRate").show();
                }
            }
        }
    }
    catch (err) {
        $("#dolarRate").hide();
        $("#euroRate").hide();
        console.log("Döviz parse edilirken hata alındı. \n" + err.message);
    }
}