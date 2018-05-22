using System;
using Xamarin.Forms;

namespace CGFSMVVM.Behaviors
{
    /// <summary>
    /// Behavior base class for UI components elements.
    /// </summary>
    public class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        /// <summary>
        /// Ons the attached to.
        /// </summary>
        /// <param name="bindable">Bindable object </param>
        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        /// <summary>
        /// Ons the detaching from.
        /// </summary>
        /// <param name="bindable">Bindable object</param>
        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        /// <summary>
        /// Ons the binding context changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args.</param>
        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        /// <summary>
        /// Ons the binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}