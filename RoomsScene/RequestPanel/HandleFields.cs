using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleFields : MonoBehaviour
{
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
