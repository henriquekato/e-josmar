using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenRequestPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    public void OpenRequestPanel()
    {
        string sKey = txtThisRoom.text.Substring(1);
        Int32.TryParse(sKey, out Current.currentRoom);

        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + Current.currentRoom.ToString() : "Laboratório: " + Current.currentRoom.ToString();

        HandleFields.ClearFields(Dropdowns:new Dropdown[] {dpdStartTime, dpdEndTime, dpdWeekDay}, InputFields:new InputField[] {inputStartHour, inputStartMin, inputEndHour, inputEndMin}, TxtMsg:txtMsg, PanelMsg:panelMsg);

        panelCover.SetActive(true);
        panelRequest.SetActive(true);
    }
}
