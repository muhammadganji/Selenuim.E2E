using System.Data.SqlClient;
using System.Text;

namespace TestProject1;

public class Info : IDisposable
{

    protected readonly IWebDriver _driver;
    protected const string URL_LOGIN = "http://192.168.1.107:5275/";
    protected const string URL_Home = "http://192.168.1.103:5036/panel/home";
    protected const string URL_DASHBOARD = "http://192.168.1.103:5036/panel/home";
    protected const string URL_SETTING = "http://192.168.1.103:5036/panel/admin/settings/data-personalization";
    protected const string URL_BILL = "http://192.168.1.103:5036/panel/admin/bill";
    protected const string URL_BILLAN = "http://192.168.1.103:5036/panel/admin/billan";
    protected const string URL_RECEIPT = "http://192.168.1.103:5036/panel/admin/receipt";
    protected const string URL_DASHBOARD_ADMIN = "http://192.168.1.103:5036/panel/admin/dashboard";

    protected const string LOGIN_USERNAME = "00000000000";
    protected const string LOGIN_CODE = "05837";
    protected const string BUILDING_TITLE = "ساختمان امیران دومی";
    protected const string BUILDING_UNIT_COUNT = "4";
    protected const string BUILDING_INIT_CASH = "0";
    protected const string BUILDING_DEFAULT_EQUAL_SHARJ = "1000000";
    protected const string BUILDING_TYPE_SCALE = "ساختمان";
    protected const string BUILDING_TYPE_USAGE = "مسکونی";
    protected const string BUILDING_MANAGER_UNIT_NO = "4";
    protected readonly SqlConnectionStringBuilder builder;
    protected const string CASHDESK_NAME_OWNER = "مالکانه";

    public Info()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        builder = new SqlConnectionStringBuilder()
        {
            DataSource = "192.168.1.102\\SQL2017",
            InitialCatalog = "SharjBookLocal",
            UserID = "sharj",
            Password = "1400",
            MultipleActiveResultSets = true,
            MaxPoolSize = 32767,
            TrustServerCertificate = true
        };
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    protected void GoToUrl(string url) => _driver.Navigate().GoToUrl(url);

    /// <summary>
    /// تاخیر 1 ثانیه
    /// </summary>
    protected void Delay() => Thread.Sleep(1000);
    protected void Delay2_Second() => Thread.Sleep(2000);
    protected void Delay5_Second() => Thread.Sleep(5000);
    protected void Delay10_Second() => Thread.Sleep(10000);
    protected void Delay20_Second() => Thread.Sleep(20000);

    /// <summary>
    /// ورود متن در فیلد متنی ساده By.Name
    /// </summary>
    protected void Input(string name, string value) => _driver.FindElement(By.Name(name)).SendKeys(value);

    /// <summary>
    /// ورود متن در فیلد متنی ساده By.Id
    /// </summary>
    protected void InputId(string name, string value) => _driver.FindElement(By.Id(name)).SendKeys(value);

    /// <summary>
    /// انتخاب کمبو باکس By.Name
    /// </summary>
    protected void MultiSelect(string name, string value)
    {
        _driver.FindElement(By.CssSelector($"mat-select[name='{name}']")).Click();
        Delay();
        _driver.FindElement(By.XPath($"//mat-option//span[contains(text(), '{value}')]")).Click();
    }

    /// <summary>
    /// فشردن دکمه کلیک
    /// </summary>
    protected void Button(string name) => _driver.FindElements(By.TagName("button")).Where(p => p.Text.Contains(name)).FirstOrDefault()?.Click();

    /// <summary>
    /// فشردن دکمه کلیک By.Class
    /// </summary>
    protected void ButtonClass(string name) => _driver.FindElements(By.ClassName(name)).FirstOrDefault()?.Click();

    /// <summary>
    /// کلیک روی چک باکس By.Id
    /// </summary>
    protected void Checkbox(string name) => RunJS($"document.getElementById('{name}').click()");

