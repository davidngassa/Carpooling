//Function to check if user is already logged in
$(document).ready(function () {
    var token = sessionStorage.getItem('tokenKey');
    if (token != null) {
        window.location.href = '/Pages/Home.aspx';
    }
})
//Function to register a user
function Register() {
    var newUserDto = {
        Identity_Number: document.getElementsByName("userID")[0].value,
        First_Name: document.getElementsByName("firstName")[0].value,
        Last_Name: document.getElementsByName("lastName")[0].value,
        Email: document.getElementsByName("emailAddress")[0].value,
        Phone: document.getElementsByName("phoneNumber")[0].value,
        Password: document.getElementsByName("Password")[0].value
    }
    $.ajax({
        url: 'https://localhost:44394/api/Register',
        type: 'POST',
        data: newUserDto,
        success: function (data) {
            LoginFromRegister(newUserDto);
        },
        error: function (request, message, error) {
            document.getElementById("errorMessage").innerHTML = request.responseJSON.Message;
            if (request.responseJSON.Message.includes("Email")) {
                highlightBox(3);
            } else if (request.responseJSON.Message.includes("Password")) {
                highlightBox(5);
            } else if (request.responseJSON.Message.includes("First name")) {
                highlightBox(1);
            } else if (request.responseJSON.Message.includes("Last name")) {
                highlightBox(2);
            } else if (request.responseJSON.Message.includes("phone number")) {
                highlightBox(4);
            }
        }
    });
}
//Call API to check user login 
function Login() {
    var LoginDto = {
        grant_type: 'password',
        password: document.getElementById("password").value,
        username: document.getElementById("emailAddress").value
    }
    $.ajax({
        url: 'https://localhost:44394/token',
        type: 'POST',
        data: LoginDto,
        success: function (data) {
            sessionStorage.setItem('tokenKey', data.access_token);
            var userDto = {
                'email': document.getElementById("emailAddress").value, 'password': document.getElementById("password").value
            }
            $.ajax({
                url: 'https://localhost:44394/api/Users/LoginUser',
                type: 'POST',
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(userDto),
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
                },
                success: function (user) {
                    checkLoginSuccess(user);
                },
                error: function (request, message, error) {
                    handleException(request, message, error);
                }
            });
        },
        error: function (request, message, error) {
            document.getElementById("errorMessage").innerHTML = "The user name or password is incorrect";
            //handleException(request, message, error);
        }
    });
}
//Login after register
function LoginFromRegister(newUserDto) {
    var LoginDto = {
        grant_type: 'password',
        password: newUserDto.Password,
        username: newUserDto.Email
    }
    $.ajax({
        url: 'https://localhost:44394/token',
        type: 'POST',
        data: LoginDto,
        success: function (data) {
            sessionStorage.setItem('tokenKey', data.access_token);
            var userDto = {
                'email': newUserDto.Email, 'password': newUserDto.Password
            }
            $.ajax({
                url: 'https://localhost:44394/api/Users/LoginUser',
                type: 'POST',
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(userDto),
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
                },
                success: function (user) {
                    checkLoginSuccess(user);
                },
                error: function (request, message, error) {
                    handleException(request, message, error);
                }
            });
        },
        error: function (request, message, error) {
            document.getElementById("errorMessage").innerHTML = "The user name or password is incorrect";
            //handleException(request, message, error);
        }
    });
}
//Process user returned when login attempt successful
function checkLoginSuccess(user) {
    if (user == null) {
        document.location.reload();
    }
    else {
        //Store object in local storage
        sessionStorage.setItem('currentUser', user.User_ID);
        sessionStorage.setItem('thisUser', user);
        sessionStorage.setItem('myUser', JSON.stringify(user));
        window.location.href = '/Pages/Home.aspx';
    }
}
//Displays error message if request is unsucessful
function handleException(request, message, error) {
    var msg = "";
    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.statusText + "\n";
    if (request.responseJSON != null) {
        msg += "Message: " + request.responseJSON.Message + "\n";
    }
    alert(msg);
}
function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
function highlightBox(itemToHighlight) {
    var elements = [document.getElementsByName("userID")[0], document.getElementsByName("firstName")[0], document.getElementsByName("lastName")[0], document.getElementsByName("emailAddress")[0], document.getElementsByName("phoneNumber")[0], document.getElementsByName("Password")[0]];
    for (i = 0; i < 6; i++) {
        elements[i].style.color = "#a1a1a1";
        if (i == itemToHighlight) {
            elements[i].style.color = "red";
        }
    }
}