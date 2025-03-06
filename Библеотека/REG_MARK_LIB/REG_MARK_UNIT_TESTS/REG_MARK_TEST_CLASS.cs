using Microsoft.VisualStudio.TestTools.UnitTesting;
using REG_MARK_LIB;
using System;

namespace REG_MARK_UNIT_TESTS
{
    [TestClass]
    public class REG_MARK_TEST_CLASS
    {
        // Группа 1: Тесты низкой сложности

        [TestMethod]
        public void CheckMark_ValidMark_True()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.IsTrue(REG_MARK_CLASS.CheckMark("A123BC77"));
        }

        [TestMethod]
        public void CheckMark_InvalidFormat_False()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.IsFalse(REG_MARK_CLASS.CheckMark("123ABC77"));
        }

        [TestMethod]
        public void CheckMark_InvalidRegion_False()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.IsFalse(REG_MARK_CLASS.CheckMark("A123BC999"));
        }

        [TestMethod]
        public void GetNextMarkAfter_NormalCase_NextMark()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("A124BC77", REG_MARK_CLASS.GetNextMarkAfter("A123BC77"));
        }

        [TestMethod]
        public void GetNextMarkAfter_MaxNumber_ChangeSeries()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("A000BT77", REG_MARK_CLASS.GetNextMarkAfter("A999BC77"));
        }

        [TestMethod]
        public void GetNextMarkAfter_LetterOverflow_NewSeries()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("B000AA77", REG_MARK_CLASS.GetNextMarkAfter("A999XX77"));
        }

        [TestMethod]
        public void GetNextMarkAfterInRange_WithinRange_NextMark()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("A124BC77", REG_MARK_CLASS.GetNextMarkAfterInRange("A123BC77", "A100BC77", "A200BC77"));
        }

        [TestMethod]
        public void GetNextMarkAfterInRange_OutOfStock_Message()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("out of stock", REG_MARK_CLASS.GetNextMarkAfterInRange("A999XX77", "A100AA77", "A999XX77"));
        }

        [TestMethod]
        public void GetCombinationsCountInRange_SimpleRange_Count()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual(100, REG_MARK_CLASS.GetCombinationsCountInRange("A000AA77", "A099AA77"));
        }

        [TestMethod]
        public void GetCombinationsCountInRange_SameMark_One()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual(1, REG_MARK_CLASS.GetCombinationsCountInRange("A123BC77", "A123BC77"));
        }


        // Группа 2: Тесты высокой сложности

        [TestMethod]
        public void GetNextMarkAfter_InvalidFormat_ThrowsArgumentException()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Action action = delegate () { REG_MARK_CLASS.GetNextMarkAfter("неверный номер"); };
            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void CheckMark_UpperBoundary_True()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.IsTrue(REG_MARK_CLASS.CheckMark("X999XX997"));
        }

        [TestMethod]
        public void GetNextMarkAfter_FullCycle_Reset()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("A000AA997", REG_MARK_CLASS.GetNextMarkAfter("X999XX997"));
        }

        [TestMethod]
        public void GetCombinationsCountInRange_MaxRange_CorrectCount()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual(1728000, REG_MARK_CLASS.GetCombinationsCountInRange("A000AA01", "X999XX997"));
        }

        [TestMethod]
        public void GetNextMarkAfterInRange_ComplexEdgeCase_OutOfStock()
        {
            REG_MARK_CLASS REG_MARK_CLASS = new REG_MARK_CLASS();
            Assert.AreEqual("out of stock", REG_MARK_CLASS.GetNextMarkAfterInRange("X999XX997", "A000AA997", "X999XX997"));
        }

    }
}
