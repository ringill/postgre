using System.Linq;
using NHibernate;

public class NHibernateMapperSession : IMapperSession
{
  private readonly ISession _session;

  public NHibernateMapperSession(ISession session)
  {
    _session = session;
  }

  public IQueryable<Book> Books => _session.Query<Book>();

  IQueryable<Book> IMapperSession.Books => throw new System.NotImplementedException();
}