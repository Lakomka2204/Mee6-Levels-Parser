using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Net;
using MaterialDesignThemes.Wpf;
using System.Deployment.Application;
using Mee6LevelsParser.Properties;
using System.Reflection;

namespace Mee6LevelsParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Dispatcher.UnhandledException += OnEx;
#if DEBUG
            bt_test.Visibility = Visibility.Visible;
#else
            bt_test.Visibility = Visibility.Collapsed;
#endif
        }

        private void OnEx(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.ToString(), Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CheckForNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }
        bool IsTextNumeric(string str)
        {
            return Regex.IsMatch(str, "[^0-9]");
        }

        void PreventPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string txt = (string)e.DataObject.GetData(typeof(string));
                if (IsTextNumeric(txt))
                    e.CancelCommand();
            }
            else e.CancelCommand();
        }
        void AddMember(LeaderboardUser user, bool fulav)
        {
            Color elipcolor = user.rank == 1 ? Color.FromRgb(255, 255, 0) :
                user.rank == 2 ? Color.FromRgb(128, 128, 128) :
                user.rank == 3 ? Color.FromRgb(210, 105, 30) : Color.FromRgb(90, 90, 90);
            //Color elipcolor = Color.FromRgb(90, 90, 90);
            Color white = Color.FromRgb(255, 255, 255),
                black = Color.FromRgb(0, 0, 0);

            Ellipse uniELIP = new Ellipse
            {
                Height = 20,
                Width = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                Fill = new SolidColorBrush(elipcolor),
            };
            TextBlock uniTB = new TextBlock
            {
                Text = user.rank.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 8,
                Foreground = new SolidColorBrush(white),
            };
            Grid g = new Grid
            {
                Margin = new Thickness(5, 10, 5, 10),
            };
            g.Children.Add(uniELIP);
            g.Children.Add(uniTB);
            uniELIP = new Ellipse
            {
                Height = 60,
                Width = 60,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(4),
                Fill = new ImageBrush(new BitmapImage(fulav ? new Uri(user.avatar) : new Uri("https://cdn.discordapp.com/avatars/" + user.id + "/" + user.avatar + ".png?size=128"))),
            };
            uniTB = new TextBlock
            {
                Text = $"{user.username}#{user.discriminator}",
                Foreground = new SolidColorBrush(white),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10),
                FontSize = 12,
                FontWeight = FontWeights.Normal,
                TextWrapping = TextWrapping.Wrap,
            };
            Viewbox nickvb = new Viewbox
            { MaxWidth = 357, Child = uniTB };
            DockPanel wowanotherchilddockpanel = new DockPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Center,
            };
            uniTB = new TextBlock
            {
                Foreground = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                Text = "Messages\n"
            };
            uniTB.Inlines.Add(new Bold(new Run { Foreground = new SolidColorBrush(white), Text = user.message_count }));
            wowanotherchilddockpanel.Children.Add(uniTB);
            uniTB = new TextBlock
            {
                Foreground = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                Text = "EXP Points\n"
            };
            uniTB.Inlines.Add(new Bold(new Run { Foreground = new SolidColorBrush(white), Text = user.xp }));
            wowanotherchilddockpanel.Children.Add(uniTB);

            uniTB = new TextBlock
            {
                Foreground = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Center,
                Text = "Level\n"
            };
            uniTB.Inlines.Add(new Bold(new Run { Foreground = new SolidColorBrush(white), Text = user.level }));
            wowanotherchilddockpanel.Children.Add(uniTB);
            DockPanel mdockp = new DockPanel();
            mdockp.Children.Add(g); // место в топе
            mdockp.Children.Add(uniELIP); // аватарка
            mdockp.Children.Add(nickvb); // никнейм
            mdockp.Children.Add(wowanotherchilddockpanel); // статы (колво сообщений, экспа, левел)
            Border brd = new Border
            {
                Margin = new Thickness(10),
                Background = new SolidColorBrush(Color.FromRgb(44, 47, 51)),
                BorderBrush = new SolidColorBrush(black),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(16),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 90,
                MinWidth = 600,
                VerticalAlignment = VerticalAlignment.Center,
                Child = mdockp,
            };
            SubPanel.Children.Add(brd);
        }

        async Task<bool> LoadMembers(string id, int page)
        {
            string json = await new WebClient().DownloadStringTaskAsync($"https://mee6.xyz/api/plugins/levels/leaderboard/{id}?page={page}");
            Ldrbrd l = JsonConvert.DeserializeObject<Ldrbrd>(json);
            Title = $"Mee6 Levels Parser - {l.guild.name} Pages: {page+1}";
            place = (uint)page * 100;
            for (int i = 0; i < l.players.Length; i++)
            {
                l.players[i].rank = ++place;
                AddMember(l.players[i], false);
            }
            return l.players.Length > 0;
        }
        uint place;
        int page = 0;
        async void bt_search_Click(object sender, RoutedEventArgs easda)
        {
            string tid = tb_serverid.Text;
            if (string.IsNullOrEmpty(tid) || string.IsNullOrWhiteSpace(tid))
                return;
            SubPanel.Children.Clear();
            bool added;
            try
            {
                added = await LoadMembers(tb_serverid.Text, page);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                Title = "Mee6 Levels Parser";
                return;
            }
            
            Button more = new Button
            {
                Content = "More",
                Margin = new Thickness( 10),
                FontSize = 20,
                Width = 125,
                FontWeight = FontWeights.Bold,

            };
            more.Click += async (s, ea) =>
            {
                more.Content = "Wait...";
                more.IsEnabled = false;
                if (await LoadMembers(tid, ++page))
                {
                    SubPanel.Children.Remove(more);
                    more.Content = "More";
                    more.IsEnabled = true;
                    SubPanel.Children.Add(more);
                }
            };
            if (added)
            SubPanel.Children.Add(more);
        }
        uint pl = 1;
        private void bt_test_Click(object sender, RoutedEventArgs e)
        {

            AddMember(new LeaderboardUser
            {
                rank = pl++,
                level = "98",
                avatar = "https://www.google.com/favicon.ico",
                discriminator = "3343",
                message_count = "3434d",
                id = "4444",
                username = "test",
                xp = "0"
            }, true);
        }

        private void tb_about_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show($"Mee6 Levels Parser\n\nBy Lakomka#9404\nv{Assembly.GetExecutingAssembly().GetName().Version}",
                "About",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
