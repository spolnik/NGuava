using System;
using System.Collections.Generic;
using System.Globalization;
using NProgramming.NGuava.Base;
using NUnit.Framework;

namespace NProgramming.NGuava.UnitTests.Base
{
    public class OptionalTest
    {
         [Test]
         public void test_absent()
         {
             var optionalName = Optional<string>.Absent();
             Assert.That(optionalName.IsPresent(), Is.False);
         }

        [Test]
        public void test_of()
        {
            Assert.That("training", Is.EqualTo(Optional<string>.Of("training").Get()));
        }

        [Test]
        public void test_of__null()
        {
            Assert.That(() => Optional<string>.Of(null), Throws.InstanceOf<NullReferenceException>());
        }

        [Test]
        public void test_from_nullable()
        {
            var optionalName = Optional<string>.FromNullable("bob");
            Assert.That("bob", Is.EqualTo(optionalName.Get()));
        }

        [Test]
        public void test_from_nullable__null()
        {
            Assert.That(Optional<string>.Absent(), Is.SameAs(Optional<string>.FromNullable(null)));
        }

        [Test]
        public void test_is_present__no()
        {
            Assert.That(Optional<string>.Absent().IsPresent(), Is.False);
        }

        [Test]
        public void test_is_present__false()
        {
            Assert.That(Optional<string>.Of("training").IsPresent(), Is.True);
        }

        [Test]
        public void test_get__absent()
        {
            var optional = Optional<string>.Absent();

            Assert.That(() => optional.Get(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void test_get__present()
        {
            Assert.That("training", Is.EqualTo(Optional<string>.Of("training").Get()));
        }

        [Test]
        public void test_or__T_present()
        {
            Assert.That("a", Is.EqualTo(Optional<string>.Of("a").Or("default")));
        }

        [Test]
        public void test_or__T_absent()
        {
            Assert.That("default", Is.EqualTo(Optional<string>.Absent().Or("default")));
        }

        [Test]
        public void test_or__supplier_present()
        {
            Assert.That("a", Is.EqualTo(Optional<string>.Of("a").Or(Suppliers.OfInstance("fallback"))));
        }

        [Test]
        public void test_or__supplier_absent()
        {
            Assert.That("fallback", Is.EqualTo(Optional<string>.Absent().Or(Suppliers.OfInstance("fallback"))));
        }

        [Test]
        public void test_or__null_supplier_absent()
        {
            var nullSupplier = Suppliers.OfInstance<string>(null);
            var absentOptional = Optional<string>.Absent();

            Assert.That(() => absentOptional.Or(nullSupplier), Throws.InstanceOf<NullReferenceException>());
        }

        [Test]
        public void test_or__null_supplier_present()
        {
            var nullSupplier = Suppliers.OfInstance<string>(null);
            Assert.That("a", Is.EqualTo(Optional<string>.Of("a").Or(nullSupplier)));
        }

        [Test]
        public void test_or__optional_present()
        {
            Assert.That(Optional<string>.Of("a"),
                        Is.EqualTo(Optional<string>.Of("a").Or(Optional<string>.Of("fallback"))));
        }

        [Test]
        public void test_or__optional_absent()
        {
            Assert.That(Optional<string>.Of("fallback"), Is.EqualTo(Optional<string>.Absent().Or(Optional<string>.Of("fallback"))));
        }

        [Test]
        public void test_or_null__present()
        {
            Assert.That("a", Is.EqualTo(Optional<string>.Of("a").OrNull()));
        }

        [Test]
        public void test_or_null__absent()
        {
            Assert.That(Optional<string>.Absent().OrNull(), Is.Null);
        }

        [Test]
        public void test_as_set__present()
        {
            var hashSet = new HashSet<string> {"a"};
            Assert.That(hashSet, Is.EqualTo(Optional<string>.Of("a").AsSet()));
        }

        [Test]
        public void test_as_set__absent()
        {
            Assert.That(Optional<string>.Absent().AsSet(), Is.Empty);
        }

        [Test]
        public void test_as_set__present_is_immutable()
        {
            var presentAsSet = Optional<string>.Of("a").AsSet();
            Assert.That(() => presentAsSet.Add("b"), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void test_as_set__absent_is_immutable()
        {
            var absentAsSet = Optional<string>.Absent().AsSet();
            Assert.That(() => absentAsSet.Add("foo"), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void test_transform__absent()
        {
            Assert.That(Optional<string>.Absent(), Is.EqualTo(Optional<string>.Absent().Transform(Functions.Identity)));
            Assert.That(Optional<string>.Absent(), Is.EqualTo(Optional<string>.Absent().Transform(Functions.ToStringFunction)));
        }

        [Test]
        public void test_transform__present_identity()
        {
           Assert.That(Optional<string>.Of("a"), Is.EqualTo(Optional<string>.Of("a").Transform(Functions.Identity)));
        }

        [Test]
        public void test_transform__present_to_string()
        {
            Assert.That(Optional<string>.Of("42"),
                        Is.EqualTo(Optional<Integer>.Of(new Integer(42)).Transform(Functions.ToStringFunction)));
        }

        [Test]
        public void test_transform__present_function_returns_null()
        {
            Assert.That(() => Optional<string>.Of("a").Transform(x => (string) null),
                        Throws.InstanceOf<NullReferenceException>());
        }

        [Test]
        public void test_transform__absent_function_returns_null()
        {
            Assert.That(Optional<string>.Absent(), Is.EqualTo(Optional<string>.Absent().Transform(x => (string) null)));
        }

        [Test]
        public void test_equals_and_hash_code__absent()
        {
            Assert.That(Optional<string>.Absent(), Is.Not.EqualTo(Optional<Integer>.Absent()));
            Assert.That(Optional<string>.Absent(), Is.EqualTo(Optional<string>.Absent()));
            Assert.That(Optional<string>.Absent().GetHashCode(), Is.EqualTo(Optional<Integer>.Absent().GetHashCode()));
        }

        [Test]
        public void test_equals_and_hash_code__present()
        {
            Assert.That(Optional<string>.Of("training"), Is.EqualTo(Optional<string>.Of("training")));
            Assert.That(Optional<string>.Of("a").Equals(Optional<string>.Of("b")), Is.False);
            Assert.That(Optional<string>.Of("a").Equals(Optional<string>.Absent()), Is.False);
            Assert.That(Optional<string>.Of("training").GetHashCode(), Is.EqualTo(Optional<string>.Of("training").GetHashCode()));
        }

        [Test]
        public void test_to_string__absent()
        {
            Assert.That("Optional.Absent()", Is.EqualTo(Optional<string>.Absent().ToString()));
        }

        [Test]
        public void test_to_string__present()
        {
            Assert.That("Optional.Of(training)", Is.EqualTo(Optional<string>.Of("training").ToString()));
        }

        class Integer
        {
            private readonly int _val;

            public Integer(int val)
            {
                _val = val;
            }

            public override string ToString()
            {
                return _val.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}