namespace api.Application.Common.Exceptions
{
    public class DuplicateReviewException : Exception
    {
        public DuplicateReviewException()
            : base("A review with this ID already exists")
        {
        }

        public DuplicateReviewException(string message)
            : base(message)
        {
        }

        public DuplicateReviewException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
