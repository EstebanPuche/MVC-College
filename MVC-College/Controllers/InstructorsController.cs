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
        /********************************************************************************************************/
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
        /*******************************************************************************************************/
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
        /******************************************************************************************************/
        // GET: Instructors/Create
        public IActionResult Create()
        {
            /**
             * 
             */
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);

            return View();
        }
        /******************************************************************************************************/
        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstMidName,HireDate,OfficeAssignmnet")] Instructor instructor, string[] selectedCourses)
        {
            /**
             * Agrego la ubicación de la oficina y los cursos a la página "Create"
             * El método Create de HttpPost agrega cada curso seleccionado a la propiedad de navegación 
             * "CourseAssignments" antes de comprobar si hay errores de validación y agrega el instructor
             * nuevo a la base de datos. Los cursos se agregan incluso si hay errores de modelo, por lo 
             * que cuando hay errores del modelo (por ejemplo, el usuario escribió una fecha no válida) 
             * y se vuelve a abrir la página con un mensaje de error,
             */
            if (selectedCourses != null)
            {
                // Para agregar cursos a la propiedad de navegación "courseAssignments" hay que inicar la propiedad como una colecciónvacia.
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorId = instructor.Id, CourseId = int.Parse(course) };
                    instructor.CourseAssignments.Add(courseToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Método añadido 
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }
        /*****************************************************************************************************/
        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //? Código origianl
            //var instructor = await _context.Instructors.FindAsync(id);
            /**
             * Cambiamos elcódigo del método "Edit" para que cargue la propiedad de navegación 
             * "OfficeAssignment" de la entidad "Instructor" y llame a "AsNoTracking".
             * NOTA: Se ha añadido los "Include" para "CourseAssignment y Course" para poner en la vista de Edición
             * los checks de loos cursos
             */
            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            // Llamada a un método para poner en las vistas los checks de las cursos
            PopulateAssignedCourseData(instructor);

            return View(instructor);
        }
        /**
         * El código en el método "PopularAssignedCourseData" lee todas las entidades "Course"  para cargar 
         * una lista de cursos mediante la clase de modelo de vista. Para cada curso, el código comprueba si 
         * existe el curso en la propiedad de navegación "Courses" del instructor.
         */
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourse = _context.Courses;
            // HasSet crea una busqueda eficaz comprobando si un curso está asignado a un instructor y se colocan en una lista
            var instructorsCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseId));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourse)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Assigned = instructorsCourses.Contains(course.CourseId)
                });
            }
            //La lista se pasa a ViewData
            ViewData ["Courses"] = viewModel;
        }
        /********************************************************************************************************/
        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //? Código original
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstMidName,HireDate")] Instructor instructor)
        //{
        //    if (id != instructor.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(instructor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!InstructorExists(instructor.Id))
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
        //    return View(instructor);
        //}
        /**
         * Reemplazamos el método "Edit" de "HttpPost" con el siguiente código para controlar las 
         * actualizaciones de asignaciones de oficina.
         */
        [HttpPost/*, ActionName("Edit")*/]
        [ValidateAntiForgeryToken]
        // Agregamos parámetro para editar las casillas de los cursos de los profesores "string [] selectedCourses"
        public async Task<IActionResult> EditPost(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                //incluimos las "Include" de ls cursos para las casillas 
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
            /**
             * Actualizamos la entidad "Instructor" recuperada convalores del enlazador de modelos. 
             */
            if (await TryUpdateModelAsync<Instructor>(instructorToUpdate, "",
                i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                /**
                 * Si la ubicación de la oficina esta en blanco, establece la propiedad "Instructor.OfficeAssignment"
                 * en NULL para que se elimine la fila relacionada en la tabla "OfficeAssignment".
                 */
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    // Log the error (uncomment es variabñle name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persist, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructorToUpdate);
        }
        //! Método que actualiza los cursos del iunstructor
        private void UpDateInstructorCourses(string [] selectedCourses, Instructor instructorToUpdate)
        {
            /**
             * Si no se ha seleccionado ninguna casilla, el código "UpDateInstructorCourses" inicializa la propiedad
             * de navegación "CourseAssignments" con una colección vacía.
             */
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }
            /**
             * Acontinuación, el código recorre en bucle los cursos de la base de datos y coteja los que están asignados
             * actualmente al instructor frente a los que se han seleccionado en la vista. Para facilitar las búsquedas
             * las dos colecciones se almacenan en objetos "HasSet"
             */
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseId));
            foreach (var course in _context.Courses)
            {
                /**
                 * Si se ha activado la casilla para un curso, pero este no se encuentra en la proiedad de navagación 
                 * "Instructor.CourseAssignments", el curso se agrega a la colección en la propiedad de navegación.
                 */
                if (selectedCoursesHS.Contains(course.CourseAssignments.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseId))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorId = instructorToUpdate.Id, CourseId = course.CourseId });
                    }
                }
                /**
                 * Si no se ha activado la casilla para un curso, pero este se encuentra en la propiedad de navegación
                 * "Instructor.CourseAssignments", el curso se quita de la colección en la propiedad de navegación.
                 */
                else
                {
                    if (instructorCourses.Contains(course.CourseId))
                    {
                        CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.FirstOrDefault(i => i.CourseId == course.CourseId);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }
        /*********************************************************************************************************/
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
        /*********************************************************************************************************/
        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /**
             * Si el instructor que se va a eliminar está asignado como administrador de cualquiera 
             * de los departamentos, quita la asignación de instructor de esos departamentos.
             */
            Instructor instructor = await _context.Instructors
                .Include(i => i.CourseAssignments).SingleAsync(i => i.Id == id);

            var departments = await _context.Departments
                .Where(d => d.InstructorId == id).ToListAsync();
            departments.ForEach(d => d.InstructorId = null);

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //? Código original
            /*var instructor = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
