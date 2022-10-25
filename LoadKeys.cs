using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadKeys : MonoBehaviour
{
    public InputField inputurl;                                     //erro

    private RequestListJson jsonLoadUserKeys;

    public void Comecar()
    {
        Utilities.apiURL = "https://80-mocno-serverjosmar-" + inputurl.text + ".gitpod.io";    //erro

        UserData userData = SaveSystem.LoadUser();
        if(userData is null | userData.token == "")
        {
            SaveSystem.SaveUser(new UserData(0, ""));
            SceneManager.LoadScene("scene-login");
            return;
        }
        User.user.UserId = userData.id;
        User.user.UserToken = userData.token;
        StartCoroutine(GetLoadUserKeys());
    }

    private IEnumerator GetLoadUserKeys()
    {
        UnityWebRequest requestLoadUserKeys = UnityWebRequest.Get(Utilities.apiURL + Utilities.requestListURL + "?status=" + ((int)Utilities.Status.not_started).ToString() + "|" + ((int)Utilities.Status.start_request).ToString() + "|" + ((int)Utilities.Status.started).ToString() + "|" + ((int)Utilities.Status.end_request).ToString() + "&date_start=" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "&date_end=" + DateTime.Now.ToString("yyyy") + "-12-31 23:59:59" + "&token=" + User.user.UserToken);
        yield return requestLoadUserKeys.SendWebRequest();

        if(requestLoadUserKeys.result == UnityWebRequest.Result.ConnectionError | requestLoadUserKeys.result == UnityWebRequest.Result.ProtocolError)
        {
            User.user.UserId = 0;
            User.user.UserToken = "";
            SaveSystem.SaveUser(new UserData(User.user.UserId, User.user.UserToken));
            SceneManager.LoadScene("scene-login");
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
                User.user.UserId = 0;
                User.user.UserToken = "";
                SaveSystem.SaveUser(new UserData(User.user.UserId, User.user.UserToken));
                SceneManager.LoadScene("scene-login");
            }
        }
    }
}
