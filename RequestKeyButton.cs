using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RequestKeyButton : MonoBehaviour
{
    [SerializeField] Text txtThisRoom;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Button btnRequestKey;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestCreateJson jsonRequestCreate;

    public void RequestKey()
    {
        Utilities.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Carregando", panelMsg);

        string sTimeStart = VerifyTime.TimeStart(dpdStartTime, inputStartHour, inputStartMin);
        if(sTimeStart is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido");
            return;
        }

        string sTimeEnd = VerifyTime.TimeEnd(dpdEndTime, inputEndHour, inputEndMin);
        if(sTimeEnd is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido");
            return;
        }

        string sDateDay = VerifyTime.VerifyDpdWeekDay(dpdWeekDay);

        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);

        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    IEnumerator GetRequestKey(string SKey, string SDateDay, string STimeStart, string STimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(Utilities.apiURL + "/api/request/create?user=" + User.user.UserId + "&key=" + SKey + "&date_start=" + SDateDay + " " + STimeStart + "&date_end=" + SDateDay + " " + STimeEnd + "&token=" + User.user.UserToken);
        yield return requestRequestCreate.SendWebRequest();

        if(requestRequestCreate.result == UnityWebRequest.Result.ConnectionError | requestRequestCreate.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro de conexão");
        }
        else
        {
            jsonRequestCreate = JsonUtility.FromJson<RequestCreateJson>(requestRequestCreate.downloadHandler.text);

            switch(jsonRequestCreate.code)
            {
                case "request_date_end_before_start":
                case "request_date_end_before_now":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: data inválida");
                    break;
                case "request_key_already_in_use":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: essa chave já está sendo usada");
                    break;
                case "request_error_on_create":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: erro ao criar o pedido");
                    break;
                case "request_created":
                    int iRoomNumber;
                    int iNumberPos = txtThisRoom.text.Substring(0, 1) == "S" ? 6 : 13;
                    Int32.TryParse(txtThisRoom.text.Substring(iNumberPos), out iRoomNumber);
                    User.user.UserKeys.Add(new Key(iRoomNumber, jsonRequestCreate.id, SDateDay + " " + STimeStart, SDateDay + " " + STimeEnd, (int)Utilities.Status.not_started));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso");

                    Utilities.ClearFields(dpdStartTime, dpdEndTime, inputStartHour, inputStartMin, inputEndHour, inputEndMin, dpdWeekDay);
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro inesperado: " + jsonRequestCreate.code);
                    break;
            }
        }
    }
}
