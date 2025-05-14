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
    public class UserValidateMethodTests
    {

        [TestMethod()]
        public void ValidateUserIdTest()
        {
            ValidateMethodForUser.ValidateUserID(1);
        }



        [TestMethod()]
        public void ValidateUserNameTest()
        {
            string name = "John";
            string name2 = "Åse";

            ValidateMethodForUser.ValidateUserName(name);
            ValidateMethodForUser.ValidateUserName(name2);
        }

        [TestMethod()]
        public void ValidateEmailTest()
        {
            string email = "test@email.com";
           
            ValidateMethodForUser.ValidateUserEmail(email);


        }
        [TestMethod()]
        public void ValidateEmailTest1()
        {       
            string email1 = "test.ok@email.com";
            
     
            ValidateMethodForUser.ValidateUserEmail(email1);
        }
        [TestMethod()]
        public void ValidateEmailTest2_Throw_Exception()
        {
           
            string email3 = "test.com";
            
            ValidateMethodForUser.ValidateUserEmail(email3);


        }

        [TestMethod()]
        public void ValidatePasswordTest_Works()
        {
            string password = "Abcdefgh1!";

            ValidateMethodForUser.ValidateUserPassword(password);
        }

        [TestMethod()]
        public void ValidatePasswordTest2_Works()
        {
            string password = "StrongPass1@!";


            ValidateMethodForUser.ValidateUserPassword(password);
        }

        [TestMethod()]
        public void ValidatePasswordTest3_Do_Not_Work()
        {
            string password = "";

            ValidateMethodForUser.ValidateUserPassword(password);
        }


        [TestMethod()]
        public void ValidateUser()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Joan",
                Email = "j.joan24@gmail.com",
                Password = "PassW0rd!",


            };

            ValidateMethodForUser.ValidateUser(user);

        }

    }
}