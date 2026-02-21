using examenInsertEF1.Context;
using examenInsertEF1.Models;
using examenInsertEF1.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace examenInsertEF1.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly ExamenInsertEf1Context _examenInsertEf1Context;
        public EmpleadoController(ExamenInsertEf1Context examenInsertEf1Context)
        {
            _examenInsertEf1Context = examenInsertEf1Context;
        }
        public async Task<IActionResult> Index(int idDep)
        {
            var vm = new EmpleadoVM();
            
            ViewBag.IdSeleccionado = idDep;
            try
            {
                vm.ListaDepartamentos = await _examenInsertEf1Context.Departamentos.ToListAsync();
                if (idDep > 0)
                {
                //Cuando usas { 0}, le dices a Entity Framework: "Oye, aquí va a ir un dato, pero no lo pegues todavía. Primero analízalo, asegúrate de que sea un número y límpialo de cualquier cosa rara".
                //El { 0} es un "Asiento reservado": Significa "el primer valor de la lista que te daré después".
                //Si tuvieras más parámetros: Usarías { 1}, { 2}, etc.
                //Ejemplo: "exec sp_Insertar {0}, {1}", nombre, edad
                    vm.ListaEmpleados = await _examenInsertEf1Context.Empleados.FromSqlRaw("exec sp_ListarEmpleadosPorDep {0}",idDep).ToListAsync();
                }
                else
                {
                //Include(tD => tD.IdDepartamentoNavigation)
                //.Include(...): Es la función que pide la relación.
                //tD: Es solo un apodo(alias) que tú le das al objeto en ese momento(puedes ponerle e de empleado, x, lo que quieras).
                //=>: Se lee como "tal que" o "ve a...".
                //tD.IdDepartamentoNavigation: Es la Propiedad de Navegación.
                //Ojo aquí: EF suele ponerle nombres largos como IdDepartamentoNavigation cuando generas el 
                //                        código desde la base de datos. Es básicamente el "puente" que conecta la tabla Empleado con la tabla Departamento.
                    vm.ListaEmpleados = await _examenInsertEf1Context.Empleados.Include(tD => tD.IdDepartamentoNavigation).ToListAsync();
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                vm.ListaEmpleados = new List<Empleado>();
                vm.ListaDepartamentos = new List<Departamento>();
                ViewData["Error"] = "Error al cargar datos: " + ex.Message;
                return View(vm);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Guardar(EmpleadoVM vm)
        {
            try
            {
                // El VM trae dentro el objeto EmpleadoModelRef poblado por la vista
                // Se lo pasamos al método de tu capa Data

                await _examenInsertEf1Context.AddAsync(vm.EmpleadoModelRef);
                int _filas_afectadas = await _examenInsertEf1Context.SaveChangesAsync();

                if (_filas_afectadas>0)
                {
                    TempData["Mensaje"] = "¡Empleado guardado exitosamente!";
                    TempData["Tipo"] = "success";
                }
                else
                {
                    TempData["Mensaje"] = "No se pudo guardar, revisa la conexión.";
                    TempData["Tipo"] = "warning";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error crítico: " + ex.Message;
                TempData["Tipo"] = "danger";
            }

            // Siempre redirigimos al Index para limpiar el formulario y ver la tabla actualizada
            return RedirectToAction("Index");
        }
    }
}
