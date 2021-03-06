using FluentResults;
using FluentValidation.Results;
using LocadoraVeiculos.Dominio.Modulo_GrupoVeiculo;
using LocadoraVeiculos.Infra.BancoDados.Modulo_GrupoVeiculo;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocadoraAutomoveis.Aplicacao.Modulo_GrupoVeiculo
{
    public class ServicoGrupoVeiculo
    {
        readonly RepositorioGrupoVeiculoEmBancoDados repositorioGrupoVeiculo;
        ValidadorGrupoVeiculo validadorGrupoVeiculo;


        public ServicoGrupoVeiculo(RepositorioGrupoVeiculoEmBancoDados repositorioGrupoVeiculo)
        {
            this.repositorioGrupoVeiculo = repositorioGrupoVeiculo;
        }

        public Result<GrupoVeiculo> Inserir(GrupoVeiculo grupoVeiculo)
        {
            Log.Logger.Debug("Tentando inserir Grupo de Veículo... {@grupo}", grupoVeiculo);

            var resultadoValidacao = Validar(grupoVeiculo);

            if (resultadoValidacao.IsFailed)
            {
                foreach (var erro in resultadoValidacao.Errors) {

                    Log.Logger.Warning("Falha ao tentar inserir Grupo de Veículo. {GrupoNome} -> Motivo: {erro}", grupoVeiculo.Id, erro.Message);

                }

                return Result.Fail(resultadoValidacao.Errors);
            }

            try
            {
                repositorioGrupoVeiculo.Inserir(grupoVeiculo);
                Log.Logger.Information("Grupo de Veículo inserido com sucesso. {@grupo}", grupoVeiculo);


                return Result.Ok(grupoVeiculo);
            }
            catch (Exception ex)
            {
                string msgErro = "Falha ao tentar inserir Grupo de Veículo";

                Log.Logger.Error(ex, msgErro + "{GrupoVeiculoId}", grupoVeiculo.Id);

                return Result.Fail(msgErro);
            }

        }

        public Result<GrupoVeiculo> Editar(GrupoVeiculo grupoVeiculo)
        {
            Log.Logger.Debug("Tentando editar Grupo de Veículo... {@grupo}", grupoVeiculo);
            
            Result resultadoValidacao = Validar(grupoVeiculo);

            if (resultadoValidacao.IsFailed)
            {
                foreach (var erro in resultadoValidacao.Errors) {

                    Log.Logger.Warning("Falha ao tentar editar Grupo de Veículo. {GrupoVeiculoId} -> Motivo: {erro}", grupoVeiculo.Id, erro.Message);

                }

                return Result.Fail(resultadoValidacao.Errors);
            }

            try
            {
                repositorioGrupoVeiculo.Editar(grupoVeiculo);

                Log.Logger.Information("Grupo de Veículo . {GrupoVeiculoId} editado com sucesso", grupoVeiculo.Id);
                

                return Result.Ok(grupoVeiculo);
            }
            catch(Exception ex)
            {
                string msgErro = "Falha ao tentar editar Grupo de Veículo";

                Log.Logger.Error(ex, msgErro + "{GrupoVeiculoId}", grupoVeiculo.Id);

                return Result.Fail(msgErro);
            }
        }

        public Result Excluir(GrupoVeiculo grupoVeiculo)
        {
            Log.Logger.Debug("Tentando excluir Grupo de Veículo... {@grupo}", grupoVeiculo);

            try
            {
                repositorioGrupoVeiculo.Excluir(grupoVeiculo);

                Log.Logger.Information("GrupoVeiculo {GrupoVeiculoId} excluído com sucesso", grupoVeiculo.Id);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar excluir o Grupo de Veiculo";

                Log.Logger.Error(ex, msgErro + "{GrupoVeiculo}", grupoVeiculo.Id);

                return Result.Fail(msgErro);

            }
        }

        public Result< List<GrupoVeiculo> > SelecionarTodos()
        {
            try
            {
                return Result.Ok(repositorioGrupoVeiculo.SelecionarTodos());
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar selecionar todos os grupos de veiculo";

                Log.Logger.Error(ex, msgErro);

                return Result.Fail(msgErro);
            }
        }

        public Result<GrupoVeiculo> SelecionarPorId(Guid id)
        {
            try
            {
                return Result.Ok(repositorioGrupoVeiculo.SelecionarPorId(id));
            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar selecionar o grupo de veiculo";

                Log.Logger.Error(ex, msgErro + "{GrupoId}", id);

                return Result.Fail(msgErro);
            }
        }

        public Result Validar(GrupoVeiculo grupoVeiculo)
        {

            validadorGrupoVeiculo = new ValidadorGrupoVeiculo();

            var resultadoValidacao = validadorGrupoVeiculo.Validate(grupoVeiculo);

            List<Error> erros = new List<Error>(); //FluentResult

            foreach (ValidationFailure item in resultadoValidacao.Errors) //FluentValidation
            {
                
                erros.Add(new Error(item.ErrorMessage));
            }

            if (NomeDuplicado(grupoVeiculo))
                erros.Add(new Error("'Nome' duplicado"));

            if(erros.Any())
                return Result.Fail(erros);

            return Result.Ok();

        }


        #region privates

        private bool NomeDuplicado(GrupoVeiculo grupoVeiculo)
        {
            repositorioGrupoVeiculo.Sql_selecao_por_parametro = @"SELECT * FROM TBGRUPOVEICULO WHERE NOMEGRUPO = @NOME";
            repositorioGrupoVeiculo.PropriedadeParametro = "Nome";

            var funcionarioEncontrado = repositorioGrupoVeiculo.SelecionarPorParametro(repositorioGrupoVeiculo.PropriedadeParametro, grupoVeiculo);

            return funcionarioEncontrado != null &&
                   funcionarioEncontrado.Nome.Equals(grupoVeiculo.Nome) &&
                  !funcionarioEncontrado.Id.Equals(grupoVeiculo.Id);
        }

        #endregion
    }
}
