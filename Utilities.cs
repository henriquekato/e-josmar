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

    public static void ClearFields(Dropdown DpdStartTime, Dropdown DpdEndTime, InputField InputStartHour, InputField InputStartMin, InputField InputEndHour, InputField InputEndMin, Dropdown DpdWeekDay)
    {
        DpdStartTime.value = 0;
        DpdStartTime.RefreshShownValue();
        DpdEndTime.value = 0;
        DpdEndTime.RefreshShownValue();
        InputStartHour.text = "";
        InputStartMin.text = "";
        InputEndHour.text = "";
        InputEndMin.text = "";
        DpdWeekDay.value = 0;
        DpdWeekDay.RefreshShownValue();
    }
}

public class VerifyTime
{
    public static bool TimeOk(Key key)
    {
        int iYear, iMonth, iDay, iHour, iMin, iSec;
        Int32.TryParse(key.dateStart.Substring(0, 4), out iYear);
        Int32.TryParse(key.dateStart.Substring(5, 2), out iMonth);
        Int32.TryParse(key.dateStart.Substring(8, 2), out iDay);
        Int32.TryParse(key.dateStart.Substring(11, 2), out iHour);
        Int32.TryParse(key.dateStart.Substring(14, 2), out iMin);
        Int32.TryParse(key.dateStart.Substring(17, 2), out iSec);
        DateTime keyDateStart = new DateTime(iYear, iMonth, iDay, iHour, iMin, iSec);

        bool isOk = DateTime.Now < keyDateStart ? false : true;
        return isOk;
    }

    public static void UpdateDpdWeekDays(Dropdown DpdWeekDay)
    {
        List<string> WeekDayOptions = new List<string>();
        string sLongDate = DateTime.Now.ToLongDateString();

        string sDay = sLongDate.Substring(0, 2);
        switch(sLongDate.Substring(0, 2))
        {
            case "Sa":
            case "Su":
            case "Mo":
                WeekDayOptions.Add("Segunda-feira");
                WeekDayOptions.Add("Terça-feira");
                WeekDayOptions.Add("Quarta-feira");
                WeekDayOptions.Add("Quinta-feira");
                WeekDayOptions.Add("Sexta-feira");
                break;
            case "Tu":
                WeekDayOptions.Add("Terça-feira");
                WeekDayOptions.Add("Quarta-feira");
                WeekDayOptions.Add("Quinta-feira");
                WeekDayOptions.Add("Sexta-feira");
                break;
            case "We":
                WeekDayOptions.Add("Quarta-feira");
                WeekDayOptions.Add("Quinta-feira");
                WeekDayOptions.Add("Sexta-feira");
                break;
            case "Th":
                WeekDayOptions.Add("Quinta-feira");
                WeekDayOptions.Add("Sexta-feira");
                break;
            case "Fr":
                WeekDayOptions.Add("Sexta-feira");
                break;
        }
        DpdWeekDay.options.Clear();
        DpdWeekDay.AddOptions(WeekDayOptions);
        DpdWeekDay.value = 0;
        DpdWeekDay.RefreshShownValue();
    }

    public static string VerifyDpdWeekDay(Dropdown DpdWeekDay)
    {
        string sWeekDay = DpdWeekDay.options[DpdWeekDay.value].text;

        switch(sWeekDay)
        {
            case "Segunda-feira":
                sWeekDay = "Monday";
                break;
            case "Terça-feira":
                sWeekDay = "Tuesday";
                break;
            case "Quarta-feira":
                sWeekDay = "Wednesday";
                break;
            case "Quinta-feira":
                sWeekDay = "Thursday";
                break;
            case "Sexta-feira":
                sWeekDay = "Friday";
                break;
        }
        
        DateTime sTodayLong = DateTime.Now;
        
        string sDateDay = "";
        if(sWeekDay.Substring(0, 2) == sTodayLong.ToLongDateString().Substring(0, 2))
        {
            sDateDay = DateTime.Now.ToString("yyyy-MM-dd");
        }
        else
        {
            List<string> Days = new List<string>() {sTodayLong.AddDays(1).ToString(), sTodayLong.AddDays(2).ToString(), sTodayLong.AddDays(3).ToString(), sTodayLong.AddDays(4).ToString()};
            foreach(string day in Days)
            {
                if(day.Substring(0, 2) == sWeekDay.Substring(0, 2))
                {
                    sDateDay = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        return sDateDay;
    }

    public static string TimeStart(Dropdown DpdStartTime, InputField InputStartHour, InputField InputStartMin)
    {
        string sTimeStart = "";
        if(DpdStartTime.gameObject.activeInHierarchy)
        {
            sTimeStart = DpdStartTime.options[DpdStartTime.value].text;
            sTimeStart = sTimeStart == "AGORA" ? DateTime.Now.ToString("HH:mm:ss") : sTimeStart + ":00";
        }
        else
        {
            string sStartHour = VerifyTime.VerifyInputHour(InputStartHour.text);
            if(sStartHour is null) return null;
            string sStartMin = VerifyTime.VerifyInputMin(InputStartMin.text);
            if(sStartMin is null) return null;
            sTimeStart = sStartHour + ":" + sStartMin + ":00";
        }
        return sTimeStart;
    }

    public static string TimeEnd(Dropdown DpdEndTime, InputField InputEndHour, InputField InputEndMin)
    {
        string sTimeEnd = "";
        if(DpdEndTime.gameObject.activeInHierarchy)
        {
            sTimeEnd = DpdEndTime.options[DpdEndTime.value].text + ":00";
        }
        else
        {
            string sEndHour = VerifyTime.VerifyInputHour(InputEndHour.text);
            if(sEndHour is null) return null;
            string sEndMin = VerifyTime.VerifyInputMin(InputEndMin.text);
            if(sEndMin is null) return null;
            sTimeEnd = sEndHour + ":" + sEndMin + ":00";
        }
        return sTimeEnd;
    }

    public static string VerifyInputHour(string STime)
    {
        if(STime != "")
        {
            int iTime;
            Int32.TryParse(STime, out iTime);
            if(iTime > 24 | iTime < 0) return null;
            else
            {
                if(iTime < 10) STime = "0" + STime;
            }
            return STime;
        }
        else return null;
    }
    
    public static string VerifyInputMin(string STime)
    {
        if(STime != "")
        {
            int iTime;
            Int32.TryParse(STime, out iTime);
            if(iTime > 60 | iTime < 0) return null;
            else
            {
                if(iTime < 10) STime = "0" + STime;
            }
            return STime;
        }
        else return "00";
    }
}
