﻿using FluentValidation;
using FluentValidation.Validators;
using LocadoraVeiculos.Dominio.Compartilhado;
using System.Text.RegularExpressions;


namespace LocadoraVeiculos.Dominio.Modulo_Cliente
{
    public class ValidadorCliente : ValidadorBase<Cliente>
    {
        public ValidadorCliente()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Campo 'Nome' é obrigatório.")
                .NotEmpty().WithMessage("Campo 'Nome' é obrigatótio.");

            RuleFor(x => x.Cpf)
              .MaximumLength(14).WithMessage("'CPF' inválido.");

            RuleFor(x => x.Cnpj)
              .MaximumLength(14).WithMessage("'CNPJ' inválido.");

            RuleFor(x => x.Endereco)
              .NotNull().WithMessage("Campo 'Endereço' é obrigatório.")
              .NotEmpty().WithMessage("Campo 'Endereço' é obrigatório.");

            RuleFor(x => x.Email)
             .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("'Email' formato incorreto")
             .NotNull().NotEmpty().WithMessage("'Email' formato incorreto");

            RuleFor(x => x.Telefone)
               .Telefone()
               .NotEqual("(  )      -");

            RuleFor(x => x.Cnh)
                .NotNull().NotEmpty()
                .MaximumLength(12)
                .WithMessage("'CNH' inválido.");

        }
    }
}
