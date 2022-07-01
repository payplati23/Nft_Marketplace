window.BitskiSignIn = () => {console.log('fdsafdsaf')
    const bitski = new Bitski.Bitski('1f1ffc56-f7a7-41ce-8613-1b200dd1d324', 'http://localhost:3000');
    const provider = bitski.getProvider();
    // Using Kovan Test network.
    //const provider = bitski.getProvider({ networkName: 'kovan' });
    // end
    const web3 = new Web3(provider);
    const accounts = await web3.eth.getAccounts();
    const network = await web3.eth.net.getId();
    if (bitski.authStatus === AuthenticationStatus.NotConnected) {
        // Show connect button or use your own button and call bitski.signIn()
    }
    console.log(web3, 'Provider Status');
    console.log(accounts, 'Authentication Status');
}

window.addEventListener("load", function () {
    // Create small button
    window.bitski.getConnectButton({ container: document.querySelector('#small-button-container'), size: 'SMALL' }, callback);

    // Create medium button
    window.bitski.getConnectButton({ container: document.querySelector('#medium-button-container'), size: 'MEDIUM' }, callback);

    // Create large button
    window.bitski.getConnectButton({ container: document.querySelector('#large-button-container'), size: 'LARGE' }, callback);

    var defaultButton = window.bitski.getConnectButton();
    defaultButton.callback = callback;

    document.querySelector('#default-button').appendChild(defaultButton.element);
});

function callback(error, user) {
    if (user) {
        window.web3.eth.getAccounts().then((accounts) => {
            var account = accounts[0];
            console.log("Getting balance for " + account);
            return window.web3.eth.getBalance(account);
        }).then((balance) => {
            alert('Signed in! Current balance: ' + balance);
            return window.bitski.signOut();
        }).catch((error) => {
            console.error("Error: " + error);
        });
    }

    if (error) {
        console.error("Error signing in: " + error);
    }
}