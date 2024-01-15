namespace TestProject1;

/// <summary>
/// Test scenario
/// </summary>
public class AutomatedUITests : Info
{
    /// <summary>
    ///سناریوی اول
    /// </summary>
    [Fact]
    public void Create_WhenExecuted_ReturnsCreateView()
    {
        try
        {
            // -1
            Login();
            // 0
            CreateBuilding(IsSeparationOwner_Tenant: true);
            // 1
            GoToDashboard();
            CreateCashDesk();
            // 2
            Delay5_Second();
            Button("تنظیم صندوق پیشفرض");
            Delay();
            MultiSelect("residentCashId", "صندوق شارژ");
            Delay();
            MultiSelect("ownerCashId", CASHDESK_NAME_OWNER);
            Delay();
            Button("ذخیره");
            Delay5_Second();
            // 3
            GoToUrl(URL_BILL);
            Delay5_Second();
            RunJS("document.getElementsByClassName('mat-focus-indicator mat-tooltip-trigger mat-fab mat-button-base mat-accent')[0].click()"); // click +
            Delay();
            RunJS("document.getElementsByClassName('mat-focus-indicator mat-tooltip-trigger mat-mini-fab mat-button-base mat-accent fab-action-item ng-star-inserted')[1].click()"); // click add
            Delay();
            MultiSelect("unitNo", "واحد ۱");
            Delay();
            MultiSelect("costTypeId", "بدهی اول دوره واحد");
            Delay();
            MultiSelect("debtorTypeId", "مالک");
            Delay();
            Input("amount", "1000");
            Delay();
            Button("ذخیره");
            Delay5_Second();
            // 4
            RunJS("document.getElementsByClassName('mat-focus-indicator mat-tooltip-trigger mat-fab mat-button-base mat-accent')[0].click()"); // click +
            Delay();
            RunJS("document.getElementsByClassName('mat-focus-indicator mat-tooltip-trigger mat-mini-fab mat-button-base mat-accent fab-action-item ng-star-inserted')[1].click()"); // click add
            Delay();
            MultiSelect("unitNo", "واحد ۱");
            Delay();
            MultiSelect("costTypeId", "بدهی اول دوره واحد");
            Delay();
            MultiSelect("debtorTypeId", "ساکن");
            Delay();
            Input("amount", "5000");
            Delay();
            Button("ذخیره");
            Delay5_Second();
            // 5
            GoToUrl(URL_BILLAN);
            Delay5_Second();
            RunJS("document.getElementsByTagName('app-billan-list-item')[0].click()"); // واحد اول
            Delay();
            Button("مالک"); // TODO: check amount of money for owner
            Delay2_Second();
            Button("ساکن"); // TODO: check amount of money for tenant
            Delay2_Second();
            // 6
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-menu-trigger mat-stroked-button mat-button-base profile-avatar ng-trigger ng-trigger-animate is-admin\")[0].click()");
            Delay2_Second();
            RunJS("document.getElementsByClassName(\"mat-list-item mat-focus-indicator px-8 cursor-pointer mat-2-line mat-list-item-with-avatar ng-star-inserted\")[1].click()"); // user panel
            Delay5_Second();
            Button("اعلام واریزی");
            Delay5_Second();
            Input("landlord", "500");
            Delay();
            Input("tenant", "4000");
            Delay();
            Button("ذخیره");
            Delay5_Second();
            // 7
            Button("اعلام واریزی");
            Delay2_Second();
            Input("tenant", "1000");
            Delay();
            Button("ذخیره");
            Delay5_Second();
            // 8
            Button("اعلام واریزی");
            Delay5_Second();
            Input("landlord", "500");
            Delay();
            Button("ذخیره");
            Delay5_Second();
            //9
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-menu-trigger mat-stroked-button mat-button-base profile-avatar ng-trigger ng-trigger-animate\")[0].click()");
            Delay2_Second();
            RunJS("document.getElementsByClassName(\"mat-list-item mat-focus-indicator px-8 cursor-pointer mat-2-line mat-list-item-with-avatar ng-star-inserted is-admin\")[0].click()"); // admin panel
            Delay5_Second();
            GoToUrl(URL_RECEIPT);
            Delay5_Second();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-stroked-button mat-button-base mat-accent ng-star-inserted\")[1].click()"); // decline
            Delay2_Second();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-button mat-button-base mat-accent\")[0].click()"); // confirm
            Delay5_Second();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-stroked-button mat-button-base mat-accent ng-star-inserted\")[0].click()"); // approve
            Delay();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-button mat-button-base mat-accent\")[0].click()"); // confirm
            Delay5_Second();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-stroked-button mat-button-base mat-accent ng-star-inserted\")[1].click()"); // approve
            Delay2_Second();
            RunJS("document.getElementsByClassName(\"mat-focus-indicator mat-button mat-button-base mat-accent\")[0].click()"); // confirm
            Delay5_Second();
            GoToUrl(URL_DASHBOARD_ADMIN);
            Delay5_Second();






            Assert.NotNull(_driver);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
        finally
        {
            DeleteBuilding();
        }
        Assert.NotNull(_driver);

        Delay5_Second();
        _driver.Close();
    }

    [Fact]
    public void RemoveRows()
    {
        try
        {
            DeleteBuilding();
            Assert.NotNull(_driver);
            _driver.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }
}