<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DinnerFormViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="NerdDinner.Models"%>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Edit: <%=Html.Encode(Model.Dinner.Title) %>
</asp:Content>

<asp:Content ID="Edit" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Dinner</h2>

    <% Html.RenderPartial("DinnerForm"); %>

</asp:Content>

