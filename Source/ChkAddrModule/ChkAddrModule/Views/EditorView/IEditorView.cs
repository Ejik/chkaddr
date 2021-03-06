//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add Foundational Module" recipe.
//
// The LayoutView usercontrol defines a layout decoupled from the shell. 
// It provides a left and right workspace, menu bar, tool bar and status bar.
// These ui elements are added as extension sites.
//
// For more information see:
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-030-How_to_Create_a_Foundational_Module.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace ACOT.ChkAddrModule.Views.EditorView
{
    public interface IEditorView
    {
        System.Windows.Forms.BindingSource bindingSource { get; set; }

        TextBox FullAddress { get; set; }
        TextBox KodRegion { get; }
        Label RegionLabel { get; }
        TextBox Index { get; }
        TextBox Raion { get; }
        TextBox Gorod { get; }
        TextBox NasPunkt { get; }
        TextBox Ulica { get; }
        TextBox Dom { get; }
        TextBox Korp { get; }
        TextBox Kvart { get; }
        PictureBox RaionErrorPicture { get; }
        PictureBox GorodErrorPicture { get; }
        PictureBox NaspunktErrorPicture { get; }
        PictureBox UlicaErrorPicture { get; }

        System.Drawing.Point ViewLocation { get; set; }

        string Title { get; set; }

        int ViewWidth { get; }

        Form ParentFm { get; }

        IWorkspace rightWorkSpace { get; }

        int UserCtrlWidth { get; set; }

        void SetCenterPosition();

    }
}
