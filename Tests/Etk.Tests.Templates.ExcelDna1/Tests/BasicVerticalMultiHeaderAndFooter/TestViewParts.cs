﻿namespace Etk.Tests.Templates.ExcelDna1.Tests.BasicVerticalMultiHeaderAndFooter
{
    using System;
    using System.Collections.Generic;
    using Etk.Excel.BindingTemplates.Views;

    class TestViewParts : ExcelTest
    {
        public TestViewParts(IExcelTestTopic parent) : base(parent, "Control the size of the rendered area of the template parts and the value of some cells of these template parts")
        { }

        override protected void RealExecute(IExcelTemplateView view)
        {
            ExcelTemplateView excelView = view as ExcelTemplateView;

            if (excelView.RenderedArea == null || excelView.Renderer == null)
            {
                StepsErrorMessages.Add("Rendered area must not be null");
                return;
            }

            // Header
            if (excelView.Renderer.HeaderPartRenderer == null || excelView.Renderer.HeaderPartRenderer.RenderedArea == null)
                StepsErrorMessages.Add("Header rendered area must not be null");
            else
            {
                if(excelView.Renderer.HeaderPartRenderer.Width != 4 || excelView.Renderer.HeaderPartRenderer.Height != 2)
                    StepsErrorMessages.Add("Header rendered area must 2*1");
                if (excelView.Renderer.HeaderPartRenderer.RenderedRange[1, 1].Value != "Shops")
                    StepsErrorMessages.Add("Header Cell[1,1] must contains 'ID'");
                if (excelView.Renderer.HeaderPartRenderer.RenderedRange[2, 1].Value != "ID")
                    StepsErrorMessages.Add("Header Cell[2,1] must contains 'ID'");
            }

            // Body
            if (excelView.Renderer.BodyPartRenderer == null || excelView.Renderer.BodyPartRenderer.RenderedArea == null)
                StepsErrorMessages.Add("Body rendered area must not be null");
            else
            {
                if (excelView.Renderer.BodyPartRenderer.Width != 4 || excelView.Renderer.BodyPartRenderer.Height != 4)
                    StepsErrorMessages.Add("Body Rendered area must be 4*4");
                if (excelView.Renderer.BodyPartRenderer.RenderedRange[1, 1].Value != 1)
                    StepsErrorMessages.Add("Body First cell must contains '1'");
                if (excelView.Renderer.BodyPartRenderer.RenderedRange[4, 4].Value != "Fourth Shop Reception Phone number")
                    StepsErrorMessages.Add("Body Last cell must contains 'Fourth Shop Reception Phone number'");
            }

            // Footer
            if (excelView.Renderer.FooterPartRenderer == null || excelView.Renderer.FooterPartRenderer.RenderedArea == null)
                StepsErrorMessages.Add("Footer rendered area must not be null");
            else
            {
                if (excelView.Renderer.FooterPartRenderer.Width != 4 || excelView.Renderer.HeaderPartRenderer.Height != 2)
                    StepsErrorMessages.Add("Footer rendered area must 4*2");
                if (excelView.Renderer.FooterPartRenderer.RenderedRange[1, 1].Value != "Footer")
                    StepsErrorMessages.Add("Footer Cell[1,1] must contains 'Footer'");
                if (excelView.Renderer.FooterPartRenderer.RenderedRange[2, 1].Value != "Shops")
                    StepsErrorMessages.Add("Footer Cell[2,1] must contains 'Shops'");
            }
        }
    }
}
