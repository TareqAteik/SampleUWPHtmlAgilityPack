using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HtmlAgilityPackSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            using (var client = new HttpClient())
            {
                var htmlString = await client.GetStringAsync("http://www.imdb.com/movies-in-theaters/");
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                List<Movie> movies = new List<Movie>();
                foreach (var div in htmlDocument.DocumentNode.Descendants().Where(i => i.Name == "div" && i.GetAttributeValue("class", "").StartsWith("list_item")))
                {
                    Movie newMovie = new Movie();
                    newMovie.Cover = div.Descendants().Where(i => i.Name == "div" && i.GetAttributeValue("class", "") == "image").FirstOrDefault().Descendants().Where(i => i.Name == "img").FirstOrDefault().GetAttributeValue("src", "");
                    newMovie.Title = div.Descendants().Where(i => i.Name == "h4" && i.GetAttributeValue("itemprop", "") == "name").FirstOrDefault().InnerText.Trim();
                    newMovie.Summary = div.Descendants().Where(i => i.Name == "div" && i.GetAttributeValue("class", "") == "outline").FirstOrDefault().InnerText.Trim();
                    movies.Add(newMovie);
                }
                lstMovies.ItemsSource = movies;
            }
        }
    }
}
