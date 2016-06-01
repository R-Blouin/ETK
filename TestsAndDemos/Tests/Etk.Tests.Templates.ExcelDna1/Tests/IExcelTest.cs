﻿using Etk.Excel.BindingTemplates.Views;

namespace Etk.Tests.Templates.ExcelDna1.Tests
{
    interface IExcelTest
    {
        string Description{ get; }
        bool Success{ get; }
        bool Done{ get; }
        string Errors { get; }

        void InitTestStatus();
        void Execute(IExcelTemplateView view);
    }
}
