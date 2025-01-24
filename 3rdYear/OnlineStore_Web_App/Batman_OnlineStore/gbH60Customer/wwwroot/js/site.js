window.onload = () => {
    if (localStorage.getItem("isAnimation")) {
        document.querySelector("header#topHeader").scrollIntoView({
            behavior: 'smooth'
        });
        animateCart();
    }
};

async function addToCart(custId, prodId) {
    if (Number.isInteger(custId) && Number.isInteger(prodId)) {
        let postBody = {
            "custId": custId,
            "prodId": prodId
        };
        await fetch('/ShoppingCart/AddToCart', {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(postBody),
        }).then(res => {
            return res.text();
        }).then(text => {
            let parsed = text.toLowerCase();

            if (parsed.includes("id")) {
                localStorage.removeItem("isAnimation");
                location.href = "/Home";
            } else if (parsed.includes("signed-in")) {
                localStorage.removeItem("isAnimation");
                location.href = "/Identity/Account/Login";
            } else {
                location.reload();
            }
        });
    }
}

async function delFromCart(custId, prodId) {
    if (Number.isInteger(custId) && Number.isInteger(prodId)) {
        let postBody = {
            "custId": custId,
            "prodId": prodId
        };
        await fetch('/ShoppingCart/DelFromCart', {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(postBody),
        }).then(res => {
            location.reload();
        });
    }
}

async function remOneFromCart(custId, prodId) {
    if (Number.isInteger(custId) && Number.isInteger(prodId)) {
        let postBody = {
            "custId": custId,
            "prodId": prodId
        };
        await fetch('/ShoppingCart/DelSingleProduct', {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(postBody),
        }).then(res => {
            location.reload();
        });
    }
}

function animateCart() {
    let cart = document.querySelector("nav li#cartLink a");
    cart.classList.add("animated");
    setTimeout(() => {
        cart.classList.remove("animated");
        localStorage.removeItem("isAnimation");
    }, 2000);
}
