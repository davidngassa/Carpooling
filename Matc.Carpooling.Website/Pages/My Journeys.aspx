<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="My Journeys.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.My_Journeys" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
    <script src="/Scripts/journeyScript.js"></script>
    <script>
        document.getElementById("nav").style.display = "inline";
        document.getElementById("navMyJourneysText").style.fontWeight = "bold";
    </script>
    <script src="../Scripts/UIScript.js"></script>
    <script src="../Scripts/bookingScript.js"></script>
    <div id="homeContainer" class="tableContainer">
        <div id="bookedMyJourneysContainer" class="table-wrapper">
            <div id="bookedTableHeader" class="tableHeader">
                <h3 id="bookedJourneysTitle" class="tableTitle">BOOKINGS</h3>
            </div>
             <div id="bookedJourneysTableContainer" class="internalTableContainer">
                <table id="bookedJourneysTable" >
                    <tr class="table-header">
                        <th>TIME</th>
                        <th>DEPARTURE</th>
                        <th>DESTINATION</th>
                        <th>AVAILABLE SEATS</th>               
                    </tr>
                    <tr class="activeJourney table-row" onclick="openMyJourneyPopUp('','bookedJourneyDetailsPopUp')"style="cursor:pointer">
                        <td>2019.10.03 19:00</td>
                        <td>Quatre Bornes</td>
                        <td>Ebene</td>
                        <td>4</td>                
                    </tr>  
                 </table>
            </div>
        </div>
        <div id="createdMyJourneysContainer">
            <div id="createdMyJourneysContainerTable" class="table-wrapper">
                 <div id="createdJourneysTableHeader" class="tableHeader">
                    <h3 id="createdJourneysTitle" class="tableTitle">MY JOURNEYS</h3>
                    <button id="createJourneyButton" onclick="openMyJourneyPopUp('','createJourneyPopUp')" class="submitButton">Create Journey</button>
                </div>
                 <div id="createdJourneysTableContainer" class="internalTableContainer">
                <table id="createdJourneysTable" >
                    <tr class="table-header">
                        <th>TIME</th>
                        <th>DEPARTURE</th>
                        <th>DESTINATION</th>
                        <th>AVAILABLE SEATS</th>               
                    </tr>
                    <tr class="activeJourney table-row" onclick="openMyJourneyPopUp('','createdJourneyDetailsPopUp')"style="cursor:pointer">
                        <td>2019.10.03 19:00</td>
                        <td>Quatre Bornes</td>
                        <td>Ebene</td>
                        <td>4</td>                
                    </tr>                          
                 </table>
            </div>
            </div>            
            
        </div>
        <div class ="popUpBox" id ="bookedJourneyDetailsPopUp"  style="display:none;">
            <button id="bookedJourneyDetailsPopUpCloseButton" class="closeButton" onclick="closeMyJourneyPopUp('bookedJourneyDetailsPopUp');">X</button>
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
            <button id="cancelBookingButton" class="submitButton" onclick="openMyJourneysConfirmationPopUp('confirmCancelBookingPopUp')">Cancel Booking</button>
        </div>
        <div class ="popUpBox" id ="createdJourneyDetailsPopUp" style="display:none;"  >
            <button id="createdJourneyDetailsPopUpCloseButton" class="closeButton" onclick="closeMyJourneyPopUp('createdJourneyDetailsPopUp');">X</button>
            <h2>Journey Details</h2>
            <div id="createdJourneyDetailsContainer" >    
                <div id="createdJourneyDetailsDiv">
                    <div id="createdJourneyDetailsTitleDiv">
                        <h4 class="journeyInputFieldsTitle">Departure Time :</h4>
                        <h4 class="journeyInputFieldsTitle">Departure :</h4>
                        <h4 class="journeyInputFieldsTitle">Destination :</h4>
                        <h4 class="journeyInputFieldsTitle">Price :</h4>
                        <h4 class="journeyInputFieldsTitle">Num. Of Seats :</h4>
                    </div>
                    <div id="createdJourneyDetailsValuesDiv">
                        <h4 class="journeyInputFieldsValue" id="DepartureTimeVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="DepartureVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="DestinationVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="JourneyPriceVal"></h4>
                        <h4 class="journeyInputFieldsValue" id="SeatsVal"></h4>
                    </div>
                </div>
                
                <div id="CheckpointDetailsDiv">
                    <div id="CheckpointHeaderDiv">
                        <h2>Checkpoints</h2>
                    </div>
                    <div id="CheckpointNameValuesDiv">
                        <div id="CheckpointNameDetailsDiv">
                        <h4 class="journeyInputFieldsValue">Name : </h4>
                        <h4 class="journeyInputFieldsValue" id="CheckpointNameVal"></h4>
                    </div>
                    <div id="CheckpointValueDetailsDiv">
                        <h4 class="journeyInputFieldsValue">Price : </h4>
                        <h4 class="journeyInputFieldsValue" id="CheckpointPriceVal"></h4>
                    </div>
                    </div>
                </div>
                <div id="CarDetailsDiv">
                    <div id="CarHeaderDiv">
                        <h2>Car</h2>
                    </div>
                    <div id="CarDetailsTitleDiv">
                        <h4 class="journeyInputFieldsValue">Number Plate : </h4>
                        <h4 class="journeyInputFieldsValue" id="NumPlateVal"></h4>
                    </div>
                    <div id="CarDetailsValueDiv">
                        <div id ="CarMakeDiv">
                            <h4 class="journeyInputFieldsValue">Car : </h4>
                        <h4 class="journeyInputFieldsValue" id="CarNameVal"></h4>
                        </div>
                        <div id ="CarModelDiv">
                            <h4 class="journeyInputFieldsValue">Model : </h4>               
                            <h4 class="journeyInputFieldsValue" id="CarModelVal"></h4>
                        </div>

                    </div>
                </div>
                <button id="updateJourneyButton" class="submitButton" onclick="openMyJourneyPopUp('', 'updateJourneyPopUp')">Update Journey</button>
                <button id="deleteJourneyButton" class="submitButton" onclick="openMyJourneysConfirmationPopUp('confirmDeleteJourneyPopUp')">Delete Journey</button>
            </div>
        </div>
        <div class ="popUpBox" id ="createJourneyPopUp" style="display:none;" >
            <button id="createJourneyPopUpCloseButton" class="closeButton" onclick="closeMyJourneyPopUp('createJourneyPopUp');">X</button>
            <h2>Create A Journey</h2>
            <div id="createJourneyDiv" >    
                <div id="createJourneyInputFieldsDiv">
                    <div id="createJourneyDetailsInputFields">
                        <div id="createJourneyDetailsInputFieldsTitleDiv">
                            <h4 class="journeyInputFieldsTitle">Departure Date & Time</h4>
                            <h4 class="journeyInputFieldsTitle">Departure Place</h4>
                            <h4 class="journeyInputFieldsTitle">Destination</h4>
                            <h4 class="journeyInputFieldsTitle">Price</h4>
                            <h4 class="journeyInputFieldsTitle">Seats Available</h4>
                            <h4 class="journeyInputFieldsTitle">Select Car</h4>
                        </div>
                        <div id="createJourneyDetailsInputFieldsDiv">
                            <div>
                                <input id="selectDateDropBox" class="inPageInputFields"  type="datetime-local" name="numberPlate" >
                            </div>
                            <div>
                                <input id="createJourneyInputDeparture" class="inPageInputFields" type="text" name="departurePlace" placeholder="Departure Place">
                            </div>
                            <div>
                                <input id="createJourneyInputDestination" class="inPageInputFields" type="text" name="destination" placeholder="Destination">
                            </div>
                            <div>
                                <input id="createJourneyInputPrice" class="inPageInputFields" type="text" name="price" placeholder="Price">
                            </div>
                            <div>
                                <input id="createJourneyInputSeats" class="inPageInputFields" type="text" name="seatsAvailable" placeholder="Seats Available">
                            </div>
                            <div>
                                <select id="selectCarDropBox" class="inPageInputFields" >                                
                                </select>
                            </div>
                        </div>
                    </div >
                    <div id="createJourneyCheckpointDetailsInputFields" >
                        <div id="checkpointsHeader" class="tableHeader">
                            <h4 id="checkpointTitle" class="tableTitle">Checkpoints</h4>
                        </div>
                        <div id="allCheckpointInputFieldsContainer" class="internalTableContainer" style="margin:auto;">                     
                                                   
                            
                        </div>
                        <button id="removeCheckpointDiv" class="addButton" onclick=" removeLastCheckpointDiv()">-</button>
                        <button id="addNewCheckpointDiv" class="addButton" onclick="addCheckpointInputDiv();">+</button>
                        
                    </div>
                </div>

                <button id="submitCreateJourneyButton" class="submitButton" onclick="openMyJourneysConfirmationPopUp('confirmAddJourneyPopUp')">Create Journey</button>
            </div>
        </div>
        <div class ="popUpBox" id ="updateJourneyPopUp" style="display:none" >
            <button id="updateJourneyPopUpCloseButton" class="closeButton" onclick="closeMyJourneyPopUp('updateJourneyPopUp');">X</button>
            <h2>Update Journey</h2>
            <div id="updateJourneyDiv" >    
                <div id="updateJourneyInputFieldsDiv">
                    <div id="updateJourneyDetailsInputFields">
                        <div id="updateJourneyDetailsInputFieldsTitleDiv">
                            <h4 class="updateJourneyInputFieldsTitle">Departure Date & Time</h4>
                            <h4 class="updateJourneyInputFieldsTitle">Departure Place</h4>
                            <h4 class="updateJourneyInputFieldsTitle">Destination</h4>
                            <h4 class="updateJourneyInputFieldsTitle">Price</h4>
                            <h4 class="updateJourneyInputFieldsTitle">Seats Available</h4>
                            <h4 class="updateJourneyInputFieldsTitle">Select Car</h4>
                        </div>
                        <div id="updateJourneyDetailsInputFieldsDiv">
                            <div>
                            <input id="dateSelectDropBox" class="updateJourneyInPageInputFields"  type="datetime-local" >
                        </div>
                        <div>
                            <input id="updateJourneyInputDeparture" class="updateJourneyInPageInputFields" type="text" placeholder="Departure Place">
                        </div>
                        <div>
                            <input id="updateJourneyInputDestination" class="updateJourneyInPageInputFields" type="text" placeholder="Destination">
                        </div>
                        <div>
                            <input id="updateJourneyInputPrice" class="updateJourneyInPageInputFields" type="text" placeholder="Price">
                        </div>
                        <div>
                            <input id="updateJourneyInputSeats" class="updateJourneyInPageInputFields" type="text" placeholder="Seats Available">
                        </div>
                        <div>
                            <select id="carSelectDropBox" class="updateJourneyInPageInputFields" >                                
                            </select>
                        </div>
                        </div>
                    </div >
                    <div id="updateJourneyCheckpointDetailsInputFields" >
                        <div id="updateJourneyCheckpointsHeader" class="tableHeader">
                            <h4 id="updateJourneyCheckpointTitle" class="tableTitle">Checkpoints</h4>
                        </div>
                        <div id="updateJourneyAllCheckpointInputFieldsContainer" class="internalTableContainer" style="margin:auto;">                     
                            <div id="UpdateJourneyCheckpointInputFieldsContainer1" class="updateJourneyCheckpointInputFieldsContainer">
                                <div>
                                    <input id="updateJourneyInputCheckpointName" class="updateJourneyInPageInputFields" type="text" placeholder="Checkpoint Name">
                                </div>
                                <div>
                                    <input id="updateJourneyInputCheckpointPrice" class="updateJourneyInPageInputFields" type="text" placeholder="Price" style="margin-top:2px;">
                                </div>
                            </div>                           
                            
                        </div>
                        <button id="updateJourneyRemoveCheckpointDiv" class="addCheckpointButton" onclick=" updateJourneyRemoveLastCheckpointDiv()">-</button>
                        <button id="updateJourneyAddNewCheckpointDiv" class="addCheckpointButton" onclick="updateJourneyAddCheckpointInputDiv();">+</button>
                        
                    </div>
                </div>

                <button id="submitUpdateJourneyButton" class="submitButton" onclick="openMyJourneysConfirmationPopUp('confirmUpdateJourneyPopUp')">Update Journey</button>
            </div>
        </div>

        <div class="confimationpopUpBox" id="confirmAddJourneyPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Add <br> This Journey?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelAddJourneyButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmAddJourneyPopUp')">No</button>
                <button id="confirmAddJourneyButton" class="confirmButton" onclick="AddJourney();closeMyJourneysConfirmationPopUp('confirmAddJourneyPopUp');">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmUpdateJourneyPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Update <br> These Details?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelUpdateJourneyButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmUpdateJourneyPopUp')">No</button>
                <button id="confirmUpdateJourneyYesButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmUpdateJourneyPopUp')">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmDeleteJourneyPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Delete <br> This Journey?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelDeleteJourneyButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmDeleteJourneyPopUp')">No</button>
                <button id="confirmDeleteJourneyButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmDeleteJourneyPopUp');">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmCancelBookingPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Cancel <br> This Booking?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelCancelBookingButton" class="confirmButton" onclick="closeMyJourneysConfirmationPopUp('confirmCancelBookingPopUp')">No</button>
                <button id="confirmCancelBookingButton" class="confirmButton" onclick="">Yes</button>
            </div>
        </div>
    </div>

    
</asp:Content>
