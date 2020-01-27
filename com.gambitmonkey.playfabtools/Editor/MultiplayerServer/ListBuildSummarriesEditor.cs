using PlayFab;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GambitMonkey.PlayFabTools.Editors
{
    public class ListBuildSummarriesEditor : MonoBehaviour
    {
        private List<BuildSummary> buildSummaries = new List<BuildSummary>();

        public List<BuildSummary> BuildSummaries
        {
            get
            {
                ListBuildSummaries();
                return buildSummaries;
            }
        }
        
        //[Button("Function")]
        #region PlayFab Multiplayer Server
        private void ListBuildSummaries()
        {
            if (!PlayFabAuthenticationAPI.IsEntityLoggedIn())
            {
                PlayFabGeneral.GetEntityToken();
            }           

            ListBuildSummariesRequest request = new ListBuildSummariesRequest();
            PlayFabMultiplayerAPI.ListBuildSummaries(request, ListBuildSummariesResponseSuccess, PlayFabGeneral.Error, null, null);
        }

        private void ListBuildSummariesResponseSuccess(ListBuildSummariesResponse result)
        {
            buildSummaries = result.BuildSummaries;            
        }
        #endregion
    }
}
