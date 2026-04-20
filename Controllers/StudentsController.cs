using MVC_Project1294727.Models;
using MVC_Project1294727.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MVC_Project1294727.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly StudentEnrollmentDBContext db = new StudentEnrollmentDBContext();
        // GET: Students
        [AllowAnonymous]
        public ActionResult Index()
        {
            var students = db.Students.Include(x => x.Enrollments.Select(s => s.Cours)).OrderByDescending(x => x.StudentID).ToList();
            return View(students);
        }
        public ActionResult AddNewCourse(int? id)
        {
            ViewBag.Cours = new SelectList(db.Courses.ToList(), "CourseID", "Title", (id != null) ? id.ToString() : "");
            return PartialView("_addNewCourse");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentVM studentVM, int[] CourseID)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_error");
            }
            Student student = new Student()
            {
                StudentName = studentVM.StudentName,
                BirthDate = studentVM.BirthDate,
                Age = studentVM.Age,
                MaritalStatus = studentVM.MaritalStatus
            };
            //Image Process
            HttpPostedFileBase file = studentVM.PictureFile;
            if (file != null)
            {

                string fileName = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));

                file.SaveAs(Server.MapPath(fileName));
                student.Picture = fileName;
            }

            //Save all Course from CourseID
            foreach (var item in CourseID)
            {
                Enrollment enrollment = new Enrollment()
                {
                    Student = student,
                    StudentID = student.StudentID,
                    CourseID = item
                };
                db.Enrollments.Add(enrollment);
            }
            db.SaveChanges();
            return PartialView("_success");
        }
        public ActionResult Edit(int? id)
        {
            Student student = db.Students.First(x => x.StudentID == id);
            var studentCourse = db.Enrollments.Where(x => x.StudentID == id).ToList();
            StudentVM studentVM = new StudentVM()
            {
                StudentID = student.StudentID,
                StudentName = student.StudentName,
                BirthDate = student.BirthDate,
                Age = student.Age,
                Picture = student.Picture,
                MaritalStatus = student.MaritalStatus
            };
            if (studentCourse.Count() > 0)
            {
                foreach (var item in studentCourse)
                {
                    studentVM.CourseList.Add(item.CourseID);
                }
            }
            return View(studentVM);
        }
        [HttpPost]
        public ActionResult Edit(StudentVM studentVM, int[] courseID)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentID = studentVM.StudentID,
                    StudentName = studentVM.StudentName,
                    BirthDate = studentVM.BirthDate,
                    Age = studentVM.Age,
                    MaritalStatus = studentVM.MaritalStatus
                };
                //image
                HttpPostedFileBase file = studentVM.PictureFile;
                if (file != null)
                {
                    string fileName = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(fileName));
                    student.Picture = fileName;
                }
                else
                {
                    student.Picture = studentVM.Picture;
                }
                //Course 
                var courseEntry = db.Enrollments.Where(x => x.StudentID == student.StudentID).ToList();

                foreach (var bo in courseEntry)
                {
                    db.Enrollments.Remove(bo);
                }
                foreach (var item in courseID)
                {
                    Enrollment enrollment = new Enrollment()
                    {
                        StudentID = student.StudentID,
                        CourseID = item
                    };
                    db.Enrollments.Add(enrollment);
                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            Student student = db.Students.First(x => x.StudentID == id);
            var studentCourse = db.Enrollments.Where(x => x.StudentID == id).ToList();
            StudentVM studentVM = new StudentVM()
            {
                StudentID = student.StudentID,
                StudentName = student.StudentName,
                BirthDate = student.BirthDate,
                Age = student.Age,
                Picture = student.Picture,
                MaritalStatus = student.MaritalStatus
            };
            if (studentCourse.Count() > 0)
            {
                foreach (var item in studentCourse)
                {
                    studentVM.CourseList.Add(item.CourseID);
                }
            }
            return View(studentVM);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);

            if (student == null)
            {
                return HttpNotFound();
            }
            var courseEntry = db.Enrollments.Where(x => x.StudentID == student.StudentID).ToList();
            db.Enrollments.RemoveRange(courseEntry);
            db.Entry(student).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}