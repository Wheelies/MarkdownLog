using System;
using System.Collections;
using System.Collections.Generic;
using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using NUnit.Framework;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class ReflectionExtensionsTests
    {
        [TestMethod]
        public void TestIsEnumerable()
        {
            Assert.IsTrue(new[] {"a", "b", "c"}.GetType().IsEnumerable());
            Assert.IsTrue(new[] {1, 2, 3}.GetType().IsEnumerable());
            Assert.IsTrue(new List<string> {"a", "b", "c"}.GetType().IsEnumerable());
            Assert.IsTrue(new List<int> {1, 2, 3}.GetType().IsEnumerable());
            Assert.IsTrue(new ArrayList {1, "a", true}.GetType().IsEnumerable());
            Assert.IsTrue("abc".GetType().IsEnumerable());

            Assert.IsFalse(123.GetType().IsEnumerable());
            Assert.IsFalse(true.GetType().IsEnumerable());
            Assert.IsFalse(new{Property1 = "value1"}.GetType().IsEnumerable());
        }
    }
}
