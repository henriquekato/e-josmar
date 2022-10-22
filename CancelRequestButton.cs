using UnityEngine;
using UnityEngine.UI;

public class CancelRequestButton : MonoBehaviour
{
    [SerializeField] GameObject panelCoverConfirm;
    [SerializeField] GameObject panelConfirm;

    public void CancelRequest()
    {
        panelCoverConfirm.SetActive(false);
        panelConfirm.SetActive(false);
    }
}
