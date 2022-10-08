using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RequestKeyButton : MonoBehaviour
{
    [SerializeField] Text txtThisRoom;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] InputField inputStartHour;
    [SerializeField] InputField inputStartMin;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputEndHour;
    [SerializeField] InputField inputEndMin;
    [SerializeField] Dropdown dpdWeekDay;
    [SerializeField] Button btnRequestKey;
    [SerializeField] Dropdown dpdRequestsList;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private RequestCreateJson jsonRequestCreate;

    private enum Months31Days
    {
        JANEIRO, MARÇO, MAIO, JULHO, AGOSTO, OUTUBRO, DEZEMBRO
    }
    private enum Months30Days
    {
        ABRIL, JUNHO, SETEMBRO, NOVEMBRO
    }

    private string VerifyInputDay()
    {
        string sTimeYear = DateTime.UtcNow.ToLocalTime().ToString("yyyy");

        string sTimeMonth = dpdMonth.value + 1 < 10 ? "0" + (dpdMonth.value + 1).ToString() : (dpdMonth.value + 1).ToString();

        string sTimeDay = "";
        if(inputDay.text != "")
        {
            int iNumberDay;
            Int32.TryParse(inputDay.text, out iNumberDay);
            
            if(Enum.IsDefined(typeof(Months31Days), dpdMonth.options[dpdMonth.value].text))
            {
                if(iNumberDay <= 0 | iNumberDay > 31) return null;
            }
            else if(Enum.IsDefined(typeof(Months30Days), dpdMonth.options[dpdMonth.value].text))
            {
                if(iNumberDay <= 0 | iNumberDay > 30) return null;
            }
            else
            {
                if(iNumberDay <= 0 | iNumberDay > 28) return null;
            }

            sTimeDay = iNumberDay < 10 ? "0" + inputDay.text : inputDay.text;
        }
        else return null;

        return sTimeYear + "-" + sTimeMonth + "-" + sTimeDay;
    }

    private string VerifyInputHour(string STime)
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
    
    private string VerifyInputMin(string STime)
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

    private string TimeStart()
    {
        string sTimeStart = "";
        if(dpdStartTime.gameObject.activeInHierarchy)
        {
            sTimeStart = dpdStartTime.options[dpdStartTime.value].text;
            sTimeStart = sTimeStart == "AGORA" ? DateTime.Now.ToString("HH:mm:ss") : sTimeStart + ":00";
        }
        else
        {
            string sStartHour = VerifyInputHour(inputStartHour.text);
            if(sStartHour is null) return null;
            string sStartMin = VerifyInputMin(inputStartMin.text);
            if(sStartMin is null) return null;
            sTimeStart = sStartHour + ":" + sStartMin + ":00";
        }
        return sTimeStart;
    }

    private string TimeEnd()
    {
        string sTimeEnd = "";
        if(dpdEndTime.gameObject.activeInHierarchy)
        {
            sTimeEnd = dpdEndTime.options[dpdEndTime.value].text + ":00";
        }
        else
        {
            string sEndHour = VerifyInputHour(inputEndHour.text);
            if(sEndHour is null) return null;
            string sEndMin = VerifyInputMin(inputEndMin.text);
            if(sEndMin is null) return null;
            sTimeEnd = sEndHour + ":" + sEndMin + ":00";
        }
        return sTimeEnd;
    }

    public void RequestKey()
    {
        Utilities.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Carregando", panelMsg);

        string sTimeStart = TimeStart();
        if(sTimeStart is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido");
            return;
        }

        string sTimeEnd = TimeEnd();
        if(sTimeEnd is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: horário inválido");
            return;
        }

        string sDateDay = VerifyInputDay();
        if(sDateDay is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: dia inválido");
            return;
        }

        string sKey = txtThisRoom.text.Substring(0, 1) == "S" ? txtThisRoom.text.Substring(6) : txtThisRoom.text.Substring(13);

        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    IEnumerator GetRequestKey(string SKey, string SDateDay, string STimeStart, string STimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(Utilities.apiURL + "/api/request/create?user=" + User.user.UserId + "&key=" + SKey + "&date_start=" + SDateDay + " " + STimeStart + "&date_end=" + SDateDay + " " + STimeEnd + "&token=" + User.user.UserToken);
        yield return requestRequestCreate.SendWebRequest();

        if(requestRequestCreate.result == UnityWebRequest.Result.ConnectionError | requestRequestCreate.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro de conexão");
        }
        else
        {
            jsonRequestCreate = JsonUtility.FromJson<RequestCreateJson>(requestRequestCreate.downloadHandler.text);

            switch(jsonRequestCreate.code)
            {
                case "request_date_end_before_start":
                case "request_date_end_before_now":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: data inválida");
                    break;
                case "request_key_already_in_use":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: essa chave já está sendo usada");
                    break;
                case "request_error_on_create":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: erro ao criar o pedido");
                    break;
                case "request_created":
                    int iRoomNumber;
                    int iNumberPos = txtThisRoom.text.Substring(0, 1) == "S" ? 6 : 13;
                    Int32.TryParse(txtThisRoom.text.Substring(iNumberPos), out iRoomNumber);
                    User.user.UserKeys.Add(new Key(iRoomNumber, jsonRequestCreate.id, SDateDay + " " + STimeStart, SDateDay + " " + STimeEnd, (int)Utilities.Status.not_started));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso");

                    Utilities.ClearFields(dpdStartTime, dpdEndTime, inputStartHour, inputStartMin, inputEndHour, inputEndMin, dpdWeekDay);
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro inesperado: " + jsonRequestCreate.code);
                    break;
            }
        }
    }
}
