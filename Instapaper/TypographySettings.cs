using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instapaper
{
    public class TypographySettings
    {
        public static TypographySettings GetDefaultSettings()
        {
            return new TypographySettings
            {
                SizeModifier = 1.5
            };
        }

        public double SizeModifier { get; set; }
    }
}
