using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromDateTimeColumnAttribute : ColumnMappingAttribute
    {

        public FromDateTimeColumnAttribute(string logicalName, bool formattedValue = false, string format = null)
            : base(logicalName, typeof(DateTime), formattedValue, format)
        {
        }

    }

}
