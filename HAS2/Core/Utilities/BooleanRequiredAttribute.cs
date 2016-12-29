using System.ComponentModel.DataAnnotations;

namespace HAS2.Core.Utilities
{
    public class BooleanRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value;
        }
    }
}