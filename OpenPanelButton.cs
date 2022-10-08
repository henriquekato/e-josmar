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

    private RequestListJson jsonRequestList;

    public void OpenPanel()
    {
        string sKey = txtThisRoom.text.Substring(1);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;

        panelCover.SetActive(true);
        panelRequest.SetActive(true);
    }

    public void SwitchPanel()
    {
        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;
        
        panelRequest.SetActive(true);
        panelUpdateRequest.SetActive(false);
    }
}
