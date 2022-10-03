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

    private RequestListJson jsonRequestList;

    void OnEnable()
    {
        Utilities.StartRequest(new Button[] {btnRequestKey}, txtHolder, "Carregando informações");

        string sKey = txtThisRoom.text.Substring(6);

        string sDate = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd") + " " + System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss");

        StartCoroutine(GetKeyStatus(sKey, sDate));
    }

    IEnumerator GetKeyStatus(string SKey, string SDate)
    {
        UnityWebRequest requestRequestList = UnityWebRequest.Get(Utilities.apiURL + "/api/request/list?user=" + User.user.UserId + "&key=" + SKey + "&status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "&date_start=" + SDate + "&token=" + User.user.UserToken);
        yield return requestRequestList.SendWebRequest();

        if(requestRequestList.result == UnityWebRequest.Result.ConnectionError | requestRequestList.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sem conexão");
        }
        else
        {
            jsonRequestList = JsonUtility.FromJson<RequestListJson>(requestRequestList.downloadHandler.text);

            if(jsonRequestList.count > 0)
            {
                Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala ocupada por: " + jsonRequestList.list[0].user_name + ", até " + jsonRequestList.list[0].date_expected_end.Substring(11));
            }
            else
            {
                Utilities.EndRequest(new Button[] {btnRequestKey}, txtHolder, "Sala desocupada");
            }
        }
    }
}
