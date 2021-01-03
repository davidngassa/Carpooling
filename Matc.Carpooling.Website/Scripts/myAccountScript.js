$(document).ready(function () {
    var token = sessionStorage.getItem("tokenKey");
    if (token === null) {
        window.location.href = "/Pages/default.aspx";
    }
    setAccountInfo();
})
//Function to get user and set name
function setAccountInfo() {
    $.ajax({
        url: 'https://localhost:44394/api/Users/GetUser?userId=' + sessionStorage.getItem('currentUser'),
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {
            localStorage.setItem('myUser', JSON.stringify(response));
            document.getElementById('userIDNumVal').innerHTML = response.Identity_Number;
            document.getElementById('userFirstNameVal').innerHTML = response.First_Name;
            document.getElementById('userLastNameVal').innerHTML = response.Last_Name;
            document.getElementById('userEmailVal').innerHTML = response.Email;
            document.getElementById('userPhoneVal').innerHTML = response.Phone;
            document.getElementsByName("firstName")[0].value = response.First_Name;
            document.getElementsByName("lastName")[0].value = response.Last_Name;
            document.getElementsByName("emailAddress")[0].value = response.Email;
            document.getElementsByName("phoneNumber")[0].value = response.Phone;
        },
        error: function (request) {
        }
    });
}

function checkPassword() {
    if (document.getElementsByName("Password")[0].value != document.getElementsByName("confirmPassword")[0].value) {
        document.getElementsByName("Password")[0].style.color = "red";
        document.getElementsByName("confirmPassword")[0].style.color = "red";
        document.getElementById("error").innerHTML = "Password must match";
    } else {
        var user = JSON.parse(localStorage.getItem('myUser'));
        var userDto = {
            User_ID: user.User_ID,
            Parameter: "Password",
            Value: document.getElementsByName("Password")[0].value
        }

        UpdateUser(userDto);
        alert("User Password Updated");
        document.getElementsByName("Password")[0].value = '';
        document.getElementsByName("confirmPassword")[0].value = '';
    }
}
function UpdateUserDetails() {
    var user = JSON.parse(localStorage.getItem('myUser'));
    if (document.getElementsByName("firstName")[0].value != user.First_Name) {
        var userDto = {
            User_ID: user.User_ID,
            Parameter: "First_Name",
            Value: document.getElementsByName("firstName")[0].value
        }
        UpdateUser(userDto);
    }
    if (document.getElementsByName("lastName")[0].value != user.Last_Name) {
        var userDto = {
            User_ID: user.User_ID,
            Parameter: "Last_Name",
            Value: document.getElementsByName("lastName")[0].value
        }
        UpdateUser(userDto);
    }
    if (document.getElementsByName("emailAddress")[0].value != user.Email) {
        var userDto = {
            User_ID: user.User_ID,
            Parameter: "Email",
            Value: document.getElementsByName("emailAddress")[0].value
        }
        UpdateUser(userDto);
    }
    if (document.getElementsByName("phoneNumber")[0].value != user.Phone) {
        var userDto = {
            User_ID: user.User_ID,
            Parameter: "Phone",
            Value: document.getElementsByName("phoneNumber")[0].value
        }
        UpdateUser(userDto);

    }
    alert("User Details Updated");
    setName();
}
function UpdateUser(userDto) {
    $.ajax({
        url: 'https://localhost:44394/api/Users/UpdateUser',
        type: 'PUT',
        data: userDto,
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {
            setAccountInfo();
        },
        error: function (request) {
        }
    });
}