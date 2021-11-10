
using System;
using Util;

namespace SpikingLibrary
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public class StdpParameters : IDeepCloneable<StdpParameters>
    {
        public double A2Negative { get; set; }
        public double A2Positive { get; set; }
        public double A3Negative { get; set; }
        public double A3Positive { get; set; }
        public double TauPositive { get; set; }
        public double TauNegative { get; set; }
        public double TauX { get; set; }
        public double TauY { get; set; }
        
        public static StdpParameters RatVisualCortexL23
        {
            get
            {
                return new StdpParameters
                           {
                               A2Positive = 5e-10,
                               A3Positive = 6.2e-3,
                               A2Negative = 7e-3,
                               A3Negative = 2.3e-4,
                               TauX = 1010,
                               TauY = 1250,
                               TauPositive = 168,
                               TauNegative = 337
                           };
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hippocampal")]
        public static StdpParameters HippocampalCulture
        {
            get
            {

                return new StdpParameters
                           {
                               A2Positive = 6.1e-3,
                               A3Positive = 6.7e-3,
                               A2Negative = 1.6e-3,
                               A3Negative = 1.4e-3,
                               TauX = 9460,
                               TauY = 270,
                               TauNegative = 337,
                               TauPositive = 168
                           };
            }
        }

        #region Implementation of IDeepCloneable<out StdpParameters>

        public StdpParameters DeepClone()
        {
            return new StdpParameters
                       {
                           A2Negative = A2Negative,
                           A2Positive = A2Positive,
                           A3Negative = A3Negative,
                           A3Positive = A3Positive,
                           TauNegative = TauNegative,
                           TauPositive = TauPositive,
                           TauX = TauX,
                           TauY = TauY
                       };            
        }

        #endregion
    }
}
