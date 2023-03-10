using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromImageColumnAttribute : ColumnMappingAttribute
    {

        public FromImageColumnAttribute(string logicalName)
            : base(logicalName, typeof(byte[]))
        {
        }

    }
}
