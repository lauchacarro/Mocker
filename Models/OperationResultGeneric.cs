namespace Mocker.Models
{
    public class OperationResult<T> : ValidateResult
    {
        public T Result { get; set; }
    }
}
