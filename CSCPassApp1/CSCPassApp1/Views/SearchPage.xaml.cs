using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CSCPassApp1.Views
{
    public class Status
    {
        public string status { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        public string timestamp { get; set; }
        public Status(string status,string name,string image,string email,string timestamp)
        {
            this.status = status;
            this.name = name;
            this.image = image;
            this.email = email;
            this.timestamp = timestamp;
        }
    }

    public class People
    {
        
        public string name { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        
        public People(string name, string image, string email)
        {
            
            this.name = name;
            this.image = image;
            this.email = email;
            
        }
    }
    public partial class SearchPage : PhoneApplicationPage
    {
        ObservableCollection<Status> statusCollection = new ObservableCollection<Status>();
        ObservableCollection<People> peopleCollection = new ObservableCollection<People>();
        public SearchPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(App.root + "hash1.php?skill=" + SearchBox.Text, UriKind.Absolute));
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
           
        }

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
            JObject BaseObj = JObject.Parse(e.Result);

            JArray statusArray = JArray.Parse(BaseObj["status"].ToString());
            JArray profileArray = JArray.Parse(BaseObj["profile"].ToString());
            statusCollection.Clear();
            foreach(var item in statusArray)
            {
                Status s = new Status(item["status"].ToString(), item["name"].ToString(), item["image"].ToString(), item["email"].ToString(), item["timestamp"].ToString());
                statusCollection.Add(s);
            }
            FeedListBox.ItemsSource = statusCollection;
            FeedListBox.DataContext = statusCollection;


            peopleCollection.Clear();
            foreach (var item in profileArray)
            {
                People p = new People(item["name"].ToString(), item["image"].ToString(), item["email"].ToString());
                peopleCollection.Add(p);
            }
            PeopleListBox.ItemsSource = peopleCollection;
            PeopleListBox.DataContext = peopleCollection;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var g = sender as Grid;
            var dc = g.DataContext as Status;
            var i = g.FindName("DP") as Image;
            if(!string.IsNullOrEmpty(dc.image))
            {
                var uri = App.root + dc.image;
                BitmapImage bm = new BitmapImage();
                bm.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bm.UriSource = new Uri(uri);
                i.Source = bm;

            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            var g = sender as Grid;
            var dc = g.DataContext as People;
            var i = g.FindName("DP") as Image;
            if (!string.IsNullOrEmpty(dc.image))
            {
                var uri = App.root + dc.image;
                BitmapImage bm = new BitmapImage();
                bm.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bm.UriSource = new Uri(uri);
                i.Source = bm;

            }
        }
    }
}