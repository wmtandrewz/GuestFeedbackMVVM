using System;
using System.Collections.Generic;

namespace CGFSMVVM.Models
{
    public class Constants
    {
		//*************************************** Development Settings ***********************
        //DEV URL
        //public static string _gatewayURL = "https://nwgateway.keells.lk:44300";
        //public static string _cookie = "sap-XSRF_NWG_100";XSRF_NWG_100
        //*************************************** Development Settings ***********************

        ////*************************************** Production settings ***********************
        ////DEV URL
        public static string _gatewayURL = "";
        public static string _cookie = "";
        //public static string _gatewayURL = "https://alastor.keells.lk:44300";
        //public static string _cookie = "sap-XSRF_GWP_100";
        ////*************************************** Production Settings ***********************

  
        //Access Token for gateway services
        public static string _access_token = "";
        public static string _expires_in = "";

        //User Login Details
        public static UserModel _user;


        //Hotel Information variables
        public static string _hotel_code = "";
        

        //Message Header
        public static string _headerConnectionLost = "Connection Lost";
        public static string _headerUpdateAvailable = "Update Available";
        public static string _headerHowTo = "How to Log In?";
        public static string _headerMessage = "Message";
        public static string _headerWrongCredentials = "Wrong Credentials";
        public static string _headerPercentageContribution = "Percentage of Contribution";


        //Messages
        public static string _userNotExistInNWGateway = "Authentication Successful. Login Unsuccessful due to unavailability of username in SAP system. Please contact your authorization team.";
        public static string _networkerror = "Unable to Connect!\n\n Note: This error may occur continously when the password is changed. Please login again!";
        public static string _networkUnreachable = "Error: ConnectFailure(Network is unreachable)";
        public static string _wentwrong = "Sorry! Something went wrong. \n\ndue to ";
        public static string _help = "Enter your active directory username along with '@jkintranet.com' and password.";
        public static string _providelogincredentials = "Please Enter Login Credetials!";
        public static string _incorrectlogincredentials = "Incorrect Username/Password.";
        public static string _mandotoryfield = "Please specify a reason with minimum 10 characters.";

        //public static string _updateMessage = "Great News! A new version of this app is available, you must update to new version in order to use this application. Press 'Okay' to update this app.";
        public static string _exitMessage = "Are you sure?";
        public static string _TokenRefreshError = "Sorry.. Unable to refresh token. This may occur when the Active Directory password is changed. Please login again!";
        public static string _unableToGetHotelCode = "Unable To Access Hotel Code. Please Contact the developers!";
        public static string _unableToCOnnectToInternet = "Unable to Connect to the Internet!";
        public static string _gatewayUrlError = "Unable to Connect. This error may occur if the gateway URL is incorrect!";



        //Message Button
        public static string _buttonTryAgain = "Try Again";
        public static string _buttonOkay = "Ok";
        public static string _buttonClose = "Close";
        public static string _buttonYes = "Yes";
        public static string _buttonNo = "No";





      
          
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




    }
}
