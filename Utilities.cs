using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities
{
    public static string apiURL;
    public static const string authURL = "/api/auth";
    public static const string editUserURL = "/api/user/edit";
    public static const string requestCreateURL = "/api/request/create";
    public static const string requestUpdateStatusURL = "/api/request/update_status";
    public static const string requestListURL = "/api/request/list";
    public static const string requestGetURL = "/api/request/get";

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

    public static void StartRequest(Button[] Buttons, Text TxtMsg, string Msg, GameObject PanelMsg = null)
    {
        if(!(PanelMsg is null)) 
        {
            PanelMsg.SetActive(true);
            Image panelImg = PanelMsg.GetComponent<Image>();
            panelImg.color = new Color(140, 140, 140, 80);
        }
        TxtMsg.text = Msg;
        foreach(Button button in Buttons)
        {
            button.interactable = false;
        }
    }

    public static void EndRequest(Button[] Buttons, Text TxtMsg, string Msg, GameObject PanelMsg = null, bool Success = false)
    {
        TxtMsg.text = Msg;
        foreach(Button button in Buttons)
        {
            button.interactable = true;
        }

        if(!(PanelMsg is null))
        {
            Image panelImg = PanelMsg.GetComponent<Image>();
            panelImg.color = Success ? new Color(0, 255, 0, 210) : new Color(255, 0, 0, 210);
        }
    }

    public static void EndUpdateRequest(Button BtnReturn, Button BtnStart, Button BtnCancel, Button BtnReturnKey, Text TxtMsg, string Msg, Text TxtStatus = null, GameObject PanelMsg = null, bool Connection = false, bool Success = false, int Status = 0, Key _Key = null)
    {
        TxtMsg.text = Msg;

        BtnReturn.interactable = true;

        if(!(PanelMsg is null))
        {
            Image panelImg = PanelMsg.GetComponent<Image>();
            panelImg.color = Success ? new Color(0, 255, 0, 210) : new Color(255, 0, 0, 210);
        }
        if(Connection)
        {
            if(Success)
            {
                if(Status == (int)Utilities.Status.start_request)
                {
                    TxtStatus.text = "Status: em uso";
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

    public static void ClearFields(Dropdown[] Dropdowns = null, InputField[] InputFields = null, Text TxtMsg = null, GameObject PanelMsg = null)
    {

        if(!(Dropdowns is null))
        {
            foreach(Dropdown dpd in Dropdowns)
            {
                dpd.value = 0;
                dpd.RefreshShownValue();
            }
        }

        if(!(InputFields is null))
        {
            foreach(InputField inpt in InputFields)
            {
                inpt.text = "";
            }
        }

        if(!(TxtMsg is null))
        {
            TxtMsg.text = "";
        }

        if(!(PanelMsg is null))
        {
            PanelMsg.SetActive(false);
            Image PanelImg = PanelMsg.GetComponent<Image>();
            PanelImg.color = new Color(140, 140, 140, 80);
        }
    }
}
