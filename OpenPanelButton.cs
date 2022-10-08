using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtNextRoom;
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

    public void OpenPanel()
    {
        string sKey = txtThisRoom.text.Substring(1);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;
        
        Utilities.ClearFields(dpdStartTime, dpdEndTime, inputStartHour, inputStartMin, inputEndHour, inputEndMin, dpdWeekDay);
        
        Utilities.UpdateDropdownList(dpdRequestsList, sKey);

        panelCover.SetActive(true);
        panelRequest.SetActive(true);
    }

    public void SwitchPanel()
    {
        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;

        Utilities.ClearFields(dpdStartTime, dpdEndTime, inputStartHour, inputStartMin, inputEndHour, inputEndMin, dpdWeekDay);

        Utilities.UpdateDropdownList(dpdRequestsList, sKey);

        txtMsg.text = "";
        panelMsg.SetActive(false);
        
        panelRequest.SetActive(true);
        panelUpdateRequest.SetActive(false);
    }
}
