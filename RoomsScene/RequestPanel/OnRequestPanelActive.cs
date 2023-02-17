using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnRequestPanelActive : MonoBehaviour
{
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtHolder;
    [SerializeField] Button btnRequestKey;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestListJson jsonRequestList;

    void OnEnable()
    {
        HandleDropdown.UpdateDropdownRoomRequests(dpdRequestsList, Current.currentRoom);
        VerifyTime.UpdateDpdWeekDays(dpdWeekDay);

        ButtonState.StartRequest(new Button[] {btnRequestKey}, txtHolder, "Carregando informações...");

        string sDateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        StartCoroutine(GetKeyStatus(Current.currentRoom, sDateNow));
    }

    private IEnumerator GetKeyStatus(int IKey, string SDateNow)
    {
        UnityWebRequest requestRequestList = UnityWebRequest.Get(URLs.apiURL + URLs.requestListURL + "?key=" + IKey.ToString() + "&status=" + ((int)RequestStatus.Status.not_started).ToString() + "|" + ((int)RequestStatus.Status.start_request).ToString() + "|" + ((int)RequestStatus.Status.started).ToString() + "|" + ((int)RequestStatus.Status.end_request) + "&date_start=" + SDateNow + "&token=" + User.user.UserToken);
        yield return requestRequestList.SendWebRequest();

        if(requestRequestList.result == UnityWebRequest.Result.ConnectionError | requestRequestList.result == UnityWebRequest.Result.ProtocolError)
        {
            ButtonState.StartRequest(new Button[] {btnRequestKey}, txtHolder, "Erro de conexão");
        }
        else
        {
            jsonRequestList = JsonUtility.FromJson<RequestListJson>(requestRequestList.downloadHandler.text);

            switch(jsonRequestList.code)
            {
                case "request_list":
                    if(jsonRequestList.count > 0)
                    {
                        ButtonState.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala ocupada por: " + jsonRequestList.list[0].user_name + ", até " + jsonRequestList.list[0].date_expected_end.Substring(11));
                    }
                    else
                    {
                        ButtonState.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala desocupada");
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    ButtonState.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Erro inesperado: " + jsonRequestList.code);
                    break;
            }
        }
    }
}
