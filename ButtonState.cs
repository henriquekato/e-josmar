using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonState : MonoBehaviour
{
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
                if(Status == RequestStatus.Status.start_request.ToString())
                {
                    TxtStatus.text = "Status: em uso";
                    BtnReturnKey.interactable = true;
                }
                else if(Status == RequestStatus.Status.canceled.ToString() | Status == RequestStatus.Status.end_request.ToString())
                {
                    TxtStatus.text = "Status: finalizado";
                }
            }
            else
            {
                if(_Key.status == RequestStatus.Status.not_started.ToString())
                {
                    BtnStart.interactable = true;
                    BtnCancel.interactable = true;
                }
                else if(_Key.status == RequestStatus.Status.started.ToString())
                {
                    BtnReturnKey.interactable = true;
                }
            }
        }
    }
}
