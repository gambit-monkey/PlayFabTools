using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GambitMonkey.PlayFabTools
{
    public class PlayFabUser
    {
        private string playFabId;
        private string sessionTicket;
        private UserSettings userSettings;
        private DateTime? lastLoginTime;
        private EntityTokenResponse entityTokenResponse;
        private GetPlayerCombinedInfoResultPayload infoResultPayLoad;
        private bool newlyCreated;
        private TreatmentAssignment treatmentAssignment;
        private string displayName;

        #region Properties
        /// <summary>
        /// If LoginTitlePlayerAccountEntity flag is set on the login request the title_player_account will also be logged in and
        /// returned.
        /// </summary>
        public EntityTokenResponse EntityToken
        {
            get { return entityTokenResponse; }
            set { entityTokenResponse = value; }
        }
        /// <summary>
        /// Results for requested info.
        /// </summary>
        public GetPlayerCombinedInfoResultPayload InfoResultPayload
        {
            get { return infoResultPayLoad; }
            set { infoResultPayLoad = value; }
        }
        /// <summary>
        /// The time of this user's previous login. If there was no previous login, then it's DateTime.MinValue
        /// </summary>
        public DateTime? LastLoginTime
        {
            get { return lastLoginTime; }
            set { lastLoginTime = value; }
        }
        /// <summary>
        /// True if the account was newly created on this login.
        /// </summary>
        public bool NewlyCreated
        {
            get { return newlyCreated; }
            set { newlyCreated = value; }
        }
        /// <summary>
        /// Player's unique PlayFabId.
        /// </summary>
        public string PlayFabId
        {
            get { return playFabId; }
            set { playFabId = value; }
        }
        /// <summary>
        /// Unique token authorizing the user and game at the server level, for the current session.
        /// </summary>
        public string SessionTicket
        {
            get { return sessionTicket; }
            set { sessionTicket = value; }
        }
        /// <summary>
        /// Settings specific to this user.
        /// </summary>
        public UserSettings UserSettings
        {
            get { return userSettings; }
            set { userSettings = value; }
        }
        /// <summary>
        /// The experimentation treatments for this user at the time of login.
        /// </summary>
        public TreatmentAssignment TreatmentAssignment
        {
            get { return treatmentAssignment; }
            set { treatmentAssignment = value; }
        }
        ///<summary>
        ///Optional Display name for user. PlayFabIds aren't easily rememberable
        /// </summary>
        public string DisplayName
        {
            get { return displayName;  }
            set { displayName = value; }
        }

        #endregion
    }
}
