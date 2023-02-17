using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestStatus : MonoBehaviour
{
    public enum Status
    {
        not_started = 1,
        start_request = 2,
        started = 3,
        end_request = 4,
        ended = 5,
        canceled = 6
    }
}
