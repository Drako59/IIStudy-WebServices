

function Submit() {
    var form = document.getElementById("form_profile");
    form.Submit();
}

function openUpdatePopup() {
    const popup = document.getElementById("updateProfilePopup");

    if (popup) {
        popup.classList.add("active");
        document.body.style.overflow = "hidden";
    }
}

function closeUpdatePopup() {
    const popup = document.getElementById("updateProfilePopup");

    if (popup) {
        popup.classList.remove("active");
        document.body.style.overflow = "";
    }
}

//document.addEventListener("click", function (event) {
//    const popup = document.getElementById("updateProfilePopup");

//    if (popup && event.target === popup) {
//        closeUpdatePopup();
//    }
//});

document.addEventListener("keydown", function (event) {
    if (event.key === "Escape") {
        closeUpdatePopup();
    }
});