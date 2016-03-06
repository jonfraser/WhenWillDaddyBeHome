using WinPhoneApp.Common;
using WinPhoneApp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace WinPhoneApp
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data

            var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
            this.DefaultViewModel["Item"] = item;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private bool SendLocation = false;
        private Guid thisIdentifier;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SendLocation = !SendLocation;

            var displayRequest = new Windows.System.Display.DisplayRequest();
            displayRequest.RequestActive();

            thisIdentifier = Guid.NewGuid();

            while (SendLocation)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://whenwilldaddybehome.azurewebsites.net/");
                    var location = await new Windows.Devices.Geolocation.Geolocator().GetGeopositionAsync();
                    var content = new
                    {
                        Id = thisIdentifier.ToString(),
                        Lat = location.Coordinate.Point.Position.Latitude,
                        Long = location.Coordinate.Point.Position.Longitude
                    };
                    await client.PostAsync("a/updatemylocation", new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));

                    await Task.Delay(TimeSpan.FromSeconds(60));
                }

            }
        }

        private async void Share_Click(object sender, RoutedEventArgs e)
        {
            var chatMessage = new Windows.ApplicationModel.Chat.ChatMessage();
            chatMessage.Body = "When will daddy be home? https://whenwilldaddybehome.azurewebsites.net/WhereIs/Daddy/" + thisIdentifier.ToString();

            //var phone = recipient.Phones.FirstOrDefault & lt; Windows.ApplicationModel.Contacts.ContactPhone & gt; ();
            //if (phone != null)
            //{
            //    chatMessage.Recipients.Add(phone.Number);
            //}
            await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
        }
    }
}
