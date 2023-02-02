using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

using BackendApi.Services.Contrato;
using BackendApi.Services.Implementacion;

using AutoMapper;
using BackendApi.DTOs;
using BackendApi.Utilidades;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbempleadoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

//ponemos asi las relaciones 
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

//agregamos como referencia la configuracion de esta clase de automapper profile que tiene los modelos para la claseDTO 
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));



builder.Services.AddCors(options =>
{
    //para no tener conflictos de relacion cuando la url de la api y de angular sea distinta y asi se puede comunicar igual sin tener problema de relacion
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PETICIONES API REST 

//LISTA DE DEPARTAMENTOS 
app.MapGet("/departamento/lista", async (
    IDepartamentoService _departamentoServicio,
    IMapper _mapper
    ) =>
{
    var listaDepartamento = await _departamentoServicio.GetList();
    var listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

    if (listaDepartamentoDTO.Count > 0)
        return Results.Ok(listaDepartamentoDTO);
    else
        return Results.NotFound(); 
});


//LISTA DE EMPLEADOS 
app.MapGet("/empleado/lista", async (
    IEmpleadoService _empleadoServico,
    IMapper _mapper
    ) =>
{
    var listaEmpleado = await _empleadoServico.GetList();
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Count > 0)
        return Results.Ok(listaEmpleadoDTO);
    else
        return Results.NotFound();
});


//GUARDAR UN EMPLEADO 
app.MapPost("/empleado/guardar", async(
    EmpleadoDTO modelo, //recibe un modelo 
    IEmpleadoService _empleadoServico,
    IMapper _mapper
    ) => {

        var _empleado = _mapper.Map<Empleado>(modelo);
        var _empleadoCreado = await _empleadoServico.Add(_empleado);

        if (_empleadoCreado.IdEmpleado != 0)
            return Results.Ok(_mapper.Map<EmpleadoDTO>(_empleadoCreado));
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
});


//ACTUALIZAR UN EMPLEADO 
app.MapPut("/empleado/actualizar/{idEmpleado}", async (
    int idEmpleado,
    EmpleadoDTO modelo, //recibe un modelo 
    IEmpleadoService _empleadoServico,
    IMapper _mapper
    ) => {

        var _encontrado = await _empleadoServico.Get(idEmpleado);

        if (_encontrado is null)
            return Results.NotFound();

        var _empleado = _mapper.Map<Empleado>(modelo);

        _encontrado.NombreCompleto = _empleado.NombreCompleto;
        _encontrado.IdDepartamento = _empleado.IdDepartamento;
        _encontrado.Sueldo = _empleado.Sueldo;
        _encontrado.FechaContrato = _empleado.FechaContrato;

        var respuesta = await _empleadoServico.Update(_encontrado);

        if (respuesta)
            return Results.Ok(_mapper.Map<EmpleadoDTO>(_encontrado));
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    });


//ELIMINAR UN EMPLEADO 
app.MapDelete("/empleado/eliminar/{idEmpleado}", async (
    int idEmpleado, 
    IEmpleadoService _empleadoServico
    ) => {

        var _encontrado = await _empleadoServico.Get(idEmpleado);

        if (_encontrado is null)
            return Results.NotFound();

        var respuesta = await _empleadoServico.Delete(_encontrado);

        if (respuesta)
            return Results.Ok();
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    });


#endregion


app.UseCors("NuevaPolitica");

app.Run();

