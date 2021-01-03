<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
    <script src="/Scripts/homeScript.js"></script>
    <script>
        document.getElementById("nav").style.display = "inline";
        document.getElementById("navHomeText").style.fontWeight = "bold";
    </script>
    <script src="../Scripts/UIScript.js"></script>
    <div id="homeContainer" class="tableContainer">
        <div id="journeysContainer" class="table-wrapper" >
            <div id="journeysTableHeader" class="tableHeader">
                <h3 id="journeysTitle" class="tableTitle">JOURNEYS</h3>
                <div class="search-container">
                    <form>
                        <input type="text" placeholder="Search..." name="search">
                        <button type="button" onclick="SearchJourney()"><i class="fa fa-search"></i></button>
                    </form>
                </div>
            </div>
            <div id="journeysTableContainer" class="internalTableContainer">
                <table id="journeysTable" >
                    <tr class="table-header">
                        <th>TIME</th>
                        <th>DEPARTURE</th>
                        <th>DESTINATION</th>
                        <th>AVAILABLE SEATS</th>               
                    </tr>              

                 </table>
            </div>
        </div>
        <div id="bookedJourneysContainer" class="table-wrapper">
            <div id="bookedHomeTableHeader" class="tableHeader">
                <h3 id="bookedHomeJourneysTitle" class="tableTitle">BOOKINGS</h3>
            </div>
            <div id="bookedJourneysTableContainer" class="internalTableContainer">
                <table id="bookedJourneysTable" >
                    <tr class="table-header">
                        <th>TIME</th>
                        <th>DEPARTURE</th>
                        <th>DESTINATION</th>
                        <th>AVAILABLE SEATS</th>               
                    </tr>
                                    
                 </table>
            </div>
        </div>
        <div class ="popUpBox" id ="journeyDetailsPopUp"  style="display:none;">
            <button id="journeyDetailsPopUpCloseButton" class="closeButton" onclick="closeHomePagePopUp('journeyDetailsPopUp');">X</button>
            <h2>Journey Details</h2>
            <div id="HomeBookJourneyDetailsContainer" >    
                <div id="HomeBookJourneyDetailsDiv">
                    <div id="HomeBookJourneyDetailsTitleDiv">
                        <h4 class="journeyInputFieldsTitle">Departure Time :</h4>
                        <h4 class="journeyInputFieldsTitle">Departure :</h4>
                        <h4 class="journeyInputFieldsTitle">Destination :</h4>
                        <h4 class="journeyInputFieldsTitle">Price :</h4>
                        <h4 class="journeyInputFieldsTitle">Num. Of Seats :</h4>
                    </div>
                    <div id="HomeBookJourneyDetailsValuesDiv">
                        <h4 class="journeyInputFieldsValue" id="HomeBookingDepartureTimeVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingDepartureVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingDestinationVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingPriceVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingSeatsVal"></h4>
                    </div>
                </div>
                <div>
                    <h2>Checkpoints</h2>
                    <div id="HomeBookJourneyCheckpointDetailsDiv">
                    </div>
                </div>
                <div id="BookJourneyCarDetailsDiv">
                    <div id="BookJourneyCarHeaderDiv">
                        <h2>Car</h2>
                    </div>
                    <div id="HomeBookJourneyCarDetailsTitleDiv">
                        <h4 class="journeyInputFieldsValue">Number Plate : </h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingNumPlateVal"></h4>
                    </div>
                    <div id="HomeBookJourneyCarDetailsValueDiv">
                        <div id ="HomeBookJourneyCarMakeDiv">
                            <h4 class="journeyInputFieldsValue">Car : </h4>
                        <h4 class="journeyInputFieldsValue" id="HomeBookingCarNameVal"></h4>
                        </div>
                        <div id ="HomeBookJourneyCarModelDiv">
                            <h4 class="journeyInputFieldsValue">Model : </h4>               
                            <h4 class="journeyInputFieldsValue" id="HomeBookingCarModelVal"></h4>
                        </div>
                    </div>
                </div>   
            </div>
            <button id="bookJourneyButton" class="submitButton" onclick="openBookJourneyPopUp();">Book</button>
        </div>
        <div class ="popUpBox" id ="bookedJourneyDetailsPopUp"  style="display:none;">
            <button id="bookedJourneyDetailsPopUpCloseButton" class="closeButton" onclick="closeHomePagePopUp('bookedJourneyDetailsPopUp');">X</button>
            <div id="bookingContainer">
                <div id="HomeBookedJourneyDetailsContainer" >    
                    <h2>Booked Journey Details</h2>
                    <div id="HomeBookedJourneyDetailsDiv">
                        <div id="BookedJourneyDetailsTitleDiv">
                            <h4 class="journeyInputFieldsTitle">Departure Time :</h4>
                            <h4 class="journeyInputFieldsTitle">Departure :</h4>
                            <h4 class="journeyInputFieldsTitle">Destination :</h4>
                            <h4 class="journeyInputFieldsTitle">Price :</h4>
                            <h4 class="journeyInputFieldsTitle">Num. Of Seats :</h4>
                        </div>
                        <div id="HomeBookedJourneyDetailsValuesDiv">
                            <h4 class="journeyInputFieldsValue" id="BookingDepartureTimeVal"></h4>
                            <h4 class="journeyInputFieldsValue" id="BookingDepartureVal"></h4>
                            <h4 class="journeyInputFieldsValue" id="BookingDestinationVal"></h4>
                            <h4 class="journeyInputFieldsValue" id="BookingPriceVal"></h4>
                            <h4 class="journeyInputFieldsValue" id="BookingSeatsVal"></h4>
                        </div>  
                    </div>
                    <div>
                        <h2>Checkpoints</h2>
                            <div id="BookedJourneyCheckpointDetailsDiv">
                        </div>                
                    </div>
                    <div id="BookedJourneyCarDetailsDiv">
                        <div id="BookedJourneyCarHeaderDiv">
                            <h2>Car</h2>
                        </div>
                        <div id="HomeBookedJourneyCarDetailsTitleDiv">
                            <h4 class="journeyInputFieldsValue">Number Plate : </h4>
                            <h4 class="journeyInputFieldsValue" id="BookingNumPlateVal"></h4>
                        </div>
                        <div id="HomeBookedJourneyCarDetailsValueDiv">
                            <div id ="HomeBookedJourneyCarMakeDiv">
                                <h4 class="journeyInputFieldsValue">Car : </h4>
                            <h4 class="journeyInputFieldsValue" id="BookingCarNameVal"></h4>
                            </div>
                            <div id ="HomeBookedJourneyCarModelDiv">
                                <h4 class="journeyInputFieldsValue">Model : </h4>               
                                <h4 class="journeyInputFieldsValue" id="BookingCarModelVal"></h4>
                            </div>

                        </div>
                    </div>               
                </div>
                <div id="HomeBookingDetailsContainer">
                <h3 id="bookingTitle">Booking Details</h3>
                <div id="bookingDetailsContainer">                    
                    <h4 id="bookingDeparture">Departure:</h4>
                    <h4 id="bookingDestination">Destination:</h4>
                    <h4 id="bookingPrice">Price:</h4>
                </div>
            </div>
            </div>
            <button id="cancelBookingButton" class="submitButton" onclick="openHomeConfirmationPopUp('confirmCancelBookingPopUp')">Cancel Booking</button>
        </div>
        <div class="popUpBox" id="bookJourneyPopUp" style="display:none;">
            <button id="bookJourneyPopUpCloseButton" class="closeButton" onclick="closeBookJourneyPopUp();">X</button>
            <h3>Enter Your Booking Details</h3>
            <div id="bookingInputsContainer">
                <div id="inputDepartureContainer">
                    <h4>Departure</h4>
                    <select id="selectDepartureDropBox" class="inPageInputFields" ></select>
                </div>
                <div id="inputDestinationContainer">
                    <h4>Destination</h4>
                    <select id="selectDestinationDropBox" class="inPageInputFields" ></select>
                </div>            
            </div>
            <h4 id="bookingPriceEstimate"></h4>
            <button id="continueBookJourneyButton" class="submitButton" onclick="openHomeConfirmationPopUp('confirmMakeBookingPopUp');">Book</button>
        </div>
        <div class="confimationpopUpBox" id="confirmMakeBookingPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Book <br> This Journey?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelMakeBookingButton" class="confirmButton" onclick="closeHomeConfirmationPopUp('confirmMakeBookingPopUp', 'journeyDetailsPopUp')">No</button>
                <button id="confirmMakeBookingButton" class="confirmButton" onclick="">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmCancelBookingPopUp" style="display:none;">             
            <h5 class="confirmationText">Do You Want To Cancel <br> This Booking?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelCancelBookingButton" class="confirmButton" onclick="closeHomeConfirmationPopUp('confirmCancelBookingPopUp', 'bookedJourneyDetailsPopUp')">No</button>
                <button id="confirmcancelBookingButton" class="confirmButton" onclick="">Yes</button>
            </div>
        </div>
    </div>    
   
       
</asp:Content>
