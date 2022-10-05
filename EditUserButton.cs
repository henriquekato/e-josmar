using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        Utilities.StartRequest(new Button[] {btnEdit}, txtMsg, "Carregando", panelMsg);
        StartCoroutine(PostEditUser());
    }
    
    IEnumerator PostEditUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", User.user.UserId);
        form.AddField("token", User.user.UserToken);
        if(inputUsername.text != "") form.AddField("name", inputUsername.text);
        if(inputEmail.text != "") form.AddField("email", inputEmail.text);
        if(inputPassword.text != "") form.AddField("password", inputPassword.text);
        
        UnityWebRequest requestEditUser = UnityWebRequest.Post(Utilities.apiURL + "/api/user/edit", form);
        requestEditUser.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return requestEditUser.SendWebRequest();

        if(requestEditUser.result == UnityWebRequest.Result.ConnectionError | requestEditUser.result == UnityWebRequest.Result.ProtocolError)
        {
            Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro de conexão");
        }
        else
        {
            jsonEditUser = JsonUtility.FromJson<EditUserJson>(requestEditUser.downloadHandler.text);

            switch(jsonEditUser.code)
            {
                case "user_name_in_use":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: nome de usuário já em uso");
                    break;
                case "user_email_in_use":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: email já em uso");
                    break;
                case "user_nothing_edited":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: você não editou nada");
                    break;
                case "user_error_on_edit":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: erro ao editar usuário");
                    break;
                case "param_filter_not_match":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: email inválido");
                    break;
                case "param_more_than":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: nome ou senha deve conter pelo menos 5 caracteres");
                    break;
                case "param_less_than":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro: nome ou senha deve ser menor");
                    break;
                case "user_edited":
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Usuário editado com sucesso");
                    inputUsername.text = "";
                    inputEmail.text = "";
                    inputPassword.text = "";
                    break;
                default:
                    Utilities.EndRequest(new Button[] {btnEdit}, txtMsg, "Erro inesperado: " + jsonEditUser.code);
                    break;
            }
        }
    }
}
