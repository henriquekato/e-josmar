using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AuthButton : MonoBehaviour
{
    public InputField inputurl;                                     //erro

    [SerializeField] InputField inputUser;
    [SerializeField] InputField inputPassword;
    [SerializeField] Button btnLogin;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private AuthJson jsonAuth;
    private RequestListJson jsonLoadUserKeys;

    public void Auth()
    {
        Utilities.apiURL = "https://80-mocno-serverjosmar-" + inputurl.text + ".gitpod.io";    //erro

        Utilities.StartRequest(new Button[] {btnLogin}, txtMsg, "Carregando", panelMsg);
        StartCoroutine(GetAuth());
    }

    IEnumerator GetAuth()
    {
        UnityWebRequest requestAuth = UnityWebRequest.Get(Utilities.apiURL + "/api/auth?login=" + inputUser.text + "&password=" + inputPassword.text);
        yield return requestAuth.SendWebRequest();

        if(requestAuth.result == UnityWebRequest.Result.ConnectionError | requestAuth.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro de conexão");
        }
        else
        {
            jsonAuth = JsonUtility.FromJson<AuthJson>(requestAuth.downloadHandler.text);
        
            switch(jsonAuth.code)
            {
                case "api_login_not_found":
                    Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro: usuário não encontrado");
                    break;
                case "api_wrong_password":
                    Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro: senha incorreta");
                    break;
                case "api_auth":
                    User.user.UserId = jsonAuth.id;
                    User.user.UserToken = jsonAuth.token;
                    Utilities.StartRequest(new Button[] {btnLogin}, txtMsg, "Carregando chaves");
                    StartCoroutine(GetLoadUserKeys());
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro inesperado: " + jsonAuth.code);
                    break;
            }
        }
    }

    IEnumerator GetLoadUserKeys()
    {
        UnityWebRequest requestLoadUserKeys = UnityWebRequest.Get(Utilities.apiURL + "/api/request/list?user=" + User.user.UserId + "&status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "&date_start=" + DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd") + " 00:00:00" + "&date_end=" + DateTime.UtcNow.ToLocalTime().ToString("yyyy") + "-12-31 23:59:59" + "&token=" + User.user.UserToken);
        yield return requestLoadUserKeys.SendWebRequest();

        if(requestLoadUserKeys.result == UnityWebRequest.Result.ConnectionError | requestLoadUserKeys.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro de conexão");
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
                Utilities.EndRequest(new Button[] {btnLogin}, txtMsg, "Erro inesperado: " + jsonLoadUserKeys.code);
            }
        }
    }
}
