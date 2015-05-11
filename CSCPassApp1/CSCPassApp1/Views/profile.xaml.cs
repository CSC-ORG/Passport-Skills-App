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
using System.Windows.Data;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CSCPassApp1.Views
{
  
    public partial class profile : PhoneApplicationPage
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

               
            }
        }

        
        public profile()
        {
            InitializeComponent();
            this.Loaded += profile_Loaded;
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

        string email = "";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("email")) ;
            {
                email = NavigationContext.QueryString["email"].ToString();
            }
        }
       

        void profile_Loaded(object sender, RoutedEventArgs e)
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
               // scrollbar.ValueChanged += OnScrollbarValueChanged;
            }

            // when the mouse (the user tapping his screen) moves
            // within the ScrollViewer, there's a chance that
            // the ScrollViewer's Content's RenderTransform will be set to
            // a new CompositeTransform that we need to grab a hold of
            Scroller.MouseMove += ScrollerOnMouseMove;


            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.root+"retrieve.php?email="+email, UriKind.Absolute));
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;

        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
         //   MessageBox.Show(e.Result);


            User myprofile = new User();
            dynamic baseobj = JObject.Parse(e.Result);

            myprofile.user_id = baseobj["user_id"].ToString();
            //App.userid = baseobj["user_id"].ToString();

            myprofile.name = baseobj["name"].ToString();
            userid.Text = baseobj["name"].ToString();

            myprofile.email = baseobj["email"].ToString();
            emailid.Text = baseobj["email"].ToString();

            myprofile.myachievements = new ObservableCollection<UserAchievements>();

            JArray achievements = JArray.Parse(baseobj["achievements"].ToString());
            foreach (var item in achievements)
            {
                UserAchievements ua = new UserAchievements(item["title"].ToString(), item["description"].ToString());
                myprofile.myachievements.Add(ua);
            }
            AchievementsListBox.ItemsSource = myprofile.myachievements;
            AchievementsListBox.DataContext = myprofile.myachievements;


            myprofile.myexp = new ObservableCollection<UserExp>();

            JArray exp = JArray.Parse(baseobj["experience"].ToString());
            foreach (var item in exp)
            {
                UserExp ua = new UserExp(item["title"].ToString(), item["from_dt"].ToString(), item["to_dt"].ToString());
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
            foreach (var item in skills)
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


        }
    }
}