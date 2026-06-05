function viewOrder(orderId) {
    window.location.href = `${window.location.origin}/Registered/ViewOrder?orderID=${orderId}`
}


function filterOrders(status) {
    const tabs = document.querySelectorAll('.filter-tab');
    const orders = document.querySelectorAll('.order-card');

    tabs.forEach(tab => tab.classList.remove('active'));
    event.target.classList.add('active');

    orders.forEach(order => {
        if (status === 'all') {
            order.style.display = 'block';
        } else {
            if (order.dataset.status === status) {
                order.style.display = 'block';
            } else {
                order.style.display = 'none';
            }
        }
    });
}