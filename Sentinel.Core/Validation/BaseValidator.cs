using Sentinel.Core.Validation.Exceptions;

namespace Sentinel.Core.Validation
{
    public abstract class BaseValidator<T>
    {
        /// <summary>
        /// Asserts that the item is valid.
        /// </summary>
        /// <param name="itemToValidate">Item To Validate</param>
        public abstract void Validate(T itemToValidate);

        /// <summary>
        /// Checks if the item is valid.
        /// </summary>
        /// <param name="itemToValidate">Item To Validate</param>
        /// <returns>TRUE if item is valid otherwise FALSE</returns>
        public virtual bool IsValid(T itemToValidate)
        {
            try
            {
                Validate(itemToValidate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}