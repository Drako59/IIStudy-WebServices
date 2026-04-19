let currentPaymentMethod = 'card';

function selectPaymentMethod(element, method) {
    // Remove active class from all payment methods
    document.querySelectorAll('.payment-method').forEach(pm => {
        pm.classList.remove('active');
    });

    // Add active class to selected method
    element.classList.add('active');

    // Hide all payment forms
    document.querySelectorAll('.payment-form').forEach(form => {
        form.classList.remove('active');
    });

    // Show appropriate form based on selection
    currentPaymentMethod = method;
    if (method === 'card') {
        document.getElementById('cardForm').classList.add('active');
    } else if (method === 'paypal') {
        document.getElementById('paypalForm').classList.add('active');
    } else if (method === 'bank') {
        document.getElementById('bankForm').classList.add('active');
    }
}

function processPayment() {
    const termsChecked = document.getElementById('terms').checked;

    if (!termsChecked) {
        alert('Please accept the Terms & Conditions to proceed.');
        return;
    }

    let paymentMethodText = '';
    switch (currentPaymentMethod) {
        case 'card':
            paymentMethodText = 'Credit Card';
            break;
        case 'paypal':
            paymentMethodText = 'PayPal';
            break;
        case 'bank':
            paymentMethodText = 'Bank Transfer';
            break;
    }

    // Simulate payment processing
    alert(`Processing payment via ${paymentMethodText}...\n\nOrder ID: #ORD-2024-1312\nTotal: $184.30\n\nThank you for your purchase!`);
}

// Format card number input
const cardNumberInput = document.querySelector('#cardForm input[placeholder="1234 5678 9012 3456"]');
if (cardNumberInput) {
    cardNumberInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\s/g, '');
        let formattedValue = value.match(/.{1,4}/g)?.join(' ') || value;
        e.target.value = formattedValue;
    });
}

// Format expiry date input
const expiryInput = document.querySelector('#cardForm input[placeholder="MM/YY"]');
if (expiryInput) {
    expiryInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');
        if (value.length >= 2) {
            value = value.slice(0, 2) + '/' + value.slice(2, 4);
        }
        e.target.value = value;
    });
}