function GetBooksByFilter() {
    const input = document.getElementById("search-input");
    const value = input.value;

    window.location.href = `${window.location.origin}&search=${value}`;


}

const searchInput = document.getElementById("search-input");

if (searchInput) {
    searchInput.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();

            const params = new URLSearchParams();

            params.set("pageNumber", "1");
            params.set("search", searchInput.value);

            const subjectID = document.getElementById("subjectID");
            const minPrice = document.querySelector("input[name='minPrice']");
            const maxPrice = document.querySelector("input[name='maxPrice']");
            const inStock = document.querySelector("input[name='inStock']");
            const isOnline = document.querySelector("input[name='isOnline']");

            if (subjectID && subjectID.value) {
                params.set("subjectID", subjectID.value);
            }

            if (minPrice && minPrice.value) {
                params.set("minPrice", minPrice.value);
            }

            if (maxPrice && maxPrice.value) {
                params.set("maxPrice", maxPrice.value);
            }

            if (inStock && inStock.checked) {
                params.set("inStock", "true");
            }

            if (isOnline && isOnline.checked) {
                params.set("isOnline", "true");
            }

            window.location.href = `${window.location.origin}/Guest/ViewBookCatalog?` + params.toString();
        }
    });
}
