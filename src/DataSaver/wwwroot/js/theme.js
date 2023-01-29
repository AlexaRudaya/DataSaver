let page = document.querySelector('.page'); // style search
let bi = document.querySelector('.bi');
let themeButton = document.querySelector('.theme');

themeButton.onclick = function () {       // class change into opposite
    page.classList.toggle('light-theme');
    page.classList.toggle('dark-theme');
    bi.classList.toggle('bi-sun');
    bi.classList.toggle('bi-moon-stars');
};