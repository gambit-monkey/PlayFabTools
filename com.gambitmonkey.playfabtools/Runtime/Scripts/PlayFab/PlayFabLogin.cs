using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using GambitMonkey.ScriptableObjects;

namespace GambitMonkey.PlayFabTools
{
    public partial class PlayFabLogin : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField]
        [Tooltip("Use Debug Crenditials so you do not have to enter them each time during testing")]
        private bool useDebugLogin = false;
        [SerializeField]
        [Tooltip("Debug Email Address to pass to PlayFab")]
        private string debugEmail = "";
        [SerializeField]
        [Tooltip("Debug Password to pass to PlayFab")]
        private string debugPassword = "";

        [Header("Unity Events")]
        public UnityPlayFabUserEvent onLoginSuccess = new UnityPlayFabUserEvent();
        public UnityStringEvent onLoginFail = new UnityStringEvent();

        /// <summary>
        /// These are optional Scriptable Object Events they raise the same events that the Unity Events do
        /// </summary>
        [Header("Scriptable Object Events")]
        public PlayFabUserEvent onSOLoginSuccess;
        public StringEvent onSOLoginFail;

        //Checks to see if user is already logged in
        private bool isLoggedIn = false;

        //PlayFabUser to store user values returned from PlayFab
        private PlayFabUser user;

        private void Awake()
        {
            if (useDebugLogin)
            {
                LoginWithEmailAddress(debugEmail, debugPassword);
            }
        }

        public void LoginWithEmailAddress(string Email, string Password)
        {
            if (isLoggedIn)
            {
                return;
            }   
            else
            {
                var request = new LoginWithEmailAddressRequest { Email = Email, Password = Password };
                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);                
            }
        }

        private void OnLoginSuccess(LoginResult result)
        {
            isLoggedIn = true;
            //Create PlayFabUser if it is null and assign result values
            if (user == null)
            {
                user = new PlayFabUser();
            }
            user.PlayFabId = result.PlayFabId;
            user.SessionTicket = result.SessionTicket;
            user.EntityToken = result.EntityToken;
            user.InfoResultPayload = result.InfoResultPayload;
            user.LastLoginTime = result.LastLoginTime;
            user.TreatmentAssignment = result.TreatmentAssignment;
            user.UserSettings = result.SettingsForUser;

            Debug.Log("[PlayFabLogin] " + user.PlayFabId + " successfully logged in");

            //Invoke success so listeners can react
            onLoginSuccess.Invoke(user);
            if (onLoginSuccess != null)
                onSOLoginSuccess.Raise(user);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            isLoggedIn = false;
            error.GenerateErrorReport();
            string errorMessage = string.Empty;
            switch (error.Error)
            {
                // Recognize and handle the error
                case PlayFabErrorCode.InvalidTitleId:
                    // Handle invalid title id error
                    errorMessage = "User already logged in";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    // Handle account not found error
                    errorMessage = "Email address not found, Do you need to register for an account?";
                    break;
                case PlayFabErrorCode.InvalidEmailOrPassword:
                    // Handle invalid email or password error
                    errorMessage = "Email address or Password is incorrect";
                    break;
                case PlayFabErrorCode.RequestViewConstraintParamsNotAllowed:
                    // Handle not allowed view params error
                    errorMessage = "Invalid login parameters";
                    break;
                default:
                    // Handle unexpected error
                    break;
            }
            // Show error message
            onLoginFail.Invoke(errorMessage);
            if (onSOLoginFail != null)
                onSOLoginFail.Raise(errorMessage);
        }

        //public void OnLogin(AckResponseCode responseCode, BaseAckMessage message)
        //{
        //    ResponseUserLoginMessage castedMessage = (ResponseUserLoginMessage)message;
        //    switch (responseCode)
        //    {
        //        case AckResponseCode.Error:
        //            string errorMessage = string.Empty;
        //            switch (castedMessage.error)
        //            {
        //                case ResponseUserLoginMessage.Error.AlreadyLogin:
        //                    errorMessage = "User already logged in";
        //                    break;
        //                case ResponseUserLoginMessage.Error.InvalidUsernameOrPassword:
        //                    errorMessage = "Invalid username or password";
        //                    break;
        //            }
        //            UISceneGlobal.Singleton.ShowMessageDialog("Cannot Login", errorMessage);
        //            if (onLoginFail != null)
        //                onLoginFail.Invoke();
        //            break;
        //        case AckResponseCode.Timeout:
        //            UISceneGlobal.Singleton.ShowMessageDialog("Cannot Login", "Connection timeout");
        //            if (onLoginFail != null)
        //                onLoginFail.Invoke();
        //            break;
        //        default:
        //            if (onLoginSuccess != null)
        //                onLoginSuccess.Invoke();
        //            break;
        //    }
        //}
    }
}