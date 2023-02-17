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
        ButtonState.StartRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Carregando...", panelMsg);
        string sKey = Current.currentRoom.ToString();
        string sTimeStart = Current.currentTimeStart;
        string sTimeEnd = Current.currentTimeEnd;
        string sDateDay = Current.currentDateDay;
        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    private IEnumerator GetRequestKey(string SKey, string SDateDay, string STimeStart, string STimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(URLs.apiURL + URLs.requestCreateURL + "?key=" + SKey + "&date_start=" + SDateDay + " " + STimeStart + "&date_end=" + SDateDay + " " + STimeEnd + "&token=" + User.user.UserToken);
        yield return requestRequestCreate.SendWebRequest();

        if(requestRequestCreate.result == UnityWebRequest.Result.ConnectionError | requestRequestCreate.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonRequestCreate = JsonUtility.FromJson<RequestCreateJson>(requestRequestCreate.downloadHandler.text);

            switch(jsonRequestCreate.code)
            {
                case "request_date_end_before_start":
                case "request_date_end_before_now":
                    ButtonState.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro: data inválida", panelMsg);
                    break;
                case "request_key_already_in_use":
                    ButtonState.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Essa chave já está sendo usada", panelMsg);
                    break;
                case "request_error_on_create":
                    ButtonState.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro ao criar o pedido", panelMsg);
                    break;
                case "request_created":
                    User.user.UserKeys.Add(new Key(Current.currentRoom, jsonRequestCreate.id, SDateDay + " " + STimeStart, SDateDay + " " + STimeEnd, RequestStatus.Status.not_started.ToString()));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    ButtonState.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso", panelMsg, true);

                    HandleFields.ClearFields(Dropdowns:new Dropdown[] {dpdStartTime, dpdEndTime, dpdWeekDay}, InputFields:new InputField[] {inputStartHour, inputStartMin, inputEndHour, inputEndMin});
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndRequest(new Button[] {btnConfirmRequest, btnCancelRequest, btnClose}, txtMsg, "Erro inesperado: " + jsonRequestCreate.code, panelMsg);
                    break;
            }
        }
        panelConfirm.SetActive(false);
        panelRequest.SetActive(true);
    }
}
