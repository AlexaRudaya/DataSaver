function copyToClipboard(url) {
    // Create a temporary input element
    const input = document.createElement('input');
    input.type = 'text';
    input.value = url;
    document.body.appendChild(input);

    // Select the input element's text
    input.select();
    input.setSelectionRange(0, 99999); // For mobile devices

    // Copy the text to the clipboard
    document.execCommand('copy');

    // Remove the temporary input element
    document.body.removeChild(input);
}