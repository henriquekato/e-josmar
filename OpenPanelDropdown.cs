using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanelDropdown : MonoBehaviour
{
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] Text txtThisRoom;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Text txtRequestId;
    [SerializeField] Text txtStatus;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnReturnKey;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    public void OnRequestSelected()
    {
        if(dpdRequestsList.interactable == true)
        {
            Key key = Utilities.WhichRequest(dpdRequestsList);

            txtNextRoom.text = txtThisRoom.text.Substring(0, 1) == "S" ? "Sala: " + key.roomNumber.ToString() : "Laboratório: " + key.roomNumber.ToString();
            txtRequestId.text = "Pedido " + key.requestId.ToString();

            string sStatus = "";
            switch(key.status)
            {
                case (int)Utilities.Status.not_started:
                    sStatus = "não começado";

                    btnStart.interactable = true;
                    btnCancel.interactable = true;
                    btnReturnKey.interactable = false;
                    break;
                case (int)Utilities.Status.started:
                    sStatus = "em uso";

                    btnStart.interactable = false;
                    btnCancel.interactable = false;
                    btnReturnKey.interactable = true;
                    break;
            }
            txtStatus.text = "Status: " + sStatus;
            txtTimeStart.text = "Hora inicial: " + key.dateStart.Substring(11);
            txtTimeEnd.text = "Hora final: " + key.dateEnd.Substring(11);
            txtDateDay.text = "Dia: " + key.dateStart.Substring(0, 10);

            txtMsg.text = "";
            panelMsg.SetActive(false);

            panelRequest.SetActive(false);
            panelUpdateRequest.SetActive(true);
        }
    }
}