<%@ Page Title="Register" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
    <script src="../Scripts/UIScript.js"></script>
    <div class="wrapper" id="wrapper">
    <div class="mycontainer">
        <div class="box registerBox">
            <h2>Create Account</h2>  
            <p id="back-login-text">Already have an account? <a href="/Pages/Default.aspx" class="page-link">Log In</a></p>
            <div id="registerInputFields" class="input-data">
                
                <div>
                    <input class="registerInput" type="text" name="firstName" placeholder="First Name">
                    <input class="registerInput" type="text" name="lastName" placeholder="Last Name">
                </div>
                <div>
                    <input class="registerInput" type="text" name="userID" placeholder="NIC/Passport Number">
                    <input class="registerInput" type="text" name="phoneNumber" placeholder="Phone Number">
                </div>
                <div>
                    <input class="registerInput" type="text" name="emailAddress" placeholder="Email Address">
                    <input class="registerInput" type="password" name="Password" placeholder="Password">
                </div>
            </div>
            <p id="policy-text">By creating an account, you agree to our <a onclick="openConditionsDiv('terms')" class="page-link">Terms of Service</a> and have read and understood the <a onclick="openConditionsDiv('privacy')" class="page-link">Privacy Policy</a></p>
            <p id="errorMessage"></p>
            <button id="register-button" class="submitButton" type="submit" onclick="Register();">Create Account</button>
        </div>   
         <div id="conditionsDiv" style="display:none; ">
            <button class="closeButton" onclick="closeConditionsDiv();" style="margin-left:95%;">X</button>
            <div id="textcontainer"><p id="divText"></p> </div> 
        </div>
        
    </div>      
   
        <script src="/Scripts/authentication.js"></script>
    </div>
    <style>
        body{
            background: white;
        }
    </style>
</asp:Content>
