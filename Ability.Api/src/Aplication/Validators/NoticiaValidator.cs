using Ability.Api.src.Aplication.Dtos;
using FluentValidation;

namespace Ability.Api.src.Aplication.Validators;


public class NoticiaValidator: AbstractValidator<NoticiaComandDto>
{
	public NoticiaValidator()
	{
		RuleFor(x => x.Titulo).NotEmpty().WithMessage("Título não pode ser vazio");
		RuleFor(x => x.Titulo).MinimumLength(10).WithMessage("Título tem que ter no mínimo 10 caracteres");
        RuleFor(x => x.Titulo).MaximumLength(50).WithMessage("Título tem que ter no máximo 50 caracteres");

        RuleFor(x => x.Url).NotEmpty().WithMessage("Url não pode ser vazia");
        RuleFor(x => x.Url).MinimumLength(10).WithMessage("Url tem que ter no mínimo 10 caracteres");
        RuleFor(x => x.Url).MaximumLength(200).WithMessage("Url tem que ter no máximo 200 caracteres");
    }
}

