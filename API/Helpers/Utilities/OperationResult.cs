namespace API.Helpers.Utilities
{
    public class OperationResult
    {
        public string Message { set; get; }
        public bool IsSuccess { set; get; }
        public object Data { set; get; }

        public OperationResult()
        {

        }

        public OperationResult(string mess)
        {
            Message = mess;
        }

        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public OperationResult(bool isSuccess, string mess)
        {
            Message = mess;
            IsSuccess = isSuccess;
        }

        public OperationResult(bool isSuccess, object data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public OperationResult(bool isSuccess, string mess, object data)
        {
            Message = mess;
            IsSuccess = isSuccess;
            Data = data;
        }
    }
}