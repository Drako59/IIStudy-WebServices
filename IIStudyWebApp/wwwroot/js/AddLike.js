

async function AddLike(reviewID, like = false, dislike = false) {
    const likeBtn = document.getElementById(`like-btn-${reviewID}`);
    const dislikeBtn = document.getElementById(`dislike-btn-${reviewID}`);

    const likeCountSpan = document.getElementById(`like-count-${reviewID}`);
    const dislikeCountSpan = document.getElementById(`dislike-count-${reviewID}`);

    let likesCount = parseInt(likeCountSpan.innerText);
    let dislikesCount = parseInt(dislikeCountSpan.innerText);

    const alreadyLiked = likeBtn.classList.contains("active");
    const alreadyDisliked = dislikeBtn.classList.contains("active");

    let finalLike = like;
    let finalDislike = dislike;

    // אם לחץ שוב על Like שכבר פעיל -> מבטל Like
    if (like && alreadyLiked) {
        finalLike = false;
        finalDislike = false;
    }

    // אם לחץ שוב על Dislike שכבר פעיל -> מבטל Dislike
    if (dislike && alreadyDisliked) {
        finalLike = false;
        finalDislike = false;
    }

    try {
        likeBtn.disabled = true;
        dislikeBtn.disabled = true;

        const response = await fetch(
            `${window.location.origin}/Registered/AddLikeToReview?reviewID=${reviewID}&like=${finalLike}&dislike=${finalDislike}`,
            {
                method: "GET"
            }
        );

        if (!response.ok) {
            throw new Error("Failed to update like");
        }

        // עדכון מצב במסך

        // קודם מורידים מצב קודם
        if (alreadyLiked) {
            likesCount--;
        }

        if (alreadyDisliked) {
            dislikesCount--;
        }

        likeBtn.classList.remove("active");
        dislikeBtn.classList.remove("active");

        // ואז מוסיפים מצב חדש
        if (finalLike) {
            likesCount++;
            likeBtn.classList.add("active");
        }

        if (finalDislike) {
            dislikesCount++;
            dislikeBtn.classList.add("active");
        }

        likeCountSpan.innerText = likesCount;
        dislikeCountSpan.innerText = dislikesCount;
    }
    catch (error) {
        console.error(error);
        alert("Something went wrong while updating your vote.");
    }
    finally {
        likeBtn.disabled = false;
        dislikeBtn.disabled = false;
    }
}