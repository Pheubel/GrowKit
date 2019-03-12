using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts.Generators
{
    public class UlongGenerator : ValueGenerator<ulong>
    {
        public override bool GeneratesTemporaryValues => false;

        public override ulong Next(EntityEntry entry) => ThreadsafeRandom.GenerateUlongId();
    }
}