    /// <summary>
    /// اسکرول صفحه به سمت پایین
    /// , باید المان هارو ببینیم که خطا نخوریم
    /// </summary>
    protected void ScrollDown(string name) => RunJS($"document.getElementById('{name}').scrollTo(0, document.getElementById('{name}').scrollHeight);");

    /// <summary>
    /// اجرای دستورات جاوا اسکریپت
    /// </summary>
    protected void RunJS(string js) => ((IJavaScriptExecutor)_driver).ExecuteScript(js);

    /// <summary>
    /// رفرش شدن صفحه
    /// </summary>
    protected void Refresh()
    {
        _driver.Navigate().Refresh();
        Delay5_Second();
    }

    /// <summary>
    /// ورود به برنامه
    /// </summary>
    protected void Login(string PhoneNumber = LOGIN_USERNAME, string Code = LOGIN_CODE, string UrlLogin = URL_LOGIN)
    {
        GoToUrl(UrlLogin);
        Delay2_Second();
        Input("Username", PhoneNumber);
        Button("ارسال");
        Delay2_Second();
        Input("Password", Code);
        Delay2_Second();
        //Button("ورود");
    }

    /// <summary>
    /// ایجاد ساختمان جدید
    /// </summary>
    /// <param name="IsSeparationOwner_Tenant">تفکیک بین مالک و ساکن باشد؟</param>
    protected void CreateBuilding(bool IsSeparationOwner_Tenant = false)
    {
        GoToUrl(URL_Home);
        Delay5_Second();
        ScrollDown("homeContent");
        Delay2_Second();
        ButtonClass("add-new-board");
        Delay2_Second();
        Button("بله");
        Delay2_Second();
        MultiSelect("typeScaleId", BUILDING_TYPE_SCALE);
        Delay();
        MultiSelect("typeUsageId", BUILDING_TYPE_USAGE);
        Delay();
        Input("title", BUILDING_TITLE);
        Delay();
        Input("unitCount", BUILDING_UNIT_COUNT);
        Delay();
        Input("initCash", BUILDING_INIT_CASH);
        Delay();
        Input("defaultEqualSharj", BUILDING_DEFAULT_EQUAL_SHARJ);
        Delay();
        if (IsSeparationOwner_Tenant) Checkbox("mat-checkbox-1-input");
        Delay();
        Button("بعدی");
        Delay();
        Input("managerUnitNo", BUILDING_MANAGER_UNIT_NO);
        Delay();
        Button("ذخیره");
        Delay10_Second();


    }

    /// <summary>
    /// حذف ساختمان
    /// </summary>
    protected void DeleteBuilding(bool RunQuery = true)
    {
        if (RunQuery) // حذف سریع ساختمان
        {
            try
            {
                using (SqlConnection connection = new(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new($"UPDATE apm SET RemovedStatus = 1 WHERE title = N'{BUILDING_TITLE}'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
        else
        {
            // TODO: Delete building with UI
        }

    }

    /// <summary>
    /// رفتن به داشبورد ساختمان خاص role Admin
    /// </summary>
    protected void GoToDashboard(string name = BUILDING_TITLE)
    {
        RunJS("var targetText = '" + name + "'; var divs = document.querySelectorAll('div.board-list-item');  divs.forEach(function(div) {  var label = div.querySelector('label.board-name'); var role = div.querySelector('span.board-role');  if (label != null && label.innerText.trim().includes(targetText) && role != null && role.innerText.trim().includes('مدیر')) {  div.click(); return true; } });");
    }

    /// <summary>
    /// ساخت صندوق در حالت اول
    /// </summary>
    protected void CreateCashDesk(string name = CASHDESK_NAME_OWNER)
    {
        GoToUrl(URL_SETTING);
        Delay5_Second();
        Button("ایجاد صندوق");
        Input("name", name);
        Delay();
        Button("ذخیره");
        Delay();
        Button("۷ روز استفاده رایگان");
        Delay10_Second();
        Button("ایجاد صندوق");
        Delay();
        Input("name", name);
        Delay();
        Button("ذخیره");
    }


}
