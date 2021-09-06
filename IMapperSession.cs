using System.Linq;

public interface IMapperSession
{
    IQueryable<Book> Books { get; }
}