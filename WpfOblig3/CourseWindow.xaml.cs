using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfOblig3.Models;

namespace WpfOblig3
{
    /// <summary>
    /// Interaction logic for CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Window
    {
        public Dat154Context? Dx { get; set; }
        public CourseWindow(Dat154Context context)
        {
            InitializeComponent();
            Dx = context;
            this.Loaded += CourseWindow_Loaded;
        }
        
        private void CourseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            courseList.DataContext = Dx?.Courses.ToList(); 
        }

        private void courseList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Course? course = courseList.SelectedItem as Course;

            if (course != null)
            {
                var combinedListData = Dx?.Students
                    .Where(s => s.Grades.Any(g => g.Coursecode == course.Coursecode))
                    .Select(s => new
                    {
                        Student = s,
                        Grade = s.Grades.FirstOrDefault(g => g.Coursecode == course.Coursecode).Grade1
                    })
                    .ToList();

                studentCourseList.DataContext = combinedListData;
            }
        }
    }
}
