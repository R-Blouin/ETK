﻿namespace Etk.Tests.Templates.ExcelDna1.Tests.BasicVerticalNoHeaderAndFooter
{
    using Etk.Excel;
    using Etk.Tests.Data.Shops;
    
    class BasicVerticalNoHeaderAndFooterTests : ExcelTestTopic
    {
        public BasicVerticalNoHeaderAndFooterTests(IExcelTestsManager testManager) : base(testManager, "Tests on a basic template without linked templates and without header or footer")
        {
            Tests.Add(new TestRendering(this));
        }

        override protected void RealInit()
        {
            CreateView("VerticalNoHeaderAndFooter", "BasicTemplates1", "BasicVerticalNoHeaderAndFooter");
            View.SetDataSource(ShopManager.GetShops());
            ETKExcel.TemplateManager.Render(View);
        }
    }
}
