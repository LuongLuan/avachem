using System;
public class Log
{
    public int ID { get; set; }
    public string ActionName { get; set; }
    public string ActionLocation { get; set; }
    public int ActionType { get; set; }
    public string ActionMeta { get; set; }
    public int ByUserID { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool SoftDelete { get; set; }
}

internal enum LogActionTypes
{
    Create = 1,
    Update,
    Delete,
    SoftDelete,
    Others
}
public class CreateLogDTO
{
    public string ActionName { get; set; }
    public string ActionLocation { get; set; }
    public int ActionType { get; set; } // LogActionTypes
    public LogActionMeta ActionMeta { get; set; }
    public int ByUserID { get; set; }
}
public class LogActionMeta
{
    public object Input { get; set; }
    public object Result { get; set; }
}