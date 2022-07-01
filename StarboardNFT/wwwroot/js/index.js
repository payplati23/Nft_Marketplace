async function BitskiSignInShow() {
    console.log('wwwwwwwwwwwwwwww');
    const bitski = new Bitski.Bitski('1f1ffc56-f7a7-41ce-8613-1b200dd1d324', 'https://localhost:44360/bitski_callback.html');
    console.log(bitski);
    const provider = bitski.getProvider();
    const web3 = new Web3(provider); console.log(web3);
    try {
        const accounts = await web3.eth.getAccounts();
        console.log("fdsafdafdf", accounts);
        return accounts[0];
    } catch (e) {
        console.log(e);
        // await bitski.signIn();
        return bitski.signIn().then((res) => {
            console.log(res);
            return new Promise((resolve, reject) => {
                resolve(res.account[0]);
            })
        }).catch((error) => {
            console.log(error);
            if (error.code === AuthenticationErrorCode?.UserCancelled) {
                // ignore error
            } else {
                // display error
            }
        });
    }

}

async function HandleLoginWithMagicLink() {
    console.log('[SignIn] HandleLoginWithFormat')
    const fm = new Fortmatic('pk_live_165CEBDDC67B3312');
    console.log(fm);
    window.web3 = new Web3(fm.getProvider());

    // Checking if the user is logged in
    let isUserLoggedIn = await fm.user.isLoggedIn(); console.log(isUserLoggedIn)

    // if the user logged in, return address
    if (isUserLoggedIn) {
        const account = await web3.eth.getAccounts();
        console.log(account);
        return account[0];
    }

    // if the user not logged in, signin request and return address

    var address = fm.user.login().then(() => {
        return web3.eth.getAccounts().then((account) => {
            return new Promise((resolve, reject) => {
                resolve(account[0]);
            })
        });
    });

    return address;

}

async function SignInWalletConnect () {console.log('[WalletConnection]')
    // Create a connector
    const Web3Modal = window.Web3Modal.default;
    const WalletConnectProvider = window.WalletConnectProvider.default;
    const EvmChains = window.EvmChains;
    const Fortmatic = window.Fortmatic;
    
    // Web3modal instance
    let web3Modal

    // Chosen wallet provider given by the dialog window
    let provider;


    // Address of the selected account
    let selectedAccount;

    const providerOptions = {
        walletconnect: {
            display: {
                name: "Mobile"
            },
            package: WalletConnectProvider,
            options: {
                infuraId: "e9752da5067149f9bf96c54f2bfe4e90",
            }
        },
   
    };

    web3Modal = new Web3Modal({
        cacheProvider: false, // optional
        providerOptions, // required
    });

    provider = await web3Modal.connect();

    const accounts = provider.accounts;
    return accounts[0];

}

async function fetchAccountData(EvmChains) {
    // Get a Web3 instance for the wallet
    const web3 = new Web3(provider);

    console.log("Web3 instance is", web3);

    // Get connected chain id from Ethereum node
    const chainId = await web3.eth.getChainId();
    // Load chain information over an HTTP API
    const chainData = await EvmChains.getChain(chainId);
    document.querySelector("#network-name").textContent = chainData.name;

    // Get list of accounts of the connected wallet
    const accounts = await web3.eth.getAccounts();

    // MetaMask does not give you all accounts, only the selected account
    console.log("Got accounts", accounts);
    selectedAccount = accounts[0];

    document.querySelector("#selected-account").textContent = selectedAccount;

    return selectedAccount;
    // Get a handl
    const template = document.querySelector("#template-balance");
    const accountContainer = document.querySelector("#accounts");

    accountContainer.innerHTML = '';

 
    const rowResolvers = accounts.map(async (address) => {
        const balance = await web3.eth.getBalance(address);
       
        // https://github.com/indutny/bn.js/
        const ethBalance = web3.utils.fromWei(balance, "ether");
        const humanFriendlyBalance = parseFloat(ethBalance).toFixed(4);
    
        const clone = template.content.cloneNode(true);
        clone.querySelector(".address").textContent = address;
        clone.querySelector(".balance").textContent = humanFriendlyBalance;
        accountContainer.appendChild(clone);
    });

  
    await Promise.all(rowResolvers);

}

