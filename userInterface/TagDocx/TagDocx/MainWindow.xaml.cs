using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Security.Permissions;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace TagDocx
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int moved;
        string []fileList = new string[10];

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MainWindow()
        {
            Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 초
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

           
            


            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            
            //시작하고 변수들 초기화
            InitializeComponent();

            
            MainHome.Content = new MainPage();
        }




        //타이머할일
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timer!");   // <- 여기다가 하고싶은 작업 넣으면 됨!
        }

        private void GridBarTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void Mimimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void CloseButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            this.Visibility = Visibility.Hidden;


        }
    }

}
