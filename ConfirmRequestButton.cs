using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ConfirmRequestButton : MonoBehaviour
{
    [SerializeField] GameObject panelConfirm;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;
    [SerializeField] Button btnConfirmRequest;
    [SerializeField] Button btnCancelRequest;
    [SerializeField] Button btnClose;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;
    [SerializeField] GameObject panelRequest;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Dropdown dpdRequestsList;

    private RequestCreateJson jsonRequestCreate;

    public void ConfirmRequest()
    {
        Utilities.StartRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Carregando...", panelMsg);
        string sKey = Utilities.currentKey.ToString();
        string sTimeStart = Utilities.currentTimeStart;
        string sTimeEnd = Utilities.currentTimeEnd;
        string sDateDay = Utilities.currentDateDay;
        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    private IEnumerator GetRequestKey(string SKey, string SDateDay, string STimeStart, string STimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestCreateURL + "?key=" + SKey + "&date_start=" + SDateDay + " " + STimeStart + "&date_end=" + SDateDay + " " + STimeEnd + "&token=" + User.user.UserToken);
        yield return requestRequestCreate.SendWebRequest();

        if(requestRequestCreate.result == UnityWebRequest.Result.ConnectionError | requestRequestCreate.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonRequestCreate = JsonUtility.FromJson<RequestCreateJson>(requestRequestCreate.downloadHandler.text);

            switch(jsonRequestCreate.code)
            {
                case "request_date_end_before_start":
                case "request_date_end_before_now":
                    Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro: data inválida", panelMsg);
                    break;
                case "request_key_already_in_use":
                    Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Essa chave já está sendo usada", panelMsg);
                    break;
                case "request_error_on_create":
                    Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro ao criar o pedido", panelMsg);
                    break;
                case "request_created":
                    User.user.UserKeys.Add(new Key(Utilities.currentKey, jsonRequestCreate.id, SDateDay + " " + STimeStart, SDateDay + " " + STimeEnd, Utilities.Status.not_started.ToString()));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso", panelMsg, true);

                    Utilities.ClearFields(Dropdowns:new Dropdown[] {dpdStartTime, dpdEndTime, dpdWeekDay}, InputFields:new InputField[] {inputStartHour, inputStartMin, inputEndHour, inputEndMin});
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro inesperado: " + jsonRequestCreate.code, panelMsg);
                    break;
            }
        }
        panelConfirm.SetActive(false);
        panelRequest.SetActive(true);
    }
}
