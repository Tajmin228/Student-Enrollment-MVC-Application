using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Project1294727.Models.ViewModels
{
    public class StudentVM
    {
        public int StudentID { get; set; }
        [Display(Name = "Student Name"), Required]
        public string StudentName { get; set; }
        [Display(Name = "Date of Birth"), Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        [Display(Name = "Marital Status")]
        public bool MaritalStatus { get; set; }

        public List<int> CourseList { get; set; } = new List<int>();
    }
}