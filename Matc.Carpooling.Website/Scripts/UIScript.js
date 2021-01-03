// #region Register Page
function openConditionsDiv(name) {
    document.getElementById("conditionsDiv").style.display = "inline";
    setConditionsText(name);
    document.body.style.backgroundColor = "rgba(206, 206, 255, 0.1)";
}
function closeConditionsDiv() {
    document.getElementById("conditionsDiv").style.display = "none";
    document.getElementById("divText").innerHTML = "empty";
    document.body.style.backgroundColor = "white";
}
function setConditionsText(name) {
    if (name == "privacy") {
        fetch('Privacy Policy.html')
            .then(response => response.text())
            .then(text => document.getElementById("divText").innerHTML = text)
    }
    else if (name == "terms") {
        fetch('Terms of Service.html')
            .then(response1 => response1.text())
            .then(text => document.getElementById("divText").innerHTML = text)
    }
}
 // #endregion

// #region Home Page
function openHomePagePopUp(name) {
    if (name == "bookedJourneyDetailsPopUp") {
        document.getElementById(name).style.display = "flex";
    }
    else {
        document.getElementById(name).style.display = "inline";
    }
    document.getElementById("bookedJourneysContainer").style.opacity = "0.1";
    document.getElementById("journeysContainer").style.opacity = "0.1";
}
function closeHomePagePopUp(name) {
    document.getElementById(name).style.display = "none";
    document.getElementById("bookedJourneysContainer").style.opacity = "1";
    document.getElementById("journeysContainer").style.opacity = "1";
    document.body.style.backgroundColor = "white";
}
function openBookJourneyPopUp() {
    document.getElementById("bookJourneyPopUp").style.display = "inline";
    document.getElementById("bookedJourneysContainer").style.opacity = "0.1";
    document.getElementById("journeysContainer").style.opacity = "0.1";
    document.getElementById("journeyDetailsPopUp").style.opacity = "0.5";
}
function closeBookJourneyPopUp() {
    document.getElementById("bookJourneyPopUp").style.display = "none";  
    document.getElementById("journeyDetailsPopUp").style.display = "none";
    document.getElementById("bookedJourneysContainer").style.opacity = "1";
    document.getElementById("journeysContainer").style.opacity = "1";
    document.getElementById("journeyDetailsPopUp").style.opacity = "1";
}
function openHomeConfirmationPopUp(name) {
    document.getElementById(name).style.display = "flex";
    document.getElementById("bookedJourneysContainer").style.opacity = "0.1";
    document.getElementById("journeysContainer").style.opacity = "0.1";
    document.getElementById("bookedJourneyDetailsPopUp").style.opacity = "0.5";
    document.getElementById("journeyDetailsPopUp").style.opacity = "0.5";

}
function closeHomeConfirmationPopUp(name, secondName) {
    document.getElementById(name).style.display = "none";
    document.getElementById(secondName).style.display = "none";    
    document.getElementById("bookJourneyPopUp").style.display = "none";
    document.getElementById("bookedJourneysContainer").style.opacity = "1";
    document.getElementById("journeysContainer").style.opacity = "1";
    document.getElementById("bookedJourneyDetailsPopUp").style.opacity = "1";
    document.getElementById("journeyDetailsPopUp").style.opacity = "1";
    document.body.style.backgroundColor = "white";
}
 // #endregion

