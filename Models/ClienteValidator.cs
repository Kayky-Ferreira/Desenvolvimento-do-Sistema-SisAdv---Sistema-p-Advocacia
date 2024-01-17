using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace SisAdv.Models
{
    class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo `Nome` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Cpf).NotEmpty().WithMessage("O campo `Cpf` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Algum `Email` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Telefone).NotEmpty().WithMessage("O campo `Telefone` é Obrigatório. Favor Preencher");
            RuleFor(x => x.Endereco.Rua).NotEmpty();
            RuleFor(x => x.Endereco.Numero).NotEmpty();
            RuleFor(x => x.Endereco.Bairro).NotEmpty();
            RuleFor(x => x.Endereco.Cidade).NotEmpty();
            RuleFor(x => x.Endereco.Estado).NotEmpty();

            //CÓDIGO DE CPF TAMBÉM
            RuleFor(x => x.Cpf).NotEqual("___.___.___-__").WithMessage("O campo `CPF` é obrigatório. Favor Preencher");
            RuleFor(x => x.Cpf).Must(CPFValidator).WithMessage("CPF inválido");

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
