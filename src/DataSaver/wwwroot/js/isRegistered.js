const isRegisteredUrl = 'https://localhost:7059/api/Accounts/IsRegistered'; 

const isRegistered = async () => {

    console.log('Making a request to', isRegisteredUrl);

    const response = await fetch(isRegisteredUrl, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    });

    if (response.ok) {
        const registered = await response.json();

        if (registered === 'User exists') {
            document.getElementById('register').style.display = 'none';
        } else {
            document.getElementById('login').style.display = 'none';
        }
    } else {
        console.log('Error checking registration status:', response.statusText);
    }
};

await isRegistered();