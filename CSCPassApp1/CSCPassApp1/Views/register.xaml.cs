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
using System.IO;
using System.Text;

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


        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (match.Visibility == System.Windows.Visibility.Collapsed)
            {
                //WebClient client = new WebClient();
                //string p = "email=" + email.Text + "&password=" + pass.Password + "&mobile=" + mob.Text + "&location=" + loc.Text + "&designation=" + desig.Text;

                //var postData = new List<KeyValuePair<string, string>>();
                //postData.Add(new KeyValuePair<string, string>("email", email.Text));
                //postData.Add(new KeyValuePair<string, string>("password", pass.Password));
                //postData.Add(new KeyValuePair<string, string>("mobile", mob.Text));
                //postData.Add(new KeyValuePair<string, string>("location", loc.Text));
                //postData.Add(new KeyValuePair<string, string>("designation", desig.Text));

                //client.UploadStringCompleted += new UploadStringCompletedEventHandler(register_completed);
                //client.UploadStringAsync(new Uri(App.root + "register.php"), "POST", postData.ToString());
                

                //client.DownloadStringCompleted += client_DownloadStringCompleted;
                //client.DownloadStringAsync(new Uri(App.root + "register.php?" + p));
                Dispatcher.BeginInvoke(() =>
                     {
                         string uri = App.root + "register.php";

                         HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                         myRequest.Method = "POST";
                         myRequest.ContentType = "application/x-www-form-urlencoded";
                         myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
                     });
            }
            else
                MessageBox.Show("Please enter matching passwords");
        }
        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            Dispatcher.BeginInvoke(() =>
                       {
                           HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                           // End the stream request operation
                           Stream postStream = myRequest.EndGetRequestStream(callbackResult);
                           // Create the post data
                           string postData = "name="+name1.Text+"&email=" + email.Text + "&password=" + pass.Password + "&mobile=" + mob.Text + "&location=" + loc.Text + "&designation=" + desig.Text;
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
                             if (result == "Mail Sent Successfully!")
                             {
                                 MessageBox.Show("You have been registered successfully. Please use the verification link sent to your email ID to login into the application. You are being redirected to the login page. Thank you");
                                 NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
                             }
                             else
                                 MessageBox.Show(result);
                         }
                     });
        }




        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        }

        private void register_completed(object sender, UploadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        }

       

        
    }
}