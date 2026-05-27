

// checkEmptyCart();

updateTotal();
function updateQty(btn, change) {
    const qtyElement = btn.parentElement.querySelector('.qty-value');
    let currentQty = parseInt(qtyElement.textContent);
    let newQty = currentQty + change;

    if (newQty >= 1) {
        qtyElement.textContent = newQty;
        updateTotal();
    }
}


async function removeOneItem(btn, bookID) {

    try {
        // btn = document.getElementById("book-" + bookID);
        // alert("here");
        const qtyElement = btn.parentElement.querySelector('.qty-value');
        let currentQty = parseInt(qtyElement.textContent);
        if (currentQty - 1 < 1) {
            return;
        }

        const response = await fetch(
            `${window.location.origin}/Registered/RemoveFromCart?bookID=${encodeURIComponent(bookID)}`,
            {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            }
        );

        // alert(`https:localhost:7121/Registered/AddToCart?bookID=${encodeURIComponent(bookID)}`);
        if (!response.ok) {
            alert("Didn't work");
            // showAddToCartError(bookId);
            return;
        }

        const data = await response.json(); // { success: true/false }
        // alert(data.success);

        
        if (data.success)
            updateQty(btn, -1);

       



    } catch (err) {
        console.error(err);
        // showAddToCartError(bookId);
    }


}


async function removeItem(btn, bookID) {

    try {
        // btn = document.getElementById("book-" + bookID);
        // alert("here");


        const response = await fetch(
            `${window.location.origin}/Registered/RemoveAllBooksFromCart?bookID=${encodeURIComponent(bookID)}`,
            {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            }
        );

        // alert(`https:localhost:7121/Registered/AddToCart?bookID=${encodeURIComponent(bookID)}`);
        if (!response.ok) {
            alert("Didn't work");
            // showAddToCartError(bookId);
            return;
        }

        const data = await response.json(); // { success: true/false }
        // alert(data.success);
        if (!data.success)
            alert("Remove failed.")

        if (data.success) {
            const cartItem = btn.closest('.cart-item');
            cartItem.style.opacity = '0';
            cartItem.style.transform = 'translateX(-20px)';
            setTimeout(() => {
                cartItem.remove();
                updateTotal();
                checkEmptyCart();
            }, 300);
        }// else {
        //     showAddToCartError(bookId);
        // }



    } catch (err) {
        console.error(err);
        // showAddToCartError(bookId);
    }


}

function updateTotal() {
    // In a real app, this would recalculate based on actual prices
    try {
        var price;
        var amount;
        var sum = 0;
        const items = document.getElementsByClassName("cart-item");
        for (const item of items) {
            price = parseFloat(item.querySelector(".item-price").textContent.slice(1), 10);
            amount = parseFloat(item.querySelector(".qty-value").textContent, 10);
            sum += amount * price;
        }
        const totalPay = document.querySelector(".total-value");
        const sumPay = document.querySelector(".sum-value");

        const taxPay = document.querySelector(".tax-value");

        sumPay.textContent = `$${sum.toFixed(2)}`;
        taxPay.textContent = `$0.00`; //${(sum / 10).toFixed(2)}
        totalPay.textContent = `$${(sum).toFixed(2)}`;// + (sum / 10)

    }
    catch (err) {
        console.log(err);
    }
}

function checkEmptyCart() {
    const cartItems = document.querySelectorAll('.cart-item');
    if (cartItems.length === 0) {
        const cartSection = document.querySelector('.cart-items-section');
        cartSection.innerHTML = `
                <div class="empty-cart">
                    <svg viewBox="0 0 24 24" fill="none">
                        <circle cx="9" cy="21" r="1"></circle>
                        <circle cx="20" cy="21" r="1"></circle>
                        <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"></path>
                    </svg>
                    <h3>Your cart is empty</h3>
                    <p>Looks like you haven't added any books yet</p>
                    <button class="checkout-btn" onclick="continueShopping()">Browse Books</button>
                </div>
            `;
    }
}

function checkout() {
    const cartItems = document.querySelectorAll('.cart-item');
    if (cartItems.length == 0) {
        ShowEmptyToast("The cart is empty....🤓");
        return;
    }
    window.location.href = `${window.location.origin}/Registered/PaymentPage`;
}

function ShowEmptyToast(msg){
    const toastDiv = document.createElement("div");
    toastDiv.innerText = msg;
    toastDiv.className = "toast";
    document.body.appendChild(toastDiv);

    setTimeout(() => {
        toastDiv.remove();
    }, 3000);
}

function continueShopping() {
    window.location.href = `${window.location.origin}/Registered/ViewBookCatalog`;
}





async function AddToCart(btn,bookId) {
    try {
        const response = await fetch(
            `${window.location.origin}/Registered/AddToCart?bookID=${encodeURIComponent(bookId)}`,
            {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            }
        );

        // alert(`${window.location.origin}/Registered/AddToCart?bookID=${encodeURIComponent(bookId)}`);
        if (!response.ok) {
            alert("Didn't work");
            // showAddToCartError(bookId);
            return;
        }

        const data = await response.json(); // { success: true/false }
        //alert(data.success);
        if (data.success)
            updateQty(btn, 1);
       
        //if (data.success) {
        //    showAddToCartSuccess(bookId);
        //} else {
        //    showAddToCartError(bookId);
        //}

    } catch (err) {
        console.error(err);
        //showAddToCartError(bookId);
    }
}

async function removeAndUpdate(button, bookID) {
    await removeOneItem(button, bookID);
    updateStockWarning(button);
}

async function addAndUpdate(button, bookID) {
    await AddToCart(button, bookID);
    updateStockWarning(button);
}
function updateStockWarning(button) {
    const cartItem = button.closest(".cart-item");

    if (!cartItem) {
        return;
    }

    const qtyElement = cartItem.querySelector(".qty-value");
    const stockElement = cartItem.querySelector(".stock-count-value");
    const warningElement = cartItem.querySelector(".stock-warning");

    if (!qtyElement || !stockElement || !warningElement) {
        return;
    }

    const quantity = parseInt(qtyElement.textContent.trim(), 10);
    const stockCount = parseInt(stockElement.textContent.trim(), 10);

    if (isNaN(quantity) || isNaN(stockCount)) {
        return;
    }

    if (quantity > stockCount || stockCount <= 0) {
        warningElement.classList.remove("stock-warning-hidden");
        cartItem.classList.add("out-of-stock-item");
    } else {
        warningElement.classList.add("stock-warning-hidden");
        cartItem.classList.remove("out-of-stock-item");
    }
}