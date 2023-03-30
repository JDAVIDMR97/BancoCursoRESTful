using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautres.Clientes.Commands.DeleteClienteCommand
{
    public class DeleteClienteCommandValidator : AbstractValidator <DeleteClienteCommand>
    {
        public DeleteClienteCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("{PropertyName} No debe estar vacio");
        }
    }
}
