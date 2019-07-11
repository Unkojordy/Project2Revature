<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Insurance_Website._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Policy Application</h1>
    </div>

    <div class="row">
        <div class="col-md-6">
            <form id="policyForm" method="post" >
                <div class="form-group">
                    <label for="firstName">First Name</label>
                    <input type="text" class="form-control" id="firstName" name="firstName" runat="server">
                </div>
                    <div class="form-group">
                    <label for="lastName">Last Name</label>
                    <input type="text" class="form-control" id="lastName" name="lastName" runat="server">
                </div>
                    <div class="form-group">
                    <label for="country">Country</label>
                    <input type="text" class="form-control" id="country" name="country" runat="server">
                </div>
                <div class="form-group">
                    <label for="numCars">Number of Cars</label>
                    <input type="text" class="form-control" id="numCars" name="numCars" runat="server">
                </div>
                <div class="form-group">
                    <label for="drivingRecord">Driving Record</label>
                    <select id="drivingRecord" runat="server">
                        <option value="volvo">Poor</option>
                        <option value="saab">Fair</option>
                        <option value="vw">Average</option>
                        <option value="audi" selected>Excellent</option>
                    </select> 
                </div>
                <button type="submit" class="btn btn-primary" runat="server">Submit</button>
            </form>
        </div>
        <div class="col-md-6">

        </div>
    </div>

</asp:Content>
