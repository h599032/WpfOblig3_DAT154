using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for GradeWindow.xaml
    /// </summary>
    public partial class GradeWindow : Window
    {
        public Dat154Context? Dx { get; set; }
        public GradeWindow(Dat154Context context)
        {
            InitializeComponent();
            Dx = context;
            this.Loaded += GradeWindow_Loaded;
        }
        private void GradeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            selectGrade.DataContext = Dx.Grades
            .GroupBy(g => g.Grade1)
            .ToList();
            failedStudentList.DataContext = Dx.Grades
                .Where(g => g.Grade1.Equals("F"))
                .Select( g => new
                { 
                    Studentname = g.Student.Studentname,
                    Course = g.CoursecodeNavigation,
                }).OrderBy(s => s.Studentname).ToList();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedGrade = selectGrade.SelectedValue.ToString();

            studentList.DataContext = Dx.Grades
                .Where(g => g.Grade1.CompareTo(selectedGrade) <= 0)
                .Select(g => new
                {
                    Studentname = g.Student.Studentname,
                    Course = g.CoursecodeNavigation,
                    Grade = g.Grade1
                }).OrderBy(s => s.Studentname)
                .ToList();
        }

    }
}
