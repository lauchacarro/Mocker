using Mocker.Web.Extensions.Validations;
using Mocker.Web.Models.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mocker.Test.ExtensionsTest.ValidationsTest
{
    public class ValidateMockModelExtensionTest
    {
        [Test]
        public void ValidateDuplicateMethod_HaveDifferentMethods_DontInvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            MockModel mockModelGET = new MockModel()
            {
                HttpMethod = "GET"
            };

            MockModel mockModelPOST = new MockModel()
            {
                HttpMethod = "POST"
            };

            List<MockModel> listMockModels = new List<MockModel>()
            {
                mockModelGET,
                mockModelPOST
            };

            listMockModels.ValidateDuplicateMethod(action);

            Assert.IsFalse(actionWasInvoked);
        }

        [Test]
        public void ValidateDuplicateMethod_HaveOneMethod_DontInvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            MockModel mockModelGET = new MockModel()
            {
                HttpMethod = "GET"
            };

            List<MockModel> listMockModels = new List<MockModel>()
            {
                mockModelGET
            };

            listMockModels.ValidateDuplicateMethod(action);

            Assert.IsFalse(actionWasInvoked);
        }

        [Test]
        public void ValidateDuplicateMethod_SendNull_DontInvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            MockModel mockModelGET = new MockModel()
            {
                HttpMethod = "GET"
            };

            IEnumerable<MockModel> listMockModels = null;

            Assert.Throws<ArgumentNullException>(() =>
                {
                    listMockModels.ValidateDuplicateMethod(action);
                }
            );
            Assert.IsFalse(actionWasInvoked);

        }

        [Test]
        public void ValidateDuplicateMethod_DontHaveMethod_DontInvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            List<MockModel> listMockModels = new List<MockModel>()
            {

            };

            listMockModels.ValidateDuplicateMethod(action);

            Assert.IsFalse(actionWasInvoked);
        }

        [Test]
        public void ValidateDuplicateMethod_HaveSameMethods_InvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            MockModel mockModelGET1 = new MockModel()
            {
                HttpMethod = "GET"
            };

            MockModel mockModelGET2 = new MockModel()
            {
                HttpMethod = "GET"
            };

            List<MockModel> listMockModels = new List<MockModel>()
            {
                mockModelGET1,
                mockModelGET2
            };

            listMockModels.ValidateDuplicateMethod(action);

            Assert.IsTrue(actionWasInvoked);
        }

        [Test]
        public void ValidateDuplicateMethod_HaveSameMethodsDifferentBound_InvokeAction()
        {
            bool actionWasInvoked = false;
            Action action = () => actionWasInvoked = true;

            MockModel mockModelGETUpper = new MockModel()
            {
                HttpMethod = "GET"
            };

            MockModel mockModelGETLower = new MockModel()
            {
                HttpMethod = "get"
            };

            List<MockModel> listMockModels = new List<MockModel>()
            {
                mockModelGETUpper,
                mockModelGETLower
            };

            listMockModels.ValidateDuplicateMethod(action);

            Assert.IsTrue(actionWasInvoked);
        }
    }
}