async function Walletlink_Request() {
    
    console.log('[WalletLink] connection');
    const WalletLink = window.walletlink.WalletLink;
    const APP_NAME = 'Starboard'
    const APP_LOGO_URL = 'https://example.com/logo.png'
    const ETH_JSONRPC_URL = 'https://mainnet.infura.io/v3/a58ceb01027546d49881cfbbbf9898ee'
    const CHAIN_ID = 1

    const walletLink_app = new WalletLink({
        appName: APP_NAME,
        appLogoUrl: APP_LOGO_URL
    })

    const ethereum = walletLink_app.makeWeb3Provider(
        ETH_JSONRPC_URL, 1
    )

    const web3 = new Web3(ethereum)
    let accountAddress = null
    
    accountAddress = await ethereum.send('eth_requestAccounts').then((accounts) => {
        return new Promise((resolve, reject) => {
            resolve(accounts[0]);
        });
    })

    return accountAddress;
}   


function hideTopSignInBtn() {
    var x = document.getElementById('topSignInBtn');
    x.style.display = "none";
}

function showTopSignInBtn() {
    var x = document.getElementById('topSignInBtn');
    x.style.display = "block";
}

function initialProfilePortfolioComponent() {
    let x, i, j, l, ll, selElmnt, a, b, c;
    /* Look for any elements with the class "custom-select": */
    x = document.getElementsByClassName("custom-select");
    l = x.length;
    for (i = 0; i < l; i++) {
        selElmnt = x[i].getElementsByTagName("select")[0];
        ll = selElmnt.length;
        /* For each element, create a new DIV that will act as the selected item: */
        a = document.createElement("DIV");
        a.setAttribute("class", "select-selected");
        a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
        x[i].appendChild(a);
        /* For each element, create a new DIV that will contain the option list: */
        b = document.createElement("DIV");
        b.setAttribute("class", "select-items select-hide");
        for (j = 1; j < ll; j++) {
            /* For each option in the original select element,
            create a new DIV that will act as an option item: */
            c = document.createElement("DIV");
            c.innerHTML = selElmnt.options[j].innerHTML;
            c.addEventListener("click", function (e) {
                /* When an item is clicked, update the original select box,
                and the selected item: */
                let y, i, k, s, h, sl, yl;
                s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                sl = s.length;
                h = this.parentNode.previousSibling;
                for (i = 0; i < sl; i++) {
                    if (s.options[i].innerHTML == this.innerHTML) {
                        s.selectedIndex = i;
                        h.innerHTML = this.innerHTML;
                        y = this.parentNode.getElementsByClassName("same-as-selected");
                        yl = y.length;
                        for (k = 0; k < yl; k++) {
                            y[k].removeAttribute("class");
                        }
                        this.setAttribute("class", "same-as-selected");
                        break;
                    }
                }
                h.click();
            });
            b.appendChild(c);
        }
        x[i].appendChild(b);
        a.addEventListener("click", function (e) {
            /* When the select box is clicked, close any other select boxes,
            and open/close the current select box: */
            e.stopPropagation();
            closeAllSelect(this);
            this.nextSibling.classList.toggle("select-hide");
            this.classList.toggle("select-arrow-active");
        });
    }

    function closeAllSelect(elmnt) {
        /* A function that will close all select boxes in the document,
        except the current select box: */
        let x, y, i, xl, yl, arrNo = [];
        x = document.getElementsByClassName("select-items");
        y = document.getElementsByClassName("select-selected");
        xl = x.length;
        yl = y.length;
        for (i = 0; i < yl; i++) {
            if (elmnt == y[i]) {
                arrNo.push(i)
            } else {
                y[i].classList.remove("select-arrow-active");
            }
        }
        for (i = 0; i < xl; i++) {
            if (arrNo.indexOf(i)) {
                x[i].classList.add("select-hide");
            }
        }
    }

    /* If the user clicks anywhere outside the select box,
    then close all select boxes: */
    document.addEventListener("click", closeAllSelect);
}

function OpenProfileUserPhotoEditDialog() {
    $("#profile_photo_image_fileUpload").trigger('click');
}

function OpenProfileUserBannerEditDialog() {
    $("#profile_banner_image").trigger('click');
}

