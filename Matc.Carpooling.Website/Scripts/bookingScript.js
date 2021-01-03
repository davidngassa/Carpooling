//function to show specific booking details on pop up
function showUserBookingDetailsOnPopUp(userID) {
    $.ajax({
        url: 'https://localhost:44394/api/Bookings/GetAllUserBookings?userID=' + userID,
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('tokenKey')
        },
        success: function (response) {
            //console.log(response);
            //Journey
            document.getElementById("BookingDepartureTimeVal").innerHTML = (response.Departure_Time.replace('T', ' ')).substring(0, response.Departure_Time.length - 3);
            document.getElementById("BookingSeatsVal").innerHTML = response.Available_Seats;
            //checkpoint
            var checkpointDiv = document.getElementById("BookedJourneyCheckpointDetailsDiv");
            while (checkpointDiv.hasChildNodes()) {
                checkpointDiv.removeChild(checkpointDiv.lastChild);
            }
            var header = document.createElement("h2");
            header.innerHTML = "Checkpoint";
            checkpointDiv.appendChild(header);
            for (i = 0; i < response.Journey_Checkpoints.length; i++) {
                if (response.Journey_Checkpoints[i].Type === 'Departure') {
                    document.getElementById("BookingDepartureVal").innerHTML = response.Journey_Checkpoints[i].Location;
                }
                if (response.Journey_Checkpoints[i].Type === 'Destination') {
                    document.getElementById("BookingDestinationVal").innerHTML = response.Journey_Checkpoints[i].Location;
                    document.getElementById("BookingPriceVal").innerHTML = response.Journey_Checkpoints[i].Price;
                }
                if (response.Journey_Checkpoints[i].Type === 'Checkpoint') {
                    var h4Checkpoint = document.createElement("h4");
                    var h4Price = document.createElement("h4");
                    h4Checkpoint.innerHTML = "Location: " + response.Journey_Checkpoints[i].Location + " Price: " + response.Journey_Checkpoints[i].Price;
                    checkpointDiv.appendChild(h4Checkpoint);
                }
            }
            //Car
            document.getElementById("BookingNumPlateVal").innerHTML = response.Journey_Car.Number_Plate;
            document.getElementById("BookingCarNameVal").innerHTML = response.Journey_Car.Make;
            document.getElementById("BookingCarModelVal").innerHTML = response.Journey_Car.Model;
        },
        error: function (request) {
            alert(request);
        }
    });
}