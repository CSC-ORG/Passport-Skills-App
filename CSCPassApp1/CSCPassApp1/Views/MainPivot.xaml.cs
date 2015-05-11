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
using System.Windows.Controls.Primitives;
using Windows.Devices.Input;
using System.Windows.Data;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Spring.Collections.Specialized;
using Spring.Social.LinkedIn.Connect;
using Spring.Social.LinkedIn.Api;
using Spring.Social.OAuth1;
using Spring.Social.LinkedIn.Api.Impl;
using System.Diagnostics;
using System.Globalization;

namespace CSCPassApp1.Views
{
   
    public class UserExp
    {
        public string title { get; set; }
        public string from_dt { get; set; }
        public string to_dt { get; set; }
        public UserExp(string title, string from_dt,string to_dt)
        {
            this.title = title;
            this.from_dt = "From : "+from_dt;
            this.to_dt = "To : "+to_dt;
        }

    }
    public class UserAchievements
    {
        public string title { get; set; }
        public string description { get; set; }
        public UserAchievements(string title,string description)
        {
            this.title = title;
            this.description = description;
        }
    }

    public class User
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string designation { get; set; }
        public string location { get; set; }
        public ObservableCollection<string> skill { get; set; }
        public ObservableCollection<string> qualification { get; set; }
        public ObservableCollection<string> mynewsfeed { get; set; }
        public ObservableCollection<UserAchievements> myachievements { get; set; }
        public ObservableCollection<UserExp> myexp { get; set; }
    }


    public partial class MainPivot : PhoneApplicationPage
    {
        private const double EmptySpace = 500;

        // this will represent the vertical position of the image
        // we're storing it in a variable because we'll need it later
        private double _startPosition;

        // this will be our backing property for the binding
        private double _verticalOffset;
        public double VerticalOffset
        {
            get { return _verticalOffset; }
            set
            {
                _verticalOffset = value;

                // update our header image position accordingly
                onVerticalOffsetChanged();
            }
        }

        public MainPivot()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        User myprofile;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Here's the magic
            // To get the image where we want, we will shift it up by its height
            // Then we'll shift it down by the height of the empty space
            // Last, we'll divide that by two so we'll be directly between the top 
            // of the page and the bottom of the empty space
            _startPosition = (-Image.ActualHeight + EmptySpace) / 2;

            // set the TranslateY of the CompositeTransform we created in the XAML
            // to our calculated start position
            ImageTransform.TranslateY = _startPosition;

            var scrollbar =
              ((FrameworkElement)VisualTreeHelper.GetChild(Scroller, 0)).FindName("VerticalScrollBar") as ScrollBar;

            if (scrollbar != null)
            {
                scrollbar.ValueChanged += OnScrollbarValueChanged;
            }

            // when the mouse (the user tapping his screen) moves
            // within the ScrollViewer, there's a chance that
            // the ScrollViewer's Content's RenderTransform will be set to
            // a new CompositeTransform that we need to grab a hold of
            Scroller.MouseMove += ScrollerOnMouseMove;


            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/statusreturn.php?user="+App.userid, UriKind.Absolute));
            wc.DownloadStringCompleted += statusreturn;


            if(!string.IsNullOrEmpty(profilejson))
            {

                


                myprofile = new User();
                dynamic baseobj = JObject.Parse(profilejson);

                myprofile.user_id = baseobj["user_id"].ToString();
                App.userid = baseobj["user_id"].ToString();

                myprofile.name = baseobj["name"].ToString();
                userid.Text = baseobj["name"].ToString();

                myprofile.email = baseobj["email"].ToString();
                emailid.Text = baseobj["email"].ToString();

                myprofile.myachievements = new ObservableCollection<UserAchievements>();
               
                JArray achievements = JArray.Parse(baseobj["achievements"].ToString());
                foreach(var item in achievements)
                {
                    UserAchievements ua = new UserAchievements(item["title"].ToString(),item["description"].ToString());
                    myprofile.myachievements.Add(ua);
                }
                AchievementsListBox.ItemsSource = myprofile.myachievements;
                AchievementsListBox.DataContext = myprofile.myachievements;


                myprofile.myexp = new ObservableCollection<UserExp>();

                JArray exp = JArray.Parse(baseobj["experience"].ToString());
                foreach (var item in exp)
                {
                    UserExp ua = new UserExp(item["title"].ToString(),item["from_dt"].ToString(),item["to_dt"].ToString());
                    myprofile.myexp.Add(ua);
                }
                ExperienceListBox.ItemsSource = myprofile.myexp;
                ExperienceListBox.DataContext = myprofile.myexp;


                myprofile.image = App.root + baseobj["image"].ToString();
                BitmapImage bm = new BitmapImage();
                bm.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bm.UriSource = new Uri(App.root + baseobj["image"].ToString());
                Image.Source = bm;

                myprofile.location = baseobj["location"].ToString();

                myprofile.designation = baseobj["designation"].ToString();

                myprofile.mobile = baseobj["mobile"].ToString();

                myprofile.skill = new ObservableCollection<string>();
                JArray skills = baseobj["skill"];
                foreach(var item in skills)
                {
                    myprofile.skill.Add(item.ToString());
                }
                skilllistbox.ItemsSource = myprofile.skill;
                skilllistbox.DataContext = myprofile.skill;


                myprofile.qualification = new ObservableCollection<string>();
                JArray qualifications = baseobj["qualification"];
                foreach (var item in qualifications)
                {
                    myprofile.qualification.Add(item.ToString());
                }
                quallistbox.ItemsSource = myprofile.qualification;
                quallistbox.DataContext = myprofile.qualification;


                myprofile.mynewsfeed = new ObservableCollection<string>();
                //statusListBox.ItemsSource = myprofile.mynewsfeed;
                //statusListBox.DataContext = myprofile.mynewsfeed;
            }

        }

        private void statusreturn(object sender, DownloadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        }

        private void ScrollerOnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
             // grab the content element
            var uiElement = Scroller.Content as UIElement;
            if (uiElement != null)
            {
                // try to grab its transform as a CompositeTransform
                var transform = uiElement.RenderTransform as CompositeTransform;

                // if it's actually a CompositeTransform
                if (transform != null)
                {
                    // we're good, let's go to town!

                    // let's set up the binding in a standard manner
                    var binding = new Binding("VerticalOffset");

                    // in a perfect world, we use a reverse OneWay binding, where
                    // the DP we're binding to can set the backing property
                    // as this doesn't exist in the world of Windows Phone
                    // we're going to cheat and use TwoWay
                    binding.Mode = BindingMode.TwoWay;
                    binding.Source = this;

                    BindingOperations.SetBinding(transform, CompositeTransform.TranslateYProperty, binding);

                    // we're going to release the event handler
                    // since we only need to bind once
                    // i recommend detaching the other event
                    // handlers we've used at some point as well, as it's good form
                    Scroller.MouseMove -= ScrollerOnMouseMove;
                }
            }
        

        }

       

        private void OnScrollbarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // if we're in positive scrolling territory
            // where positive means down
            if (e.NewValue > 0)
            {
                // we're going to scroll our Image along with the content
                // but only half as fast
                // try removing the /2.0 and see what happens
            //    ImageTransform.TranslateY = _startPosition - e.NewValue / 2.0;

                // instead of using the scrollbar's value, you can also use the VerticalOffset
                // of the ScrollViewer itself
                // try it, see which one works better for you performance-wise
                // ImageTransform.TranslateY = _startPosition - Scroller.VerticalOffset/2.0;
            }
        }

        private void onVerticalOffsetChanged()
        {
            // now let's do the same thing we did with the scrollbar

            // we only want to handle the squishes here, as the 
            // scrollbar events handle the normal scrolling
            // so we'll only respond to the squishes, when the content
            // is being moved down
            if (VerticalOffset >= 0)
            {
            //    ImageTransform.TranslateY = _startPosition + VerticalOffset / 2.0;
            }
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        string profilejson;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("result")) ;
            {
                profilejson = NavigationContext.QueryString["result"].ToString();
            }
        }

        LinkedInServiceProvider sp = new LinkedInServiceProvider(linkedin_csc.ConsumerKey, linkedin_csc.ConsumerSecret);

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           // Spring.Social.LinkedIn.Api.LinkedInFullProfile p = new Spring.Social.LinkedIn.Api.LinkedInFullProfile();
            
            NameValueCollection nc = new NameValueCollection();
            sp.OAuthOperations.FetchRequestTokenAsync("https://linkedin.com", nc, operCompleted);
         //   AuthenticationBrowser.Visibility = System.Windows.Visibility.Visible;
          //  AuthenticationBrowser.Navigate(new Uri("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id=" + linkedin_csc.ConsumerKey + "&scope=r_fullprofile&redirect_uri=" + linkedin_csc.RedirectUrl + "&state=DCEeFWf45A53sdfKef424", UriKind.Absolute));
        }

        private void operCompleted(Spring.Rest.Client.RestOperationCompletedEventArgs<Spring.Social.OAuth1.OAuthToken> obj)
        {
            //MessageBox.Show("Secret : "+obj.Response.Secret +"Value :"+ obj.Response.Value);
            linkedin_csc.tokensecret = obj.Response.Secret;
            linkedin_csc.tokenvalue = obj.Response.Value;

              NameValueCollection nc = new NameValueCollection();
            var x = sp.GetApi(linkedin_csc.tokenvalue, linkedin_csc.tokensecret);
            OAuth1Parameters a = new OAuth1Parameters();
            var ur = sp.OAuthOperations.BuildAuthenticateUrl(linkedin_csc.tokenvalue, a);
            var u = sp.OAuthOperations.BuildAuthorizeUrl(linkedin_csc.tokenvalue,a);

            sp.OAuthOperations.FetchRequestTokenAsync("https://www.linkedin.com", nc, requesttokenreceived);

       //     AuthenticationBrowser.Visibility = System.Windows.Visibility.Visible;
         //    AuthenticationBrowser.Navigate(new Uri(u, UriKind.Absolute));
          //  Spring.Social.LinkedIn.Api.Impl.LinkedInTemplate lt = new Spring.Social.LinkedIn.Api.Impl.LinkedInTemplate(linkedin_csc.ConsumerKey, linkedin_csc.ConsumerSecret, linkedin_csc.tokenvalue, linkedin_csc.tokensecret);
            //lt.ProfileOperations.GetUserFullProfileAsync(GetProfilecompleted);
            
            
        }

        private void requesttokenreceived(Spring.Rest.Client.RestOperationCompletedEventArgs<OAuthToken> obj)
        {
            
            NameValueCollection nc = new NameValueCollection();
            OAuthToken t = new OAuthToken(obj.Response.Value,obj.Response.Secret);
            AuthorizedRequestToken rt = new AuthorizedRequestToken(t, "tryme");
            sp.OAuthOperations.ExchangeForAccessTokenAsync(rt, nc, accesstokenreceived);
        }

        private void accesstokenreceived(Spring.Rest.Client.RestOperationCompletedEventArgs<OAuthToken> obj)
        {
            MessageBox.Show(obj.Response.Value);
        }

     

      

        string code = "";
        private void AuthenticationBrowser_Navigated(object sender, NavigationEventArgs e)
        {
        if (AuthenticationBrowser.Visibility == Visibility.Collapsed) {
                AuthenticationBrowser.Visibility = Visibility.Visible;
            }
        string htmlString ="";
        
        string state = "";
        if (e.Uri.AbsoluteUri.Contains("oauth_verifier"))
        {

            AuthenticationBrowser.Visibility = Visibility.Collapsed;
            
            LinkedInTemplate lt = new LinkedInTemplate(linkedin_csc.ConsumerKey, linkedin_csc.ConsumerSecret, linkedin_csc.tokenvalue, linkedin_csc.tokensecret);
            //lt.(GetProfilecompleted);
            //htmlString = AuthenticationBrowser.SaveToString();

            //var script = e.ToString();

            

            //string[] a = htmlString.Split('?');
            //string[] b = a[1].Split('&');
            //code = b[0];
            //state = "DCEeFWf45A53sdfKef424";

           
            //// Create the post data
            //string data = "grant_type=authorization_code&" + code + "&redirect_uri=" + linkedin_csc.RedirectUrl + "&client_id=" + linkedin_csc.ConsumerKey + "&client_secret=" + linkedin_csc.ConsumerSecret;
            //byte[] byteArray = Encoding.UTF8.GetBytes(data);

            //AuthenticationBrowser.Navigate(new Uri("https://www.linkedin.com/uas/oauth2/accessToken?"+data, UriKind.Absolute), null, "Content-Type:application/x-www-form-urlencoded");
         
            //Dispatcher.BeginInvoke(() =>
            //{
            //    string uri = "https://www.linkedin.com/uas/oauth2/accessToken";

            //    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            //    myRequest.Method = "POST";
            //    myRequest.ContentType = "application/x-www-form-urlencoded";
            //    myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
            //});
        }
            //MessageBox.Show(htmlString);
        //var pinFinder = new Regex(@"<div class=""access-code"">(?<pin>[A-Za-z0-9_]+)</div>", RegexOptions.IgnoreCase);
        //var match = pinFinder.Match(htmlString);
        //if (match.Length > 0) {
        //    var group = match.Groups["pin"];
        //    if (group.Length > 0) {
        //        var pin = group.Captures[0].Value;
        //        if (!string.IsNullOrEmpty(pin)) {
        //            MessageBox.Show(pin);

        //            if (string.IsNullOrEmpty(pin))
        //            {
        //                Dispatcher.BeginInvoke(() => MessageBox.Show("Authorization denied by user"));
        //            }
        //            // Make sure pin is reset to null
        //            pin = null;
 
        //        }
        //    }
        //}
        
     //   AuthenticationBrowser.Visibility = Visibility.Collapsed;
        }

        private void GetProfilecompleted(Spring.Rest.Client.RestOperationCompletedEventArgs<LinkedInFullProfile> obj)
        {
            MessageBox.Show(obj.Response.FirstName);
        }
        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);
                // Create the post data
                string data = "grant_type=authorization_code&code=" + code + "&redirect_uri=" + linkedin_csc.RedirectUrl + "&client_id=" + linkedin_csc.ConsumerKey + "&client_secret=" + linkedin_csc.ConsumerSecret;
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
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
                   
                        MessageBox.Show(result);
                }
            });
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SearchPage.xaml",UriKind.Relative));
        }

        private void AutoCompleteBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(acbox.Text))
            {
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/skillsearch.php?skill=" + acbox.Text, UriKind.Absolute));
                wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string res = e.Result;

                dynamic BaseObj = JObject.Parse(res);
                JArray skillsArray = JArray.Parse(BaseObj["skills"].ToString());
                //   var arres = (JValue)res;
                ObservableCollection<string> suggestion = new ObservableCollection<string>();
                foreach (var item in skillsArray)
                {
                    suggestion.Add(item.ToString());
                }
                acbox.DataContext = suggestion;
                acbox.ItemsSource = suggestion;
            
            }
            catch { }
           
            //acbox.MinimumPopulateDelay = 0;
            acbox.IsDropDownOpen = true;
        }

        private void skillsAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if(string.IsNullOrEmpty(acbox.Text))
            {
                MessageBox.Show("Enter a valid skill");
            }
            else
            {
                myprofile.skill.Add(acbox.Text);
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/skilladd.php?skill="+acbox.Text+"&user="+App.userid, UriKind.Absolute));
                wc.DownloadStringCompleted += skilladdedResponse;
                acbox.Text = "";
            }
        }

        private void skilladdedResponse(object sender, DownloadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Result.ToString());
        }

        private void statusadd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(string.IsNullOrEmpty(statusBox.Text))
            {
                MessageBox.Show("Enter valid text");
            }
            else
            {
               // myprofile.Add(statusBox.Text);
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/statusadd.php?status=" + statusBox.Text + "&user=" + App.userid, UriKind.Absolute));
                wc.DownloadStringCompleted += statusAddedResponse;
                myprofile.mynewsfeed.Insert(0,statusBox.Text);
                statusBox.Text = "";
            }
        }

        private void statusAddedResponse(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            Debug.WriteLine(e.Result);
        }

        private void AchievementsAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!string.IsNullOrEmpty(acTitleBox.Text))
            {
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/addachieve.php?title=" + acTitleBox.Text + "&user=" + App.userid + "&descr=" + acDescBox.Text, UriKind.Absolute));
                wc.DownloadStringCompleted += achievementsresponse;

                UserAchievements ua = new UserAchievements(acTitleBox.Text, acDescBox.Text);
                myprofile.myachievements.Add(ua);

                acTitleBox.Text ="";
                acDescBox.Text = "";
            }
            else
            {
                MessageBox.Show("Title cannot be empty");
            }
        }

        private void achievementsresponse(object sender, DownloadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Result);
        }

        private void ExpAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(!string.IsNullOrEmpty(exTitleBox.Text))
            {
                string title = exTitleBox.Text;
                
                var month = from_dt.Value.Value.Month;
                string months="";
                if(month<10)
                    months = "0"+month;
                else
                    months = ""+month;

                var day = from_dt.Value.Value.Day;
                string days = "";
                 if(day<10)
                    days = "0"+day;
                else
                    days = ""+day;

                // string fromdate = String.Format(from_dt.Value("d", culture)) + "\n";  // Displays 15/3/2008
                string fromdate = from_dt.Value.Value.Year + "-" +months+"-"+days;
                string todate = "";
                if(rbpresent.IsChecked == true)
                {
                    todate = "Present";
                }
                else
                {
                    var month1 = to_dt.Value.Value.Month;
                    string months1 = "";
                    if (month1 < 10)
                        months1 = "0" + month1;
                    else
                        months1 = "" + month1;

                    var day1 = to_dt.Value.Value.Day;
                    string days1 = "";
                    if (day1 < 10)
                        days1 = "0" + day1;
                    else
                        days1 = "" + day1;

                    
                    todate = to_dt.Value.Value.Year + "-" +months1+"-"+days1;
                }

                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/addexp.php?title=" + title + "&user=" + App.userid + "&from_dt=" + fromdate+"&to_dt="+todate, UriKind.Absolute));
                wc.DownloadStringCompleted += achievementsresponse;

                UserExp ua = new UserExp(title, fromdate,todate);
                myprofile.myexp.Add(ua);

                exTitleBox.Text = "";
                //acDescBox.Text = "";
            }
            else
            {
                MessageBox.Show("Title cannot be empty");
            }
        }

        private void qualAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(!string.IsNullOrEmpty(qbox.Text))
            {
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri("http://cscpassapp1.cloudapp.net/root/addqual.php?qual=" + qbox.Text + "&user=" + App.userid, UriKind.Absolute));
                wc.DownloadStringCompleted += achievementsresponse;
                myprofile.qualification.Add(qbox.Text);
                qbox.Text = "";
            }
        }




    }
}