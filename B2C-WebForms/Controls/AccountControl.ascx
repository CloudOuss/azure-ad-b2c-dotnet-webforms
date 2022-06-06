<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountControl.ascx.cs" Inherits="B2C_WebForms.Controls.AccountControl" %>
<%if (Request.IsAuthenticated){%>    
    <ul class="nav navbar-nav navbar-right">
        <li><asp:LinkButton runat="server" ID="linkSignOut" OnClick="LinkSignOut_Click">Sign out</asp:LinkButton></li>
    </ul>
<%}else{%>
    <ul class="nav navbar-nav navbar-right">
        <li><asp:LinkButton runat="server" ID="linkSignIn" OnClick="LinkSignIn_Click">Sign in</asp:LinkButton></li>        
    </ul>
<%}%>