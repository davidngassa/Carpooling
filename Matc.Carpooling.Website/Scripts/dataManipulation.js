////Call API to get list of users
//function userList() {
//    $.ajax({
//        url: 'https://localhost:44321/api/Users/GetAllUsers/',
//        type: 'GET',
//        dataType: 'json',

//        success: function (users) {
//            userListSuccess(users);
//        },

//        error: function (request, message, error) {
//            handleException(request, message, error);
//        }
//    });
//}

////Process the collection of users returned when successfully retrieved data
//function userListSuccess(users) {
//    //Iterate over the collection of data
//    $.each(users, function (index, user) {
//        //Add user row
//        console.log(user);
//    });
//}

////Displays error message if request is unsucessful
//function handleException(request, message, error) {
//    var msg = "";
//    msg += "Code: " + request.status + "\n";
//    msg += "Text: " + request.statusText + "\n";

//    if (request.responseJSON != null) {
//        msg += "Message: " + request.responseJSON.Message + "\n";
//    }
//    alert(msg);
//}

////Get all users on page load
////$(document).ready(function () {
////    userList();
////})
