using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class BooleanConverter : ValueConverter
    {
        public override Type SourceType => typeof(bool);

        protected override Type[] RestrictDestinationTypes => new Type[] { };

    }
}
