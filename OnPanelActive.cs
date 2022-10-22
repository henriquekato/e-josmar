using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnPanelActive : MonoBehaviour
{
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtHolder;
    [SerializeField] Button btnRequestKey;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestListJson jsonRequestList;

    void OnEnable()
    {
        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);
        Utilities.UpdateDropdownList(dpdRequestsList, sKey);
        VerifyTime.UpdateDpdWeekDays(dpdWeekDay);

        Utilities.ClearFields(Dropdowns:new Dropdown[] {dpdStartTime, dpdEndTime, dpdWeekDay}, InputFields:new InputField[] {inputStartHour, inputStartMin, inputEndHour, inputEndMin}, TxtMsg:txtMsg, PanelMsg:panelMsg);

        Utilities.StartRequest(new Button[] {btnRequestKey}, txtHolder, "Carregando informações...");

        string sDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        StartCoroutine(GetKeyStatus(sKey, sDate));
    }

    private IEnumerator GetKeyStatus(string SKey, string SDate)
    {
        UnityWebRequest requestRequestList = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestListURL + "?key=" + SKey + "&status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "&date_start=" + SDate + "&token=" + User.user.UserToken);
        yield return requestRequestList.SendWebRequest();

        if(requestRequestList.result == UnityWebRequest.Result.ConnectionError | requestRequestList.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Erro de conexão");
        }
        else
        {
            jsonRequestList = JsonUtility.FromJson<RequestListJson>(requestRequestList.downloadHandler.text);

            if(jsonRequestList.code == "request_list")
            {
                if(jsonRequestList.count > 0)
                {
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala ocupada por: " + jsonRequestList.list[0].user_name + ", até " + jsonRequestList.list[0].date_expected_end.Substring(11));
                }
                else
                {
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala desocupada");
                }
            }
            else
            {
                Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Erro inesperado: " + jsonRequestList.code);
            }
        }
    }
}
