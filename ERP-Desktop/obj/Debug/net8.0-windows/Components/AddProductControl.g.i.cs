﻿#pragma checksum "..\..\..\..\Components\AddProductControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0E0AC47911A5E02591CD7497BCFA39B9E7580BFA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ERP_Desktop.Components;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ERP_Desktop.Components {
    
    
    /// <summary>
    /// AddProductControl
    /// </summary>
    public partial class AddProductControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUserGeneratedCode;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtProductName;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtProductDescription;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCostPrice;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSalesPrice;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCategory;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkStatus;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Components\AddProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddProductButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ERP-Desktop;component/components/addproductcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Components\AddProductControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtUserGeneratedCode = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.txtProductName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtProductDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtCostPrice = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtSalesPrice = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.cmbCategory = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.chkStatus = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.AddProductButton = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\Components\AddProductControl.xaml"
            this.AddProductButton.Click += new System.Windows.RoutedEventHandler(this.AddProduct);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

