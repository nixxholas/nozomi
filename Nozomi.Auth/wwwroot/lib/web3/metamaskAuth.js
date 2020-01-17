document.getElementById("metamaskBtn").addEventListener('click', (e) => metamaskAuth(e), false);

async function metamaskAuth(e) {
    e.preventDefault();
    let returnUrl = document.getElementById('ReturnUrl').value;
    
    try {
        // Modern dapp browsers...
        if (window.ethereum) {
            // Propagate Web3
            window.web3 = new Web3(ethereum);
            await window.ethereum.enable();
            window.ethereum.autoRefreshOnNetworkChange = false;

            // Obtain the user accounts
            let authMsg = 'This is a Nozomi auth message';
            let accounts = await window.web3.eth.getAccounts();

            // Ensure that the user is holding the wallet/s by asking him to unlock his
            // account with a random message.
            // https://ethereum.stackexchange.com/questions/48489/how-to-prove-that-a-user-owns-their-public-key-for-free=
            if (accounts != null && accounts.length > 0) {
                // let shaMsg = window.web3.utils.sha3(authMsg);
                let signed = await window.web3.eth.personal.sign(authMsg, accounts[0],
                    function (err, sig) {
                        if (err) {
                            console.dir(err);
                        }
                    });

                document.getElementById('Message').value = authMsg;
                document.getElementById('Signature').value = signed;
                document.getElementById('Address').value = accounts[0];
                
                document.getElementById('web3login').submit();

                // let web3Payload = {
                //     "address": accounts[0],
                //     "signature": signed,
                //     "message": authMsg
                // };
                
                //console.dir("web3payload: " + web3Payload);

                // Validate the signed object on server side and provide an auth
                // await axios({
                //     method: 'post',
                //     headers: {"Content-Type": "application/json"},
                //     url: '/account/Web3Login?ReturnUrl=' 
                //         + returnUrl,
                //     data: web3Payload
                // }).then(function (response) {
                // }).catch(function (error) {
                //     console.dir(error);
                // });
            }
        }
        // Legacy dapp browsers...
        else if (window.web3) {
            window.web3 = new Web3(web3.currentProvider);
            // Acccounts always exposed
            web3.eth.sendTransaction({/* ... */});
        }
        // Non-dapp browsers...
        else {
            console.dir("Browser is does not support web3.");
        }
    } catch (error) {
        if (error && error.message) {
            console.dir(error.message);
        }
    }
}