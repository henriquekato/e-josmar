using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUpdatePanelDropdown : MonoBehaviour
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
        if(dpdRequestsList.interactable)
        {
            Key key = Utilities.WhichRequest(dpdRequestsList);

            txtNextRoom.text = txtThisRoom.text;
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
            string sYear = key.dateStart.Substring(0, 4);
            string sMonth = key.dateStart.Substring(5, 2);
            string sDay = key.dateStart.Substring(8, 2);
            txtDateDay.text = "Dia: " + sDay + "/" + sMonth + "/" + sYear;

            Utilities.ClearFields(TxtMsg:txtMsg, PanelMsg:panelMsg);

            panelRequest.SetActive(false);
            panelUpdateRequest.SetActive(true);
        }
    }
}