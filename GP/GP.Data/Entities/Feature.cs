using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class Feature
    {
        public Guid FeatureId { get; set; }
        public string FeatureName { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

    }
}