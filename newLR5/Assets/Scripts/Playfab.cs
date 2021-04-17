using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.PfEditor.Json;

namespace Game
{
    public class Playfab : MonoBehaviour
    {
        private void Start()
        {
            LoginToPlayFab();
        }

        private void LoginToPlayFab()
        {

            var request = new LoginWithCustomIDRequest { CustomId = "EDITOR_DEBUG_ID", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, LoginSuccessHandler, LoginFailureHandler);

        }

        private void PlayFabHelloWorld()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "helloWorld",
                FunctionParameter = new { inputValue = "YOUR NAME" },
            };
            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                var str = JsonWrapper.SerializeObject(result.FunctionResult);
                var deserialized = JsonWrapper.DeserializeObject(str);
                JsonObject jsonResult = (JsonObject)deserialized;
                object messageValue;
                jsonResult.TryGetValue("messageValue", out messageValue);
                Debug.Log("PlayFab/ " + (string)messageValue);
            }, error =>
            {
                Debug.LogError(error.ErrorMessage);
            });
        }

        private void LoginSuccessHandler(LoginResult loginResult)
        {
            Debug.Log("PlayFab/Login");
            PlayFabHelloWorld();
        }

        private void LoginFailureHandler(PlayFabError error)
        {
            Debug.LogError(error.ErrorMessage);
        }
    }
}