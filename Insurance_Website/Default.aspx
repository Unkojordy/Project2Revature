<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Insurance_Website._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Policy Application</h1>
    </div>

    <div class="row">
        <div class="col-md-6">
            <form id="policyForm" method="POST" >
                <div class="form-group">
                    <label for="firstName">First Name</label>
                    <input type="text" class="form-control" ID="firstName" name="firstName">
                </div>
                <div class="form-group">
                    <label for="lastName">Last Name</label>
                    <input type="text" class="form-control" ID="lastName" name="lastName">
                </div>
                    <div class="form-group">
                    <label for="country">Country</label>
                    <input type="text" class="form-control" ID="country" name="country">
                </div>
                <div class="form-group">
                    <label for="numCars">Number of Cars</label>
                    <input type="text" class="form-control" ID="numCars" name="numCars">
                </div>
                <div class="form-group">
                    <label for="drivingRecord">Driving Record</label>
                    <select ID="drivingRecord" name="drivingRecord">
                        <option value="Perfect">Excellent</option>
                        <option value="Good">Average</option>
                        <option value="Bad">Fair</option>
                        <option value="Horrible">Poor</option>
                    </select> 
                </div>
                <asp:button type="submit" id="submitButton" text="Submit" class="btn btn-primary" runat="server"></asp:button>
            </form>
        </div>
        <div class="col-md-6">

        </div>
    </div>

</asp:Content>
