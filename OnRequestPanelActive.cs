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
        Utilities.UpdateDropdownRoomRequests(dpdRequestsList, Utilities.currentRoom);
        VerifyTime.UpdateDpdWeekDays(dpdWeekDay);

        Utilities.StartRequest(new Button[] {btnRequestKey}, txtHolder, "Carregando informações...");

        string sDateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        StartCoroutine(GetKeyStatus(Utilities.currentRoom, sDateNow));
    }

    private IEnumerator GetKeyStatus(int IKey, string SDateNow)
    {
        UnityWebRequest requestRequestList = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestListURL + "?key=" + IKey.ToString() + "&status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.start_request).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "|" + ((int)Utilities.Status.end_request) + "&date_start=" + SDateNow + "&token=" + User.user.UserToken);
        yield return requestRequestList.SendWebRequest();

        if(requestRequestList.result == UnityWebRequest.Result.ConnectionError | requestRequestList.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Erro de conexão");
        }
        else
        {
            jsonRequestList = JsonUtility.FromJson<RequestListJson>(requestRequestList.downloadHandler.text);

            switch(jsonRequestList.code)
            {
                case "request_list":
                    if(jsonRequestList.count > 0)
                    {
                        Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala ocupada por: " + jsonRequestList.list[0].user_name + ", até " + jsonRequestList.list[0].date_expected_end.Substring(11));
                    }
                    else
                    {
                        Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala desocupada");
                    }
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Erro inesperado: " + jsonRequestList.code);
                    break;
            }
        }
    }
}
