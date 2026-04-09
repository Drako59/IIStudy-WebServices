let currentDate = new Date();
let eventsData = [];

// Mock WebSocket connection - Replace with actual WS endpoint
function connectToWebSocket() {
    // Simulating WebSocket connection
    // In production: const ws = new WebSocket('ws://your-server.com/events');

    // Mock data for demonstration
    fetchEventsFromWS();
}

// Simulate fetching events from WebSocket
function fetchEventsFromWS() {
    // Show loading state
    document.getElementById('loadingState').style.display = 'block';
    document.getElementById('calendarGrid').style.display = 'none';

    // Simulate API delay
    setTimeout(() => {
        // Mock events data - In production, this would come from WebSocket
        eventsData = [
            {
                id: 1,
                title: "Book Club Meeting",
                date: "2026-01-15",
                time: "14:00",
                location: "Main Hall",
                type: "meeting"
            },
            {
                id: 2,
                title: "Author Seminar",
                date: "2026-01-20",
                time: "10:00",
                location: "Conference Room A",
                type: "seminar"
            },
            {
                id: 3,
                title: "Writing Workshop",
                date: "2026-01-25",
                time: "15:30",
                location: "Room 203",
                type: "workshop"
            },
            {
                id: 4,
                title: "Literature Exam",
                date: "2026-01-28",
                time: "09:00",
                location: "Exam Hall",
                type: "exam"
            },
            {
                id: 5,
                title: "Reading Workshop",
                date: "2026-02-05",
                time: "16:00",
                location: "Library",
                type: "workshop"
            }
        ];

        // Hide loading state
        document.getElementById('loadingState').style.display = 'none';
        document.getElementById('calendarGrid').style.display = 'block';

        renderCalendar();
        renderEventsList();
    }, 1000);
}

function renderCalendar() {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    // Update title
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
    document.getElementById('calendarTitle').textContent = `${monthNames[month]} ${year}`;

    // Get first day of month and number of days
    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const daysInPrevMonth = new Date(year, month, 0).getDate();

    const calendarGrid = document.querySelector('.calendar-grid');

    // Clear existing days (keep headers)
    const headers = calendarGrid.querySelectorAll('.calendar-day-header');
    calendarGrid.innerHTML = '';
    headers.forEach(header => calendarGrid.appendChild(header));

    // Add days from previous month
    for (let i = firstDay - 1; i >= 0; i--) {
        const day = daysInPrevMonth - i;
        const dayDiv = createDayElement(day, true, year, month - 1);
        calendarGrid.appendChild(dayDiv);
    }

    // Add days of current month
    for (let day = 1; day <= daysInMonth; day++) {
        const dayDiv = createDayElement(day, false, year, month);
        calendarGrid.appendChild(dayDiv);
    }

    // Add days from next month
    const remainingCells = 42 - (firstDay + daysInMonth);
    for (let day = 1; day <= remainingCells; day++) {
        const dayDiv = createDayElement(day, true, year, month + 1);
        calendarGrid.appendChild(dayDiv);
    }
}

function createDayElement(day, isOtherMonth, year, month) {
    const dayDiv = document.createElement('div');
    dayDiv.className = 'calendar-day';
    if (isOtherMonth) dayDiv.classList.add('other-month');

    // Check if today
    const today = new Date();
    if (day === today.getDate() && month === today.getMonth() &&
        year === today.getFullYear() && !isOtherMonth) {
        dayDiv.classList.add('today');
    }

    const dayNumber = document.createElement('div');
    dayNumber.className = 'day-number';
    dayNumber.textContent = day;
    dayDiv.appendChild(dayNumber);

    // Add events for this day
    const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
    const dayEvents = eventsData.filter(event => event.date === dateStr);

    if (dayEvents.length > 0) {
        const eventsContainer = document.createElement('div');
        eventsContainer.className = 'day-events';

        dayEvents.forEach(event => {
            const eventDot = document.createElement('div');
            eventDot.className = `event-dot ${event.type}`;
            eventDot.textContent = event.title;
            eventDot.title = `${event.title} - ${event.time}`;
            eventsContainer.appendChild(eventDot);
        });

        dayDiv.appendChild(eventsContainer);
    }

    return dayDiv;
}

function renderEventsList() {
    const eventsList = document.getElementById('eventsList');
    eventsList.innerHTML = '';

    // Filter and sort upcoming events
    const today = new Date();
    const upcomingEvents = eventsData
        .filter(event => new Date(event.date) >= today)
        .sort((a, b) => new Date(a.date) - new Date(b.date));

    if (upcomingEvents.length === 0) {
        eventsList.innerHTML = '<p style="text-align: center; color: #718096; padding: 40px;">No upcoming events</p>';
        return;
    }

    upcomingEvents.forEach(event => {
        const eventDate = new Date(event.date);
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        const eventItem = document.createElement('div');
        eventItem.className = 'event-item';
        eventItem.innerHTML = `
                    <div class="event-date-badge">
                        <div class="event-month">${monthNames[eventDate.getMonth()]}</div>
                        <div class="event-day">${eventDate.getDate()}</div>
                    </div>
                    <div class="event-details">
                        <div class="event-title">${event.title}</div>
                        <div class="event-info">
                            <div class="event-info-item">
                                <svg viewBox="0 0 24 24">
                                    <circle cx="12" cy="12" r="10"></circle>
                                    <polyline points="12 6 12 12 16 14"></polyline>
                                </svg>
                                ${event.time}
                            </div>
                            <div class="event-info-item">
                                <svg viewBox="0 0 24 24">
                                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"></path>
                                    <circle cx="12" cy="10" r="3"></circle>
                                </svg>
                                ${event.location}
                            </div>
                        </div>
                    </div>
                    <div class="event-type-badge ${event.type}">${event.type}</div>
                `;
        eventsList.appendChild(eventItem);
    });
}

function previousMonth() {
    currentDate.setMonth(currentDate.getMonth() - 1);
    renderCalendar();
}

function nextMonth() {
    currentDate.setMonth(currentDate.getMonth() + 1);
    renderCalendar();
}

function goToToday() {
    currentDate = new Date();
    renderCalendar();
}

// Initialize calendar on page load
window.addEventListener('DOMContentLoaded', () => {
    connectToWebSocket();
});