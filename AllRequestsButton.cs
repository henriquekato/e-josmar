using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AllRequestsButton : MonoBehaviour
{
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelAllRequests;

    public void AllRequests()
    {
        Utilities.UpdateDropdownAllRequests(dpdRequestsList);
        panelCover.SetActive(true);
        panelAllRequests.SetActive(true);
    }
}
