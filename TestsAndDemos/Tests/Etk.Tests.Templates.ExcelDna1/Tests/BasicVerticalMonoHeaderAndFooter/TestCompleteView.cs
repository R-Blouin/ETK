﻿namespace Etk.Tests.Templates.ExcelDna1.Tests.BasicVerticalMonoHeaderAndFooter
{
    using Etk.Excel.BindingTemplates.Views;

    class TestCompleteView : ExcelTest
    {
        public TestCompleteView() : base("Check rendering")
        { }

        override protected void RealExecute(IExcelTemplateView view)
        {
            if (view.RenderedArea == null)
                StepsErrorMessages.Add("Rendered area must not be null");
            else
            {
                if (view.RenderedArea == null || view.RenderedArea.Width != 4 || view.RenderedArea.Height != 6)
                    StepsErrorMessages.Add("Rendered area must be 4*6");

                if (view.RenderedRange[1, 1].Value != "ID")
                    StepsErrorMessages.Add("First cell must contains 'ID'");

                if (view.RenderedRange[6, 1].Value != "Shops")
                    StepsErrorMessages.Add("First cell of last row must contains 'Shops'");
            }
        }
    }
}
