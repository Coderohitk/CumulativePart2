using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: AuthorPage/Create
        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int courseId = _api.AddCourse(NewCourse);

            // redirects to "Show" action on "Author" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = courseId });
        }
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int courseId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }
    }
}
