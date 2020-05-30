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

using System.Security.Permissions;
using System.IO;

namespace TagDocx
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        int moved;
        string []fileList = new string[10];
        public MainWindow()
        {
            InitializeComponent();

            moved = 0;
            for(int i = 0; i < 10; i++)
            {
                fileList[i] = "";
            }
            //FileSystemWatcher fsw = new FileSystemWatcher("C:\\AutomaticDocumentClassificationService\\Dataset\\한글\\기사\\IT과학\\");
            Console.WriteLine(Directory.GetCurrentDirectory()); // = TagDocx\bin\debug 
                                                                // dataset 상대경로
                                                                //string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\Dataset\"));

            // test용 경로
            string testPath = @"C:\test\";
            string TempPath = @"C:\Temp\";
            FileSystemWatcher fsw = new FileSystemWatcher(testPath);
            FileSystemWatcher fswTemp = new FileSystemWatcher(TempPath);

            fsw.EnableRaisingEvents = true;
            fswTemp.EnableRaisingEvents = true;

            //  Register a handler that gets called when a
            //  file is created, changed, or deleted.
            fsw.Changed += new FileSystemEventHandler(OnChanged);

            fsw.Created += new FileSystemEventHandler(OnCreated);

            fsw.Deleted += new FileSystemEventHandler(OnDeleted);

            fswTemp.Changed += new FileSystemEventHandler(OnChanged);
               
            fswTemp.Created += new FileSystemEventHandler(OnCreated);
               
            fswTemp.Deleted += new FileSystemEventHandler(OnDeleted);

            // fsw.Renamed += new FileSystemEventHandler(OnRenamed);

            Console.WriteLine("!!!!");
            MainHome.Content = new MainPage();
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
       void OnCreated(object soruce,FileSystemEventArgs e)
        {
            Console.WriteLine("\n" + e.Name + "created start "+moved);
            for(int i = moved-1; i >= 0; i--)
            {
                    Console.Write(fileList[i] + " " + i);
                if (fileList[i].Equals(e.Name))
                {
                    // 이 경우 db에 추가
                    fileList[i] = "";
                    Console.WriteLine("일치");
                    return;
                }

            }
            fileList[moved] = e.Name;
            if (moved < 9)
                moved++;
            else
                moved = 0;
            Console.WriteLine("추가");

            Console.WriteLine(
                "Change noticed: Object Name = {0}, Object Event: {1} File Content",
                e.Name, e.ChangeType);
        }

        void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("\n" + e.Name + "deleted start "+moved);
            for (int i = moved-1; i >= 0; i--)
            {
                    Console.Write(fileList[i]+" "+i);
                if (fileList[i].Equals(e.Name))
                {
                    // 이 경우 db에 추가
                    fileList[i] = "";
                    Console.WriteLine("일치");
                    return;
                }

            }
            fileList[moved] = e.Name;
            if (moved < 9)
                moved++;
            else
                moved = 0;
            Console.WriteLine("스택 추가");

            Console.WriteLine(
                "Change noticed: Object Name = {0}, Object Event: {1} File Content",
                e.Name, e.ChangeType);

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
            
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        

    }
}
