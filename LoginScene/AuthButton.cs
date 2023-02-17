using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AuthButton : MonoBehaviour
{
    [SerializeField] InputField inputUser;
    [SerializeField] InputField inputPassword;
    [SerializeField] Button btnLogin;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private AuthJson jsonAuth;
    private RequestListJson jsonLoadUserKeys;

    public void Auth()
    {
        ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Carregando...", panelMsg);
        StartCoroutine(GetAuth());
    }

    private IEnumerator GetAuth()
    {
        UnityWebRequest requestAuth = UnityWebRequest.Get(URLs.apiURL + URLs.authURL + "?login=" + inputUser.text + "&password=" + inputPassword.text);
        yield return requestAuth.SendWebRequest();

        if(requestAuth.result == UnityWebRequest.Result.ConnectionError | requestAuth.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonAuth = JsonUtility.FromJson<AuthJson>(requestAuth.downloadHandler.text);
        
            switch(jsonAuth.code)
            {
                case "api_login_not_found":
                    ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro: usuário não encontrado", panelMsg);
                    break;
                case "api_wrong_password":
                    ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro: senha incorreta", panelMsg);
                    break;
                case "api_auth":
                    User.user.UserId = jsonAuth.id;
                    User.user.UserToken = jsonAuth.token;
                    SaveSystem.SaveUser(new UserData(jsonAuth.id, jsonAuth.token));
                    ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Carregando chaves...");
                    StartCoroutine(GetLoadUserKeys());
                    break;
                default:
                    ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro inesperado: " + jsonAuth.code, panelMsg);
                    break;
            }
        }
    }

    private IEnumerator GetLoadUserKeys()
    {
        UnityWebRequest requestLoadUserKeys = UnityWebRequest.Get(URLs.apiURL + URLs.requestListURL + "?status=" + ((int)RequestStatus.Status.not_started).ToString() + "|" + ((int)RequestStatus.Status.start_request).ToString() + "|" + ((int)RequestStatus.Status.started).ToString() + "|" + ((int)RequestStatus.Status.end_request).ToString() + "&date_start=" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "&date_end=" + DateTime.Now.ToString("yyyy") + "-12-31 23:59:59" + "&token=" + User.user.UserToken + "&user=" + User.user.UserId);
        yield return requestLoadUserKeys.SendWebRequest();

        if(requestLoadUserKeys.result == UnityWebRequest.Result.ConnectionError | requestLoadUserKeys.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonLoadUserKeys = JsonUtility.FromJson<RequestListJson>(requestLoadUserKeys.downloadHandler.text);
            
            if(jsonLoadUserKeys.code == "request_list")
            {
                if(jsonLoadUserKeys.count > 0)
                {
                    foreach(var request in jsonLoadUserKeys.list)
                    {
                        User.user.UserKeys.Add(new Key(request.key, request.id, request.date_expected_start, request.date_expected_end, request.status));
                    }
                }
                SceneManager.LoadScene("scene-keys");
            }
            else
            {
                ButtonState.StartRequest(new Button[] {btnLogin}, txtMsg, "Erro inesperado: " + jsonLoadUserKeys.code, panelMsg);
            }
        }
    }
}
