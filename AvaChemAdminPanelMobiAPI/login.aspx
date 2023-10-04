<%@ page language="C#" autoeventwireup="true" codebehind="Login.aspx.cs" inherits="AvaChemAdminPanelMobiAPI.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <!-- BEGIN HEAD -->
    <meta charset="utf-8" />
    <title>AvaChem | Login</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link href="assets/global/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/global/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- END GLOBAL MANDATORY STYLES -->

    <!-- BEGIN THEME GLOBAL STYLES -->
    <link href="assets/global/css/components.min.css" rel="stylesheet" id="style_components" type="text/css" />
    <!-- END THEME GLOBAL STYLES -->
    <!-- BEGIN PAGE LEVEL STYLES -->
    <link href="assets/pages/css/login.css" rel="stylesheet" type="text/css" />
    <!-- END PAGE LEVEL STYLES -->
    <!-- BEGIN THEME LAYOUT STYLES -->
    <!-- END THEME LAYOUT STYLES -->
    <script src="assets/global/plugins/jquery.min.js" type="text/javascript"></script>
    <link rel="shortcut icon" href="favicon.ico" />
    <!-- END HEAD -->
</head>
<body>
    <form runat="server">
        <section class="ftco-section">
            <div class="container">
                <div class="row justify-content-center">
                </div>
                <div class="row justify-content-center">
                    <div class="col-md-7 col-lg-5">
                        <div class="wrap">
                            <%--<div class="top-wrap" style="position: relative"> 
                                 <div class="logo" style="max-width: 300px; position: absolute; top: 10px; left: 10px">
                                    <img class="w-100" src="assets/pages/img/login/Avachem_Logo.png" alt="Alternate Text" />
                                 </div>
                                <img class="w-100" src="assets/pages/img/login/LoginPage_Graphic.png" alt="bg" />                                
                            </div>--%>

                            <div class="text-center">
                                <img style="width: 120px" src="assets/pages/img/login/Avachem_Logo.png" alt="Alternate Text" />
                            </div>
                            <div class="img" style="background-image: url(../assets/pages/img/login/LoginPage_Graphic.png);"></div>
                            <%--</div>--%>
                            <div class="login-wrap p-4 p-md-5">
                                <div class="d-flex">
                                    <div class="w-100">
                                        <h2 class="mb-5">Sign In</h2>
                                    </div>
                                </div>
                                <div style="margin-bottom: 25px" class="alert alert-danger" id="warningLabel" runat="server" visible="false"></div>
                                <div style="margin-bottom: 25px" class="alert alert-success" id="successLabel" runat="server" visible="false"></div>
                                <div class="signin-form">
                                    <div class="form-group mt-3">
                                        <asp:TextBox ID="tbxUsername" type="text" class="form-control" runat="server"></asp:TextBox>
                                        <label class="form-control-placeholder" for="username">Username</label>
                                        <br />
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="tbxPassword" type="password" class="form-control password-field" runat="server"></asp:TextBox>
                                        <label class="form-control-placeholder" for="password">Password</label>
                                        <span toggle=".password-field" class="fa fa-fw fa-eye field-icon toggle-password"></span>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="Button1" type="submit" class="form-control btn btn-primary rounded submit px-3" Text="Login" OnClick="btnSubmit_Click" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

</body>
<!-- BEGIN CORE PLUGINS -->
<script src="assets/pages/scripts/login/popper.js" type="text/javascript"></script>
<script src="assets/global/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
<!-- END CORE PLUGINS -->
<!-- BEGIN PAGE LEVEL SCRIPTS -->
<script src="assets/pages/scripts/login/main.js" type="text/javascript"></script>
<script src="assets/pages/scripts/utils.js"></script>
<!-- END PAGE LEVEL SCRIPTS -->
<!-- BEGIN THEME LAYOUT SCRIPTS -->
<!-- END THEME LAYOUT SCRIPTS -->

</html>

