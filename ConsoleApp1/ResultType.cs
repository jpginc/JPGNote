namespace ConsoleApp1
{
    //todo move this out of the useractionresult?
    public partial class UserActionResult
    {
        public enum ResultType
        {
            Accept,
            Save,
            Canceled,
            NoInput
        }
    }
}