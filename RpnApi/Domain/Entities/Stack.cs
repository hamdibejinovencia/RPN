using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rpn.Api.Domain.Entities;

public class Stack
{
    [Key]
    public Guid StackId { get; set; }
    
    //[NotMapped]
    public Stack<string> Elements { get; set; }

    //public IEnumerable<Item> Items { get; set; }
}

public class Item
{
    public int ItemId { get; set; }
    public virtual Guid StackId { get; set; }
    public string ItemValue { get; set; }
}