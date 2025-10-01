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
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {

        public Dat154Context dx { get; set; }

        public Student s;
        public Editor()
        {
            InitializeComponent();
        }

        public Editor(Student selectedStudent, Dat154Context context)
        {
            InitializeComponent();
            dx = context;
            s = selectedStudent;
            courseList.DataContext = dx.Courses
            .Include(c => c.Grades)
            .Where(c => !c.Grades.Any(g => g.Studentid == s.Id))
            .Select(c => c.Coursename)
            .ToList();

            gradeList.DataContext = dx.Grades
            .GroupBy(g => g.Grade1)
            .Select(group => group.First())
            .ToList();

            removeCourseList.DataContext = dx.Courses
             .Include(c => c.Grades)
            .Where(c => c.Grades.Any(g => g.Studentid == s.Id))
            .Select(c => c.Coursename)
            .ToList();
        }

        private void addToCourse_Click(object sender, RoutedEventArgs e)
        {
            String? selectedCourse = courseList.SelectedValue.ToString();
            String? selectedGrade = gradeList.SelectedValue.ToString();

            if (selectedCourse != null && selectedGrade != null && s != null)
            {
                Course course = dx.Courses.Where(a => a.Coursename.Equals(selectedCourse)).First();
                Grade? newGrade = new Grade
                {
                    Studentid = s.Id,
                    Coursecode = course.Coursecode,
                    Grade1 = selectedGrade,
                    CoursecodeNavigation = course,
                    Student = s
                };
                s.Grades.Add(newGrade);
                dx.Grades.Add(newGrade);
                dx.SaveChanges();
                Console.WriteLine("GRADE IS ADDED");
                this.Close();

            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            String? selectedCourse = removeCourseList.SelectedValue.ToString();

            if (selectedCourse != null)
            {
                Course course = dx.Courses.Where(c => c.Coursename.Equals(selectedCourse)).First();
                Grade? grade = dx.Grades.FirstOrDefault(g => g.Studentid == s.Id && g.CoursecodeNavigation.Coursename.Equals(selectedCourse));
                if (grade != null)
                {
                    s.Grades.Remove(grade);
                    dx.Grades.Remove(grade);
                    dx.SaveChanges();
                    this.Close();
                }
            }
        }

        
    }
}
