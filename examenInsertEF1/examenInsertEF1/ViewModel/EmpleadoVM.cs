using examenInsertEF1.Models;

namespace examenInsertEF1.ViewModel
{
    public class EmpleadoVM
    {
        public List<Empleado> ListaEmpleados { get; set; } = new();
        public Empleado EmpleadoModelRef { get; set; } = new();
        public List<Departamento> ListaDepartamentos { get;set; } = new();
    }
}