// #region MyCar Page
function carUpdateInputsVisible() {
    document.getElementById('cancelUpdateButton').style.display = "inline";
    document.getElementById('confirmUpdateCarButton').style.display = "inline";
    document.getElementById('carDetailsUpdateInfo').style.display = "flex";
    document.getElementById('deleteCarButton').style.display = "none";
    document.getElementById('updateCarButton').style.display = "none";
    document.getElementById('carDetailsInfo').style.display = "none";
}
function carUpdateInputsHide() {
    document.getElementById('cancelUpdateButton').style.display = "none";
    document.getElementById('confirmUpdateCarButton').style.display = "none";
    document.getElementById('carDetailsUpdateInfo').style.display = "none";
    document.getElementById('deleteCarButton').style.display = "inline";
    document.getElementById('updateCarButton').style.display = "inline";
    document.getElementById('carDetailsInfo').style.display = "inline";
}
function openCarConfirmationPopUp(name) {
    document.getElementById(name).style.display = "flex";
    document.getElementById("myCarsContainer").style.opacity = "0.1";    
    document.getElementById("addCarPopUp").style.opacity = "0.5";
    document.getElementById("viewCarPopUp").style.opacity = "0.5";
}
function closeCarConfirmationPopUp(name) {
    document.getElementById(name).style.display = "none";
    document.getElementById("myCarsContainer").style.opacity = "1";
    document.getElementById("addCarPopUp").style.opacity = "1";
    document.getElementById("viewCarPopUp").style.opacity = "1";
    //document.getElementById("addCarPopUp").style.display = "none";
    //document.getElementById("viewCarPopUp").style.display = "none";
    carUpdateInputsHide();
}
function openCarPopUp(id, name) {
    document.getElementById(name).style.display = "inline";
    document.getElementById("myCarsContainer").style.opacity = "0.1";
    if (name == "viewCarPopUp"){
        showCarDetailsOnPopUp(id);
    }
}
function closeCarPopUp(name) {
    document.getElementById(name).style.display = "none";
    document.getElementById("myCarsContainer").style.opacity = "1";
    document.body.style.backgroundColor = "white";
    AddAllCarsToTable()
}
// #endregion

// #region MyAccount Page
function setViewDetailsDivVisible() {
    document.getElementById("displayDetails").style.display = "inherit";
    document.getElementById("editUserDetailsDiv").style.display = "none";
}
function setEditDetailsDivVisible() {
    document.getElementById("displayDetails").style.display = "none";
    document.getElementById("editUserDetailsDiv").style.display = "inline";
}

function openMyAccountConfirmationPopUp() {

}

function closeMyAccountConfirmationPopUp() {

}
// #endregion

// #region MyJourneys Page
//add a new checkpoint input field
function addCheckpointInputDiv() {
    var allCheckpointsContainer = document.getElementById('allCheckpointInputFieldsContainer');
    var count = allCheckpointsContainer.childElementCount + 1;
    //create individual divs for each checkpoint.
    var individualFieldContainer = document.createElement("div");
    individualFieldContainer.id = 'checkpointInputFieldsContainer' + count;
    individualFieldContainer.className = 'checkpointInputFieldsContainer';

    //create first div for checkpoint name input
    var nameInputFieldContainer = document.createElement("div");
    var nameInputField = document.createElement("input");
    nameInputField.id = "createJourneyInputCheckpointName" + count;
    nameInputField.className = "inPageInputFields";
    nameInputField.type = "text";
    nameInputField.placeholder = "Checkpoint Name";
    nameInputFieldContainer.appendChild(nameInputField);
    //create second div for checkpoint price input
    var priceInputFieldContainer = document.createElement("div");
    var priceInputField = document.createElement("input");
    priceInputField.id = "createJourneyInputCheckpointPrice" + count;
    priceInputField.className = "inPageInputFields";
    priceInputField.type = "text";
    priceInputField.placeholder = "Price";
    priceInputField.style.marginTop = "2px";
    priceInputFieldContainer.appendChild(priceInputField);
    //append children to containers
    individualFieldContainer.appendChild(nameInputFieldContainer);
    individualFieldContainer.appendChild(priceInputFieldContainer);

    allCheckpointsContainer.appendChild(individualFieldContainer);
}

function removeLastCheckpointDiv() {
    var allCheckpointsContainer = document.getElementById('allCheckpointInputFieldsContainer');
    var count = allCheckpointsContainer.childElementCount;
    if (count != 0) {
        var toBeDeletedDiv = document.getElementById('checkpointInputFieldsContainer' + count);
        toBeDeletedDiv.remove();
    } else {

    }
}

