using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities
{
    public static string apiURL;
    public const string authURL = "/api/auth";
    public const string editUserURL = "/api/user/edit";
    public const string requestCreateURL = "/api/request/create";
    public const string requestUpdateStatusURL = "/api/request/update_status";
    public const string requestListURL = "/api/request/list";
    public const string requestGetURL = "/api/request/get";

    public enum Status
    {
        not_started = 1,
        start_request = 2,
        started = 3,
        end_request = 4,
        ended = 5,
        canceled = 6
    }

    public enum Rooms
    {
        sala1 = 1,
        sala2 = 2,
        lab3 = 3,
        lab4 = 4
    }

    public static int currentRoom;
    public static string currentTimeStart;
    public static string currentTimeEnd;
    public static string currentDateDay;
    public static Key currentKey;

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
        foreach(Button button in Buttons)
        {
            button.interactable = true;
        }
        TxtMsg.text = Msg;
        if(!(PanelMsg is null))
        {
            if(!(PanelMsg.activeInHierarchy)) PanelMsg.SetActive(true);
            Image panelImg = PanelMsg.GetComponent<Image>();
            panelImg.color = Success ? new Color(0, 255, 0, 210) : new Color(255, 0, 0, 210);
        }
    }

    public static void EndUpdateRequest(Button BtnReturn, Button BtnStart, Button BtnCancel, Button BtnReturnKey, Button BtnClose, Text TxtMsg, string Msg, Text TxtStatus = null, GameObject PanelMsg = null, bool Connection = false, bool Success = false, string Status = "", Key _Key = null)
    {
        TxtMsg.text = Msg;

        BtnReturn.interactable = true;
        BtnClose.interactable = true;

        if(!(PanelMsg is null))
        {
            Image panelImg = PanelMsg.GetComponent<Image>();
            panelImg.color = Success ? new Color(0, 255, 0, 210) : new Color(255, 0, 0, 210);
        }
        if(Connection)
        {
            if(Success)
            {
                if(Status == Utilities.Status.start_request.ToString())
                {
                    TxtStatus.text = "Status: em uso";
                    BtnReturnKey.interactable = true;
                }
                else if(Status == Utilities.Status.canceled.ToString() | Status == Utilities.Status.end_request.ToString())
                {
                    TxtStatus.text = "Status: finalizado";
                }
            }
            else
            {
                if(_Key.status == Utilities.Status.not_started.ToString())
                {
                    BtnStart.interactable = true;
                    BtnCancel.interactable = true;
                }
                else if(_Key.status == Utilities.Status.started.ToString())
                {
                    BtnReturnKey.interactable = true;
                }
            }
        }
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
