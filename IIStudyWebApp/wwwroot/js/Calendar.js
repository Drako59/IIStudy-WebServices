let currentDate = new Date();
let eventsData = [];

function connectToWebSocket() {
    fetchEventsFromWS();
}

async function fetchEventsFromWS() {
    document.getElementById('loadingState').style.display = 'block';
    document.getElementById('calendarGrid').style.display = 'none';

    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, "0");

    try {
        const response = await fetch(`${window.location.origin}/Guest/GetEventsByMonthAndYear?year=${year}&month=${month}`);

        if (!response.ok) {
            throw new Error("Failed to load events");
        }

        eventsData = await response.json();
    }
    catch (error) {
        console.error(error);
        eventsData = [];
    }

    document.getElementById('loadingState').style.display = 'none';
    document.getElementById('calendarGrid').style.display = 'block';

    renderCalendar();
    renderEventsList();
}

function getEventID(event) {
    return event.eventID ?? event.EventID ?? event.eventId ?? "";
}

function getEventName(event) {
    return event.event_name ?? event.Event_name ?? event.eventName ?? "No name";
}

function getEventDate(event) {
    return event.date_event ?? event.Date_event ?? event.eventDate ?? "";
}

function getEventDetails(event) {
    return event.details ?? event.Details ?? "";
}

function escapeHtml(value) {
    if (value === null || value === undefined) return "";

    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

function parseLocalDate(dateStr) {
    if (!dateStr) return new Date();

    const parts = dateStr.split("-");
    if (parts.length !== 3) return new Date(dateStr);

    return new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2]));
}

function formatDate(dateStr) {
    if (!dateStr) return "No date";

    const date = parseLocalDate(dateStr);

    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    return `${date.getDate()} ${monthNames[date.getMonth()]} ${date.getFullYear()}`;
}

function renderCalendar() {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    document.getElementById('calendarTitle').textContent = `${monthNames[month]} ${year}`;

    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const daysInPrevMonth = new Date(year, month, 0).getDate();

    const calendarGrid = document.querySelector('.calendar-grid');

    const headers = calendarGrid.querySelectorAll('.calendar-day-header');
    calendarGrid.innerHTML = '';
    headers.forEach(header => calendarGrid.appendChild(header));

    for (let i = firstDay - 1; i >= 0; i--) {
        const day = daysInPrevMonth - i;
        const dayDiv = createDayElement(day, true, year, month - 1);
        calendarGrid.appendChild(dayDiv);
    }

    for (let day = 1; day <= daysInMonth; day++) {
        const dayDiv = createDayElement(day, false, year, month);
        calendarGrid.appendChild(dayDiv);
    }

    const remainingCells = 42 - (firstDay + daysInMonth);

    for (let day = 1; day <= remainingCells; day++) {
        const dayDiv = createDayElement(day, true, year, month + 1);
        calendarGrid.appendChild(dayDiv);
    }
}

function createDayElement(day, isOtherMonth, year, month) {
    const dayDiv = document.createElement('div');
    dayDiv.className = 'calendar-day';

    if (isOtherMonth) {
        dayDiv.classList.add('other-month');
    }

    const today = new Date();

    if (
        day === today.getDate() &&
        month === today.getMonth() &&
        year === today.getFullYear() &&
        !isOtherMonth
    ) {
        dayDiv.classList.add('today');
    }

    const dayNumber = document.createElement('div');
    dayNumber.className = 'day-number';
    dayNumber.textContent = day;
    dayDiv.appendChild(dayNumber);

    const realDate = new Date(year, month, day);

    const dateStr =
        `${realDate.getFullYear()}-${String(realDate.getMonth() + 1).padStart(2, '0')}-${String(realDate.getDate()).padStart(2, '0')}`;

    const dayEvents = eventsData.filter(event => getEventDate(event) === dateStr);

    if (dayEvents.length > 0) {
        const eventsContainer = document.createElement('div');
        eventsContainer.className = 'day-events';

        dayEvents.forEach(event => {
            const eventDot = document.createElement('div');
            eventDot.className = 'event-dot';
            eventDot.textContent = getEventName(event);
            eventDot.title = getEventName(event);

            eventDot.addEventListener('click', function (e) {
                e.stopPropagation();
                openEventPopup(event);
            });

            eventsContainer.appendChild(eventDot);
        });

        dayDiv.appendChild(eventsContainer);
    }

    return dayDiv;
}

function renderEventsList() {
    const eventsList = document.getElementById('eventsList');
    eventsList.innerHTML = '';

    const sortedEvents = [...eventsData].sort((a, b) => {
        return parseLocalDate(getEventDate(a)) - parseLocalDate(getEventDate(b));
    });

    if (sortedEvents.length === 0) {
        eventsList.innerHTML = `
            <p style="text-align: center; color: #718096; padding: 40px;">
                No events this month
            </p>
        `;
        return;
    }

    const monthNames = [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];

    sortedEvents.forEach(event => {
        const eventDate = parseLocalDate(getEventDate(event));

        const eventItem = document.createElement('div');
        eventItem.className = 'event-item';

        eventItem.innerHTML = `
            <div class="event-date-badge">
                <div class="event-month">${monthNames[eventDate.getMonth()]}</div>
                <div class="event-day">${eventDate.getDate()}</div>
            </div>

            <div class="event-details">
                <div class="event-title">${escapeHtml(getEventName(event))}</div>

                <div class="event-info">
                    <div class="event-info-item">
                        <svg viewBox="0 0 24 24">
                            <path d="M8 7V3"></path>
                            <path d="M16 7V3"></path>
                            <path d="M4 11H20"></path>
                            <rect x="4" y="5" width="16" height="16" rx="2"></rect>
                        </svg>
                        ${escapeHtml(formatDate(getEventDate(event)))}
                    </div>

                    <div class="event-info-item">
                        <svg viewBox="0 0 24 24">
                            <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
                            <polyline points="14 2 14 8 20 8"></polyline>
                            <line x1="16" y1="13" x2="8" y2="13"></line>
                            <line x1="16" y1="17" x2="8" y2="17"></line>
                        </svg>
                        ${escapeHtml(getEventDetails(event))}
                    </div>
                </div>
            </div>
        `;

        eventItem.addEventListener('click', function () {
            openEventPopup(event);
        });

        eventsList.appendChild(eventItem);
    });
}

function openEventPopup(event) {
    document.getElementById("popupEventID").textContent = getEventID(event);
    document.getElementById("popupEventName").textContent = getEventName(event);
    document.getElementById("popupEventDate").textContent = formatDate(getEventDate(event));
    document.getElementById("popupEventDetails").textContent = getEventDetails(event) || "No details";

    document.getElementById("eventPopup").classList.add("active");
}

function closeEventPopup() {
    document.getElementById("eventPopup").classList.remove("active");
}

document.addEventListener("click", function (e) {
    const popup = document.getElementById("eventPopup");

    if (!popup) return;

    if (e.target === popup) {
        closeEventPopup();
    }
});

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        closeEventPopup();
    }
});

async function previousMonth() {
    currentDate.setMonth(currentDate.getMonth() - 1);
    await fetchEventsFromWS();
}

async function nextMonth() {
    currentDate.setMonth(currentDate.getMonth() + 1);
    await fetchEventsFromWS();
}

async function goToToday() {
    currentDate = new Date();
    await fetchEventsFromWS();
}

window.addEventListener('DOMContentLoaded', () => {
    connectToWebSocket();
});