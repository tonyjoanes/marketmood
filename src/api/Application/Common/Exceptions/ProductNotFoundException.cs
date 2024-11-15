public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string id) 
        : base($"Product with ID {id} was not found")
    {
    }
}