using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using MicrosoftAdvertising;



namespace Bangla_Calendar_Win_10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ComboBox box_mon;
        ComboBox box_year;
        TextBlock debug;
        int[] english_date_on_bangla_month_starting = { 14, 15, 15, 16, 16, 16, 16, 15, 15, 14, 13, 15 };
        int[] english_month_starting_on_bangla_month = { 18, 18, 17, 17, 17, 16, 17, 17, 18, 19, 17, 18 };
        String[] english_week_day_name = { "Saturday", "Sunday", "Monday", "Thuesday", "Wednesday", "Thursday", "Friday" };
        String[] bangla_digit = { "০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯" };
        String[] week_day_name = { "শনি", "রবি", "সোম", "মঙ্গল", "বুধ", "বৃহঃস্পতি", "শুক্র" };
        String[] bangla_month_name = { "বৈশাখ", "জ্যৈষ্ঠ", "আষাঢ়", "শ্রাবণ", "ভাদ্র", "আশ্বিন", "কার্তিক", "অগ্রায়ণ", "পৌষ", "মাঘ", "ফাল্গুন", "চৈত্র" };
        String[] english_month_name = { "জানুয়ারী", "ফেব্রুয়ারী", "মার্চ", "এপ্রিল ", "মে ", "জুন ", "জুলাই ", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বের" };
        TextBlock mn;
        int[] bangla_month_days = new int[12];
        int today_bangla_month, today_bangla_date;
        int current_month_starting_day;
        int current_month;
        int bangla_month_starting_day;
        int current_bangla_year;
        int current_english_year;
        int[] h_day = { 21, 26, 14, 16 };
        int[] h_month = { 1, 2, 3, 11 };
        int today_bangla_year;
        String[] h_string = { "মাতৃভাষা ও শহীদ দিবস", "স্বাধীনতা দিবস", "শুভ নববর্ষ", "বিজয় দিবস" };
        public MainPage()
        {
            this.InitializeComponent();
            int i;
            int week_day_no;
            int k = 103;
            for (week_day_no = 0; !System.DateTime.Now.DayOfWeek.ToString().Equals(english_week_day_name[week_day_no]); week_day_no++) ;
            
            for (i = 0; i <= 4; i++) bangla_month_days[i] = 31;
            for (i = 5; i <= 11; i++) bangla_month_days[i] = 30;

            int english_mounth_day = System.DateTime.Now.Day;
            int english_year_day = System.DateTime.Now.DayOfYear;
            int english_year = System.DateTime.Now.Year;
            if (english_year % 4 == 0 && english_year % 100 != 0)
            {
                bangla_month_days[10] = 31;
                k = 104;
                english_month_starting_on_bangla_month[10] = 18;
            }
            today_bangla_year = english_year - 593;

            if (english_year_day > k) english_year_day -= k;
            else
            {
                english_year_day += 261;
                today_bangla_year--;
            }
            i = 0;
            while (english_year_day > bangla_month_days[i])
            {
                english_year_day -= bangla_month_days[i];
                i++;
            }
            today_bangla_month = i;
            today_bangla_date = english_year_day;
            TextBlock today = new TextBlock();
            today.FontSize = 18;
            today.Margin = new Thickness(50, 25, 0, 0);
            String str;
            str = "আজ ";
            str += week_day_name[week_day_no] + "বার ";
            str += bangla_Number(today_bangla_date);
            str += " ";
            str += bangla_month_name[today_bangla_month];
            str += " ";
            str += bangla_Number_Long(today_bangla_year);
            str += " ( ";
            str += bangla_Number(System.DateTime.Now.Day);
            str += " ";
            str += english_month_name[System.DateTime.Now.Month - 1];
            str += " ";
            str += bangla_Number_Long(System.DateTime.Now.Year);
            str += " ) ";
            today.Text = str;
            MainLayout.Children.Add(today);
            bangla_month_starting_day = week_day_no - (today_bangla_date - 1) % 7;
            if (bangla_month_starting_day < 0) bangla_month_starting_day += 7;
            current_month_starting_day = bangla_month_starting_day;
            current_month = today_bangla_month;
            current_english_year = english_year;
            mn = new TextBlock();
            mn.Margin = new Thickness(50, 60, 0, 0);
            mn.FontSize = 32;
            MainLayout.Children.Add(mn);
            current_bangla_year = today_bangla_year;
            current_month = today_bangla_month;
            current_month_starting_day = bangla_month_starting_day;
            display_Bangla_Month(today_bangla_month, today_bangla_year, bangla_month_starting_day);
            Button next = new Button();
            next.Content = "পরবর্তী মাস";
            next.Margin = new Thickness(800, 580, 0, 0);
            next.Click += next_Click;
            MainLayout.Children.Add(next);
            Button pre = new Button();
            pre.Content = "পূর্ববর্তী মাস";
            pre.Margin = new Thickness(50, 580, 0, 0);
            pre.Click += pre_Click;
            MainLayout.Children.Add(pre);
            /* Set Up Combo*/

            box_mon = new ComboBox();
            box_mon.Margin = new Thickness(950, 60, 0, 0);
            box_mon.Height = 30;
            box_mon.Width = 100;
            box_mon.FontSize = 16;
            for (i = 0; i < 12; i++)
            {
                box_mon.Items.Add(bangla_month_name[i]);
            }
            MainLayout.Children.Add(box_mon);
            box_mon.SelectedIndex=today_bangla_month;
            
            box_mon.SelectionChanged += Box_mon_SelectionChanged;

            TextBlock ms = new TextBlock();
            ms.Margin = new Thickness(950, 40, 0, 0);
            ms.FontSize = 14;
            ms.Text = "মাস নির্বাচন:";
            MainLayout.Children.Add(ms);
            box_year = new ComboBox();
            box_year.Margin = new Thickness(950, 160, 0, 0);
            box_year.Height = 30;
            box_year.Width = 180;
            box_year.FontSize = 14;
            for (i = 1308; i < 1506; i++)
            {
                String cm_year;
                cm_year = bangla_Number_Long(i);
                cm_year += "(" + (i + 593) + "-" + (i + 594) + ")";
                box_year.Items.Add(cm_year);
            }
            MainLayout.Children.Add(box_year);
            box_year.SelectedIndex= current_bangla_year - 1308;
            box_year.SelectionChanged += Box_year_SelectionChanged;
            debug = new TextBlock();
            debug.Margin = new Thickness(950,300, 0, 0);
            debug.FontSize = 10;
            debug.Text = week_day_no.ToString();
            Ad.ErrorOccurred += Ad_ErrorOccurred;
            Ad.AdRefreshed += Ad_AdRefreshed;
            
            MainLayout.Children.Add(debug);
            
        }

        private void Ad_AdRefreshed(object sender, RoutedEventArgs e)
        {
            debug.Text = e.ToString();
        }

        private void Ad_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            debug.Text = sender.ToString()+" "+e.ErrorCode.ToString();
        }

        private void Box_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeOnSelection();
        }

        private void Box_mon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeOnSelection();
        }
        private void changeOnSelection()
        {
            Ad.Refresh();
            int selectedMon = box_mon.SelectedIndex;
            int selectedYear = box_year.SelectedIndex + 1308;
            int enmon = selectedMon + 3;
            int eyear = selectedYear + 593;
            if (selectedMon >= 10)
            {
                eyear++;
                enmon -= 12;
            }
            int enday = english_date_on_bangla_month_starting[selectedMon];
            int selected_mon_starting = convert(enday, enmon, eyear);
            current_bangla_year = selectedYear;
            current_english_year = eyear;
            current_month = selectedMon;
            current_month_starting_day = selected_mon_starting;
            if (current_english_year % 4 == 0 && current_english_year % 100 != 0)
            {
                bangla_month_days[10] = 31;
                english_month_starting_on_bangla_month[10] = 18;
            }
            else
            {
                bangla_month_days[10] = 30;
                english_month_starting_on_bangla_month[10] = 17;
            }
            display_Bangla_Month(current_month, current_bangla_year, current_month_starting_day);
        }

        private int convert(int dd, int mm, int yy)
        {   
            long ndays; 
            long ncycles;  
            int nyears; 
            int day; 
            yy -= 1900;
            mm+=1;
            ndays = (long)((30.42 * (mm - 1)) + dd);
            if (mm ==2) ++ndays;
            if((mm > 2) && (mm < 8))--ndays; 
            if (yy % 4 == 0 && (mm > 2)) ++ndays;
            ncycles = yy / 4; 
            ndays += ncycles * 1461; 
            nyears = yy % 4; 
            if (nyears > 0) ndays += 365 * nyears + 1;
            if (ndays > 59) --ndays; 
            day =(int)(ndays) % 7;
            day += 1;
            day %= 7;
            return day;
        }

        void pre_Click(object sender, RoutedEventArgs e)
        {

            if (current_month == 0)
            {
                box_mon.SelectedIndex = 11;
                box_year.SelectedIndex--;
            }
            else box_mon.SelectedIndex--;
            changeOnSelection();
        }

        void next_Click(object sender, RoutedEventArgs e)
        {
            if (current_month == 11)
            {
                box_mon.SelectedIndex = 0;
                box_year.SelectedIndex++;
            }
            else box_mon.SelectedIndex++;
            changeOnSelection();
        }
        String bangla_Number(int i)
        {
            String str = "";
            str += bangla_digit[(i - (i % 10)) / 10];
            str += bangla_digit[i % 10];
            return str;
        }
        String bangla_Number_Long(int i)
        {
            String str = "";
            str += bangla_digit[i / 1000];
            str += bangla_digit[(i % 1000) / 100];
            str += bangla_digit[(i % 100) / 10];
            str += bangla_digit[i % 10];
            return str;
        }
        

        private void display_Bangla_Month(int bangla_month, int bangla_year, int starting_day)
        {
            int i;
            int cem, cey;
            mn.Text = bangla_month_name[bangla_month] + " " + bangla_Number_Long(bangla_year);
            mn.UpdateLayout();
            for (i = 0; i < 7; i++)
            {
                Grid gr = new Grid();
                gr.Margin = new Thickness(50 + i * 120, 120, 0, 0);
                gr.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                gr.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                gr.Background = new SolidColorBrush(Windows.UI.Colors.Chocolate);
                gr.Height = 50;
                gr.Width = 110;
                TextBlock wdn = new TextBlock();
                wdn.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                wdn.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                //wdn.Margin = new Thickness(50 + i * 100,120, 0, 0);
                wdn.FontSize = 14;
                wdn.Text = week_day_name[i] + "বার";
                gr.Children.Add(wdn);
                MainLayout.Children.Add(gr);
            }

            int w = starting_day;
            int h = 0;
            int engDay = english_date_on_bangla_month_starting[bangla_month];
            cey = current_english_year;
            cem = (bangla_month + 3) % 12;
            for (w = 0; w <= 6; w++)
            {
                for (h = 0; h <= 5; h++)
                {
                    Grid gr = new Grid();
                    gr.Margin = new Thickness(50 + w * 120, 185 + h * 70, 0, 0);
                    gr.Width = 110;
                    gr.Height = 60;
                    gr.Background = new SolidColorBrush(Windows.UI.Colors.White);
                    gr.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                    gr.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                    MainLayout.Children.Add(gr);
                }
            }
            w = starting_day;
            h = 0;

            for (i = 1; i <= bangla_month_days[bangla_month]; i++, engDay++)
            {
                TextBlock monthCalender = new TextBlock();
                monthCalender.FontSize = 18;
                monthCalender.Text = bangla_Number(i);
                monthCalender.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                monthCalender.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

                TextBlock emonthCalender = new TextBlock();
                emonthCalender.FontSize = 10;
                emonthCalender.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                emonthCalender.Text = bangla_Number(engDay);
                emonthCalender.TextAlignment = Windows.UI.Xaml.TextAlignment.Left;
                emonthCalender.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;


                Grid gr = new Grid();
                gr.Margin = new Thickness(50 + w * 120, 185 + h * 70, 0, 0);
                gr.Width = 110;
                gr.Height = 60;
                gr.Background = new SolidColorBrush(Windows.UI.Colors.DarkCyan);
                gr.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                gr.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                if (w == 6)
                {
                    gr.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    w = 0;
                    h++;
                }

                else w++;
                bool hday_info = false;
                for (int l = 0; l < 4; l++)
                {
                    if (h_month[l] == cem && h_day[l] == engDay)
                    {
                        emonthCalender.Text += " " + english_month_name[cem];
                        TextBlock hday = new TextBlock();
                        hday.Text = h_string[l];
                        hday.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                        hday.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
                        hday.FontSize = 12;
                        hday_info = true;
                        gr.Background = new SolidColorBrush(Windows.UI.Colors.BlueViolet);
                        gr.Children.Add(hday);

                    }
                }

                if (i == 1 && !hday_info)
                {
                    emonthCalender.Text += " " + english_month_name[(bangla_month + 3) % 12] + " " + bangla_Number_Long(cey);
                    emonthCalender.FontSize = 12;
                }
                else if (i == english_month_starting_on_bangla_month[bangla_month] && !hday_info)
                {
                    if (cem == 11) cey++;
                    cem = (bangla_month + 4) % 12;
                    engDay = 1;
                    emonthCalender.Text = bangla_Number(engDay) + " ";
                    emonthCalender.FontSize = 12;
                    emonthCalender.Text += english_month_name[(bangla_month + 4) % 12] + " " + bangla_Number_Long(cey);

                }

                if (i == today_bangla_date && bangla_month == today_bangla_month && bangla_year==today_bangla_year) 
                {
                    TextBlock today_info = new TextBlock();
                    today_info.Text = "আজ";
                    today_info.FontSize = 6;
                    today_info.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
                    today_info.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    gr.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                    gr.Children.Add(today_info);
                }
                gr.Children.Add(emonthCalender);
                gr.Children.Add(monthCalender);
                MainLayout.Children.Add(gr);

            }
        }
    }
 

}