function initializeIndexPageComponent() {

    //$(window).scroll(function () {
    //    $('header').toggleClass('scrolled', $(this).scrollTop() > 80);
    //});

    //$("#sidebar").mCustomScrollbar({
    //    theme: "minimal"
    //});

    //$('#dismiss, .overlay-body').on('click', function () {
    //// hide sidebar
    //$('#sidebar').removeClass('active');
    //    // hide overlay
    //    $('.overlay').removeClass('active');
    //});

    //$('#sidebarCollapse').on('click', function () {
    //// open sidebar
    //$('#sidebar').addClass('active');
    //    // fade in the overlay
    //    //$('.overlay').addClass('active');
    //    $('.collapse.in').toggleClass('in');
    //    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    //});

    // popular_seller_swiper swiper
    // jquery ui input datepicker
    $(function () {
        $(".datepicker").datepicker();
    });



    // popular_seller_swiper swiper
    new Swiper(".popular_seller_swiper", {
        slidesPerView: 1,
        spaceBetween: 1,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: '[data-swiper_popular_seller="next"]',
            prevEl: '[data-swiper_popular_seller="prev"]',
        },
        breakpoints: {
            0: {
                slidesPerView: 1.3,
                spaceBetween: 10,
            },
            640: {
                slidesPerView: 2,
                spaceBetween: 10,
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 15,
            },
            1024: {
                slidesPerView: 4,
                spaceBetween: 20,
            },
            1200: {
                slidesPerView: 5,
                spaceBetween: 20,
            },
        },
    });

    // collectors_card_swiper swiper

    new Swiper(".collectors_card_swiper", {
        slidesPerView: 1,
        spaceBetween: 1,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: '[data-swiper_collectors_only="next"]',
            prevEl: '[data-swiper_collectors_only="prev"]',
        },
        breakpoints: {
            0: {
                slidesPerView: 1.2,
                spaceBetween: 5,
            },
            640: {
                slidesPerView: 2,
                spaceBetween: 10,
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 15,
            },
            1024: {
                slidesPerView: 4,
                spaceBetween: 20,
            }
        },
    });


    // collectors_card_swiper swiper

    new Swiper(".swiper_video_gala_container", {
        navigation: {
            nextEl: '[data-swiper_video_gala="next"]',
            prevEl: '[data-swiper_video_gala="prev"]',
        }
    });



    //gala_collections_card_swiper

    new Swiper(".gala_collections_card_swiper", {
        slidesPerView: 1,
        spaceBetween: 1,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: '[data-swiper_gala_collections="next"]',
            prevEl: '[data-swiper_gala_collections="prev"]',
        },
        breakpoints: {
            640: {
                slidesPerView: 1,
                spaceBetween: 10,
            },
            768: {
                slidesPerView: 2,
                spaceBetween: 15,
            },
            1024: {
                slidesPerView: 3,
                spaceBetween: 20,
            }
        },
    });

    // 

    // source: https://css-tricks.com/value-bubbles-for-range-inputs/
    const allRanges = document.querySelectorAll(".range-wrap");
    allRanges.forEach(wrap => {
        const range = wrap.querySelector(".range");
        const bubble = wrap.querySelector(".bubble");

        range.addEventListener("input", () => {
            setBubble(range, bubble);
        });
        setBubble(range, bubble);
    });

    function setBubble(range, bubble) {
        const val = range.value;
        const min = range.min ? range.min : 0;
        const max = range.max ? range.max : 100;
        const newVal = Number(((val - min) * 100) / (max - min));
        bubble.innerHTML = val + ' ETH';

        // Sorta magic numbers based on size of the native UI thumb
        bubble.style.left = `calc(${newVal}% + (${8 - newVal * 0.15}px))`;
    }

    // timer
    // Set the date we're counting down to
    let countDownDate = new Date("Dec 5, 2025 15:37:25").getTime();

    // Update the count down every 1 second
    let updateCountTime = setInterval(function () {

        // Get today's date and time
        let now = new Date().getTime();

        // Find the distance between now and the count down date
        let distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        let days = Math.floor(distance / (1000 * 60 * 60 * 24));
        let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Display the result in the element with id="demo"

        $('.counter_wrapper .hours').text(hours);
        $('.counter_wrapper .minutes').text(minutes);
        $('.counter_wrapper .seconds').text(seconds);


        // If the count down is finished, write some 
        if (distance < 0) {
            clearInterval(updateCountTime);
            document.getElementById("demo").innerHTML = "EXPIRED";
        }
    }, 1000);
}

