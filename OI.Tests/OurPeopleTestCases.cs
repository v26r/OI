
using NUnit.Framework;
using OI.TestFramework.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OI.Tests
{
    [TestFixture]
    public class OurPeopleTestCases
    {
        #region Variables

        private static IWebDriver _driver;
        private static OurPeople _ourPeoplePage;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize the one time setup 
        /// </summary>
        [OneTimeSetUp]
        public void SetupTest()
        {
            //Settings to maximise the browser
            var chromeoptions = new ChromeOptions();
            chromeoptions.AddArguments("start-maximized");

            //Initilaize the driver and browser
            _driver = new ChromeDriver(chromeoptions);
            _ourPeoplePage = new OurPeople(_driver);

        }

        #endregion

        #region Tests

        /// <summary>
        /// Test Case to check if the "Our people" page is opened or not by comparing actual page title 
        /// with the expected page title
        /// </summary>

        [Test, Order(1)]
        public void CanOpenOurPeoplePage()
        {
            //Open the Our People Page
            _ourPeoplePage.Open();

            // Accept the notification to continue
            _ourPeoplePage.ClickContinue();

            //Assert that page title matches the expected title
            Assert.IsTrue(_ourPeoplePage.IsAt());

        }

        /// <summary>
        /// Test Case to check if the team members can be searched by using the search box and this test case also covers the scenario of 
        /// case insensitive search as well
        /// </summary>

        [Test, Order(2)]
        public void CanSearchByName()
        {
            //Arrange the input and also the expected count
            string name = "carla";
            int expectedCount = 1;

            // Search by given name(should work in both capital or small letters) and return the count of matching records
            int resultCount = _ourPeoplePage.SearchByName(name);

            //Assert that result is not zero and equals to the expected result
            Assert.NotZero(resultCount);
            Assert.AreEqual(resultCount, expectedCount);

        }


        /// <summary>
        /// Test Case to check if the team members can be searched by using the search box and if the user enters name but with either 
        /// with a space suffix("carla ") or with a space prefix(" carla") then search should return matching records by doing the left trim
        /// and right trim ( As this usually happens when user copies and pastes in the box)
        /// </summary>

        [Test, Order(3)]
        public void CanSearchByNameWithSpacePrefixOrSuffix()
        {
            //Arrange the input and also the expected count
            string name = " carla";
            int expectedCount = 1;

            // Search by given name(should ignore the left and right spaces and then search for exact word) and return the count of matching records
            int resultCount = _ourPeoplePage.SearchByName(name);

            //Assert that result is not zero and matches the expected result
            Assert.NotZero(resultCount);
            Assert.AreEqual(resultCount, expectedCount);

        }

        /// <summary>
        /// Test Case to check if the searched team member is not found then it should show the proper message
        ///  </summary>

        [Test, Order(4)]
        public void CanGetMessageIfNotFound()
        {
            //Arrange the input and also the expected count
            string name = "suma";
            
            //Search the name which is not in the list and check if the message is displayed and also compare it with the expected message
            bool messageDisplayed = _ourPeoplePage.SearchByUnknownName(name);

            //Assert that message is displayed and matches to the expected "No results found" message
            Assert.IsTrue(messageDisplayed);

        }

        [Ignore("Work In Progress")]
        [Test,Order(5)]
        public void CanSortByDescending()
        {
            bool sorted = false;

            //Act
            _ourPeoplePage.Open();
            _ourPeoplePage.ClickContinue();

            //To DO: Implement the Sort By Descending method
            //sorted = _ourPeoplePage.SortByDescending();

            //Assert
            Assert.IsTrue(sorted);

        }

        #endregion

        #region Tear Down

        /// <summary>
        /// Closes the browser and driver 
        /// </summary>

        [OneTimeTearDown]
        public void TearDownTest()
        {
            _driver.Close();
            _driver.Quit();
        }

        #endregion
    }
}
