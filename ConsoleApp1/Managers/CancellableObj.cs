namespace ConsoleApp1
{
    public class CancellableObj<T>
    {
        public CancellableObj()
        {
        }

        public CancellableObj(T result, UserActionResult.ResultType responseType)
        {
            Result = result;
            ResponseType = responseType;
        }

        public UserActionResult.ResultType ResponseType { get; set; }
        public T Result { get; set; }

    }
}