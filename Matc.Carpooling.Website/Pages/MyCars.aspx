<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="MyCars.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.MyCars" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
     



    <script>
        document.getElementById("nav").style.display = "inline";
        document.getElementById("navMyCarsText").style.fontWeight = "bold";
    </script>
    <script src="../Scripts/UIScript.js"></script>   
    <script src="../Scripts/CarScript.js"></script>
      
    <div id="homeContainer" class="tableContainer"> 
        <div id="myCarsContainer">
            <div id="myCarsContainerTable">
                <div id="carTableHeader" class="tableHeader">
                    <h3 id="carsTitle" class="tableTitle">MY CARS</h3>
                </div>
                <div id="myCarContainer" class="internalTableContainer">
                    <table id="myCarTable" >
                        <tr class="table-header">
                            <th>NUMBER PLATE</th>
                            <th>MAKE</th>
                            <th>MODEL</th>
                            <th>YEAR</th>      
                            <th>AVAILABLE SEATS</th> 
                        </tr>
                        
                    </table>
                </div>
            </div>
            <button id="addCarButton" class="submitButton" onclick="openCarPopUp('','addCarPopUp');">Add Car</button>            
        </div>
        <div class ="popUpBox" id ="addCarPopUp" style="display:none;">
            <button id="addCarPopUpCloseButton" class="closeButton" type="submit" onclick="closeCarPopUp('addCarPopUp');">X</button>
            <h2>Details</h2>
            <div id="addCarcontainer">
                <div class="popup-parameter">
                    <h3 class= "carTitle" id="addCarNumPlate">Number Plate :</h3>
                    <input class="inPageInputFields" type="text" name="numberPlate" placeholder="Number Plate">
                </div>
                <div class="popup-parameter">
                    <h3 class= "carTitle" id="addCarMake">Make :</h3>
                    <input class="inPageInputFields" type="text" name="make" placeholder="Make">
                </div>
                <div class="popup-parameter">
                    <h3 class= "carTitle" id="addCarModel">Model :</h3>
                    <input class="inPageInputFields" type="text" name="model" placeholder="Model">
                </div>
                <div class="popup-parameter">
                    <h3 class= "carTitle" id="addCarYear">Year :</h3>
                    <input class="inPageInputFields" type="text" name="year" placeholder="Year">
                </div>
                <div class="popup-parameter">
                    <h3 class= "carTitle" id="addCarSeats">Num. Of Seats :</h3>
                    <input class="inPageInputFields" type="text" name="numOfSeats" placeholder="Num. Of Seats">
                </div>              
                
            </div>
             <p id="errorMessage" class="errorText"></p>
            <div id="createCarButtonDiv">
                <button id="createCarButton" class="submitButton" type="submit" onclick="openCarConfirmationPopUp('confirmAddCarPopUp');" >Create Car</button>
            </div>     
        </div>
        <div class="popUpBox" id ="viewCarPopUp" style="display:none;">
            <button id="viewCarPopUpCloseButton" class="closeButton" onclick="closeCarPopUp('viewCarPopUp');carUpdateInputsHide();">X</button>
                <h2>Details</h2>
            <div id ="carDetailsContainer">
                <div id="viewCarDetailsContainer">
                    <div class="viewCarDetails" id="carDetailsTitles">
                        <h3 class= "carTitle" id="carNumPlate">Number Plate :</h3>
                        <h3 class= "carTitle" id="carMake">Make :</h3>
                        <h3 class= "carTitle" id="carModel">Model :</h3>
                        <h3 class= "carTitle" id="carYear">Year :</h3>
                        <h3 class= "carTitle" id="carSeats">Num. Of Seats :</h3>
                    </div>
                    <div class="viewCarDetails" id="carDetailsInfo">
                        <h4 class= "carValue" id="carNumPlateInfo"></h4>
                        <h4 class= "carValue" id="carMakeInfo"></h4>
                        <h4 class= "carValue" id="carModelInfo"></h4>
                        <h4 class= "carValue" id="carYearInfo"></h4>
                        <h4 class= "carValue" id="carSeatsInfo"></h4>
                    </div>
                    <div class="viewCarDetails" id="carDetailsUpdateInfo"  onclick="" style="display:none;">
                        <div>
                            <input class="inPageInputFields" type="text" name="numberPlateInput">
                        </div>
                        <div>
                            <input class="inPageInputFields" type="text" name="makeInput" disabled>
                        </div>
                        <div>
                            <input class="inPageInputFields" type="text" name="modelInput" disabled>
                        </div>
                        <div>
                            <input class="inPageInputFields" type="text" name="yearInput" disabled>
                        </div>
                        <div>
                            <input class="inPageInputFields" type="text" name="numOfSeatsInput">
                        </div>                                                
                    </div>
                </div>
            </div>
            <div id="viewCarButtonsDiv">   
                <button id="deleteCarButton" class="submitButton" type="submit"  onclick="openCarConfirmationPopUp('confirmDeleteCarPopUp');">Delete</button>
                <button id="cancelUpdateButton" class="submitButton" type="submit"  onclick="carUpdateInputsHide()" style="display:none;">Cancel</button>
                <button id="updateCarButton" class="submitButton" type="submit" onclick="carUpdateInputsVisible()">Update</button>
                <button id="confirmUpdateCarButton" class="submitButton" type="submit" onclick="openCarConfirmationPopUp('confirmUpdateCarPopUp');"  style="display:none;">Confirm</button>
            </div>  
        </div>

        <div class="confimationpopUpBox" id="confirmAddCarPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Add <br> This Car?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelCarButton" class="confirmButton" onclick=" closeCarConfirmationPopUp('confirmAddCarPopUp')">No</button>
                <button id="confirmCarButton" class="confirmButton" onclick="AddCar();closeCarConfirmationPopUp('confirmAddCarPopUp');">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmUpdateCarPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Update <br> These Details?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelUpdateCarButton" class="confirmButton" onclick=" closeCarConfirmationPopUp('confirmUpdateCarPopUp')">No</button>
                <button id="confirmUpdateCarYesButton" class="confirmButton" onclick="updateCar();closeCarConfirmationPopUp('confirmUpdateCarPopUp');">Yes</button>
            </div>
        </div>
        <div class="confimationpopUpBox" id="confirmDeleteCarPopUp" style="display:none;">
            <h5 class="confirmationText">Do You Want to Delete <br> This Car?</h5>
            <div class="confirmationButtonsDiv">
                <button id="cancelDeleteCarButton" class="confirmButton" onclick=" closeCarConfirmationPopUp('confirmDeleteCarPopUp')">No</button>
                <button id="confirmDeleteCarButton" class="confirmButton" onclick="DeleteCar();closeCarConfirmationPopUp('confirmDeleteCarPopUp');">Yes</button>
            </div>
        </div>
        <object data="../Svg-Images/undraw_Vehicle_sale_a645.svg" type ="image/svg+xml" width="1100" height="400" style="position:absolute; bottom: 0; opacity: 0.7; z-index:0" />
    </div>

    
</asp:Content>