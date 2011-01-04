using System;
using NUnit.Framework;

namespace Themis
{
    public static class DateTimeAssert
    {
        /// <summary>
        /// Asserts that the expected and actual values are exactly equal, and the same kind
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreEqual(DateTime expected, DateTime actual)
        {
            // it seems that this statement won't catch differences in kinds
            Assert.AreEqual(expected, actual, "DateTime Value");
            Assert.AreEqual(expected.Kind, actual.Kind, "DateTime Kind");
        }

    }
}
