using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_College.Data;
using MVC_College.Models;
using MVC_College.Models.SchoolViewModels;

namespace MVC_College.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly MVC_CollegeContexto _context;

        public InstructorsController(MVC_CollegeContexto context)
        {
            _context = context;
        }

        // GET: Instructors
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Instructors.ToListAsync());
        //}
        /**
         * Reemplazo el método Index con el código siguiente para realizar la carga diligente de los datos
         * relacionados y colocarlos en el modelo de vista.
         * El método acepta datos de ruta opcionales (id) y un parámetro de cadena de consulta (courseID) 
         * que proporcionan los valores ID del instructor y el curso seleccionados. Los parámetros se 
         * proporcionan mediante los hipervínculos Select de la página.
         */
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            //! estudia este código!!!!!
            /**
             * El código comienza creando una instancia del modelo de vista y coloca en ella la lista de
             * instructores. El código especifica la carga diligente para "Instructor.OfficeAssignment" y 
             * las propiedades de navegación de "Instructor.CourseAssignments". Dentro de la propiedad 
             * "CourseAssignments" se carga la propiedad "Course" y dentro de esta se cargan las propiedades 
             * "Enrollments" y "Department", y dentro de cada entidad "Enrollment" se carga la propiedad 
             * "Student".
             */
            var viewModel = new InstructorIndexData();
            viewModel.Intructors = await _context.Instructors.Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Department)
                .AsNoTracking().OrderBy(i => i.LastName).ToListAsync();
            /**
             * El código siguiente se ejecuta cuando se ha seleccionado un instructor. El instructor 
             * seleccionado se recupera de la lista de instructores del modelo de vista. Después, se 
             * carga la propiedad "Courses" del modelo de vista con las entidades "Course" de la propiedad
             * de navegación "CourseAssignments" de ese instructor.
             */
            if (id != null)
            {
                ViewData ["InstructorId"] = id.Value;
                Instructor instructor = viewModel.Intructors.Where(i => i.Id == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }
            /**
             * A continuación, si se ha seleccionado un curso, se recupera de la lista de cursos en el
             * modelo de vista. Después, se carga la propiedad "Enrollments" del modelo de vista con las
             * entidades "Enrollment" de la propiedad de navegación Enrollments de ese curso.
             */
            if (courseId != null)
            {
                ViewData ["CourseId"] = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseId == courseId).Single().Enrollments;
            }
            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
