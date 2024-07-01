
var wallet = wallet || new (class Wallet {
    constructor() {
        this.walletAddress = null;

    }



    async sign(walletType,expTime) {
        let walletAddress = null;
        let msg = null;
        let nickName = null;
        let signature = null;
        let back = [];
        if (walletType == "tron") {
            if (window.tronWeb) {
                this.tronWeb = window.tronWeb;
                walletAddress = this.tronWeb.defaultAddress.base58;

                if (walletAddress) {
                    nickName = walletAddress.subStr(5, 10);
                    msg = walletType + "#" + walletAddress + "#O" + nickName + "#" + expTime;
                    signature = await this.tronWeb.trx.signMessageV2(msg);
                    var hex = this.tronWeb.address.toHex(walletAddress);
                    console.log("hex address: ", hex);
                } else { back = ["err", "请登录钱包"]; }
            } else {
                back = ["err", "请安装TronLink插件"];
            }
        } else if (walletType === "ether") {
            const accounts = await ethereum.request({ method: 'eth_requestAccounts' });
            walletAddress = accounts[0];
            nickName = walletAddress.subStr(5, 10);
            msg = walletType + "#" + walletAddress + "#O" + nickName + "#" + expTime;
            signature = await ethereum
                .request({
                    method: "personal_sign",
                    params: [msg, walletAddress],
                });
        }
        if (walletAddress) {
            var arr = [];
            arr.push(walletAddress);
            arr.push(nickName);
            arr.push(signature);
            return arr;
        }
        if (back) return back;
        return ["err", "请登录钱包"];
    }
    //查询钱包余额
    async getBalance() {
        //当前连接的钱包地址获取 window.tronWeb.defaultAddress.base58
        var balance = await this.tronWeb.trx.getBalance(this.walletAddress);
        console.log("balance=", balance)
    }
    //trx转账交易
    async transaction() {
        var tx = await this.tronWeb.transactionBuilder.sendTrx(
            "TN9RRaXkCFtTXRso2GdTZxSxxwufzxLQPP", 10 * Math.pow(10, 6), this.walletAddress
        );
        var signedTx = await this.tronWeb.trx.sign(tx);
        var broastTx = await this.tronWeb.trx.sendRawTransaction(signedTx);
        console.log(broastTx)
    }

    //trx-token转账交易
    async transactionToken() {

        //转账方式1

        let contract = await this.tronWeb.contract().at("TURwoLuFy7maq1Vea7wVwRNz3HgmcAsJzb");
        let result = await contract.transfer(
            "TN9RRaXkCFtTXRso2GdTZxSxxwufzxLQPP",
            this.tronWeb.toHex(55 * Math.pow(10, 18))
        ).send({
            feeLimit: 10000000
        }).then(output => { console.log('- Output:', output, '\n'); });
        console.log('result: ', result);

        //转账方式2
        /*const parameter = [{type:'address',value:'TN9RRaXkCFtTXRso2GdTZxSxxwufzxLQPP'},{type:'uint256',value:this.tronWeb.toHex(25 * Math.pow(10,18))}]
        var tx  = await this.tronWeb.transactionBuilder.triggerSmartContract("TURwoLuFy7maq1Vea7wVwRNz3HgmcAsJzb", "transfer(address,uint256)", {},parameter,this.walletAddress);
        var signedTx = await this.tronWeb.trx.sign(tx.transaction);
        var broastTx = await this.tronWeb.trx.sendRawTransaction(signedTx);
        console.log(broastTx)*/

        /*let contract = await this.tronWeb.contract().at("TURwoLuFy7maq1Vea7wVwRNz3HgmcAsJzb");
       let result1 = await contract.decimals().call();
       console.log('result: ', result1);*/
    }
    //调用合约中的方法
    async transactionContract() {

        //调用方式1
        /*let contract = await this.tronWeb.contract().at("TSbJGFA8UyYGTuXBRbYB2GJeh2CY1X5F4d");
        console.log("contract=",contract)
        let result = await contract.registrationExt(
          "TN9RRaXkCFtTXRso2GdTZxSxxwufzxLQPP"
        ).send({
          callValue: this.tronWeb.toHex(25 * Math.pow(10,6)),//发送TRX余额
          feeLimit: 10000000
        }).then(output => {console.log('- Output:', output, '\n');});
        console.log('result: ', result)*/

        //调用方式2
        const parameter = [{ type: 'address', value: 'TN9RRaXkCFtTXRso2GdTZxSxxwufzxLQPP' }];
        var tx = await this.tronWeb.transactionBuilder.triggerSmartContract(
            "TSbJGFA8UyYGTuXBRbYB2GJeh2CY1X5F4d",
            "registrationExt(address)",
            {},
            parameter,
            this.walletAddress
        );
        var signedTx = await this.tronWeb.trx.sign(tx.transaction);
        var broastTx = await this.tronWeb.trx.sendRawTransaction(signedTx);
        console.log(broastTx)
    }

})();