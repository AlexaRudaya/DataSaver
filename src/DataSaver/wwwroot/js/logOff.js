﻿async function redirectToPage() {
    window.location.href = await '/Account/Login';
}

const apiUrl = 'https://localhost:7059/api/Accounts/LogOff';

const logOff = async () => {

    const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify()
    });

    if (response.ok) {
        form.reset();
        alert("You logged off");
        redirectToPage();
    }
    else {
        alert("Invalid log off attempt! Try again or contact administrator.");
    }
};