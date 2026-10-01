using Reservas.API;
using Reservas.Application;
using Reservas.Application.CasosUsos.Commands;
using Reservas.Application.CasosUsos.Queries;
using Reservas.Application.Cqrs;
using Reservas.Domain.Errores;
using Reservas.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opciones =>
       opciones.SwaggerEndpoint("/openapi/v1.json", "Reservas API v1"));
}


app.MapPost("/reservas", async
    (CrearReservaRequest request,
    IDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    try
    {
        var id = await dispatcher.EnviarAsync(new CrearReserva(request.SalaId,request.Inicio,request.Fin));

    return Results.Created($"/reservas/{id}", new { id });


    }catch(ReglasDeNegocioException ex)
    {
        var estado = ex switch
        {
            ConflictoDeEstadoException => StatusCodes.Status409Conflict,
            ReservaInvalidaException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status422UnprocessableEntity,
        };

        return Results.Problem(
            title: "No se pudo crear la reserva",
            detail: ex.Message,
            statusCode: estado,
            extensions: new Dictionary<string, object?>
            {
                ["codigo"] = ex.Codigo,
                ["detalles"] = ex.Detalles,
            });
    }

});


app.MapGet("/salas/{salaId:guid}/reservas", async (
    Guid salaId,
    IDispatcher dispatcher,
    CancellationToken ct) =>
{

    var resultado = await dispatcher.ConsultarAsync(new ReservasDeSala(salaId), ct);

    return Results.Ok(resultado);
});

app.UseHttpsRedirection();


app.Run();