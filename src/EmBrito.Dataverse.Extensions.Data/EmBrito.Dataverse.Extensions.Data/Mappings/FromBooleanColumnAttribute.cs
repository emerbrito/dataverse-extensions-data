using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromBooleanColumnAttribute : ColumnMappingAttribute
    {

        public FromBooleanColumnAttribute(string logicalName, bool formattedValue = false)
            : base(logicalName, typeof(bool), formattedValue)
        {
        }

    }

}
