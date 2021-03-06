//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The ShellApplication class is the entry point for your application. ShellApplication 
// contains the Main method and derives from FormShellApplication base class which is
// provided by the Composite UI Application Block (CAB).
// 
// Note that the RootWorkItem is the default WorkItem provided by CAB.
// 
// It also implements basic exception handling using Enterprise Library Exception
// Handling Application Block.
//
// The basic shell in this Guidance Package (ShellForm) has a menu, a statusbar and
// the screen is divided in 2 workspaces (left and right panes) separated by a spliter
//
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-010-How_to_Create_Smart_Client_Solutions.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Forms;
using ACOT.ChkAddrModule;
using ACOT.Infrastructure.Interface.Constants;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using ACOT.Infrastructure.Interface;
using ACOT.Infrastructure.Interface.Services;
using ACOT.Infrastructure.Library;


namespace ACOT.Infrastructure.Shell
{
    /// <summary>
    /// Main application entry point class.
    /// Note that the class derives from CAB supplied base class FormShellApplication, and the 
    /// main form will be ShellForm, also created by default by this solution template
    /// </summary>
    class ShellApplication : SmartClientApplication<WorkItem, ShellForm>
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Console", true);
            key.SetValue("FullScreen", 0);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);
#if (DEBUG)
			RunInDebugMode();
#else
            RunInReleaseMode();
#endif
        }

        private static void RunInDebugMode()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            new ShellApplication().Run();
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainUnhandledException);
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                new ShellApplication().Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Sets the extension site registration after the shell has been created.
        /// </summary>
        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            //RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.MainMenu, this.Shell.MainMenuStrip);
            //RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.MainStatus, this.Shell.MainStatusStrip);

            // Add window workspace to be used for modal windows
            WindowWorkspaceExtended wspe = new WindowWorkspaceExtended(this.Shell);
            RootWorkItem.Workspaces.Add(wspe, WorkspaceNames.ModalWindows);

            ControlledWorkItem<ModuleController> chkAddrModuleWorkItem = RootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>();
            chkAddrModuleWorkItem.Controller.Run();

            ControlledWorkItem<ShellModuleController> workItem = RootWorkItem.WorkItems.AddNew<ControlledWorkItem<ShellModuleController>>();
            workItem.Controller.Run();

        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            ExceptionPolicy.HandleException(ex, "Default Policy");
            MessageBox.Show("��������� ������, ���������� ��������� ������. ��� ��������� ����������, ���������� � ������ ������� ����������.");
            Application.Exit();
        }

        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = "C:\\Program files\\Common files\\Acot\\Modules\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            //Load the assembly from the specified path. 					
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;
        }
    }
}
