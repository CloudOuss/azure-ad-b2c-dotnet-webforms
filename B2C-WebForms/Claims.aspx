<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Claims.aspx.cs" Inherits="B2C_WebForms.Claims" Async="true" %>
<asp:Content ID="ClaimsContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Claims</h2>
    <% if(Request.IsAuthenticated) 
    { %>
    <h4>Claims Present in the Claims Identity: <%=FirstName%> <%=LastName%></h4>

    <div><asp:LinkButton  runat="server" ID="buttonApiCall" OnClick="buttonApiCall_Click">Call Healthcheck</asp:LinkButton></div>
    
    <div>API RESPONSE :  <%=ApiResponse%></div>

    <table class="table table-bordered">
        <tr>
            <th class="claim-type claim-data claim-head">Claim Type</th>
            <th class="claim-data claim-head">Claim Value</th>
        </tr>


                 <%foreach (System.Security.Claims.Claim claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
                {%>
                <tr>
                    <td class="claim-type claim-data"><%: claim.Type %></td>
                    <td class="claim-data"><%:claim.Value %></td>
                </tr>
                <%}%>
           
    </table>
    <%}%>
    <% else { %>
    <h4>not logged in</h4>
    <%}%>
</asp:Content>
