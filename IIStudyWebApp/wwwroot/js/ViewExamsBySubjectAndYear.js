// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    const options = { year: "numeric", month: "short", day: "numeric" };
    return date.toLocaleDateString("en-US", options);
}

// Search exams
function searchExams() {
    const searchInput = document.getElementById("searchInput");

    if (!searchInput) {
        return;
    }

    const searchTerm = searchInput.value.trim().toLowerCase();

    const exams = document.querySelectorAll(".exam-row");

    let count = 0;

    exams.forEach(exam => {
        const searchData = exam.dataset.search
            ? exam.dataset.search.trim().toLowerCase()
            : exam.textContent.trim().toLowerCase();

        if (searchData.includes(searchTerm)) {
            exam.style.display = "";
            count++;
        } else {
            exam.style.display = "none";
        }
    });

    const resultsCount = document.getElementById("resultsCount");

    if (resultsCount) {
        resultsCount.textContent = `${count} Exam${count !== 1 ? "s" : ""} found`;
    }
}

// View exam
function viewExam(examId) {
    alert(`Opening exam with ID: ${examId}`);
}

// View solutions
function viewSolutions(examId) {
    alert(`Opening solutions for exam ID: ${examId}`);
}