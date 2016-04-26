﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Etk.Excel.Application;
using Etk.Excel.BindingTemplates;
using Etk.Excel.ContextualMenus;
using Etk.Excel.RequestManagement;
using Etk.ModelManagement;

namespace Etk.Excel
{
    /// <summary> 
    /// Framework main class. 
    /// Gateway to all the Etk framework fonctionalities 
    /// </summary>
    [Export]
    public sealed class ETKExcel
    {
        #region properties
        private bool isDisposed = false;
        private static readonly object syncObj = new object();

        private List<Microsoft.Office.Interop.Excel.Workbook> managedWorkbooks = new List<Microsoft.Office.Interop.Excel.Workbook>();

        [Import(AllowDefault = false)]
        private ExcelApplication excelApplication = null;
        [Import(AllowDefault = false)]
        private ExcelTemplateManager templateManager = null;
        [Import(AllowDefault = false)]
        private ContextualMenuManager contextualMenuManager = null;

        [Import(AllowDefault = false)]
        private ModelDefinitionManager modelDefinitionManager = null;
        [Import(AllowDefault = false)]
        private RequestsManager RequestsManager = null;

        #region singleton
        internal static ETKExcel Instance;
        #endregion

        /// <summary>Give acces to the <see cref="IExcelTemplateManager"/> part of the  framework</summary>
        public static IExcelTemplateManager TemplateManager
        {
            get 
            {
                if (Instance == null || Instance.isDisposed)
                    throw new EtkException("'ETKExcel' is not initialyzed or was disposed");
                return Instance.templateManager; 
            }
        }

        /// <summary>Give acces to the <see cref="IExcelApplication"/> part of the  framework</summary>
        public static IExcelApplication ExcelApplication
        {
            get 
            {
                if (Instance == null || Instance.isDisposed)
                    throw new EtkException("'ETKExcel' is not initialyzed or was disposed");
                return Instance.excelApplication; 
            }
        }

        /// <summary>Give acces to the <see cref="IContextualMenuManager"/> part of the  framework</summary>
        public static IContextualMenuManager ContextualMenuManager
        {
            get
            {
                if (Instance == null || Instance.isDisposed)
                    throw new EtkException("'ETKExcel' is not initialyzed or was disposed");
                return Instance.contextualMenuManager;
            }
        }

        /// <summary>Give acces to the <see cref="IModelDefinitionManager"/> part of the  framework</summary>
        public static IModelDefinitionManager ModelDefinitionManager
        {
            get
            {
                if (Instance == null || Instance.isDisposed)
                    throw new EtkException("'ETKExcel' is not initialyzed or was disposed");
                return Instance.modelDefinitionManager;
            }
        }
        #endregion

        #region .ctors
        private ETKExcel(Microsoft.Office.Interop.Excel.Application application)
        {
            if (application == null)
                throw new EtkException("ETKExcel initialization: the 'application' parameter is mandatory");

            // Init System.Windows.Application (Wpf)
            ////////////////////////////////////////
            //if (System.Windows.Application.Current == null)
            //{
            //    new System.Windows.Application();
            //    System.Windows.Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            //}
        }

        ~ETKExcel()
        {
            Dispose();
        }
        #endregion

        #region public methods
        /// <summary>Init the framework. Must be called before any other uses of the framework</summary>
        /// <param name="application">A reference to the current Excel application instance</param>
        public static void Init(Microsoft.Office.Interop.Excel.Application application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentException("the 'application' parameter is mandatory");

                lock (syncObj)
                {
                    if (Instance == null)
                    {
                        // Init ETKExcel
                        ////////////////
                        Instance = new ETKExcel(application);

                        // Inject the Excel application reference
                        CompositionManager.Instance.ComposeExportedValue(application);
                        // Compose the current instance
                        CompositionManager.Instance.ComposeParts(Instance);

                        Microsoft.Office.Interop.Excel.Workbook workbook = application.ActiveWorkbook;
                        Instance.AddManagedWorkbook(application.ActiveWorkbook);

                        application.WorkbookOpen += Instance.AddManagedWorkbook;
                        application.WorkbookBeforeClose += Instance.OnWorkbookBeforeClose;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new EtkException(string.Format("'ETKExcel' initialization failed:{0}", ex.Message), ex);
            }
        }

        #endregion

        #region internal methods
        internal void Dispose()
        {
            lock (syncObj)
            {
                if (!isDisposed)
                {

                    if (excelApplication != null)
                    {
                        if (excelApplication.Application != null)
                        {
                            excelApplication.Application.WorkbookBeforeClose -= OnWorkbookBeforeClose;
                            excelApplication.Application.WorkbookOpen -= Instance.AddManagedWorkbook;
                        }
                        excelApplication.Dispose();
                    }
                    if (templateManager != null)
                        templateManager.Dispose();
                    if (RequestsManager != null)
                        RequestsManager.Dispose();
                    if (contextualMenuManager != null)
                        contextualMenuManager.Dispose();

                    isDisposed = true;
                    Instance = null;
                }
            }
        }
        #endregion

        #region private methods
        private void AddManagedWorkbook(Microsoft.Office.Interop.Excel.Workbook workbook)
        {
            Microsoft.Office.Interop.Excel.Workbook managedWorkbook = Instance.managedWorkbooks.FirstOrDefault(w => w == workbook);
            if (managedWorkbook == null)
            {
                managedWorkbooks.Add(workbook);
                contextualMenuManager.RegisterWorkbook(workbook);
                //workbook.SheetActivate += OnActivateSheetViewsManagement;
            }
            managedWorkbook = null;
        }

        private void OnWorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook workbook, ref bool cancel)
        {
            if (!cancel && workbook.Application.Workbooks.Count >= 1)
                Instance.managedWorkbooks.Remove(workbook);
        }
        #endregion
    }
}