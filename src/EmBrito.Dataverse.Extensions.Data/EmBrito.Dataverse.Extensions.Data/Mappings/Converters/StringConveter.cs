using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class StringConveter : ValueConverter
    {
        public override Type SourceType => typeof(string);

        protected override Type[] RestrictDestinationTypes => new Type[] { };

    }
}
