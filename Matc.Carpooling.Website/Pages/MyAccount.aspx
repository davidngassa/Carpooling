<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.MyAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
    <script>
        document.getElementById("nav").style.display = "inline";
        document.getElementById("navHelloText").style.fontWeight = "bold";
    </script>
    <script src="../Scripts/myAccountScript.js"></script>
    <script src="../Scripts/UIScript.js"></script>
    <div id="myAccountContainer">
        <div id="myAccountOptionsDiv">            
            <div id="myAccountOptionsLabels">
                <div id="myDetailsDiv" class="myAccountSelectOptionsDiv" ><a id="myDetailsLabelLink"  class="myAccountSelectOptionsLabel" onclick="setViewDetailsDivVisible()"><P id="myDetailsLabel" class="names">MY DETAILS</P></a></div>
                <div id="editDetailsDiv" class="myAccountSelectOptionsDiv"><a id="editDetailsLabelLink" class="myAccountSelectOptionsLabel" onclick="setEditDetailsDivVisible()"><p id="editDetailsLabel" class="names">EDIT DETAILS</p></a></div>             
            </div>
        </div>
        <div id="detailsDisplayDiv">
            <div id="displayDetails" >
                <h2>DETAILS</h2>
                <div id="detailsContainer">
                    <div id ="detailsTitleContainer">
                        <h3 class="detailsTitle">Identity Number :</h3>
                        <h3 class="detailsTitle" id="fNamediv">First Name :</h3>
                        <h3 class="detailsTitle" id="lNameDiv">Last Name :</h3>
                        <h3 class="detailsTitle" id="emailDiv">Email Address :</h3>
                        <h3 class="detailsTitle" id="phoneDiv">Phone Number:</h3>
                    </div>
                    <div id="detailsInfoContainer">
                        <h4 class="detailsInfo" id="userIDNumVal"></h4>
                        <h4 class="detailsInfo" id="userFirstNameVal"></h4>
                        <h4 class="detailsInfo" id="userLastNameVal"></h4>
                        <h4 class="detailsInfo" id="userEmailVal"></h4>
                        <h4 class="detailsInfo" id="userPhoneVal"></h4>
                    </div>
                </div>
            </div>
            <div id="editUserDetailsDiv" style="display:none;">
                <div id="userDetailsEdit">
                <h2 id="EditDetailsTitleDiv">EDIT DETAILS</h2>
                <div id="editDetailsContainer">
                    <div id ="editDetailsTitleContainer">
                        <h3 class="detailsTitle" id="firstNameDiv">First Name :</h3>
                        <h3 class="detailsTitle" id="lastNameDiv">Last Name :</h3>
                        <h3 class="detailsTitle" id="emailAddDiv">Email Address :</h3>
                        <h3 class="detailsTitle" id="phoneNumDiv">Phone Number :</h3>                       
                    </div>
                    <div id="EditdetailsInputFieldsContainer">
                        <div id="editDetailsInputFields">
                            <div>
                                <input class="inPageInputFields" type="text" name="firstName">
                            </div>
                            <div>
                                <input class="inPageInputFields" type="text" name="lastName">
                            </div>
                            <div>
                                <input class="inPageInputFields" type="text" name="emailAddress">
                            </div>
                            <div>
                                <input class="inPageInputFields" type="text" name="phoneNumber">
                            </div>  
                        </div>                      
                    </div>                   
                </div>                    
                    <div id="confirmButtonDiv">
                        <button id="confirmEditDetailsButton" class="submitButton" onclick="UpdateUserDetails()">Confirm Changes</button>
                    </div>  
                    
                    <div id="editPasswordContainer">
                    <div id ="editPasswordTitleContainer">
                        <h3 class="detailsTitle" id="passwordDiv">New Password :</h3>
                        <h3 class="detailsTitle" id="confirmPasswordDiv">Confirm New password :</h3>                       
                    </div>
                    <div id="EditPasswordInputFieldsContainer">
                        <div id="editPasswordInputFields">
                            <div>
                                <input class="inPageInputFields" type="password" name="Password">
                            </div>
                            <div>
                                <input class="inPageInputFields" type="password" name="confirmPassword">
                            </div>  

                        </div>                      
                    </div>                   
                </div>
                    <p id="error" class="errorText"></p>
                    <div id="confirmChangePasswordButtonDiv">
                        <button id="confirmEditPasswordButton" class="submitButton" onclick="checkPassword()">Confirm Changes</button>
                    </div>    



                </div>
            </div>
        </div>
        <object data="../Svg-Images/undraw_profile_6l1l.svg" type ="image/svg+xml" width="200" height="600" style="position:absolute; bottom: 0; opacity: 0.7; z-index:0" />
    </div>

   
</asp:Content>
