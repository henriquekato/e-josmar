using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UpdateKeyStatusButton : MonoBehaviour
{
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] Text txtStatus;
    [SerializeField] Button btnReturn;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnReturnKey;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestUpdateJson jsonRequestUpdate;

    public void UpdateKeyStatusToStart()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);

        Key key = Utilities.WhichRequest(dpdRequestsList);
        
        // if(Utilities.TimeOk(key))
        // {
            StartCoroutine(PostUpdateKeyStatus(key, (int)Utilities.Status.start_request));
        // }
        // else
        // {
        //     Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "A hora do seu pedido ainda não chegou", Connection:true, _Key:key);
        //     return;
        // }
    }

    public void UpdateKeyStatusToEnded()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);
        Key key = Utilities.WhichRequest(dpdRequestsList);
        StartCoroutine(PostUpdateKeyStatus(key, (int)Utilities.Status.end_request));
    }

    public void UpdateKeyStatusToCancel()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);
        Key key = Utilities.WhichRequest(dpdRequestsList);
        StartCoroutine(PostUpdateKeyStatus(key, (int)Utilities.Status.canceled));
    }

    IEnumerator PostUpdateKeyStatus(Key key, int IStatus)
    {
        string sStatus = "";
        switch(IStatus)
        {
            case (int)Utilities.Status.start_request:
                sStatus = "start_request";
                break;
            case (int)Utilities.Status.end_request:
                sStatus = "ended";
                break;
            case (int)Utilities.Status.canceled:
                sStatus = "canceled";
                break;
        }

        WWWForm form = new WWWForm();
        form.AddField("id", key.requestId);
        form.AddField("status", sStatus);
        form.AddField("token", User.user.UserToken);
        
        UnityWebRequest requestRequestUpdate = UnityWebRequest.Post(Utilities.apiURL + "/api/request/update_status", form);
        requestRequestUpdate.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return requestRequestUpdate.SendWebRequest();

        if(requestRequestUpdate.result == UnityWebRequest.Result.ConnectionError | requestRequestUpdate.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Erro de conexão");
        }
        else
        {
            jsonRequestUpdate = JsonUtility.FromJson<RequestUpdateJson>(requestRequestUpdate.downloadHandler.text);

            switch(jsonRequestUpdate.code)
            {
                case "request_error_on_update_status":
                    Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Erro: erro ao devolver a chave", Connection: true, _Key:key);
                    break;
                case "request_updated_status":
                    switch(IStatus)
                    {
                        case (int)Utilities.Status.start_request:
                            int i = User.user.UserKeys.IndexOf(key);
                            User.user.UserKeys[i].status = (int)Utilities.Status.started;

                            Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Chave liberada com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:IStatus, _Key:key);
                            break;
                        case (int)Utilities.Status.end_request:
                            User.user.UserKeys.Remove(key);

                            Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Chave devolvida com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:IStatus, _Key:key);
                            break;
                        case (int)Utilities.Status.canceled:
                            User.user.UserKeys.Remove(key);

                            Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Pedido cancelado com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:IStatus, _Key:key);
                            break;
                    }
                    break;
                default:
                    Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Erro inesperado: " + jsonRequestUpdate.code, TxtStatus:txtStatus, Connection:true);
                    break;
            }
        }
    }
}
