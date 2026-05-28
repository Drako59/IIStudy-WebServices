function openBook(bookTitle) {
    // alert(`Opening "${bookTitle}"...\n\nThe book reader will open in a new window.`);

    // In a real application, this would open a PDF viewer or book reader
    // window.open('/reader?book=' + encodeURIComponent(bookTitle), '_blank');
}

function searchBooks() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const bookCards = document.querySelectorAll('.book-card');

    bookCards.forEach(card => {
        const title = card.dataset.title.toLowerCase();
        const author = card.querySelector(".book-author").textContent.toLowerCase();
        if (title.includes(searchTerm) || author.includes(searchTerm)) {
            card.style.display = 'block';
        } else {
            card.style.display = 'none';
        }
    });
}

function openQrModal(event, button) {
    event.preventDefault();
    event.stopPropagation();

    const bookCard = button.closest(".book-card");

    if (!bookCard) {
        return;
    }

    if (bookCard.dataset.hasFile != "True")
        return;

    const bookUrl = bookCard.dataset.bookUrl;
    const bookName = bookCard.dataset.bookName;

    const modal = document.getElementById("qrModal");
    const qrImage = document.getElementById("qrImage");
    const qrBookName = document.getElementById("qrBookName");

    qrBookName.textContent = bookName;

    // רק כאן מתבצעת הפנייה ל-API, רק אחרי לחיצה
    qrImage.src =
        "https://api.qrserver.com/v1/create-qr-code/?size=220x220&data="
    + encodeURIComponent(`${window.location.origin}${bookUrl}`);

    modal.classList.remove("hidden");
}

function closeQrModal() {
    const modal = document.getElementById("qrModal");
    const qrImage = document.getElementById("qrImage");

    modal.classList.add("hidden");

    // מנקה את התמונה כדי שלא תהיה קריאה/טעינה מיותרת בפעם הבאה
    qrImage.src = "";
}