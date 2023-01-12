using System;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public interface IValueConverter
    {
        Type SourceType { get; }
        object Convert(object value, Type destinationType, string format = null);
    }
}