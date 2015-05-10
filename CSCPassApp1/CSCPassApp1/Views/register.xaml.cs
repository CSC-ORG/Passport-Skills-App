using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace CSCPassApp1.Views
{
    public partial class register : PhoneApplicationPage
    {
        public register()
        {
            InitializeComponent();
        }

        

        private void cpass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pass.Password == cpass.Password)
                match.Visibility = Visibility.Collapsed;
            else
                match.Visibility = Visibility.Visible;
        }
    }
}