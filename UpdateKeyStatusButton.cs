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

    private bool TimeOk(Key key)
    {
        int iYear, iMonth, iDay, iHour, iMin, iSec;
        Int32.TryParse(key.dateStart.Substring(0, 4), out iYear);
        Int32.TryParse(key.dateStart.Substring(5, 2), out iMonth);
        Int32.TryParse(key.dateStart.Substring(8, 2), out iDay);
        Int32.TryParse(key.dateStart.Substring(11, 2), out iHour);
        Int32.TryParse(key.dateStart.Substring(14, 2), out iMin);
        Int32.TryParse(key.dateStart.Substring(17, 2), out iSec);
        DateTime keyDateStart = new DateTime(iYear, iMonth, iDay, iHour, iMin, iSec);

        bool isOk = DateTime.Now < keyDateStart ? false : true;
        return isOk;
    }

    public void UpdateKeyStatusToStart()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);

        Key key = Utilities.WhichRequest(dpdRequestsList);
        
        // if(TimeOk(key))
        // {
            StartCoroutine(PostUpdateKeyStatus(key, Utilities.Status.started.ToString()));
        // }
        // else
        // {
        //     Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "A hora do seu pedido ainda não chegou", Connection:true, _Key:key);
        //     return;
        // }
    }

    public void UpdateKeyStatusToCancel()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);
        Key key = Utilities.WhichRequest(dpdRequestsList);
        StartCoroutine(PostUpdateKeyStatus(key, Utilities.Status.canceled.ToString()));
    }

    public void UpdateKeyStatusToEnded()
    {
        Utilities.StartRequest(new Button[] {btnReturn, btnStart, btnCancel, btnReturnKey}, txtMsg, "Carregando", panelMsg);
        Key key = Utilities.WhichRequest(dpdRequestsList);
        StartCoroutine(PostUpdateKeyStatus(key, Utilities.Status.ended.ToString()));
    }

    IEnumerator PostUpdateKeyStatus(Key key, string Status)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", key.requestId);
        form.AddField("status", Status);
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
                case "request_not_found":
                    Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Erro: pedido não encontrado", Connection: true, _Key:key);
                    break;
                case "request_error_on_update_status":
                    Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Erro: erro ao devolver a chave", Connection: true, _Key:key);
                    break;
                case "request_updated_status":
                    if(Status == Utilities.Status.started.ToString())
                    {
                        int i = User.user.UserKeys.IndexOf(key);
                        User.user.UserKeys[i].status = (int)Utilities.Status.started;

                        Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Chave liberada com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:(int)Utilities.Status.started, _Key:key);
                    }
                    else if(Status == Utilities.Status.canceled.ToString())
                    {
                        User.user.UserKeys.Remove(key);

                        Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Pedido cancelado com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:(int)Utilities.Status.canceled, _Key:key);
                    }
                    else if(Status == Utilities.Status.ended.ToString())
                    {
                        User.user.UserKeys.Remove(key);

                        Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, "Chave devolvida com sucesso", TxtStatus:txtStatus, Connection:true, Success:true, Status:(int)Utilities.Status.ended, _Key:key);
                    }
                    break;
                default:
                    Utilities.EndUpdateRequest(btnReturn, btnStart, btnCancel, btnReturnKey, txtMsg, jsonRequestUpdate.code, TxtStatus:txtStatus, Connection:true);
                    break;
            }
        }
    }
}
