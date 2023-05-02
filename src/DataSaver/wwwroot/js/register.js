async function redirectToPage() {
    window.location.href = await '/Link/Index';
}

const apiUrlRegister = 'https://localhost:7059/api/Accounts/Register';

const register = async () => {

    const form = document.querySelector('form');

    const data = {
        Email: document.getElementById("Email").value,
        UserName: document.getElementById("UserName").value,
        Password: document.getElementById("Password").value
    };

    console.log('Data to be sent:', JSON.stringify(data));

    const response = await fetch(apiUrlRegister, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            form.reset();
            alert("Registration was successful!");
            redirectToPage();
        }
        else {
            alert("Registration failed! Try again or contact administrator.");
        }
};
