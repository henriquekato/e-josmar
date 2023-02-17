using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllRequestsDropdown : MonoBehaviour
{
    [SerializeField] GameObject panelAllRequests;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] Text txtNextRoom;
    [SerializeField] Text txtRequestId;
    [SerializeField] Text txtStatus;
    [SerializeField] Text txtTimeStart;
    [SerializeField] Text txtTimeEnd;
    [SerializeField] Text txtDateDay;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnReturnKey;

    public void OnRequestSelected()
    {
        if(dpdRequestsList.interactable)
        {
            Key key = HandleDropdown.WhichRequest(dpdRequestsList);
            Current.currentKey = key;
            
            Current.currentRoom = key.roomNumber;

            string room = ((Rooms.Room)Current.currentRoom).ToString().Substring(0, 1);

            txtNextRoom.text = room == "s" ? "Sala: " + key.roomNumber.ToString() : "Laboratório: " + key.roomNumber.ToString();
            txtRequestId.text = "Pedido " + key.requestId.ToString();

            string sStatus = "";
            if(key.status == RequestStatus.Status.not_started.ToString())
            {
                sStatus = "não começado";

                btnStart.interactable = true;
                btnCancel.interactable = true;
                btnReturnKey.interactable = false;
            }
            else if(key.status == RequestStatus.Status.started.ToString())
            {
                sStatus = "em uso";

                btnStart.interactable = false;
                btnCancel.interactable = false;
                btnReturnKey.interactable = true;
            }
            txtStatus.text = "Status: " + sStatus;
            txtTimeStart.text = "Hora inicial: " + key.dateStart.Substring(11);
            txtTimeEnd.text = "Hora final: " + key.dateEnd.Substring(11);
            string sYear = key.dateStart.Substring(0, 4);
            string sMonth = key.dateStart.Substring(5, 2);
            string sDay = key.dateStart.Substring(8, 2);
            txtDateDay.text = "Dia: " + sDay + "/" + sMonth + "/" + sYear;

            panelAllRequests.SetActive(false);
            panelUpdateRequest.SetActive(true);
        }
    }
}
