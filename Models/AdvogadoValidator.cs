using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class AdvogadoValidator : AbstractValidator<Advogado>
    {
        public AdvogadoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo `Nome` é Obrigatório. Favor Preencher");
            //RuleFor(x => x.Email).NotEmpty().WithMessage("O campo `Email` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Telefone).NotEmpty().WithMessage("Algum `Telefone` é Obrigatório. Favor Preencher");
            RuleFor(x => x.DataNasc).NotEmpty().WithMessage("O campo `Data de Nascimento` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Telefone).NotEmpty().WithMessage("O campo `Telefone` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Rg).NotEmpty().WithMessage("O campo `RG` é Obrigatório. Favor Preencher");


            RuleFor(x => x.Cpf).NotEqual("___.___.___-__").WithMessage("O campo `CPF` é obrigatório. Favor Preencher");
            RuleFor(x => x.Cpf).Must(CPFValidator).WithMessage("CPF inválido");

            //CRIAR UM PARA TELEFONE
            RuleFor(x => x.Telefone).NotEqual("(__) _ ____-____").WithMessage("Algum `Telefone` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Telefone).Must(PhoneValidator).WithMessage("Telefone inválido");
        }

        public bool CPFValidator(string cpf)
        {
            return true;
        }

        public bool PhoneValidator(string telefone)
        {
            return true;
        }

    }
}
