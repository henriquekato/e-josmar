using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UpdateKeyStatusButton : MonoBehaviour
{
    [SerializeField] Text txtStatus;
    [SerializeField] Button btnReturn;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnReturnKey;
    [SerializeField] Button btnClose;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestUpdateJson jsonRequestUpdate;
    private RequestGetJson jsonRequestGetStarted;
    private RequestGetJson jsonRequestGetEnded;

    public void UpdateKeyStatusToStart()
    {
        ButtonState.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey, btnClose}, txtMsg, "Carregando...", panelMsg);

        Key key = Current.currentKey;
        
        if(VerifyTime.TimeOk(key))
        {
            StartCoroutine(PostUpdateKeyStatus(key, RequestStatus.Status.start_request.ToString()));
        }
        else
        {
            ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "A hora do seu pedido ainda não chegou", PanelMsg:panelMsg, Connection:true, _Key:key);
            return;
        }
    }

    public void UpdateKeyStatusToEnded()
    {
        ButtonState.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey, btnClose}, txtMsg, "Carregando...", panelMsg);
        Key key = Current.currentKey;
        StartCoroutine(PostUpdateKeyStatus(key, RequestStatus.Status.end_request.ToString()));
    }

    public void UpdateKeyStatusToCancel()
    {
        ButtonState.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey, btnClose}, txtMsg, "Carregando...", panelMsg);
        Key key = Current.currentKey;
        StartCoroutine(PostUpdateKeyStatus(key, RequestStatus.Status.canceled.ToString()));
    }

    private IEnumerator PostUpdateKeyStatus(Key key, string SStatus)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", key.requestId.ToString());
        form.AddField("status", SStatus);
        form.AddField("token", User.user.UserToken);
        
        UnityWebRequest requestRequestUpdate = UnityWebRequest.Post(URLs.apiURL + URLs.requestUpdateStatusURL, form);
        requestRequestUpdate.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return requestRequestUpdate.SendWebRequest();

        if(requestRequestUpdate.result == UnityWebRequest.Result.ConnectionError | requestRequestUpdate.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro de conexão", PanelMsg:panelMsg);
        }
        else
        {
            jsonRequestUpdate = JsonUtility.FromJson<RequestUpdateJson>(requestRequestUpdate.downloadHandler.text);

            switch(jsonRequestUpdate.code)
            {
                case "request_error_on_update_status":
                    ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro ao atualizar status do pedido", PanelMsg:panelMsg, Connection: true, _Key:key);
                    break;
                case "request_updated_status":
                    int i = User.user.UserKeys.IndexOf(key);
                    if(SStatus == RequestStatus.Status.start_request.ToString())
                    {
                        txtMsg.text = "Liberando chave " + key.roomNumber.ToString() + "...";
                        User.user.UserKeys[i].status = RequestStatus.Status.start_request.ToString();
                        StartCoroutine(GetStartedKeyStatus(key, SStatus));
                    }
                    else if(SStatus == RequestStatus.Status.end_request.ToString())
                    {
                        txtMsg.text = "Devolvendo chave " + key.roomNumber.ToString() + "...";
                        User.user.UserKeys[i].status = RequestStatus.Status.end_request.ToString();
                        StartCoroutine(GetEndedKeyStatus(key, SStatus));
                    }
                    else if(SStatus == RequestStatus.Status.canceled.ToString())
                    {
                        User.user.UserKeys.Remove(key);

                        ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Pedido cancelado com sucesso", TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, Success:true, Status:SStatus, _Key:key);
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro inesperado: " + jsonRequestUpdate.code, TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, _Key:key);
                    break;
            }
        }
    }

    private IEnumerator GetStartedKeyStatus(Key key, string SStatus)
    {
        UnityWebRequest requestGetKeyStatus = UnityWebRequest.Get(URLs.apiURL + URLs.requestGetURL + "?id=" + key.requestId.ToString() + "&token=" + User.user.UserToken);
        yield return requestGetKeyStatus.SendWebRequest();

        if(requestGetKeyStatus.result == UnityWebRequest.Result.ConnectionError | requestGetKeyStatus.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro de conexão", PanelMsg:panelMsg);
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

                        ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Chave " + key.roomNumber.ToString() + " liberada com sucesso", TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, Success:true, Status:SStatus, _Key:key);
                    }
                    else
                    {
                        yield return new WaitForSeconds(5);
                        StartCoroutine(GetStartedKeyStatus(key, SStatus));
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro inesperado: " + jsonRequestGetStarted.code, TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, _Key:key);
                    break;
            }
        }
    }

    private IEnumerator GetEndedKeyStatus(Key key, string SStatus)
    {
        UnityWebRequest requestGetKeyStatus = UnityWebRequest.Get(URLs.apiURL + URLs.requestGetURL + "?id=" + key.requestId.ToString() + "&token=" + User.user.UserToken);
        yield return requestGetKeyStatus.SendWebRequest();

        if(requestGetKeyStatus.result == UnityWebRequest.Result.ConnectionError | requestGetKeyStatus.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro de conexão", PanelMsg:panelMsg);
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

                        ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Chave " + key.roomNumber.ToString() + " devolvida com sucesso", TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, Success:true, Status:SStatus, _Key:key);
                    }
                    else
                    {
                        yield return new WaitForSeconds(5);
                        StartCoroutine(GetEndedKeyStatus(key, SStatus));
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, btnClose, txtMsg, "Erro inesperado: " + jsonRequestGetEnded.code, TxtStatus:txtStatus, PanelMsg:panelMsg, Connection:true, _Key:key);
                    break;
            }
        }
    }
}
