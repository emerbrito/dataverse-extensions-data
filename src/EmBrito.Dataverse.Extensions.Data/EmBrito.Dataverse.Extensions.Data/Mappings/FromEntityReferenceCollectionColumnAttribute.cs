using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromEntityReferenceCollectionColumnAttribute : ColumnMappingAttribute
    {

        public FromEntityReferenceCollectionColumnAttribute(string logicalName, bool formattedValue = false)
            : base(logicalName, typeof(EntityReferenceCollection), formattedValue)
        {
        }

    }

}
