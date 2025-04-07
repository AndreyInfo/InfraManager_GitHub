using InfraManager.BLL.Extensions;
using InfraManager.BLL.Localization;
using NUnit.Framework;

namespace IM.Core.BLL.Test
{
    public class EnumExtensionsTest
    {

        enum TestEnumFirst
        {
            [FriendlyName("Первый")]
            FirtValue = 1,

            [FriendlyName("Второй")]
            SecondValue = 2,

            [FriendlyName("Третий")]
            ThirdValue = 3,
        }

        enum TestEnumSecond
        {
            [FriendlyName("SomeName")]
            FirtValue = 0,
        }

        #region GetValueAttributeName
        [Test]
        public void CheckingGet_ValueEnumToId_And_NameFromAttributeToName_Test1()
        {
            var result = EnumExtensions.GetEnumListFriendlyName<TestEnumFirst>();
            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);

            Assert.AreEqual("Первый", result[0].Name);
            Assert.AreEqual("Второй", result[1].Name);
            Assert.AreEqual("Третий", result[2].Name);
        }



        
        [Test]
        public void CheckingGet_ValueEnumToId_And_NameFromAttributeToName_Test2()
        {
            var result = EnumExtensions.GetEnumListFriendlyName<TestEnumSecond>();
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(0, result[0].Id);

            Assert.AreEqual("SomeName", result[0].Name);
        }
        #endregion

        #region GetNameFromFirendlyname
        [Test]
        public void CheckEqualsName_Test1()
        {
            var result = EnumExtensions.GetNameFromFriendlyName<TestEnumSecond>(TestEnumSecond.FirtValue);

            Assert.AreEqual("SomeName", result);
        }


        [Test]
        public void CheckEqualsSevaralVariants_Test2()
        {
            var name = EnumExtensions.GetNameFromFriendlyName<TestEnumFirst>(TestEnumFirst.FirtValue);
            Assert.AreEqual("Первый", name);

            name = EnumExtensions.GetNameFromFriendlyName<TestEnumFirst>(TestEnumFirst.SecondValue);
            Assert.AreEqual("Второй", name);

            name = EnumExtensions.GetNameFromFriendlyName<TestEnumFirst>(TestEnumFirst.ThirdValue);
            Assert.AreEqual("Третий", name);
        }
        #endregion

    }
}
