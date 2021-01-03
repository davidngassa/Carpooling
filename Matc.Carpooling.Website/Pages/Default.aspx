<%@ Page Title="Welcome Back" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Matc.Carpooling.Website.Pages.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFirst" runat="server">
     <script src="/Scripts/authentication.js"></script>
    <div class="wrapper white-background">
        <div class="mycontainer" id="welcome-page-container">
            <div class="box">
                <h2>Log into Powa Panzer</h2>
                <p id="create-account-text">Or <a href="/Pages/Register.aspx" class="page-link">Create Account</a></p>
                <div class="input-data">
                    <input id="emailAddress" type="text" name="emailAddress" placeholder="Email Address" required>
                    <input id="password" type="password" name="Password" placeholder="Password" required>
                </div>
                <p id="errorMessage"></p>
                <button id="login-button" class="submitButton" onclick="Login();" type="submit">Log In</button>            
            </div>
            <object data="../Svg-Images/vectorpaint2.svg" type ="image/svg+xml" width="908" height="684" />
        </div>        
    </div>
    <style>
        body{
            background: white;
        }
        .wrapper{
            padding-left: 5%;
        }
        #content{
            overflow-x: hidden;
        }
    </style>
</asp:Content>
