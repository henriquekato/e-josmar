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
        askToStarted = 2,
        started = 3,
        askToEnded = 4,
        ended = 5,
        canceled = 6
    }

    public static Key WhichRequest(Dropdown DpdRequestsList)
    {
        string sId = (DpdRequestsList.options[DpdRequestsList.value].text).Substring(7);
        int iId;
        Int32.TryParse(sId, out iId);

        foreach(Key key in User.user.UserKeys)
        {
            if(Id == key.requestId)
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
                if(Status == (int)Utilities.Status.started)
                {
                    TxtStatus.text = "Status: começado";
                    BtnReturnKey.interactable = true;
                }
                else if(Status == (int)Utilities.Status.canceled | Status == (int)Utilities.Status.ended)
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
}
