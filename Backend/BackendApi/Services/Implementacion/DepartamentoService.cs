using Microsoft.EntityFrameworkCore;
using BackendApi.Models;
using BackendApi.Services.Contrato;

namespace BackendApi.Services.Implementacion
{
    public class DepartamentoService : IDepartamentoService
    {
        private DbempleadoContext _dbContext;

        public DepartamentoService (DbempleadoContext dbContext)
        {
            _dbContext = dbContext;
        }


        //METODO PARA RETORNAR LISTA DE DEPARTAMENTOS 
        public async Task<List<Departamento>> GetList()
        {
            try
            {
                List<Departamento> lista = new List<Departamento>();
                lista = await _dbContext.Departamentos.ToListAsync(); //devuelve lista de forma asincrona
                
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
