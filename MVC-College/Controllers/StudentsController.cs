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
    public class StudentsController : Controller
    {
        /**
         * El controlador toma "MVC_CollegeContexto" como parámetro de constructor.
         */
        private readonly MVC_CollegeContexto _context;

        public StudentsController(MVC_CollegeContexto context)
        {
            _context = context;
        }
        /**
         * El controlador contiene un método de acción "Index", que muestra todos los alumnos en la base de datos. 
         * El método obtiene una lista de estudiantes de la entidad Students, que se establece leyendo la propiedad 
         * "Students" de la instancia del contexto de base de datos.
         */
        /**
         * FUNCIONALIDAD ORDENACIÓN
         * Añadimos el parámetro "string sortOrder"
         * FUNCIONALIDAD CUADRO DE BUSQUEDA
         * Añadimos el parámetro "string searchString"
         * FUNCIONALIDAD AGREAGR PAGINACIÓN
         * Añadimos los parámetros "string currentFilter" y "int? pagenumber"
         */
        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            // Funcionalidad agregar paginación
            ViewData ["CurrentSort"] = sortOrder;
            // Funcionalidad ordenación desde las propias columnas
            ViewData ["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData ["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            // Funcionalidad agregar paginación
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            // Funcionalidad cuadro de busqueda
            ViewData ["CurrentFilter"] = searchString;
            var students = from s in _context.Students
                           select s;
            // Funcionalidad cuador de busqueda
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString) || s.FisrtMidName.Contains(searchString));
            }
            //Funcionalidad títulos columnas ordenación
            switch (sortOrder)
            {

                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            //? Código original
            //return View(await _context.Students.ToListAsync());
            //? Código utilizado para "busqueda" y "Ordenación"
            //return View(await students.AsNoTracking().ToListAsync());
            // Cubre todas las funcionalidades creadas
            int pageSize = 5;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Students/Details/5
        /**
         * Vamos a introducir la propiedad "Enrollments" que contiene una colección en una tabla HTML,
         * la depositamos aquí en la página de detalles
         */
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Código original con "FirstOrDefaultAsync" solo recupera una entidad "Student"...
            //var student = await _context.Students
            //    .FirstOrDefaultAsync(m => m.Id == id);

            // Código para incluir mas detalles...
            /**
             * Los métodos "Include" y "ThenInclude" hacen que el contexto cargue la propiedad de 
             * navegación "Student.Enrollments"y, dentro de cada incripción, la propiedad de navegación
             * "Enrollment.Course".
             * El método "AsNoTracking" mejora el rendimineto en casos en los que no se actualizarán las 
             * entidades devueltas en la duración del contexto actual.
             */
            var student = await _context.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /**
         * Hemos quitado el "Id" del atributo Bind, por que "Id" es el valor de clave principal que SQL
         * Server establecerá automáticamente cuando se inserte la fila.
         * Se ha insertado el bloque "Try..Catch"  que si detecta alguna excepción derivada de "DbUpdateException"
         * mientras se guardan los cambios, se muestra un mensaje de error genérico.
         * Es un acto ideal para producciones en serie y de calidad.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,LastName,FisrtMidName,EnrollmentDate")] Student student)
        public async Task<IActionResult> Create([Bind("LastName,FisrtMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment es variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes." + "Try again, and if the problem persist " +
                     "see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FisrtMidName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        /**
         * Reemplazamos el código original por el siguiente que administra los informes de errores.
         */
        //? Código original..
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(student);
        //}
        /**
         * Este código acepta un parámetro opcional que indica si se llamó al método despues de un 
         * error al guardar los cambios.
         */
        public async Task<IActionResult> Delete(int? id, bool? saveCAhngesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveCAhngesError.GetValueOrDefault())
            {
                ViewData ["ErrorMessage"] = "Delet failed. Try again, and if the problem persist " + "see your system administrator.";
            }
            return View(student);
        }

        // POST: Students/Delete/5
        /**
         * Vamos agregar un bloque "Try...Catch" al método HttpPost "Delete" para controlar los errores 
         * que puedan producirse cuando se actualice la base de datos. Si se produice un error, el método 
         * HttpPost Delete, pasando un parámetro que indica que se ha produciddo un error.
         * Despues, el método HttpGet Delete vuelve a mostrar la página de confirmación junto con el 
         * mensaje de error, dando al usuario la oportunidad de cancelar o volver a intentarlo.
         */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //? Código original...
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var student = await _context.Students.FindAsync(id);
        //    _context.Students.Remove(student);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex*/)
            {
                // Log the error (uncomment ex variable name and write s log.)
                return RedirectToAction(nameof(Delete), new
                {
                    id = id,
                    saveChangesError = true
                });
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
