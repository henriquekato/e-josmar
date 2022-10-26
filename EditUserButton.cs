using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EditUserButton : MonoBehaviour
{
    [SerializeField] InputField inputUsername;
    [SerializeField] InputField inputEmail;
    [SerializeField] InputField inputPassword;
    [SerializeField] Button btnEdit;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    private EditUserJson jsonEditUser;

    public void EditUser()
    {
        Utilities.StartRequest(new Button[] {btnEdit}, txtMsg, "Carregando...", panelMsg);
        StartCoroutine(PostEditUser());
    }
    
    private IEnumerator PostEditUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", User.user.UserId);
        form.AddField("token", User.user.UserToken);
        if(inputUsername.text != "") form.AddField("name", inputUsername.text);
        if(inputEmail.text != "") form.AddField("email", inputEmail.text);
        if(inputPassword.text != "") form.AddField("password", inputPassword.text);
        
        UnityWebRequest requestEditUser = UnityWebRequest.Post(Utilities.apiURL + Utilities.editUserURL, form);
        requestEditUser.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return requestEditUser.SendWebRequest();

        if(requestEditUser.result == UnityWebRequest.Result.ConnectionError | requestEditUser.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro de conexão", panelMsg);
        }
        else
        {
            jsonEditUser = JsonUtility.FromJson<EditUserJson>(requestEditUser.downloadHandler.text);

            switch(jsonEditUser.code)
            {
                case "user_name_in_use":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Nome de usuário já está em uso", panelMsg);
                    break;
                case "user_email_in_use":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Email já está em uso", panelMsg);
                    break;
                case "user_nothing_edited":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Você não editou nada", panelMsg);
                    break;
                case "user_error_on_edit":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro ao editar usuário", panelMsg);
                    break;
                case "param_filter_not_match":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: email inválido", panelMsg);
                    break;
                case "param_more_than":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: nome ou senha deve conter pelo menos 5 caracteres", panelMsg);
                    break;
                case "param_less_than":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: nome ou senha deve ser menor", panelMsg);
                    break;
                case "user_edited":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Usuário editado com sucesso", panelMsg, true);
                    Utilities.ClearFields(InputFields:new InputField[] {inputUsername, inputEmail, inputPassword});
                    break;
                case "api_invalid_token":
                    ExitButton.Exit();
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro inesperado: " + jsonEditUser.code, panelMsg);
                    break;
            }
        }
    }
}
