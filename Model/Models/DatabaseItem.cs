using Models.Utils;

namespace Models
{
    public abstract class DatabaseItem
    {
        [Property] public int Id { get; set; }
    }
}