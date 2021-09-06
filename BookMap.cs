using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

public class BookMap : ClassMapping<Book>
{
    public BookMap()
    {
        Id(x => x.Id, x =>
        {
            x.Generator(Generators.Guid);
            x.Type(NHibernateUtil.Guid);
            x.Column("Id");
            x.UnsavedValue(Guid.Empty);
        });
 
        Property(b => b.Title, x =>
        {
            x.Length(250);
            x.Type(NHibernateUtil.StringClob);
            x.NotNullable(true);
        });
 
        Table("BOOK");
    }
}