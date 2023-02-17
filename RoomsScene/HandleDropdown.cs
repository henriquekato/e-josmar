using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleDropdown : MonoBehaviour
{
    public static Key WhichRequest(Dropdown DpdRequestsList)
    {
        string sId = (DpdRequestsList.options[DpdRequestsList.value].text).Substring(7);
        int keyIndex = 0;
        for(int i = 0; i < User.user.UserKeys.Count; i++)
        {
            if(sId == User.user.UserKeys[i].requestId.ToString())
            {
                keyIndex = i;
                break;
            }
        }
        return User.user.UserKeys[keyIndex];
    }

    public static void UpdateDropdownRoomRequests(Dropdown DpdRequestsList, int IKey)
    {
        DpdRequestsList.interactable = false;
        DpdRequestsList.options.Clear();
        DpdRequestsList.options.Add(new Dropdown.OptionData("Pedidos"));
        foreach(Key key in User.user.UserKeys)
        {
            if(key.roomNumber == IKey)
            {
                DpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + key.requestId.ToString()));
            }
        }
        DpdRequestsList.value = 0;
        DpdRequestsList.RefreshShownValue();
        DpdRequestsList.interactable = true;
    }

    public static void UpdateDropdownAllRequests(Dropdown DpdRequestsList)
    {
        DpdRequestsList.interactable = false;
        DpdRequestsList.options.Clear();
        DpdRequestsList.options.Add(new Dropdown.OptionData("Pedidos"));
        foreach(Key key in User.user.UserKeys)
        {
            DpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + key.requestId.ToString()));
        }
        DpdRequestsList.value = 0;
        DpdRequestsList.RefreshShownValue();
        DpdRequestsList.interactable = true;
    }
}
