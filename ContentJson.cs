using System;
using UnityEngine;

[Serializable]
public class AuthJson
{
    public string code;
    public string token;
    public int id;
}

[Serializable]
public class RequestCreateJson
{
    public string code;
    public int id;
}

[Serializable]
public class RequestListJson
{
    public string code;
    public _list[] list;
    public int count;
}

[Serializable]
public class _list
{
    public int id;
    public int key;
    public string status;
    public string date_expected_start;
    public string date_expected_end;
    public string user_name;
}

[Serializable]
public class RequestGetJson
{
    public string code;
    public _request request;
}

[Serializable]
public class _request
{
    public string status;
}

[Serializable]
public class RequestUpdateJson
{
    public string code;
}

[Serializable]
public class EditUserJson
{
    public string code;
}
