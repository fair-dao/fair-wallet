
const tronWeb = new TronWeb({
    fullHost: 'https://api.trongrid.io',
    headers: { "TRON-PRO-API-KEY": '30cca307-6c9d-47df-9dd2-aab15fafa287' },
    privateKey: ''
});
console.log(await tronWeb.createAccount());
export function getMessage() {
    return 'Olá do Blazor!';
}

alert("oooo");