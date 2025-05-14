using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuatroCleanUpBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodTests
{

    [TestClass()]
    public class PictureRepositoryTests
    {
        private readonly IConfiguration  c;

        public PictureRepositoryTests(IConfiguration configuration)
        {
            c = configuration;
                }

        [TestMethod()]
        public void AddTest()
        {
            //Arrange
            PicturesRepository pic = new PicturesRepository(c);


            //Act

            Picture picture = pic.Add(new Picture());

            //Assert
            Assert.AreEqual(1, picture.PictureId);

        }
    }
}