using PlayFab;
using PlayFab.AuthenticationModels;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace GambitMonkey.PlayFabTools.Editors
{
    public static class PlayFabGeneral
    {
        private static bool loggedin = false;

        public static void GetEntityToken()
        {
            GetEntityTokenRequest request = new GetEntityTokenRequest();

            PlayFabAuthenticationAPI.GetEntityToken(request, GetEntityTokenResponseSuccess, Error, null, null);

            while (!loggedin)
            {
                Debug.Log("Not logged in");
            }
        }

        private static void GetEntityTokenResponseSuccess(GetEntityTokenResponse result)
        {
            var entityId = result.Entity.Id;
            var entityType = result.Entity.Type;
            Debug.Log("[PlayFabServerInstance] Retrieved Entity Token: " + entityId + " Type: " + entityType);
            loggedin = true;
        }

        public static void Error(PlayFabError error)
        {

        }

        public static void MakePlayFabSharedSettings()
        {
            PlayFabSharedSettings asset = ScriptableObject.CreateInstance<PlayFabSharedSettings>();

            AssetDatabase.CreateAsset(asset, "Assets/PlayFabSdk/Shared/Public/Resources/PlayFabSharedSettings.asset"); // TODO: Path should not be hard coded
            AssetDatabase.SaveAssets();
        }
    }
}
