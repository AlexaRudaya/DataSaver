async function redirectToPage() {
    window.location.href = await '/Link/Index';
}

const apiUrlLogin = 'https://localhost:7059/api/Accounts/Login';

const login = async () => {

    const form = document.querySelector('form');

    const data = {
        UserName: document.getElementById("UserName").value,
        Password: document.getElementById("Password").value
    };

    const response = await fetch(apiUrlLogin, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    if (response.ok) {
        form.reset();
        alert("Login was successful!");
        redirectToPage();
    }

    else {
        console.log('Status code:', response.status);
        alert("Invalid login attempt! Try again or contact administrator.");
    }
};