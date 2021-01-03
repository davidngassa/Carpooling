// H O M E S C R I P T . J S
// F O R
// H O M E P A G E
var journey;
var routes;
var bookings;
//Function to fill tables on page load
$(document).ready(function () {
    var token = sessionStorage.getItem("tokenKey");
    if (token === null) {
        window.location.href = "/Pages/default.aspx";
    }
    GetAllJourneys();
    AddAllBookingsToTable();
    document.getElementById("confirmMakeBookingButton").setAttribute("onclick", "BookJourney()");
});
//Function to get all journeys
function GetAllJourneys() {
    $.ajax({
        url: "https://localhost:44394/api/Journeys/GetAllJourneys",
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            if (response.length == 0) {
                ShowNoJourney();
            } else {
                AddAllJourneysToTable(response);
            }
        },
        error: function (request) {
            ShowNoJourney();
        }
    });
}
//Function to search journey
function SearchJourney() {
    var locationToSearch = document.getElementsByName("search")[0].value;
    if (locationToSearch === "") {
        GetAllJourneys();
    } else {
        if (!locationToSearch.replace(/\s/g, "").length) {
            GetAllJourneys();
        } else {
            $.ajax({
                url:
                    "https://localhost:44394/api/Journeys/SearchJourney?destination=" +
                    locationToSearch,
                type: "GET",
                headers: {
                    Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
                },
                success: function (response) {
                    AddAllJourneysToTable(response);
                },
                error: function (request) {
                    ShowNoJourney();
                }
            });
        }
    }
}
//Function to display all journeys
function AddAllJourneysToTable(response) {
    console.log(response);
    var tb = document.getElementById("journeysTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    for (i = 0; i < response.length; i++) {
        if (response[i].Journey_Car.CarOwner.User_ID != sessionStorage.getItem("currentUser")) {
            var newRow = document.getElementById("journeysTable").insertRow(1);
            newRow.setAttribute("id", response[i].Journey_ID);
            newRow.setAttribute("style", "cursor:pointer;");
            newRow.setAttribute("class", "table-row");
            newRow.setAttribute(
                "onclick",
                "openHomePagePopUp('journeyDetailsPopUp');ShowGeneralJourneyDetailsOnPopUp('" +
                response[i].Journey_ID +
                "')"
            );
            var dateTime = newRow.insertCell(0);
            var depature = newRow.insertCell(1);
            var destination = newRow.insertCell(2);
            var numSeats = newRow.insertCell(3);
            dateTime.innerHTML = response[i].Departure_Time.replace("T", " ").substring(
                0,
                response[i].Departure_Time.length - 3
            );
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
        else {
            ShowNoJourney();
        }
    }
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
                    "openHomePagePopUp('bookedJourneyDetailsPopUp');ShowBookingDetails('" +
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
function ShowNoJourney() {
    var tb = document.getElementById("journeysTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var newRow = document.getElementById("journeysTable").insertRow(1);
    var newCell = newRow.insertCell();
    newCell.setAttribute("colspan", "5");
    newCell.innerHTML = "No Journey Available";
}
//Function to show no booking
function ShowNoBookings() {
    var tb = document.getElementById("bookedJourneysTable");
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var newRow = document.getElementById("bookedJourneysTable").insertRow(1);
    var newCell = newRow.insertCell();
    newCell.setAttribute("colspan", "5");
    newCell.innerHTML = "You do not have any bookings";
}
//Function to add journey
function AddJourney() {
    var newJourneyDto = {
        Departure_Time: document.getElementsByName("departureDate")[0].value,
        Available_Seats: document.getElementsByName("seatsAvailable")[0].value,
        Status: "Active"
        //Year: document.getElementsByName("year")[0].value,
        //Number_Of_Seats: document.getElementsByName("numOfSeats")[0].value,
        //Owner_ID: sessionStorage.getItem('currentUser')
    };
}
function ShowGeneralJourneyDetailsOnPopUp(journeyID) {
    var selectDepartureDropBox = document.getElementById(
        "selectDepartureDropBox"
    );
    var selectDestinationDropBox = document.getElementById(
        "selectDestinationDropBox"
    );
    selectDepartureDropBox.setAttribute(
        "onchange",
        "SetDestinationOptions();UpdatePriceAndSeats"
    );
    selectDepartureDropBox.options.length = 0;
    selectDestinationDropBox.setAttribute("onchange", "UpdatePriceAndSeats()");
    selectDestinationDropBox.options.length = 0;
    $.ajax({
        url:
            "https://localhost:44394/api/Routes/GetJourneyRoutes?journeyID=" +
            journeyID,
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            //console.log(response);
            routes = response;
        },
        error: function (request) {
            alert(request);
        }
    });
    $.ajax({
        url:
            "https://localhost:44394/api/Journeys/GetJourney?journeyId=" + journeyID,
        type: "GET",
        headers: {
            Authorization: "Bearer " + sessionStorage.getItem("tokenKey")
        },
        success: function (response) {
            journey = response;
            //Journey
            document.getElementById(
                "HomeBookingDepartureTimeVal"
            ).innerHTML = response.Departure_Time.replace("T", " ").substring(
                0,
                response.Departure_Time.length - 3
            );
            document.getElementById("HomeBookingSeatsVal").innerHTML =
                response.Available_Seats;
            //Checkpoints
            var checkpointDiv = document.getElementById(
                "HomeBookJourneyCheckpointDetailsDiv"
            );
            while (checkpointDiv.hasChildNodes()) {
                checkpointDiv.removeChild(checkpointDiv.lastChild);
            }
            for (i = 0; i < response.Journey_Checkpoints.length; i++) {
                if (response.Journey_Checkpoints[i].Type === "departure") {
                    document.getElementById("HomeBookingDepartureVal").innerHTML =
                        response.Journey_Checkpoints[i].Location;
                    var option = new Option(response.Journey_Checkpoints[i].Location);
                    option.setAttribute(
                        "value",
                        response.Journey_Checkpoints[i].Checkpoint_ID
                    );
                    selectDepartureDropBox.options[
                        selectDepartureDropBox.options.length
                    ] = option;
                }
                if (response.Journey_Checkpoints[i].Type === "destination") {
                    document.getElementById("HomeBookingDestinationVal").innerHTML =
                        response.Journey_Checkpoints[i].Location;
                    document.getElementById("HomeBookingPriceVal").innerHTML =
                        response.Journey_Checkpoints[i].Price;
                    selectDestinationDropBox.options[
                        selectDestinationDropBox.options.length
                    ] = new Option(response.Journey_Checkpoints[i].Location);
                }
                if (response.Journey_Checkpoints[i].Type === "stopover") {
                    var h4Checkpoint = document.createElement("h4");
                    var h4Price = document.createElement("h4");
                    h4Checkpoint.innerHTML =
                        "Location: " +
                        response.Journey_Checkpoints[i].Location +
                        " Price: " +
                        response.Journey_Checkpoints[i].Price;
                    checkpointDiv.appendChild(h4Checkpoint);
                    var option = new Option(response.Journey_Checkpoints[i].Location);
                    option.setAttribute(
                        "value",
                        response.Journey_Checkpoints[i].Checkpoint_ID
                    );
                    selectDepartureDropBox.options[
                        selectDepartureDropBox.options.length
                    ] = option;
                    selectDestinationDropBox.options[
                        selectDestinationDropBox.options.length
                    ] = new Option(response.Journey_Checkpoints[i].Location);
                }
            }
            //Car
            document.getElementById("HomeBookingNumPlateVal").innerHTML =
                response.Journey_Car.Number_Plate;
            document.getElementById("HomeBookingCarNameVal").innerHTML =
                response.Journey_Car.Make;
            document.getElementById("HomeBookingCarModelVal").innerHTML =
                response.Journey_Car.Model;

        },
        error: function (request) {
            alert(request);
        }
    });
}
function SetDestinationOptions() {
    var selectDestinationDropBox = document.getElementById(
        "selectDestinationDropBox"
    );
    selectDestinationDropBox.options.length = 0;
    var e = document.getElementById("selectDepartureDropBox");
    var checkpointID = e.options[e.selectedIndex].value;
    for (i = 0; i < routes.length; i++) {
        if (routes[i].Departure == checkpointID) {
            for (j = 0; j < journey.Journey_Checkpoints.length; j++) {
                if (
                    routes[i].Destination == journey.Journey_Checkpoints[j].Checkpoint_ID
                ) {
                    var option = new Option(journey.Journey_Checkpoints[j].Location);
                    option.setAttribute("value", routes[i].Route_ID);
                    selectDestinationDropBox.options[
                        selectDestinationDropBox.options.length
                    ] = option;
                    document.getElementById("bookingPriceEstimate").innerHTML =
                        "Price: " +
                        routes[i].Price +
                        " | Seats available: " +
                        routes[i].Available_Seats;
                }
            }
        }
    }
    var f = document.getElementById("selectDestinationDropBox");
    f.options[f.options.length - 1].selected = true;
    UpdatePriceAndSeats();
}
function BookJourney() {
    var e = document.getElementById("selectDestinationDropBox");
    var routeID = e.options[e.selectedIndex].value;
    var journeyID;
    var user = JSON.parse(sessionStorage.getItem("myUser"));
    for (i = 0; i < routes.length; i++) {
        if (routes[i].Route_ID == routeID) {
            journeyID = routes[i].Route_Journey.Journey_ID;
        }
    }
    var bookingDto = {
        routeID: routeID,
        userID: user.User_ID
    };
    $.ajax({
        url: "https://localhost:44394/api/Bookings/BookJourneyRoute",
        type: "POST",
        data: bookingDto,
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
function UpdatePriceAndSeats() {
    var e = document.getElementById("selectDestinationDropBox");
    var routeID = e.options[e.selectedIndex].value;
    for (i = 0; i < routes.length; i++) {
        if (routeID == routes[i].Route_ID) {
            document.getElementById("bookingPriceEstimate").innerHTML =
                "Price: " +
                routes[i].Price +
                " | Seats available: " +
                routes[i].Available_Seats;
        }
    }
}
function ShowBookingDetails(bookingID) {
    for (i = 0; i < bookings.length; i++) {
        if (bookings[i].Booking_ID == bookingID) {
            var checkpointDiv = document.getElementById("BookedJourneyCheckpointDetailsDiv");
            while (checkpointDiv.hasChildNodes()) {
                checkpointDiv.removeChild(checkpointDiv.lastChild);
            }
            document.getElementById("confirmcancelBookingButton").setAttribute("onclick", "CancelBooking('" + bookingID + "')");
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