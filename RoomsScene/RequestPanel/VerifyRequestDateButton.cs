using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VerifyRequestDateButton : MonoBehaviour
{
    [SerializeField] GameObject panelRequest;
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
    [SerializeField] GameObject panelConfirm;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;

    public void VerifyRequestDate()
    {
        string sTimeStart = VerifyTime.TimeStart(dpdStartTime, inputStartHour, inputStartMin);
        if(sTimeStart is null)
        {
            ButtonState.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido", panelMsg);
            return;
        }

        string sTimeEnd = VerifyTime.TimeEnd(dpdEndTime, inputEndHour, inputEndMin);
        if(sTimeEnd is null)
        {
            ButtonState.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido", panelMsg);
            return;
        }

        string sDateDay = VerifyTime.VerifyDpdWeekDay(dpdWeekDay);

        txtNextRoom.text = txtThisRoom.text;
        txtTimeStart.text = "Hora inicial: " + sTimeStart;
        txtTimeEnd.text = "Hora final: " + sTimeEnd;
        string sYear = sDateDay.Substring(0, 4);
        string sMonth = sDateDay.Substring(5, 2);
        string sDay = sDateDay.Substring(8, 2);
        txtDateDay.text = "Dia: " + sDay + "/" + sMonth + "/" + sYear;
        Current.currentTimeStart = sTimeStart;
        Current.currentTimeEnd = sTimeEnd;
        Current.currentDateDay = sDateDay;

        HandleFields.ClearFields(TxtMsg:txtMsg, PanelMsg:panelMsg);

        panelRequest.SetActive(false);
        panelConfirm.SetActive(true);
    }
}
