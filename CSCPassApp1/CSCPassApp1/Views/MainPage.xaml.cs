using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CSCPassApp1.Resources;
using System.Windows.Media;
using System.IO;
using System.Text;
using Spring.Collections.Specialized;
using Spring.Social.OAuth1;
using Spring.Social.LinkedIn.Connect;

namespace CSCPassApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void password_GotFocus(object sender, RoutedEventArgs e)
        {
            password.Password = "";
        }

        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            if(password.Password=="")
                password.Password = "gjsdgj";
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {


            Dispatcher.BeginInvoke(() =>
            {
                string uri = App.root + "login.php";

                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
            });
          //  NavigationService.Navigate(new Uri("/Views/MainPivot.xaml", UriKind.Relative));
        }


        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);

                // Create the post data
                string postData = "email=nipun.birla@gmail.com&password=n";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                
                // Add the post data to the web request
                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                // Start the web request
                myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
            });
        }
        void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    if(result =="Error")
                    {
                        MessageBox.Show("Invalid User Id or Password");
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/Views/MainPivot.xaml?result="+result, UriKind.Relative));
                        //MessageBox.Show(result);
                    }
                }
            });
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/register.xaml", UriKind.Relative));
        }
        LinkedInServiceProvider sp = new LinkedInServiceProvider(linkedin_csc.ConsumerKey, linkedin_csc.ConsumerSecret);
        private void LinkedIn_Click(object sender, RoutedEventArgs e)
        {
            NameValueCollection nc = new NameValueCollection();
            sp.OAuthOperations.FetchRequestTokenAsync(linkedin_csc.AuthorizeUrl, nc, operCompleted);
        }

        private void operCompleted(Spring.Rest.Client.RestOperationCompletedEventArgs<Spring.Social.OAuth1.OAuthToken> obj)
        {
            //MessageBox.Show("Secret : "+obj.Response.Secret +"Value :"+ obj.Response.Value);
            linkedin_csc.tokensecret = obj.Response.Secret;
            linkedin_csc.tokenvalue = obj.Response.Value;

            NameValueCollection nc = new NameValueCollection();
            var x = sp.GetApi(linkedin_csc.tokenvalue, linkedin_csc.tokensecret);





            sp.OAuthOperations.FetchRequestTokenAsync(linkedin_csc.AuthorizeUrl, nc, requesttokenreceived);

            //     AuthenticationBrowser.Visibility = System.Windows.Visibility.Visible;
            //    AuthenticationBrowser.Navigate(new Uri(u, UriKind.Absolute));
            //  Spring.Social.LinkedIn.Api.Impl.LinkedInTemplate lt = new Spring.Social.LinkedIn.Api.Impl.LinkedInTemplate(linkedin_csc.ConsumerKey, linkedin_csc.ConsumerSecret, linkedin_csc.tokenvalue, linkedin_csc.tokensecret);
            //lt.ProfileOperations.GetUserFullProfileAsync(GetProfilecompleted);
        }

        private void requesttokenreceived(Spring.Rest.Client.RestOperationCompletedEventArgs<OAuthToken> obj)
        {
            
            NameValueCollection nc = new NameValueCollection();

            OAuth1Parameters a = new OAuth1Parameters();
            var ur = sp.OAuthOperations.BuildAuthenticateUrl(linkedin_csc.tokenvalue, a);
            var u = sp.OAuthOperations.BuildAuthorizeUrl(linkedin_csc.tokenvalue, a);

            AuthenticationBrowser.Visibility = System.Windows.Visibility.Visible;
            AuthenticationBrowser.Navigate(new Uri(u,UriKind.Absolute));

            //OAuthToken t = new OAuthToken(obj.Response.Value, obj.Response.Secret);
            //AuthorizedRequestToken rt = new AuthorizedRequestToken(t, "tryme");
            //sp.OAuthOperations.ExchangeForAccessTokenAsync(rt, nc, accesstokenreceived);
        }

        private void accesstokenreceived(Spring.Rest.Client.RestOperationCompletedEventArgs<OAuthToken> obj)
        {
            MessageBox.Show(obj.Response.Value);
        }

        private void AuthenticationBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            MessageBox.Show(e.Uri.AbsoluteUri.ToString());
            if(e.Uri.AbsoluteUri.Contains("oauth_verifier"))
            {
                string a = e.Uri.AbsoluteUri.ToString();
                string[] b = a.Split('?');
                string[] c = b[1].Split('&');
                MessageBox.Show(c[0] + c[1]);
                linkedin_csc.outhtoken = c[0].Split('=').Last();
                linkedin_csc.outhverifier = c[1].Split('=').Last();
                MessageBox.Show(linkedin_csc.outhtoken + linkedin_csc.outhverifier);
          
                OAuthToken t = new OAuthToken(linkedin_csc.outhtoken, linkedin_csc.outhverifier);
                AuthorizedRequestToken rt = new AuthorizedRequestToken(t, "");
                NameValueCollection nc = new NameValueCollection();
                nc.Add("grant_type", "authorization_code");
                nc.Add("code",linkedin_csc.outhtoken);
                
                sp.OAuthOperations.ExchangeForAccessTokenAsync(rt, nc, accesstokenreceived);
               // AuthenticationBrowser.Navigate(new Uri("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id=" + linkedin_csc.ConsumerKey + "scope=r_fullprofile",UriKind.Absolute));
            }
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}