using AutoMapper;
using BackendApi.DTOs;
using BackendApi.Models;
using System.Globalization;

namespace BackendApi.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //aca mapeamos el departamento con el departamentoDTO y para que el modelo se convierta en un modeloDTO 
            #region Departamento
            CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
            #endregion

            //aca vamos a tener que hacer lo mismo pero configurando los cambios realizados en el modelo dto ya que no es igual al modelo original 
            #region Empleado
            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(destino =>
                destino.NombreDepartamento,
                opt => opt.MapFrom(origen => origen.IdDepartamentoNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.FechaContrato,
                opt => opt.MapFrom(origen => origen.FechaContrato.Value.ToString("dd/MM/yyyy"))
             );


            CreateMap<EmpleadoDTO, Empleado>()
                .ForMember(destino =>
                destino.IdDepartamentoNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.FechaContrato,
                opt => opt.MapFrom(origen => DateTime.ParseExact(origen.FechaContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture)) //aca cambiamos la fecha de string a int, pone la cultura que tiene nuestra aplicacion. 
                );
            #endregion
        }
    }
}
