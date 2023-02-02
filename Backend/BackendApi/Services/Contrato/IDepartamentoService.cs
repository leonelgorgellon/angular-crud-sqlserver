using BackendApi.Models;

namespace BackendApi.Services.Contrato
{
    public interface IDepartamentoService
    {
        //devuelve lista de departamentos
        Task<List<Departamento>> GetList();
    }
}