function updateJourneyAddCheckpointInputDiv() {
    var allCheckpointsContainer = document.getElementById('updateJourneyAllCheckpointInputFieldsContainer');
    var count = allCheckpointsContainer.childElementCount + 1;
    //create individual divs for each checkpoint.
    var individualFieldContainer = document.createElement("div");
    individualFieldContainer.id = 'updateJourneyCheckpointInputFieldsContainer' + count;
    individualFieldContainer.className = 'updateJourneyCheckpointInputFieldsContainer';

    //create first div for checkpoint name input
    var nameInputFieldContainer = document.createElement("div");
    var nameInputField = document.createElement("input");
    nameInputField.id = "updateJourneyInputCheckpointName" + count;
    nameInputField.className = "updateJourneyInPageInputFields";
    nameInputField.type = "text";
    nameInputField.placeholder = "Checkpoint Name";
    nameInputFieldContainer.appendChild(nameInputField);
    //create second div for checkpoint price input
    var priceInputFieldContainer = document.createElement("div");
    var priceInputField = document.createElement("input");
    priceInputField.id = "updateJourneyInputCheckpointPrice" + count;
    priceInputField.className = "updateJourneyInPageInputFields";
    priceInputField.type = "text";
    priceInputField.placeholder = "Price";
    priceInputField.style.marginTop = "2px";
    priceInputFieldContainer.appendChild(priceInputField);
    //append children to containers
    individualFieldContainer.appendChild(nameInputFieldContainer);
    individualFieldContainer.appendChild(priceInputFieldContainer);

    allCheckpointsContainer.appendChild(individualFieldContainer);
}

function updateJourneyRemoveLastCheckpointDiv() {
    var allCheckpointsContainer = document.getElementById('updateJourneyAllCheckpointInputFieldsContainer');
    var count = allCheckpointsContainer.childElementCount;
    if (count != 0) {
        var toBeDeletedDiv = document.getElementById('updateJourneyCheckpointInputFieldsContainer' + count);
        toBeDeletedDiv.remove();
    } else {

    }
}

function openMyJourneyPopUp(id, name) {
    if (id != '') {
        showCreatedJourneyDetailsOnPopUp(id);
    }
    document.getElementById(name).style.display = "inline";
    document.getElementById("bookedMyJourneysContainer").style.opacity = "0.1";
    document.getElementById("createdMyJourneysContainer").style.opacity = "0.1";
    
    if (name == "updateJourneyPopUp") { // when you click on update journey
        document.getElementById('createdJourneyDetailsPopUp').style.display = "none";   
    }
}

function closeMyJourneyPopUp(name) {
    document.getElementById(name).style.display = "none";
    document.getElementById("bookedMyJourneysContainer").style.opacity = "1";
    document.getElementById("createdMyJourneysContainer").style.opacity = "1";
    document.body.style.backgroundColor = "white";
}   

function openMyJourneysConfirmationPopUp(name) {
    document.getElementById(name).style.display = "flex";
    document.getElementById("bookedMyJourneysContainer").style.opacity = "0.1";
    document.getElementById("createdMyJourneysContainer").style.opacity = "0.1";
    document.getElementById("createJourneyPopUp").style.opacity = "0.5";
    document.getElementById("updateJourneyPopUp").style.opacity = "0.5";
    document.getElementById("bookedJourneyDetailsPopUp").style.opacity = "0.5";
    document.getElementById("createdJourneyDetailsPopUp").style.opacity = "0.5";
}

function closeMyJourneysConfirmationPopUp(name) {
    document.getElementById(name).style.display = "none";    
    document.getElementById("bookedMyJourneysContainer").style.opacity = "1";
    document.getElementById("createdMyJourneysContainer").style.opacity = "1";
    document.getElementById("createJourneyPopUp").style.opacity = "1";   
    document.getElementById("updateJourneyPopUp").style.opacity = "1";
    document.getElementById("bookedJourneyDetailsPopUp").style.opacity = "1";
    document.getElementById("createdJourneyDetailsPopUp").style.opacity = "1";
    document.getElementById("createJourneyPopUp").style.display = "none";
    document.getElementById("updateJourneyPopUp").style.display = "none";  
    document.getElementById("bookedJourneyDetailsPopUp").style.display = "none";
    document.getElementById("createdJourneyDetailsPopUp").style.display = "none"; 
    document.body.style.backgroundColor = "white";
}



 // #endregion
