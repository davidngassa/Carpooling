// C A R S C R I P T . J S
// F O R
// M Y  C A R S  P A G E
//Function to fill tables on page load
$(document).ready(function () {
    var token = sessionStorage.getItem("tokenKey");
    if (token === null) {
        window.location.href = "/Pages/default.aspx";
    }
    AddAllCarsToTable();
});

var carUpdated = 0;
//Function to add a car
function AddCar() {
    var newCarDto = {
        Number_Plate: document.getElementsByName("numberPlate")[0].value,
        Make: document.getElementsByName("make")[0].value,
        Model: document.getElementsByName("model")[0].value,
        Year: document.getElementsByName("year")[0].value,
        Number_Of_Seats: document.getElementsByName("numOfSeats")[0].value,
        Owner_ID: sessionStorage.getItem("currentUser")
    };
    $.ajax({
        url: "https://localhost:44394/api/Cars/AddCar",
        type: "POST",
        data: newCarDto,
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (request) {
            //alert("Car registered successfully!");
           
            closeCarPopUp('addCarPopUp');
        },
        error: function (request, message, error) {
            document.getElementById("errorMessage").innerHTML =
                request.responseJSON.Message;
            if (request.responseJSON.Message.includes("Number plate")) {
                highlightBox(0);
            } else if (request.responseJSON.Message.includes("make")) {
                highlightBox(1);
            } else if (request.responseJSON.Message.includes("model")) {
                highlightBox(2);
            } else if (request.responseJSON.Message.includes("valid year")) {
                highlightBox(3);
            } else if (request.responseJSON.Message.includes("seats")) {
                highlightBox(4);
            }
        }
    });
}
//Function to highlight input with error
function highlightBox(itemToHighlight) {
    var elements = [
        document.getElementsByName("numberPlate")[0],
        document.getElementsByName("make")[0],
        document.getElementsByName("model")[0],
        document.getElementsByName("year")[0],
        document.getElementsByName("numOfSeats")[0]
    ];
    for (i = 0; i < elements.length; i++) {
        elements[i].style.color = "#a1a1a1";
        if (i == itemToHighlight) {
            elements[i].style.color = "red";
        }
    }
}
//Function to display all user cars
function AddAllCarsToTable() {
    var tb = document.getElementById("myCarTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    $.ajax({
        url:
            "https://localhost:44394/api/Cars/GetUserCars?userId=" +
            sessionStorage.getItem("currentUser"),
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            if (response.length == 0) {
                ShowNoCar();
            }
            for (i = 0; i < response.length; i++) {
                var newRow = document.getElementById("myCarTable").insertRow(1);
                newRow.setAttribute("id", response[i].Car_ID);
                newRow.setAttribute(
                    "onclick",
                    "openCarPopUp('" + response[i].Car_ID + "','viewCarPopUp');"
                );
                newRow.setAttribute("class", "table-row");
                newRow.setAttribute("style", "cursor:pointer;");
                var numberPlate = newRow.insertCell(0);
                var make = newRow.insertCell(1);
                var model = newRow.insertCell(2);
                var year = newRow.insertCell(3);
                var numSeats = newRow.insertCell(4);
                numberPlate.innerHTML = response[i].Number_Plate;
                make.innerHTML = response[i].Make;
                model.innerHTML = response[i].Model;
                year.innerHTML = response[i].Year;
                numSeats.innerHTML = response[i].Number_Of_Seats;
            }
        },
        error: function (request) {
            alert(request);
        }
    });
}
//Function to show no car
function ShowNoCar() {
    var tb = document.getElementById("myCarTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var newRow = document.getElementById("myCarTable").insertRow(1);
    var newCell = newRow.insertCell();
    newCell.setAttribute("colspan", "5");
    newCell.innerHTML = "You do not have any cars registered";
}
//Function to show specific car details on pop-up
function showCarDetailsOnPopUp(carID) {
    $.ajax({
        url: "https://localhost:44394/api/Cars/GetCar?carId=" + carID,
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            //console.log(response);
            document.getElementById("carNumPlateInfo").innerHTML =
                response.Number_Plate;
            document.getElementById("carMakeInfo").innerHTML = response.Make;
            document.getElementById("carModelInfo").innerHTML = response.Model;
            document.getElementById("carYearInfo").innerHTML = response.Year;
            document.getElementById("carSeatsInfo").innerHTML =
                response.Number_Of_Seats;
            document.getElementsByName("numberPlateInput")[0].value =
                response.Number_Plate;
            document.getElementsByName("makeInput")[0].value = response.Make;
            document.getElementsByName("modelInput")[0].value = response.Model;
            document.getElementsByName("yearInput")[0].value = response.Year;
            document.getElementsByName("numOfSeatsInput")[0].value =
                response.Number_Of_Seats;
            document
                .getElementById("confirmUpdateCarButton")
                .setAttribute("onclick", "openCarConfirmationPopUp('confirmUpdateCarPopUp');");
            localStorage.setItem("thisCar", JSON.stringify(response));
        },
        error: function (request) {
            alert(request);
        }
    });
}
//Function to update car details
function updateCar() {
    var thisCar = JSON.parse(localStorage.getItem("thisCar"));
    if (
        document.getElementsByName("numberPlateInput")[0].value !=
        thisCar.Number_Plate
    ) {
        var carDto = {
            Car_ID: thisCar.Car_ID,
            Parameter: "Number_Plate",
            value: document.getElementsByName("numberPlateInput")[0].value
        };
        ConsumeUpdateCar(carDto);
    }
    if (document.getElementsByName("makeInput")[0].value != thisCar.Make) {
        var carDto = {
            Car_ID: thisCar.Car_ID,
            Parameter: "Make",
            value: document.getElementsByName("makeInput")[0].value
        };
        ConsumeUpdateCar(carDto);
    }
    if (document.getElementsByName("modelInput")[0].value != thisCar.Model) {
        var carDto = {
            Car_ID: thisCar.Car_ID,
            Parameter: "Model",
            value: document.getElementsByName("modelInput")[0].value
        };
        ConsumeUpdateCar(carDto);
    }
    if (document.getElementsByName("yearInput")[0].value != thisCar.Year) {
        var carDto = {
            Car_ID: thisCar.Car_ID,
            Parameter: "Year",
            value: document.getElementsByName("yearInput")[0].value
        };
        ConsumeUpdateCar(carDto);
    }
    if (
        document.getElementsByName("numOfSeatsInput")[0].value !=
        thisCar.Number_Of_Seats
    ) {
        var carDto = {
            Car_ID: thisCar.Car_ID,
            Parameter: "Number_Of_Seats",
            value: document.getElementsByName("numOfSeatsInput")[0].value
        };
        ConsumeUpdateCar(carDto);
    }
    
    //closeCarPopUp('viewCarPopUp'); carUpdateInputsHide();
    showCarDetailsOnPopUp(thisCar.Car_ID);
    if (carUpdated == 1) {
        alert("Car updated");
    }
    window.setTimeout(function () { window.location.href = '/Pages/Mycars.aspx'; }, 1000);
    
    
    
}
function ConsumeUpdateCar(carDto) {
    $.ajax({
        url: "https://localhost:44394/api/Cars/UpdateCar",
        type: "PUT",
        data: carDto,
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) { },
        error: function (request) {
            alert(request.responseJSON.Message);
            carUpdated = 1;
        }
    });
}
function DeleteCar() {
    var thisCar = JSON.parse(localStorage.getItem("thisCar"));
    $.ajax({
        url: "https://localhost:44394/api/Cars/DeleteCar?carId=" + thisCar.Car_ID,
        type: "DELETE",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            closeCarPopUp("viewCarPopUp");
            carUpdateInputsHide();
            window.location.href = "/Pages/MyCars.aspx";
        },
        error: function (request) {
            alert(request);
        }
    });
}