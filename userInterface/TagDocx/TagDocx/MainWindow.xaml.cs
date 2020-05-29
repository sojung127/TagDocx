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



namespace TagDocx
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

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
            

            //FileSystemWatcher fsw = new FileSystemWatcher("C:\\AutomaticDocumentClassificationService\\Dataset\\한글\\기사\\IT과학\\");
            Console.WriteLine(Directory.GetCurrentDirectory()); // = TagDocx\bin\debug 
            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\Dataset\"));
            FileSystemWatcher fsw = new FileSystemWatcher(path);

            fsw.EnableRaisingEvents = true;

            //  Register a handler that gets called when a
            //  file is created, changed, or deleted.
            fsw.Changed += new FileSystemEventHandler(OnChanged);

            fsw.Created += new FileSystemEventHandler(OnChanged);

            fsw.Deleted += new FileSystemEventHandler(OnChanged);

           // fsw.Renamed += new FileSystemEventHandler(OnRenamed);

            Console.WriteLine("!!!!");
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
            Application.Current.Shutdown();
        }


        private void watch()
        {

            string path = "C:\\AutomaticDocumentClassificationService\\Dataset\\한글\\기사\\IT과학\\";
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        void OnChanged(object source, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                Console.WriteLine("변경");

            }));


            //  Show that a file has been created, changed, or deleted.
            WatcherChangeTypes wct = e.ChangeType;
            Console.WriteLine("File {0} {1}", e.FullPath, wct.ToString());
            Console.WriteLine("변경!!!!");

          
 



            Console.WriteLine("Press the Enter key to exit the program... ");

            Console.ReadLine();

            Console.WriteLine("Terminating the application...");

        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        



    }

}
