using System;
using System.Collections;
using System.Collections.Generic;

using Xunit;

namespace SimpleJson.Facts
{
    public static class ShouldExtensions
    {
        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.Equal(expected, actual);
        }

        public static void ShouldEqual<T>(this T actual, T expected, IEqualityComparer<T> comparer)
        {
            Assert.Equal(expected, actual, comparer);
        }

        public static void ShouldNotEqual<T>(this T actual, T expected)
        {
            Assert.NotEqual(actual, expected);
        }

        public static void ShouldNotEqual<T>(this T actual, T expected, IEqualityComparer<T> comparer)
        {
            Assert.NotEqual(actual, expected, comparer);
        }

        public static void ShouldNotBeNull(this object @object)
        {
            Assert.NotNull(@object);
        }

        public static void ShouldBeNull(this object @object)
        {
            Assert.Null(@object);
        }

        public static void ShouldBeSame(this object actual, object expected)
        {
            Assert.Same(expected, actual);
        }

        public static void ShouldNotBeSame(this object actual, object expected)
        {
            Assert.NotSame(expected, actual);
        }

        public static void ShouldEqual(this float actual, float expected, int precision)
        {
            Assert.Equal(expected, actual, precision);
        }

        public static void ShouldEqual(this double actual, double expected, int precision)
        {
            Assert.Equal(expected, actual, precision);
        }

        public static void ShouldEqual(this decimal actual, decimal expected, int precision)
        {
            Assert.Equal(expected, actual, precision);
        }

        public static void ShouldBeEmpty(this IEnumerable collection)
        {
            Assert.Empty(collection);
        }

        public static void ShouldNotBeEmpty(IEnumerable collection)
        {
            Assert.NotEmpty(collection);
        }

        public static void ShouldBeSingle(this IEnumerable collection)
        {
            Assert.Single(collection);
        }

        public static void ShouldBeSingle(this IEnumerable collection, object expected)
        {
            Assert.Single(collection, expected);
        }

        public static void ShouldBeSingle<T>(this IEnumerable<T> collection)
        {
            Assert.Single(collection);
        }

        public static void ShouldBeSingle<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            Assert.Single(collection, predicate);
        }

        public static void ShouldNotEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.NotEqual(expected, actual);
        }

        public static void ShouldContains<T>(this IEnumerable<T> collection, T expected)
        {
            Assert.Contains(expected, collection);
        }

        public static void ShouldContains<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> comparer)
        {
            Assert.Contains(expected, collection, comparer);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected)
        {
            Assert.DoesNotContain(expected, collection);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> comparer)
        {
            Assert.DoesNotContain(expected, collection, comparer);
        }

        public static void ShouldContains(this string actualString, string expectedSubstring)
        {
            Assert.Contains(expectedSubstring, actualString);
        }

        public static void ShouldContains(this string actualString, string expectedSubstring, StringComparison comparisonType)
        {
            Assert.Contains(expectedSubstring, actualString, comparisonType);
        }

        public static void ShouldNotContain(this string actualString, string expectedSubstring)
        {
            Assert.DoesNotContain(expectedSubstring, actualString);
        }

        public static void ShouldNotContain(this string actualString, string expectedSubstring, StringComparison comparisonType)
        {
            Assert.DoesNotContain(expectedSubstring, actualString, comparisonType);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.True(condition);
        }

        public static void ShouldBeTrue(this bool condition, string userMessage)
        {
            Assert.True(condition, userMessage);
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.False(condition);
        }

        public static void ShouldBeFalse(this bool condition, string userMessage)
        {
            Assert.False(condition, userMessage);
        }

        public static void ShouldEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.Equal(expected, actual);
        }

        public static void ShouldEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            Assert.Equal(expected, actual, comparer);
        }

        public static void ShouldBeInRange<T>(this T actual, T low, T high) where T : IComparable
        {
            Assert.InRange(actual, low, high);
        }

        public static void ShouldBeInRange<T>(this T actual, T low, T high, IComparer<T> comparer)
        {
            Assert.InRange(actual, low, high, comparer);
        }

        public static void ShouldNotBeInRange<T>(this T actual, T low, T high) where T : IComparable
        {
            Assert.NotInRange(actual, low, high);
        }

        public static void ShouldNotBeInRange<T>(this T actual, T low, T high, IComparer<T> comparer)
        {
            Assert.NotInRange(actual, low, high, comparer);
        }

        public static T ShouldBeAssignableFrom<T>(this object @object)
        {
            return Assert.IsAssignableFrom<T>(@object);
        }

        public static void ShouldBeAssignableFrom(this object @object, Type expectedType)
        {
            Assert.IsAssignableFrom(expectedType, @object);
        }

        public static T ShouldBeType<T>(this object @object)
        {
            return Assert.IsType<T>(@object);
        }

        public static void ShouldBeType(this object @object, Type expectedType)
        {
            Assert.IsType(expectedType, @object);
        }

        public static void ShouldNotBeType<T>(this object @object)
        {
            Assert.IsNotType<T>(@object);
        }

        public static void ShouldNotBeType(this object @object, Type expectedType)
        {
            Assert.IsNotType(expectedType, @object);
        }

        public static T ShouldThrow<T>(this Assert.ThrowsDelegate testCode) where T : Exception
        {
            return Assert.Throws<T>(testCode);
        }

        public static T ShouldThrow<T>(this Assert.ThrowsDelegateWithReturn testCode) where T : Exception
        {
            return Assert.Throws<T>(testCode);
        }

        public static Exception ShouldThrow(this Assert.ThrowsDelegate testCode, Type exceptionType)
        {
            return Assert.Throws(exceptionType, testCode);
        }

        public static Exception ShouldThrow(this Assert.ThrowsDelegateWithReturn testCode, Type exceptionType)
        {
            return Assert.Throws(exceptionType, testCode);
        }

        public static void ShouldNotThrow(this Assert.ThrowsDelegate testCode)
        {
            Assert.DoesNotThrow(testCode);
        }
    }
}