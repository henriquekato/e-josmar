using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadKeys : MonoBehaviour
{
    public InputField inputUrl;

    private RequestListJson jsonLoadUserKeys;

    public void Comecar()
    {
        Utilities.apiURL = inputUrl.text;

        UserData userData = SaveSystem.LoadUser();
        if(userData.token == "")
        {
            SaveSystem.SaveUser(userData);
            SceneManager.LoadScene("scene-login");
            return;
        }
        User.user.UserId = userData.id;
        User.user.UserToken = userData.token;
        StartCoroutine(GetLoadUserKeys());
    }

    private IEnumerator GetLoadUserKeys()
    {
        UnityWebRequest requestLoadUserKeys = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestListURL + "?status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.start_request).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "|" + ((int)Utilities.Status.end_request).ToString() + "&date_start=" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "&date_end=" + DateTime.Now.ToString("yyyy") + "-12-31 23:59:59" + "&token=" + User.user.UserToken + "&user=" + User.user.UserId);
        yield return requestLoadUserKeys.SendWebRequest();

        if(requestLoadUserKeys.result == UnityWebRequest.Result.ConnectionError | requestLoadUserKeys.result == UnityWebRequest.Result.ProtocolError)
        {
            ExitButton.Exit();
        }
        else
        {
            jsonLoadUserKeys = JsonUtility.FromJson<RequestListJson>(requestLoadUserKeys.downloadHandler.text);
            
            if(jsonLoadUserKeys.code == "request_list")
            {
                if(jsonLoadUserKeys.count > 0)
                {
                    foreach(var request in jsonLoadUserKeys.list)
                    {
                        User.user.UserKeys.Add(new Key(request.key, request.id, request.date_expected_start, request.date_expected_end, request.status));
                    }
                }
                SceneManager.LoadScene("scene-keys");
            }
            else
            {
                ExitButton.Exit();
            }
        }
    }
}
