//Function to fill tables on page load
$(document).ready(function () {
    CheckCarExists();
    var token = sessionStorage.getItem('tokenKey');
    if (token === null) {
        window.location.href = '/Pages/default.aspx';
    }
    GetUserCars();
    
    GetUserJourneys();
    AddAllBookingsToTable();
})

//////////////////
function ShowBookingDetails(bookingID) {
    for (i = 0; i < bookings.length; i++) {
        if (bookings[i].Booking_ID == bookingID) {
            var checkpointDiv = document.getElementById("BookedJourneyCheckpointDetailsDiv");
            while (checkpointDiv.hasChildNodes()) {
                checkpointDiv.removeChild(checkpointDiv.lastChild);
            }
            document.getElementById("confirmCancelBookingButton").setAttribute("onclick", "CancelBooking('" + bookingID + "')");
            //Journey
            document.getElementById("BookingDepartureTimeVal").innerHTML = "  " + bookings[i].Booking_Route.Route_Journey.Departure_Time.replace("T", " ").substring(
                0,
                bookings[i].Booking_Route.Route_Journey.Departure_Time.length - 3
            );
            document.getElementById("BookingSeatsVal").innerHTML = bookings[i].Booking_Route.Available_Seats;
            document.getElementById("bookingPrice").innerHTML = "Price: " + bookings[i].Booking_Route.Price;
            //Car
            document.getElementById("BookingNumPlateVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Car.Number_Plate;
            document.getElementById("BookingCarNameVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Car.Make;
            document.getElementById("BookingCarModelVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Car.Model;
            for (j = 0; j < bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints.length; j++) {
                if (bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Type == "departure") {
                    document.getElementById("BookingDepartureVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Location;
                }
                if (bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Type == "destination") {
                    document.getElementById("BookingDestinationVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Location;
                    document.getElementById("BookingPriceVal").innerHTML = bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Price;
                }
                if (bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Type == "stopover") {
                    var h4Checkpoint = document.createElement("h4");
                    var h4Price = document.createElement("h4");
                    h4Checkpoint.innerHTML = "Location: " + bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Location + " Price: " + bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Price;
                    checkpointDiv.appendChild(h4Checkpoint);
                }
                if (bookings[i].Booking_Route.Departure == bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Checkpoint_ID) {
                    document.getElementById("bookingDeparture").innerHTML = "Departure: " + bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Location;
                }
                if (bookings[i].Booking_Route.Destination == bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Checkpoint_ID) {
                    document.getElementById("bookingDestination").innerHTML = "Destination: " + bookings[i].Booking_Route.Route_Journey.Journey_Checkpoints[j].Location;
                }
            }
        }
    }
}
function CancelBooking(bookingId) {
    $.ajax({
        url: "https://localhost:44394/api/Bookings/CancelBooking?bookingID=" + bookingId,
        type: "DELETE",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            window.location.href = "/Pages/Home.aspx";
        },
        error: function (request) {
            alert(request.responseJSON.Message);
        }
    });
}
//Function to display all bookings
function AddAllBookingsToTable() {
    var tb = document.getElementById("bookedJourneysTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    $.ajax({
        url:
            "https://localhost:44394/api/Bookings/GetUserBookings?userID=" +
            sessionStorage.getItem("currentUser"),
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            bookings = response;
            //console.log(response);
            if (response.length == 0) {
                ShowNoBookings();
            }
            for (i = 0; i < response.length; i++) {
                var newRow = document
                    .getElementById("bookedJourneysTable")
                    .insertRow(1);
                newRow.setAttribute("id", response[i].Booking_ID);
                newRow.setAttribute("style", "cursor:pointer;");
                newRow.setAttribute(
                    "onclick",
                    "openMyJourneyPopUp('','bookedJourneyDetailsPopUp');ShowBookingDetails('" +
                    response[i].Booking_ID +
                    "')"
                );
                if (response[i].Status == "active") {
                    newRow.setAttribute("class", "activeJourney table-row");
                } else {
                    newRow.setAttribute("class", "inactiveJourney table-row");
                }
                var dateTime = newRow.insertCell(0);
                var depature = newRow.insertCell(1);
                var destination = newRow.insertCell(2);
                var numSeats = newRow.insertCell(3);
                dateTime.innerHTML = response[
                    i
                ].Booking_Route.Route_Journey.Departure_Time.replace(
                    "T",
                    " "
                ).substring(
                    0,
                    response[i].Booking_Route.Route_Journey.Departure_Time.length - 3
                );
                for (
                    j = 0;
                    j <
                    response[i].Booking_Route.Route_Journey.Journey_Checkpoints.length;
                    j++
                ) {
                    if (
                        response[i].Booking_Route.Route_Journey.Journey_Checkpoints[j]
                            .Type == "departure"
                    ) {
                        depature.innerHTML =
                            response[i].Booking_Route.Route_Journey.Journey_Checkpoints[
                                j
                            ].Location;
                    }
                    if (
                        response[i].Booking_Route.Route_Journey.Journey_Checkpoints[j]
                            .Type == "destination"
                    ) {
                        destination.innerHTML =
                            response[i].Booking_Route.Route_Journey.Journey_Checkpoints[
                                j
                            ].Location;
                    }
                }
                numSeats.innerHTML = response[i].Booking_Route.Available_Seats;
            }
        },
        error: function (request) {
            //alert(request);
        }
    });
}

//Function to show no journey
function ShowNoBookings() {
    var tb = document.getElementById('bookedJourneysTable');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var newRow = document.getElementById('bookedJourneysTable').insertRow(1);
    var newCell = newRow.insertCell();
    newCell.setAttribute("colspan", "5");
    newCell.innerHTML = "You do not have any bookings";
}


//Function to get all journeys
function GetUserJourneys() {
    $.ajax({
        url: 'https://localhost:44394/api/Journeys/GetUserJourneys?userId=' + sessionStorage.getItem('currentUser'),
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {
            var tb = document.getElementById('createdJourneysTable');
            while (tb.rows.length > 1) {
                tb.deleteRow(1);
            }
            for (i = 0; i < response.length; i++) {
                var newRow = document.getElementById('createdJourneysTable').insertRow(1);
                newRow.setAttribute("id", response[i].Journey_ID);
                newRow.setAttribute("style", "cursor:pointer;");
                newRow.setAttribute("class", "table-row");
                newRow.setAttribute("onclick", "openMyJourneyPopUp('" + response[i].Journey_ID + "','createdJourneyDetailsPopUp');");
                var dateTime = newRow.insertCell(0);
                var depature = newRow.insertCell(1);
                var destination = newRow.insertCell(2);
                var numSeats = newRow.insertCell(3);
                dateTime.innerHTML = response[i].Departure_Time;
                for (j = 0; j < response[i].Journey_Checkpoints.length; j++) {
                    if (response[i].Journey_Checkpoints[j].Type == "departure") {
                        depature.innerHTML = response[i].Journey_Checkpoints[j].Location;
                    }
                    if (response[i].Journey_Checkpoints[j].Type == "destination") {
                        destination.innerHTML = response[i].Journey_Checkpoints[j].Location;
                    }
                }
                numSeats.innerHTML = response[i].Available_Seats;
            }
        },
        error: function (request) {
            var tb = document.getElementById('createdJourneysTable');
            while (tb.rows.length > 1) {
                tb.deleteRow(1);
            }
            var newRow = document.getElementById('createdJourneysTable').insertRow(1);
            var newCell = newRow.insertCell();
            newCell.setAttribute("colspan", "5");
            newCell.innerHTML = "You have not created any journey yet";
        }
    });
}


//Function to add journey
function AddJourney() {
    var e = document.getElementById("selectCarDropBox");
    var carId = e.options[e.selectedIndex].value;

    var newJourneyDto = {
        Departure_Time: document.getElementsByName("numberPlate")[0].value,
        Available_Seats: document.getElementsByName("seatsAvailable")[0].value,
        Car_ID: carId,
        Driver_ID: sessionStorage.getItem('currentUser')
    }


    var test = $.ajax({
        url: 'https://localhost:44394/api/Journeys/AddJourney/',
        type: 'POST',
        data: newJourneyDto,
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (request) {

            
            var checkpointsDto =
                [
                    {
                        Price: 0,
                        Location: document.getElementsByName("departurePlace")[0].value,
                        Type: "departure",
                        JourneyId: test.responseJSON
                    },
                    {
                        Price: document.getElementsByName("price")[0].value,
                        Location: document.getElementsByName("destination")[0].value,
                        Type: "destination",
                        JourneyId: test.responseJSON
                    }
                ]

            // get all the child div from the field container
            var searchElem = document.getElementById("allCheckpointInputFieldsContainer").children;
            for (var i = 1; i < searchElem.length + 1; i++) {
                checkpointsDto.push({
                    Price: document.getElementById("createJourneyInputCheckpointPrice".concat(i)).value,
                    Location: document.getElementById("createJourneyInputCheckpointName".concat(i)).value,
                    Type: "stopover",
                    JourneyId: test.responseJSON
                });
            }

            AddCheckpoint(checkpointsDto);
            //window.location.href = '/Pages/My%20Journeys.aspx';
        },
        error: function (request, message, error) {
            
            document.getElementById("error").innerHTML = request.responseJSON.Message;
        }
    });
    
}

//function to add checkpoints to a journey
function AddCheckpoint(checkpointsDto) {

    for (i = 0; i < checkpointsDto.length; i++) {
        ////console.log(checkpointsDto[i]);
        $.ajax({
            url: 'https://localhost:44394/api/Checkpoints/AddCheckpoint/',
            type: 'POST',
            data: checkpointsDto[i],
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
            },
            success: function (request) {

            },
            error: function (request, message, error) {
                alert(request);
            }
        });
    }
}

//Function to get user cars
function GetUserCars() {

    $.ajax({
        url: 'https://localhost:44394/api/Cars/GetUserCars?userId=' + sessionStorage.getItem('currentUser'),
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {

            if (response.length == 0) {
                //ShowNoCar();
            }
            for (i = 0; i < response.length; i++) {

                //dropbox in update journey in create journey
                var select = document.getElementById("selectCarDropBox");
                select.options[select.options.length] = new Option(response[i].Number_Plate, response[i].Car_ID);

                //dropbox in update journey
                var select1 = document.getElementById("carSelectDropBox");
                select1.options[select1.options.length] = new Option(response[i].Number_Plate, response[i].Car_ID);

            }
        },
        error: function (request) {
            alert(request);
        }
    });
}

//Function to show specific Created Journey details on pop-up
function showCreatedJourneyDetailsOnPopUp(journeyId) {
    $.ajax({
        url: 'https://localhost:44394/api/Journeys/GetJourney?journeyId=' + journeyId,
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {
            //console.log(response);

            // #region Journey Details Popup
            // Journey details part

            document.getElementById("DepartureTimeVal").innerHTML = response.Departure_Time;
            for (var i = 0; i < response.Journey_Checkpoints.length; i++) {
                if (response.Journey_Checkpoints[i].Type == "departure") {
                    document.getElementById("DepartureVal").innerHTML = response.Journey_Checkpoints[i].Location;
                }
                else if (response.Journey_Checkpoints[i].Type == "destination") {
                    document.getElementById("DestinationVal").innerHTML = response.Journey_Checkpoints[i].Location;
                    document.getElementById("JourneyPriceVal").innerHTML = response.Journey_Checkpoints[i].Price;
                }
            }
            
            document.getElementById("SeatsVal").innerHTML = response.Available_Seats;

            // Checkpoint details part
            var checkpointDiv = document.getElementById("CheckpointNameValuesDiv");
            while (checkpointDiv.hasChildNodes()) {
                checkpointDiv.removeChild(checkpointDiv.lastChild);
            }

            for (i = 0; i < response.Journey_Checkpoints.length; i++) {
                if (response.Journey_Checkpoints[i].Type === 'stopover') {

                    var h4Checkpoint = document.createElement("h4");
                    var h4Price = document.createElement("h4");

                    h4Checkpoint.innerHTML = "Location: " + response.Journey_Checkpoints[i].Location + " Price: " + response.Journey_Checkpoints[i].Price;
                    checkpointDiv.appendChild(h4Checkpoint);

                }
            }

            // Car details part
            document.getElementById("NumPlateVal").innerHTML = response.Journey_Car.Number_Plate;
            document.getElementById("CarNameVal").innerHTML = response.Journey_Car.Make;
            document.getElementById("CarModelVal").innerHTML = response.Journey_Car.Model;


            document.getElementById("updateJourneyButton").setAttribute("onclick", "openMyJourneyPopUp('" + journeyId + "','updateJourneyPopUp');");

            // #endregion

            // #region Update Popup placeholders
            var e = document.getElementById("carSelectDropBox");
            var carId = e.options[e.selectedIndex].value;

            document.getElementById("dateSelectDropBox").setAttribute("value", response.Departure_Time);

            for (var i = 0; i < response.Journey_Checkpoints.length; i++) {
                if (response.Journey_Checkpoints[i].Type == "departure") {
                   // document.getElementById("updateJourneyInputDeparture").setAttribute("placeholder", response.Journey_Checkpoints[i].Location);
                    document.getElementById("updateJourneyInputDeparture").value = response.Journey_Checkpoints[i].Location;
                }
                else if (response.Journey_Checkpoints[i].Type == "destination") {
                    //document.getElementById("updateJourneyInputDestination").setAttribute("placeholder", response.Journey_Checkpoints[i].Location);
                    document.getElementById("updateJourneyInputDestination").value =  response.Journey_Checkpoints[i].Location;
                    document.getElementById("updateJourneyInputPrice").setAttribute("placeholder", response.Journey_Checkpoints[i].Price);
                }
            }

            document.getElementById("updateJourneyInputSeats").setAttribute("placeholder", response.Available_Seats);



            // #endregion
            localStorage.setItem('thisJourney', JSON.stringify(response));
        },
        error: function (request) {
            alert(request);
        }
    });
}

function UpdateJourney() {
    var thisJourney = JSON.parse(localStorage.getItem("thisJourney"));

    ////console.log(thisJourney.Departure_Time);
    ////console.log(document.getElementById("dateSelectDropBox").value);
    if (document.getElementById("dateSelectDropBox").value != thisJourney.Departure_Time) { // to confirm
        var journeyDto = {
            Journey_ID: thisJourney.Journey_ID,
            Parameter: "Departure_Time",
            Value: document.getElementById("dateSelectDropBox").value
        };
        //alert("error in backend!");
        //console.log(journeyDto);
        ConsumeUpdateJourney(journeyDto);
    }

    // get the destination and store it in a variable
    for (var i = 0; i < thisJourney.Journey_Checkpoints.length; i++) {
        if (thisJourney.Journey_Checkpoints[i].Type == "destination") {
            var checkpointDestination = thisJourney.Journey_Checkpoints[i];
        }
    }
    
    if (document.getElementById("updateJourneyInputPrice").getAttribute("placeholder") != document.getElementById("updateJourneyInputPrice").value
        && document.getElementById("updateJourneyInputPrice").value > 0) {
        var checkpointDto = {
            Checkpoint_ID: checkpointDestination.Checkpoint_ID,
            Journey_ID: thisJourney.Journey_ID,
            Price: document.getElementById("updateJourneyInputPrice").value
        };
        ConsumeUpdateCheckpoint(checkpointDto);
        alert("Changed successfully");
    } 

    if (document.getElementById("updateJourneyInputSeats").getAttribute("placeholder") != document.getElementById("updateJourneyInputSeats").value
        && document.getElementById("updateJourneyInputSeats").value > thisJourney.Available_Seats) {
        var journeyDto = {
            Journey_ID: thisJourney.Journey_ID,
            Parameter: "Available_Seats",
            Value: document.getElementById("updateJourneyInputSeats").value
        };
        ConsumeUpdateJourney(journeyDto);
        alert("Number of Available seats successfully updated");
    }

    if (document.getElementById("carSelectDropBox").value != thisJourney.Journey_Car.Car_ID) {
        var journeyDto = {
            Journey_ID: thisJourney.Journey_ID,
            Parameter: "Car_ID_FK",
            Value: document.getElementById("carSelectDropBox").value
        };
        ConsumeUpdateJourney(journeyDto);
        alert("Car changed successfully");
    }
}


//Function to use the UpdateJourney Endpoint
function ConsumeUpdateJourney(journeyDto) {
    $.ajax({
        url: "https://localhost:44394/api/Journeys/UpdateJourney/",
        type: "PUT",
        data: journeyDto,
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
        },
        error: function (request) {
            alert(request);
        }
    });
}

//Function to use the UpdateCheckpoint Endpoint
function ConsumeUpdateCheckpoint(checkpointDto) {
    $.ajax({
        url: "https://localhost:44394/api/Checkpoints/UpdateCheckpoint/",
        type: "PUT",
        data: checkpointDto,
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
        },
        error: function (request) {
            alert(request);
        }
    });
}


function CheckCarExists() {
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
                document.getElementById("createdMyJourneysContainer").style.display = "none";
                document.getElementById("bookedMyJourneysContainer").style.height = "50%";
            }
        },
        error: function (request) {
            
        }
    });
}