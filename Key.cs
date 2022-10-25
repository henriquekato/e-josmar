public class Key
{
    public int roomNumber;
    public int requestId;
    public string dateStart;
    public string dateEnd;
    public string status;

    public Key(int roomNumber, int requestId, string dateStart, string dateEnd, string status)
    {
        this.roomNumber = roomNumber;
        this.requestId = requestId;
        this.dateStart = dateStart;
        this.dateEnd = dateEnd;
        this.status = status;
    }
}