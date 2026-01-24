async function AddToCart(bookId) {
    try {
        const response = await fetch(
            `https://localhost:7121/Registered/AddToCart?bookID=${encodeURIComponent(bookId)}`,
            {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            }
        );

        // alert(`https://localhost:7121/Registered/AddToCart?bookID=${encodeURIComponent(bookId)}`);
        if (!response.ok) {
            alert("Didn't work");
            // showAddToCartError(bookId);
            return;
        }

        const data = await response.json(); // { success: true/false }
        //alert(data.success);


        if (data.success) {
            showAddToCartSuccess(bookId);
        } else {
            showAddToCartError(bookId);
        }

    } catch (err) {
        console.error(err);
        showAddToCartError(bookId);
    }
}

function showAddToCartSuccess(bookId) {
    const add_to_cart_button = document.getElementById('book-' + bookId);
    if (!add_to_cart_button) return;

    add_to_cart_button.textContent = 'Added ✔';
    add_to_cart_button.disabled = true;
    add_to_cart_button.classList.add('in-cart');
}

function showAddToCartError(bookId) {
    const btn = document.getElementById('book-' + bookId);
    if (!btn) return;

    btn.textContent = 'Failed';
    btn.classList.add('error');
    btn.disabled = true;

    setTimeout(() => {
        btn.textContent = `
            <svg viewBox="0 0 24 24" style="width: 20px; height: 20px; stroke: currentColor; fill: none;">
                                <circle cx="9" cy="21" r="1"></circle>
                                <circle cx="20" cy="21" r="1"></circle>
                                <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"></path>
                            </svg>
            Add to Cart;`;
        btn.classList.remove('error');
        btn.disabled = false;
    }, 2000);
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}