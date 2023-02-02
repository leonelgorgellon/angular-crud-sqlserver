using BackendApi.Models;

namespace BackendApi.Services.Contrato
{
    public interface IEmpleadoService
    {
        //devuelve lista de empleados 
        Task<List<Empleado>> GetList();

        //devuelve solo un empleado 
        Task<Empleado> Get(int idEmpleado);

        //agrega un empleado 
        Task<Empleado> Add(Empleado modelo);

        //actualizar un empleado 
        Task<bool> Update(Empleado modelo);

        //elimina empleado 
        Task<bool> Delete(Empleado modelo);

    }
}
