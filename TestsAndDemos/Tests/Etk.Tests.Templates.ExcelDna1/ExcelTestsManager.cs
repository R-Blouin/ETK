﻿namespace Etk.Tests.Templates.ExcelDna1
{
    using System.Collections.Generic;
    using Etk.Tests.Templates.ExcelDna1.Tests;
    using Etk.Tests.Templates.ExcelDna1.Tests.BasicVerticalMonoHeaderAndFooter;
    using Etk.Tests.Templates.ExcelDna1.Tests.BasicVerticalNoHeaderAndFooter;

    class ExcelTestsManager
    {
        private List<IExcelTests> tests;

        public IEnumerable<IExcelTests> Tests
        { get { return tests; } }

        public ExcelTestsManager()
        {
            tests = new List<IExcelTests>();
            tests.Add(new BasicVerticalNoHeaderAndFooterTests());
            tests.Add(new BasicVerticalMonoHeaderAndFooterTests());

            foreach (IExcelTests test in tests)
                test.Init();
        }

        public void Execute()
        {
            foreach (IExcelTests test in tests)
                test.Execute();
        }
    }
}