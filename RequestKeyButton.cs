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
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;
    [SerializeField] GameObject panelCoverConfirm;
    [SerializeField] GameObject panelConfirm;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;

    public void RequestKey()
    {
        Utilities.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Carregando...", panelMsg);

        string sTimeStart = VerifyTime.TimeStart(dpdStartTime, inputStartHour, inputStartMin);
        if(sTimeStart is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido", panelMsg);
            return;
        }

        string sTimeEnd = VerifyTime.TimeEnd(dpdEndTime, inputEndHour, inputEndMin);
        if(sTimeEnd is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido", panelMsg);
            return;
        }

        string sDateDay = VerifyTime.VerifyDpdWeekDay(dpdWeekDay);

        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);

        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;
        txtTimeStart.text = sTimeStart;
        txtTimeEnd.text = sTimeEnd;
        txtDateDay.text = sDateDay;

        panelCoverConfirm.SetActive(true);
        panelConfirm.SetActive(true);
    }
}
