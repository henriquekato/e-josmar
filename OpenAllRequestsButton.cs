using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAllRequestsButton : MonoBehaviour
{
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelAllRequests;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    public void OpenAllRequests()
    {
        Utilities.UpdateDropdownAllRequests(dpdRequestsList);
        Utilities.ClearFields(TxtMsg:txtMsg, PanelMsg:panelMsg);
        panelCover.SetActive(true);
        panelAllRequests.SetActive(true);
    }
}
