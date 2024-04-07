namespace Rpn.Api.Domain.Entities;

public class Stack
{
    public Guid StackId { get; set; }
    public Stack<string> Elements { get; set; }

    public Stack()
    {
        
    }
}