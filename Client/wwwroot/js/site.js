// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const getUserData = () => {
    const userData = localStorage.getItem('user-data');
    return userData ? JSON.parse(userData) : null;
}

const redirectToHomeIfLoggedIn = () => {
    const userData = getUserData();
    const currentPage = window.location.pathname;
    const loginPages = ['/authen/login', '/authen/register', '/authen/forgotpassword'];

    if (userData && loginPages.includes(currentPage)) {
        window.location.href = '/';
    }
}

const redirectToHomeIfNotAuth = () => {
    const userData = getUserData();
    const currentPage = window.location.pathname;
    const authPage = ['/blog/create', '/blog/edit']
    const adminPage = ['/blog/manage']

    if (!userData && (authPage.includes(currentPage) || adminPage.includes(currentPage))) {
        window.location.href = '/';
        return;
    }

    if (userData && !userData.role == "Admin" && adminPage.includes(currentPage)) {
        window.location.href = '/';
    }
}

const showToast = (message="") => {
    const toastEle = document.querySelector(".toast");
    const toastBody = document.querySelector(".toast-body");

    toastEle.classList.add('show');
    toastBody.innerHTML = message;

    setTimeout(() => {
        toastEle.classList.remove('show');
        toastBody.innerHTML = '';
    }, 2000)
}
