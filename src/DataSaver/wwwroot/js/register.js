const apiUrl = 'https://localhost:7059/api/Accounts/Register';

console.log('register function called')

const register = async () => {
    console.log('register() called');

    const form = document.querySelector('form');
    console.log(form);

    const data = {
        Email: document.getElementById("Email").value,
        UserName: document.getElementById("UserName").value,
        Password: document.getElementById("Password").value
    };

    console.log(data);

    console.log('Data to be sent:', JSON.stringify(data));

        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            form.reset();
            alert("Registration was successful!");
        }
        else {
            alert("Registration failed! Try again or contact admin.");
        }
};

document.querySelector('.btn-primary').addEventListener('click', async () => {
    await register();
});
