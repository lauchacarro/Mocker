using Mocker.Web.Extensions.Validations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mocker.Test.ExtensionsTest.ValidationsTest
{
    public class ValidateFileSizeExtensionTest
    {
        private const long NinetyMegaBytes = 90 * 1000 * 1000;
        private const long HundredTenMegaBytes = 110 * 1000 * 1000;

        [Test]
        public void IsGreaterThan100MB_LessThan100MB_DontInvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            NinetyMegaBytes.IsGreaterThan100MB(action);

            Assert.IsFalse(actionWasInvoked);
        }

        [Test]
        public void IsGreaterThan100MB_GreaterThan100MB_InvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            HundredTenMegaBytes.IsGreaterThan100MB(action);

            Assert.IsTrue(actionWasInvoked);
        }
    }
}
