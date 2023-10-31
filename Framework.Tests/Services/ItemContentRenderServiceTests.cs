using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Services.Tests
{
    [TestClass()]
    public class ItemContentRenderServiceTests
    {
        [TestMethod()]
        public void RenderContentTest_Ok()
        {
            var contentString = "A simple ToDo item";
            var content = ItemContentRenderService.RenderContent(contentString);
            var contentNoOuterDiv = content.Replace("<div>", string.Empty).Replace("</div>", string.Empty);

            Assert.IsNotNull(content);
            Assert.AreEqual(contentString, contentNoOuterDiv);
        }

        [TestMethod()]
        public void RenderContentTest_Replaced_Url()
        {
            var contentString = "A simple ToDo item with a link: www.google.com";
            var content = ItemContentRenderService.RenderContent(contentString);
            var contentNoOuterDiv = content.Replace("<div>", string.Empty).Replace("</div>", string.Empty);

            Assert.IsNotNull(contentNoOuterDiv);
            Assert.AreNotSame(contentString, contentNoOuterDiv);
            Assert.IsTrue(contentNoOuterDiv.Length > contentString.Length);
        }

        [TestMethod()]
        public void RenderContentTest_Replaced_Email()
        {
            var contentString = "A simple ToDo item with an email-address: admin@google.com";
            var content = ItemContentRenderService.RenderContent(contentString);
            var contentNoOuterDiv = content.Replace("<div>", string.Empty).Replace("</div>", string.Empty);

            Assert.IsNotNull(contentNoOuterDiv);
            Assert.AreNotSame(contentString, contentNoOuterDiv);
            Assert.IsTrue(contentNoOuterDiv.Length > contentString.Length);
        }
    }
}