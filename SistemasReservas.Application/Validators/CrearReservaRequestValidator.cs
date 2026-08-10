using FluentValidation;
using FluentValidation.Validators;
using SistemasReservas.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemasReservas.Application.Validators;

public class CrearReservaRequestValidator: AbstractValidator<CrearReservaRequest>
{
    public CrearReservaRequestValidator()
    {
        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage("La fecha de inicio es obligatoria");

        RuleFor(x => x.FechaFin)
            .NotEmpty()
            .WithMessage("La fecha de fin es obligatoria.");

        RuleFor(x => x.FechaFin)
            .GreaterThan(x => x.FechaInicio)
            .WithMessage("La fecha de fin debe ser mayor que la fecha de inicio.");
    }
}
