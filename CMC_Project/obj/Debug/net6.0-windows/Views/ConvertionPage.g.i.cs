﻿#pragma checksum "..\..\..\..\Views\ConvertionPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5F1DCCB0D37B73F7B4309D475A49B09DF2EB61FA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CMC_Project.Views;
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


namespace CMC_Project.Views {
    
    
    /// <summary>
    /// ConvertionPage
    /// </summary>
    public partial class ConvertionPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BidOpenFile;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BIDList;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button XlsOpenFile;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox XlsList;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConvertButton;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InitButton;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\ConvertionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AdjustButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CMC_Project;component/views/convertionpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\ConvertionPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.BidOpenFile = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\Views\ConvertionPage.xaml"
            this.BidOpenFile.Click += new System.Windows.RoutedEventHandler(this.BIDOpenClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BIDList = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.XlsOpenFile = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\..\Views\ConvertionPage.xaml"
            this.XlsOpenFile.Click += new System.Windows.RoutedEventHandler(this.XlsOpenClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.XlsList = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.ConvertButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\Views\ConvertionPage.xaml"
            this.ConvertButton.Click += new System.Windows.RoutedEventHandler(this.ConvertButtonClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.InitButton = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\Views\ConvertionPage.xaml"
            this.InitButton.Click += new System.Windows.RoutedEventHandler(this.InitButtonClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AdjustButton = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Views\ConvertionPage.xaml"
            this.AdjustButton.Click += new System.Windows.RoutedEventHandler(this.AdjustBtnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

