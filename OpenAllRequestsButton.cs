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

    public void OpenAllRequests()
    {
        Utilities.UpdateDropdownAllRequests(dpdRequestsList);
        dpdRequestsList.Show();
        panelCover.SetActive(true);
        panelAllRequests.SetActive(true);
    }
}
