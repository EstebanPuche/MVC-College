using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_College.Data;
using MVC_College.Models;

namespace MVC_College.Controllers
{
    public class CoursesController : Controller
    {
        private readonly MVC_CollegeContexto _context;

        public CoursesController(MVC_CollegeContexto context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            /**
             * Agrego el siguiente código para la página principal de "Courses"
             */
            var courses = _context.Courses.Include(e => e.Department).AsNoTracking();
            return View(await courses.ToListAsync());
            //return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        /**
         * El método "Create" de HttpGet llama al método "PopulateDepartmentsDropDownList" sin configurar el 
         * elemento seleccionado, ya que el departamento todavía no está establecido para un nuevo curso:
         */
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Credist,DepartmentId,Title")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CourseId,Title,Credits")] Course course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(course);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(course);
        //}
        /********************************************************************************************/

        // GET: Courses/Edit/5
        /**
         * El método Edit de HttpGet establece el elemento seleccionado, basándose en el identificador 
         * del departamento que ya está asignado a la línea que se está editando
         */
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var course = await _context.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(course);
        //}


        // POST: Courses/Edit/5
        /**
         * Los métodos HttpPost para Create y Edit también incluyen código que configura el elemento 
         * seleccionado cuando vuelven a mostrar la página después de un error. Esto garantiza que, 
         * cuando vuelve a aparecer la página para mostrar el mensaje de error, el departamento que 
         * se haya seleccionado permanece seleccionado.
         */
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);

            if (await TryUpdateModelAsync<Course>(courseToUpdate, "", c => c.Credits,
                c => c.DepartmentId,
                c => c.Title))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persist, "
                        + "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentId);
            return View(courseToUpdate);
        }

        /**
         * El método "PopulateDepartmentsDropDownList" obtiene una lista de todos los departamentos 
         * ordenados por nombre, crea una colección "SelectList" para obtener una lista desplegable 
         * y pasa la colección a la vista en "ViewBag". El método acepta el parámetro opcional 
         * "selectedDepartment", que permite al código que realiza la llamada especificar el elemento 
         * que se seleccionará cuando se procese la lista desplegable. La vista pasará el nombre 
         * "DepartmentID" al asistente de etiquetas <select>, y luego el asistente sabe que puede 
         * buscar en el objeto "ViewBag" una "SelectList" denominada "DepartmentID".
         */
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in _context.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Credits")] Course course)
        //{
        //    if (id != course.CourseId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(course);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CourseExists(course.CourseId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(course);
        //}

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.Include(c => c.Department).AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
