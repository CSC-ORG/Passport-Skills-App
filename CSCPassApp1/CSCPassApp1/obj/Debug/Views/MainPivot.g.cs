﻿#pragma checksum "C:\Users\nipun\Documents\Visual Studio 2013\Projects\CSCPassApp1\Passport-Skills-App-2015\CSCPassApp1\CSCPassApp1\Views\MainPivot.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "43BD30A3109878875E235647396B0F28"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CSCPassApp1.Views {
    
    
    public partial class MainPivot : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PhoneTextBox statusBox;
        
        internal System.Windows.Controls.Image statusadd;
        
        internal System.Windows.Controls.ListBox FeedListBox;
        
        internal System.Windows.Controls.Grid ImageContainer;
        
        internal System.Windows.Media.CompositeTransform ImageTransform;
        
        internal System.Windows.Controls.Image Image;
        
        internal System.Windows.Controls.ScrollViewer Scroller;
        
        internal System.Windows.Controls.Grid ScrollGrid;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock userid;
        
        internal System.Windows.Controls.TextBlock emailid;
        
        internal System.Windows.Controls.StackPanel ContentPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox exTitleBox;
        
        internal Microsoft.Phone.Controls.DatePicker from_dt;
        
        internal System.Windows.Controls.RadioButton rbpresent;
        
        internal System.Windows.Controls.RadioButton rbtd;
        
        internal Microsoft.Phone.Controls.DatePicker to_dt;
        
        internal System.Windows.Controls.Image ExpAdd;
        
        internal System.Windows.Controls.ListBox ExperienceListBox;
        
        internal Microsoft.Phone.Controls.AutoCompleteBox acbox;
        
        internal System.Windows.Controls.Image skillsAdd;
        
        internal System.Windows.Controls.ListBox skilllistbox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox qbox;
        
        internal System.Windows.Controls.Image qualAdd;
        
        internal System.Windows.Controls.ListBox quallistbox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox acTitleBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox acDescBox;
        
        internal System.Windows.Controls.Image AchievementsAdd;
        
        internal System.Windows.Controls.ListBox AchievementsListBox;
        
        internal Microsoft.Phone.Controls.WebBrowser AuthenticationBrowser;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CSCPassApp1;component/Views/MainPivot.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.statusBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("statusBox")));
            this.statusadd = ((System.Windows.Controls.Image)(this.FindName("statusadd")));
            this.FeedListBox = ((System.Windows.Controls.ListBox)(this.FindName("FeedListBox")));
            this.ImageContainer = ((System.Windows.Controls.Grid)(this.FindName("ImageContainer")));
            this.ImageTransform = ((System.Windows.Media.CompositeTransform)(this.FindName("ImageTransform")));
            this.Image = ((System.Windows.Controls.Image)(this.FindName("Image")));
            this.Scroller = ((System.Windows.Controls.ScrollViewer)(this.FindName("Scroller")));
            this.ScrollGrid = ((System.Windows.Controls.Grid)(this.FindName("ScrollGrid")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.userid = ((System.Windows.Controls.TextBlock)(this.FindName("userid")));
            this.emailid = ((System.Windows.Controls.TextBlock)(this.FindName("emailid")));
            this.ContentPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ContentPanel")));
            this.exTitleBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("exTitleBox")));
            this.from_dt = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("from_dt")));
            this.rbpresent = ((System.Windows.Controls.RadioButton)(this.FindName("rbpresent")));
            this.rbtd = ((System.Windows.Controls.RadioButton)(this.FindName("rbtd")));
            this.to_dt = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("to_dt")));
            this.ExpAdd = ((System.Windows.Controls.Image)(this.FindName("ExpAdd")));
            this.ExperienceListBox = ((System.Windows.Controls.ListBox)(this.FindName("ExperienceListBox")));
            this.acbox = ((Microsoft.Phone.Controls.AutoCompleteBox)(this.FindName("acbox")));
            this.skillsAdd = ((System.Windows.Controls.Image)(this.FindName("skillsAdd")));
            this.skilllistbox = ((System.Windows.Controls.ListBox)(this.FindName("skilllistbox")));
            this.qbox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("qbox")));
            this.qualAdd = ((System.Windows.Controls.Image)(this.FindName("qualAdd")));
            this.quallistbox = ((System.Windows.Controls.ListBox)(this.FindName("quallistbox")));
            this.acTitleBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("acTitleBox")));
            this.acDescBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("acDescBox")));
            this.AchievementsAdd = ((System.Windows.Controls.Image)(this.FindName("AchievementsAdd")));
            this.AchievementsListBox = ((System.Windows.Controls.ListBox)(this.FindName("AchievementsListBox")));
            this.AuthenticationBrowser = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("AuthenticationBrowser")));
        }
    }
}

