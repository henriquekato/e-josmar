using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AllRequestsButton : MonoBehaviour
{
    [SerializeField] Dropdown dpdRequestsList;

    public void AllRequests()
    {
        Utilities.UpdateDropdownAllRequests(dpdRequestsList);
    }
}
