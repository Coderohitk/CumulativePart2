using Cumulative_1.Models;
using Cumulative_1.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{

    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        [HttpGet]
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudent();
            return View(Students);
        }
        [HttpGet]
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            // redirects to "Show" action on "Author" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = StudentId });
        }
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteStudent(id);
            // redirects to list action
            return RedirectToAction("List");
        }
    }
}
