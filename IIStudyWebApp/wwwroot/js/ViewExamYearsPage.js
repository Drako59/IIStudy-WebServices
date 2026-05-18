










// Open modal
function openYearModal() {
    document.getElementById('yearModal').classList.add('active');
}

// Close modal
function closeYearModal() {
    document.getElementById('yearModal').classList.remove('active');
}

// Close modal when clicking overlay
function closeModalOnOverlay(event) {
    if (event.target.classList.contains('modal-overlay')) {
        closeYearModal();
    }
}




//const allYears = [2026, 2025, 2024, 2023, 2022, 2021, 2020, 2019, 2018, 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010];

// Show only 4 latest years by default
//const defaultYearsCount = 4;

// Initialize the page
//function init() {
//    renderMainYears();
//    renderModalYears();
//}




// Render the 4 latest years on main page
//function renderMainYears() {
//    const container = document.getElementById('yearsContainer');
//    const showAllBtn = container.querySelector('.show-all-btn');

//    // Clear container
//    container.innerHTML = '';

//    // Add back the "Show All" button first
//    container.appendChild(showAllBtn);

//    // Add the 4 latest years
//    const latestYears = allYears.slice(0, defaultYearsCount);
//    latestYears.forEach(year => {
//        const link = document.createElement('a');
//        link.className = 'year-link';
//        link.href = '#'; // Replace with actual route: `/exams/${year}`
//        link.textContent = year;
//        link.onclick = (e) => {
//            e.preventDefault();

//        };
//        container.appendChild(link);
//    });
//}

//// Render all years in modal
//function renderModalYears() {
//    const grid = document.getElementById('yearsModalGrid');
//    grid.innerHTML = '';

//    allYears.forEach(year => {
//        const link = document.createElement('a');
//        link.className = 'year-modal-link';
//        link.href = '#'; // Replace with actual route: `/exams/${year}`
//        link.textContent = year;
//        link.onclick = (e) => {
//            e.preventDefault();
//        };
//        grid.appendChild(link);
//    });
//}

// Initialize on page load
//window.addEventListener('DOMContentLoaded', init);