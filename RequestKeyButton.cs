using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RequestKeyButton : MonoBehaviour
{
    [SerializeField] Text txtRoom;
    [SerializeField] Dropdown dpdStartTime;
    [SerializeField] Dropdown dpdEndTime;
    [SerializeField] InputField inputDay;
    [SerializeField] Dropdown dpdMonth;
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

    private string DateDay()
    {
        string sTimeYear = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy");

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

    private string TimeStart()
    {
        string sTimeStart = dpdStartTime.options[dpdStartTime.value].text;
        sTimeStart = sTimeStart == "AGORA" ? System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss") : sTimeStart + ":00";
        return sTimeStart;
    }

    public void RequestKey()
    {
        Utilities.StartRequest(new Button[] {btnRequestKey}, txtMsg, "Carregando", panelMsg);

        string sDateDay = DateDay();
        if(sDateDay is null)
        {
            Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: dia inválido");
            return;
        }
        string sTimeStart = TimeStart();
        string sTimeEnd = dpdEndTime.options[dpdEndTime.value].text + ":00";
        string sKey = txtRoom.text.Substring(6);

        StartCoroutine(GetRequestKey(sKey, sDateDay, sTimeStart, sTimeEnd));
    }

    IEnumerator GetRequestKey(string sKey, string sDateDay, string sTimeStart, string sTimeEnd)
    {
        UnityWebRequest requestRequestCreate = UnityWebRequest.Get(Utilities.apiURL + "/api/request/create?user=" + User.user.UserId + "&key=" + sKey + "&date_start=" + sDateDay + " " + sTimeStart + "&date_end=" + sDateDay + " " + sTimeEnd + "&token=" + User.user.UserToken);
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
                case "request_user_not_found":
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Erro: usuário não encontrado");
                    break;
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
                    Int32.TryParse(txtRoom.text.Substring(6), out iRoomNumber);
                    User.user.UserKeys.Add(new Key(iRoomNumber, jsonRequestCreate.id, sDateDay + " " + sTimeStart, sDateDay + " " + sTimeEnd, (int)Utilities.Status.not_started));

                    dpdRequestsList.options.Add(new Dropdown.OptionData("Pedido " + jsonRequestCreate.id.ToString()));
                    dpdRequestsList.RefreshShownValue();

                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, "Pedido " + jsonRequestCreate.id + " feito com sucesso");
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnRequestKey}, txtMsg, jsonRequestCreate.code);
                    break;
            }
        }
    }
}
