using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
using WpfOblig3.Models;

namespace WpfOblig3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Dat154Context dx = new();

        private readonly LocalView<Student> Students;

        public MainWindow()
        {
            InitializeComponent();

            Students = dx.Students.Local;

            dx.Students.Load();

            studentList.DataContext = Students.OrderBy(s => s.Studentname);

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

            studentList.DataContext = Students.Where(s => s.Studentname.ToLower().Contains(searchField.Text.ToLower())).
                OrderBy(s => s.Studentname);

        }

        private void studentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Student? s = studentList.SelectedItem as Student;

            if (s != null)
            {

                Editor ed = new Editor(s, dx);

                ed.Show();

            }
        }

        private void ShowCourses_Click(object sender, RoutedEventArgs e)
        {
            CourseWindow course = new(dx);
            
            course.Show();
        }

        private void showGrades_Click(object sender, RoutedEventArgs e)
        {
            GradeWindow grade = new(dx);

            grade.Show();
        }

    }
}