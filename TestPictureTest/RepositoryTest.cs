using QuatroCleanUpBackend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace TestPictureTest
{
    [TestClass]
    public sealed class PictureRepositoryTest
    {
        

        [TestMethod]
        
        public void AddTestMethod1()
        {
            //Arrange
            PicturesRepository pic = new PicturesRepository();
            

            //Act

            Pictures picture = pic.Add(new Pictures());

            //Assert
            Assert.AreEqual(1, picture.PictureId);

        }
    }
}
