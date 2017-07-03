using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace OI.TestFramework.PageObjects
{
    public class OurPeople
    {
        #region Private Variables

        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private const string Url = @"https://www.octopusinvestments.com/adviser/about-us/our-people";
        private const string TitleText = "Our people | Adviser | Octopus Investments";
        private const string NotFoundMessage = "No results found";

        #endregion Private Variables

        #region Page Elements

        [FindsBy(How = How.Id, Using = "myAdviser")]
        public IWebElement Modal { get; set; }

        [FindsBy(How = How.ClassName, Using = "pink")]
        public IWebElement BtnContinue { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div[3]/div/div/div[1]/div[2]/div[1]/div/div/input")]
        public IWebElement SearchBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "selectOne")]
        public IWebElement SortBy { get; set; }

        [FindsBy(How = How.ClassName, Using = "selectTwo")]
        public IWebElement Team { get; set; }

        [FindsBy(How = How.ClassName, Using = "effect-3")]
        public IWebElement SearchResults { get; set; }
        

        #endregion

        #region Constructor
        public OurPeople(IWebDriver browser)
        {
            _driver = browser;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }

        #endregion

        #region Page Actions and Helper methods

        /// <summary>
        /// Navigates to the url and initializes the elements in page objects
        /// </summary>
        public void Open()
        {
            //Navigate to Our people page
            _driver.Navigate().GoToUrl(Url);

            //Initialize the our people page elements in page objects
            PageFactory.InitElements(_driver, this);
        }

        /// <summary>
        /// Compares the expected page title with actual page title and returns true or false
        /// </summary>
        /// <returns></returns>
        public bool IsAt()
        {
            return _driver.Title == TitleText;
        }

        /// <summary>
        /// Accepts the notification and click on continue button
        /// </summary>
        public void ClickContinue()
        {
            //Wait for notification Modal to load
            WaitForVisibilityById(Modal);

            //Click on the continue button
            Actions actions = new Actions(_driver);
            actions.MoveToElement(BtnContinue).Click().Perform();
        }

        /// <summary>
        /// Searches the team members by using the name entered in the search box and return the no. of matching records
        /// </summary>
        /// <param name="name"></param>
        /// <returns>count</returns>
        public int SearchByName(string name)
        {
            //Wait for Search Box to load
            WaitForVisibilityByXpath("/html/body/div[3]/div/div/div[1]/div[2]/div[1]/div/div/input");

            // Search for the given person name by entering in the searchbox
            SearchBox.Click();
            SearchBox.Clear();
            SearchBox.SendKeys(name);
            SearchBox.SendKeys(Keys.Return);

            // Get the list of persons from Results
            IList<IWebElement> persons = SearchResults.FindElements(By.TagName("a"));

            // Return the no of persons matching to the given name
            return persons.Count(a => a.Text.ToLower().Contains(name.ToLower()));
        }

        /// <summary>
        /// Searches the team members by using the name not in the list and compare the not found message is same as the expected
        /// </summary>
        /// <param name="name"></param>
        /// <returns>count</returns>
        public bool SearchByUnknownName(string name)
        {
           // Search for the given person name by entering in the searchbox
            SearchBox.Click();
            SearchBox.Clear();
            SearchBox.SendKeys(name);
            SearchBox.SendKeys(Keys.Return);

            // Get the list of paragraph elements from Results
            IList<IWebElement> elements = SearchResults.FindElements(By.TagName("p"));

            // Return the no of elements matching to the given not found message
            return elements.Any(a => a.Text.Contains(NotFoundMessage));
        }
        
        /// <summary>
        /// Waits for the element to load based on the given element id
        /// </summary>
        /// <param name="element"></param>
        private void WaitForVisibilityById(IWebElement element)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id(element.GetAttribute("id"))));
        }

        /// <summary>
        /// Waits for the element to load based on the given element XPath
        /// </summary>
        /// <param name="xPath"></param>
        private void WaitForVisibilityByXpath(string xPath)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }

        #endregion
    }
}
