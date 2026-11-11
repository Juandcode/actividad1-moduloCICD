namespace IntegrationTests.Setup
{
    public class ResultResponse<T>
    {
        public T Value { get; set; } = default!;
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
    }
}
