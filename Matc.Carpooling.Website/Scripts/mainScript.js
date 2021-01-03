$(document).ready(function () {
    setName();
})
//Function to get user and set name
function setName() {
    $.ajax({
        url: 'https://localhost:44394/api/Users/GetUser?userId=' + sessionStorage.getItem('currentUser'),
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {           
            sessionStorage.setItem('thisUser', response);
            document.getElementById('navHelloText').innerHTML = "Hello, " + response.First_Name;
            document.getElementById('userInfoText').innerHTML = response.First_Name + " " + response.Last_Name;
            document.getElementById('userInfoText2').innerHTML = response.Email;
        },
        error: function (request) {
        }
    });
}

function LogOut() {
    sessionStorage.removeItem('tokenKey');
    window.location.href = 'https://localhost:44398/Pages/default.aspx';
}