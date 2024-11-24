namespace CRUDApplication.Core.Result
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        bool IsFailure { get; }
        bool IsWarning { get; set; }
        bool IsInfo { get; set; }
        string Message { get; set; }
        string Exception { get; set; }
        string Class { get; set; }
        int ReturnId { get; set; }
        object data { get; set; }
        string RedirectUrl { get; set; }
        string Condition { get; set; }
        string PageName { get; set; }


        Result Success();
        Result Success(string message);
        Result Success(string message, string redirectUrl);
        Result Success(string message, int returnId);
        Result Success(string message, object data);
        Result Info(string message);
        Result Warning(string message);
        Result Warning(string message, string condition);
        Result Warning(string message, string condition, object data);
        Result Fail(string message, string exception);
    }
}
