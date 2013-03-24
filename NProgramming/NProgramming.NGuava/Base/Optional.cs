using System;
using System.Collections.Generic;
using System.Linq;

namespace NProgramming.NGuava.Base
{
    /// <summary>
    /// An immutable object that may contain a non-null reference to another object. Each
    /// instance of this type either contains a non-null reference, or contains nothing (in
    /// which case we say that the reference is "absent"); it is never said to "contain <c>null</c>.
    /// 
    /// <p>A non-null <see cref="Optional{T}"/> reference can be used as a replacement for a nullable
    /// <c>T</c> reference. It allows you to represent "a <c>T</c> that must be present" and
    /// a "a <c>T</c> that might be absent" as two distinct types in your program, which can
    /// aid clarity.</p>
    /// 
    /// <p>Some uses of this class include
    /// 
    /// <ul>
    /// <li>As a method return type, as an alternative to returning <c>null</c> to indicate
    ///     that no value was available</li>
    /// <li>To distinguish between "unknown" (for example, not present in a map) and "known to
    ///     have no value" (present in the map, with value <c>Optional&lt;T&gt;.Absent()</c>)</li>
    /// <li>To wrap nullable references for storage in a collection that does not support
    ///     <c>null</c> (though there are
    ///     <a href="http://code.google.com/p/guava-libraries/wiki/LivingWithNullHostileCollections">
    ///     several other approaches to this</a> that should be considered first)</li>
    /// </ul></p>
    /// 
    /// <p>A common alternative to using this class is to find or create a suitable
    /// <a href="http://en.wikipedia.org/wiki/Null_Object_pattern">null object</a> for the
    /// type in question.</p>
    /// 
    /// <p>This class is not intended as a direct analogue of any existing "option" or "maybe"
    /// construct from other programming environments, though it may bear some similarities.</p>
    /// 
    /// <p>See the Guava User Guide article on <a
    /// href="http://code.google.com/p/guava-libraries/wiki/UsingAndAvoidingNullExplained#Optional">
    /// using <c>Optional</c></a>.</p>
    /// 
    /// </summary>
    /// <typeparam name="T">
    /// the type of instance that can be contained. <c>Optional</c> is naturally  covariant 
    /// on this type, so it is safe to cast an <c>Optional&lt;T&gt;</c> to <c>Optional&lt;S&gt;</c> 
    /// for any supertype <c>S</c> of <c>T</c>. 
    /// </typeparam>
    public abstract class Optional<T> where T : class
    {
        /// <summary>
        /// Returns an <c>Optional</c> instance with no contained reference.
        /// </summary>
        /// <returns><c>Optional</c> instance with no contained reference.</returns>
        public static Optional<T> Absent()
        {
            return Absent<T>.Instance;
        }

        /// <summary>
        /// Returns an <c>Optional</c> instance containing the given non-null reference.
        /// </summary>
        /// <param name="reference">Instance to be contained.</param>
        /// <returns><c>Optional</c> instance containing the given non-null reference.</returns>
        public static Optional<T> Of(T reference)
        {
            return new Present<T>(Preconditions.CheckNotNull(reference));
        }

        /// <summary>
        /// If <c>nullableReference</c> is non-null, returns an <c>Optional</c> instance containing that
        /// reference; otherwise returns <see cref="Optional{T}.Absent"/>.
        /// </summary>
        /// <param name="nullableReference"></param>
        /// <returns></returns>
        public static Optional<T> FromNullable([Nullable] T nullableReference)
        {
            return (nullableReference == null)
                       ? Absent()
                       : new Present<T>(nullableReference);
        }

        /// <summary>
        /// Returns <c>true</c> if this holder contains a (non-null) instance.
        /// </summary>
        /// <returns><c>true</c> if this holder contains a (non-null) instance, otherwise returns <c>false</c>.</returns>
        public abstract bool IsPresent();

        /**
           * Returns the contained instance, which must be present. If the instance might be
           * absent, use {@link #or(Object)} or {@link #orNull} instead.
           *
           * @throws IllegalStateException if the instance is absent ({@link #isPresent} returns
           *     {@code false})
           */

        /// <summary>
        /// Returns the contained instance, which must be present. If the instance might be
        /// absent, use <see cref="Or(T)"/> or <see cref="OrNull"/> instead.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the instance is absent (<see cref="IsPresent"/> returns <c>false</c>)</exception>
        /// <returns>Contained instance.</returns>
        public abstract T Get();

        /// <summary>
        /// Returns the contained instance if it is present; <c>defaultValue</c> otherwise. If
        /// no default value should be required because the instance is known to be present, use
        /// <see cref="Get"/> instead. For a default value of <c>null</c>, use <see cref="OrNull"/>. 
        /// </summary>
        public abstract T Or(T defaultValue);

        /// <summary>
        /// Returns this <c>Optional</c> if it has a value present; <c>secondChoice</c>  otherwise.
        /// </summary>
        public abstract Optional<T> Or(Optional<T> secondChoice);

        /// <summary>
        /// Returns the contained instance if it is present; <c>Supplier.Get()</c> otherwise. If the
        /// supplier returns <c>null</c>, a <see cref="NullReferenceException"/> is thrown.
        /// </summary>
        /// <exception cref="NullReferenceException">if the supplier returns <c>null</c></exception>
        public abstract T Or(ISupplier<T> supplier);

        /// <summary>
        /// Returns the contained instance if it is present; <c>null</c> otherwise. If the
        /// instance is known to be present, use <see cref="Get"/> instead.
        /// </summary>
        [return: Nullable]
        public abstract T OrNull();

        /// <summary>
        /// Returns an immutable singleton <see cref="ISet{T}"/> whose only element is the contained instance
        /// if it is present; an empty immutable <see cref="ISet{T}"/> otherwise.
        /// </summary>
        public abstract ISet<T> AsSet();

        /// <summary>
        /// If the instance is present, it is transformed with the given <see cref="Func{T1,TResult}"/>; otherwise,
        /// <see cref="Absent"/> is returned. If the function returns <c>null</c>, a
        /// <see cref="NullReferenceException"/> is thrown.
        /// <exception cref="NullReferenceException">if the function returns <c>null</c></exception>
        /// </summary>
        public abstract Optional<TResult> Transform<TResult>(Func<T, TResult> function) where TResult : class;

        /// <summary>
        /// Returns <c>true</c> if <c>object</c> is an <c>Optional</c> instance, and either
        /// the contained references are <see cref="object.Equals(object)"/> to each other or both
        /// are absent. Note that <c>Optional</c> instances of differing parameterized types can
        /// be equal. 
        /// </summary>
        /// <param name="object">Object to compare against.</param>
        /// <returns>The result of comparison.</returns>
        public abstract override bool Equals([Nullable] object @object);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>hash code for this instance</returns>
        public abstract override int GetHashCode();

        /// <summary>
        /// Returns a string representation for this instance. The form of this string
        /// representation is unspecified.
        /// </summary>
        /// <returns>string representation for this instance</returns>
        public abstract override string ToString();

        public static IEnumerable<T> PresentInstances(IEnumerable<Optional<T>> optionals)
        {
            if (optionals == null)
                throw new NullReferenceException();

            return optionals.Where(x => x.IsPresent()).Select(x => x.Get());
        }
    }
}