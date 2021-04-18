using System;

namespace Songhay.Models
{
    /// <summary>
    /// Defines an undo pattern for MVVM.
    /// </summary>
    public abstract class EditableObjectCache
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is restoring from cache.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is restoring from cache; otherwise, <c>false</c>.
        /// </value>
        public bool IsRestoringFromCache { get; set; }

        /// <summary>
        /// Restores the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">action</exception>
        public void Restore(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            this.IsRestoringFromCache = true;
            action.Invoke();
            this.IsRestoringFromCache = false;
        }
    }
}
