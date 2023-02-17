using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CheckKeys : MonoBehaviour
{
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;
    [SerializeField] Button btnRoom1;
    [SerializeField] Button btnRoom2;
    [SerializeField] Button btnRoom3;
    [SerializeField] Button btnRoom4;
    [SerializeField] Button btnAllRequests;
    [SerializeField] Button btnEditUser;

    private RequestGetJson jsonRequestGetStarted;
    private RequestGetJson jsonRequestGetEnded;

    private void Start()
    {
        for(int i = 0; i < User.user.UserKeys.Count; i++)
        {
            if(User.user.UserKeys[i].status == RequestStatus.Status.start_request.ToString())
            {
                ButtonState.StartRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Liberando chave " + User.user.UserKeys[i].roomNumber.ToString() + "...", panelMsg);
                StartCoroutine(GetStartedKeyStatus(User.user.UserKeys[i]));
            }
            else if(User.user.UserKeys[i].status == RequestStatus.Status.end_request.ToString())
            {
                ButtonState.StartRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Devolvendo chave " + User.user.UserKeys[i].roomNumber.ToString() + "...", panelMsg);
                StartCoroutine(GetEndedKeyStatus(User.user.UserKeys[i]));
            }
        }
    }

    private IEnumerator GetStartedKeyStatus(Key key)
    {
        UnityWebRequest requestGetKeyStatus = UnityWebRequest.Get(URLs.apiURL + URLs.requestGetURL + "?id=" + key.requestId.ToString() + "&token=" + User.user.UserToken);
        yield return requestGetKeyStatus.SendWebRequest();

        if(requestGetKeyStatus.result == UnityWebRequest.Result.ConnectionError | requestGetKeyStatus.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonRequestGetStarted = JsonUtility.FromJson<RequestGetJson>(requestGetKeyStatus.downloadHandler.text);

            switch(jsonRequestGetStarted.code)
            {
                case "request_got":
                    if(jsonRequestGetStarted.request.status == "started")
                    {
                        int i = User.user.UserKeys.IndexOf(key);
                        User.user.UserKeys[i].status = RequestStatus.Status.started.ToString();

                        ButtonState.EndRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Chave " + key.roomNumber.ToString() + " liberada com sucesso", panelMsg, Success:true);
                    }
                    else
                    {
                        yield return new WaitForSeconds(5);
                        StartCoroutine(GetStartedKeyStatus(key));
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Erro inesperado: " + jsonRequestGetStarted.code, panelMsg);
                    break;
            }
        }
    }

    private IEnumerator GetEndedKeyStatus(Key key)
    {
        UnityWebRequest requestGetKeyStatus = UnityWebRequest.Get(URLs.apiURL + URLs.requestGetURL + "?id=" + key.requestId.ToString() + "&token=" + User.user.UserToken);
        yield return requestGetKeyStatus.SendWebRequest();

        if(requestGetKeyStatus.result == UnityWebRequest.Result.ConnectionError | requestGetKeyStatus.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonRequestGetEnded = JsonUtility.FromJson<RequestGetJson>(requestGetKeyStatus.downloadHandler.text);

            switch(jsonRequestGetEnded.code)
            {
                case "request_got":
                    if(jsonRequestGetEnded.request.status == "ended")
                    {
                        User.user.UserKeys.Remove(key);

                        ButtonState.EndRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Chave " + key.roomNumber.ToString() + " devolvida com sucesso", panelMsg, Success:true);
                    }
                    else
                    {
                        yield return new WaitForSeconds(5);
                        StartCoroutine(GetEndedKeyStatus(key));
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndRequest(new Button[] {btnRoom1, btnRoom2, btnRoom3, btnRoom4, btnAllRequests, btnEditUser}, txtMsg, "Erro inesperado: " + jsonRequestGetStarted.code, panelMsg);
                    break;
            }
        }
    }
}
