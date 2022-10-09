using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities
{
    public static string apiURL;

    public enum Status
    {
        not_started = 1,
        start_request = 2,
        started = 3,
        end_request = 4,
        ended = 5,
        canceled = 6
    }

    public static Key WhichRequest(Dropdown DpdRequestsList)
    {
        string sId = (DpdRequestsList.options[DpdRequestsList.value].text).Substring(7);

        foreach(Key key in User.user.UserKeys)
        {
            if(sId == key.requestId.ToString())
            {
                return key;
            }
        }
        return null;
    }

    public static void StartRequest(Button[] Buttons, Text TxtMsg, string Msg, GameObject PanelMsg = null)
    {
        if(!(PanelMsg is null)) PanelMsg.SetActive(true);
        TxtMsg.text = Msg;
        foreach(Button button in Buttons)
        {
            button.interactable = false;
        }
    }

    public static void EndRequest(Button[] Buttons, Text TxtMsg, string Msg)
    {
        TxtMsg.text = Msg;
        foreach(Button button in Buttons)
        {
            button.interactable = true;
        }
    }

    public static void EndUpdateRequest(Button BtnReturn, Button BtnStart, Button BtnCancel, Button BtnReturnKey, Text TxtMsg, string Msg, Text TxtStatus = null, bool Connection = false, bool Success = false, int Status = 0, Key _Key = null)
    {
        TxtMsg.text = Msg;

        BtnReturn.interactable = true;

        if(Connection)
        {
            if(Success)
            {
                if(Status == (int)Utilities.Status.start_request)
                {
                    TxtStatus.text = "Status: começado";
                    BtnReturnKey.interactable = true;
                }
                else if(Status == (int)Utilities.Status.canceled | Status == (int)Utilities.Status.end_request)
                {
                    TxtStatus.text = "Status: finalizado";
                }
            }
            else
            {
                if(_Key.status == (int)Utilities.Status.not_started)
                {
                    BtnStart.interactable = true;
                    BtnCancel.interactable = true;
                }
                else if(_Key.status == (int)Utilities.Status.started)
                {
                    BtnReturnKey.interactable = true;
                }
            }
        }
    }

    public static void UpdateDropdownList(Dropdown DpdRequestsList, string SKey)
    {
        DpdRequestsList.interactable = false;
        DpdRequestsList.options.Clear();
        DpdRequestsList.options.Add(new Dropdown.OptionData("Pedidos"));
        foreach(Key key in User.user.UserKeys)
        {
            if(key.roomNumber.ToString() == SKey)
            {
                DpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + key.requestId.ToString()));
            }
        }
        DpdRequestsList.value = 0;
        DpdRequestsList.RefreshShownValue();
        DpdRequestsList.interactable = true;
    }

    public static void ClearFields(Dropdown[] Dropdowns, InputField[] InputFields, Text TextMsg = null)
    {
        foreach(Dropdown dpd in Dropdowns)
        {
            dpd.value = 0;
            dpd.RefreshShownValue();
        }
        foreach(InputField inpt in InputFields)
        {
            inpt.text = "";
        }
        if(!(TextMsg is null))
        {
            TextMsg.text = "";
        }
    }
}
