using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestListJson jsonRequestList;

    private void UpdateDropdownList(string SKey)
    {
        dpdRequestsList.interactable = false;
        dpdRequestsList.options.Clear();
        dpdRequestsList.options.Add(new Dropdown.OptionData("Pedidos"));
        foreach(Key key in User.user.UserKeys)
        {
            if(key.roomNumber.ToString() == SKey)
            {
                dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + key.requestId.ToString()));
            }
        }
        dpdRequestsList.value = 0;
        dpdRequestsList.RefreshShownValue();
        dpdRequestsList.interactable = true;
    }

    public void OpenPanel()
    {
        string sKey = txtThisRoom.text.Substring(1);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;
        
        UpdateDropdownList(sKey);

        panelCover.SetActive(true);
        panelRequest.SetActive(true);
    }

    public void SwitchPanel()
    {
        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);
        txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + sKey : "Laboratório: " + sKey;

        UpdateDropdownList(sKey);

        txtMsg.text = "";
        panelMsg.SetActive(false);
        
        panelUpdateRequest.SetActive(false);
        panelRequest.SetActive(true);

    }
}
