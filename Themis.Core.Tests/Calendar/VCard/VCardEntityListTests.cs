using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Themis.Calendar.VCard
{
    [TestFixture]
    public class VCardEntityListTests
    {
        [Test]
        public void TryGetFirstEntity_Return_False_When_None_Exist()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2"),
            };

            VCardSimpleValue value;
            bool result = list.TryGetFirstEntity("three", out value);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetFirstEntity_When_One_Exists()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2"),
            };

            VCardSimpleValue value;
            bool result = list.TryGetFirstEntity("two", out value);

            Assert.That(result, Is.True);
            Assert.That(value.Name, Is.EqualTo("two"));
        }

        [Test]
        public void TryGetFirstEntity_Return_First_When_Two_Exist()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2A"),
                new VCardSimpleValue("two", "2B"),
            };

            VCardSimpleValue value;
            bool result = list.TryGetFirstEntity("two", out value);

            Assert.That(result, Is.True);
            Assert.That(value.Name, Is.EqualTo("two"));
            Assert.That(value.EscapedValue, Is.EqualTo("2A"));
        }

        [Test]
        public void TryGetFirstEntity_By_Different_Case()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2A"),
                new VCardSimpleValue("TWO", "2B"),
            };

            VCardSimpleValue value;
            bool result = list.TryGetFirstEntity("TWO", out value);

            Assert.That(result, Is.True);
            Assert.That(value.Name, Is.EqualTo("two"));
            Assert.That(value.EscapedValue, Is.EqualTo("2A"));
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetFirstEntity_Throw_When_None_Exist()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2A"),
                new VCardSimpleValue("two", "2B"),
            };

            list.GetFirstEntity("three");
        }

        [Test]
        public void GetFirstEntity_Success_When_One_Exists()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2"),
            };

            VCardSimpleValue value = list.GetFirstEntity("two");

            Assert.That(value.Name, Is.EqualTo("two"));
        }

        [Test]
        public void GetEntities_No_Matches()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2"),
            };

            var result = list.GetEntities("three");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetEntities_Two_Of_Three_Matches()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2A"),
                new VCardSimpleValue("two", "2B"),
            };

            VCardSimpleValue[] result = list.GetEntities("two").ToArray();

            Assert.That(result.GetLength(0), Is.EqualTo(2));
            CollectionAssert.AllItemsAreUnique(result);
        }

        [Test]
        public void GetEntities_Two_Matches_With_Different_Case()
        {
            VCardEntityList<VCardSimpleValue> list = new VCardEntityList<VCardSimpleValue>() 
            {
                new VCardSimpleValue("one", "1"),
                new VCardSimpleValue("two", "2A"),
                new VCardSimpleValue("TwO", "2B"),
            };

            VCardSimpleValue[] result = list.GetEntities("TWO").ToArray();

            Assert.That(result.GetLength(0), Is.EqualTo(2));
            CollectionAssert.AllItemsAreUnique(result);

        }

    }
}
