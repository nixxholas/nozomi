document.getElementById("metamaskBtn").addEventListener('click', (e) => metamaskAuth(e), false);

async function metamaskAuth(e) {
    e.preventDefault();
    
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

                let web3Payload = {
                    "claimerAddress": accounts[0],
                    "signature": signed,
                    "rawMessage": authMsg
                };

                // Validate the signed object on server side and provide an auth
                await axios({
                    method: 'post',
                    headers: {"Content-Type": "application/json"},
                    url: '/api/auth/ethauth',
                    data: {
                        "claimerAddress": accounts[0],
                        "signature": signed,
                        "rawMessage": authMsg
                    }
                }).then(function (response) {
                    console.dir(response);
                }).catch(function (error) {
                    console.dir(response);
                });
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
        console.dir(error);
    }
}