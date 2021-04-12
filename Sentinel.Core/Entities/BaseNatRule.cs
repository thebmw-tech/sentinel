using Sentinel.Core.Enums;

namespace Sentinel.Core.Entities
{
    public abstract class BaseNatRule<T> : BaseRule<T> where T : BaseNatRule<T>
    {
        public string TranslationAddress { get; set; }
        public byte? TranslationSubnetMask { get; set; }

        public ushort? TranslationPortRangeStart { get; set; }
        public ushort? TranslationPortRangeEnd { get; set; }
    }
}

