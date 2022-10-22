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
        var todayDate = DateTime.Now;
        string sTodayLongDate = todayDate.ToString("F", ci);
        string sTodayWeekDay = sTodayLongDate.Substring(0, 3);

        int daysToCome = 0;
        weekDayOptions.Add("Sexta-feira");
        if(sTodayWeekDay != "Fri")
        {
            weekDayOptions.Add("Quinta-feira");
            if(sTodayWeekDay != "Thu")
            {
                weekDayOptions.Add("Quarta-feira");
                if(sTodayWeekDay != "Wed")
                {
                    weekDayOptions.Add("Terça-feira");
                    if(sTodayWeekDay != "Tue")
                    {
                        weekDayOptions.Add("Segunda-feira");
                        if(sTodayWeekDay != "Mon")
                        {
                            daysToCome = sTodayWeekDay == "Sun" ? 1 : 2;
                        }
                    }
                }
            }
        }
        weekDayOptions.Reverse();

        for(int i = 0; i < weekDayOptions.Count; i++)
        {
            weekDayOptions[i] += " " + todayDate.AddDays(daysToCome+i).ToString("(dd/MM)");
        }

        DpdWeekDay.options.Clear();
        DpdWeekDay.AddOptions(weekDayOptions);
        DpdWeekDay.value = 0;
        DpdWeekDay.RefreshShownValue();
    }

    public static string VerifyDpdWeekDay(Dropdown DpdWeekDay)
    {
        string sWeekDay = DpdWeekDay.options[DpdWeekDay.value].text;
        string sDay = sWeekDay.Substring(sWeekDay.Length-6, 2);
        string sMonth = sWeekDay.Substring(sWeekDay.Length-3, 2);
        string sYear = DateTime.Now.ToString("yyyy");
        return sYear + "-" + sMonth + "-" + sDay;
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
            if(STime != "00")
            {
                if(iTime >= 24 | iTime < 0) return null;
                else
                {
                    if(iTime < 10) STime = "0" + STime;
                }
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
            if(STime != "00")
            {
                if(iTime > 60 | iTime < 0) return null;
                else
                {
                    if(iTime < 10) STime = "0" + STime;
                }
            }
            return STime;
        }
        else return "00";
    }
}