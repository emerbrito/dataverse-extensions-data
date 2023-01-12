using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public abstract class ColumnMappingAttribute : Attribute
    {

        public Type ColumnType { get; private set; }

        public bool FormattedValue { get; private set; }

        public string LogicalName { get; private set; }

        public string StringFormat { get; private set; }

        public ColumnMappingAttribute(string logicalName, Type columnType, bool formattedValue = false, string stringFormat = null)
        {
            LogicalName = logicalName;
            FormattedValue = formattedValue;
            ColumnType = columnType;
            StringFormat = stringFormat;
        }

    }
}
