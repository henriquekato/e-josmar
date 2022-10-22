using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ConfirmRequestButton : MonoBehaviour
{
    [SerializeField] GameObject panelCoverConfirm;
    [SerializeField] GameObject panelConfirm;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Button btnRequestKey;

    private RequestCreateJson jsonRequestCreate;

    public void CancelRequest()
    {
        panelCoverConfirm.SetActive(false);
        panelConfirm.SetActive(false);
    }

    public void ConfirmRequest()
    {
        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);
        string sTimeStart = txtTimeStart.text;
        string sTimeEnd = txtTimeEnd.text;
        string sDateDay = txtDateDay.text;
        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    private IEnumerator GetRequestKey(string SKey, string SDateDay, string STimeStart, string STimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestCreateURL + "?key=" + SKey + "&date_start=" + SDateDay + " " + STimeStart + "&date_end=" + SDateDay + " " + STimeEnd + "&token=" + User.user.UserToken);
        yield return requestRequestCreate.SendWebRequest();

        if(requestRequestCreate.result == UnityWebRequest.Result.ConnectionError | requestRequestCreate.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonRequestCreate = JsonUtility.FromJson<RequestCreateJson>(requestRequestCreate.downloadHandler.text);

            switch(jsonRequestCreate.code)
            {
                case "request_date_end_before_start":
                case "request_date_end_before_now":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: data inválida", panelMsg);
                    break;
                case "request_key_already_in_use":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Essa chave já está sendo usada", panelMsg);
                    break;
                case "request_error_on_create":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro ao criar o pedido", panelMsg);
                    break;
                case "request_created":
                    string sRoomNumber = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6, 1) : txtThisRoom.text.Substring(13, 1);
                    int iRoomNumber;
                    Int32.TryParse(sRoomNumber, out iRoomNumber);
                    User.user.UserKeys.Add(new Key(iRoomNumber, jsonRequestCreate.id, SDateDay + " " + STimeStart, SDateDay + " " + STimeEnd, (int)Utilities.Status.not_started));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    panelCoverConfirm.SetActive(false);
                    panelConfirm.SetActive(false);

                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso", panelMsg, true);

                    Utilities.ClearFields(Dropdowns:new Dropdown[] {dpdStartTime, dpdEndTime, dpdWeekDay}, InputFields:new InputField[] {inputStartHour, inputStartMin, inputEndHour, inputEndMin});
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro inesperado: " + jsonRequestCreate.code, panelMsg);
                    break;
            }
        }
    }
}
