using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

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
        List<string> weekDayOptions = new List<string>();

        CultureInfo ci = new CultureInfo("en-US");
        string sLongDate = DateTime.Now.ToString("F", ci);
        string sDay = sLongDate.Substring(0, 3);
        
        weekDayOptions.Add("Sexta-feira");
        if(sDay != "Fri")
        {
            weekDayOptions.Add("Quinta-feira");
            if(sDay != "Thu")
            {
                weekDayOptions.Add("Quarta-feira");
                if(sDay != "Wed")
                {
                    weekDayOptions.Add("Terça-feira");
                    if(sDay != "Tue") weekDayOptions.Add("Segunda-feira");
                }
            }
        }
        weekDayOptions.Reverse();

        DpdWeekDay.options.Clear();
        DpdWeekDay.AddOptions(weekDayOptions);
        DpdWeekDay.value = 0;
        DpdWeekDay.RefreshShownValue();
    }

    public static string VerifyDpdWeekDay(Dropdown DpdWeekDay)
    {
        string sWeekDay = DpdWeekDay.options[DpdWeekDay.value].text;

        switch(sWeekDay)
        {
            case "Segunda-feira":
                sWeekDay = "Mon";
                break;
            case "Terça-feira":
                sWeekDay = "Tue";
                break;
            case "Quarta-feira":
                sWeekDay = "Wed";
                break;
            case "Quinta-feira":
                sWeekDay = "Thu";
                break;
            case "Sexta-feira":
                sWeekDay = "Fri";
                break;
        }
        
        CultureInfo ci = new CultureInfo("en-US");
        DateTime sToday = DateTime.Now;
        string sDateDay = "";
        if(sToday.ToLongDateString().Substring(0, 3) == sWeekDay)
        {
            sDateDay = DateTime.Now.ToString("yyyy-MM-dd");
        }
        else
        {
            List<string> days = new List<string>() {sToday.AddDays(1).ToString("F", ci), sToday.AddDays(2).ToString("F", ci), sToday.AddDays(3).ToString("F", ci), sToday.AddDays(4).ToString("F", ci), sToday.AddDays(5).ToString("F", ci), sToday.AddDays(6).ToString("F", ci)};
            for(int i = 0; i < days.Count; i++)
            {
                if(days[i].Substring(0, 3) == sWeekDay)
                {
                    sDateDay = sToday.AddDays(i+1).ToString("yyyy-MM-dd");
                    break;
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