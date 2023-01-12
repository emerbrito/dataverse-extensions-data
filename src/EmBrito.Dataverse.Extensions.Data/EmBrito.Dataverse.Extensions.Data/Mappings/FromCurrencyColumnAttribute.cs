using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromCurrencyColumnAttribute : ColumnMappingAttribute
    {

        public FromCurrencyColumnAttribute(string logicalName, bool formattedValue = false, string format = null)
            : base(logicalName, typeof(Money), formattedValue, format)
        {
        }

    }

}
