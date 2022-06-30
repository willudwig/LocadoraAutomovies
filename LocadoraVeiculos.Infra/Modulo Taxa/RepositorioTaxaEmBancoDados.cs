﻿using LocadoraVeiculos.Dominio.Modulo_Taxa;
using LocadoraVeiculos.Infra.BancoDados.Compartilhado;


namespace LocadoraVeiculos.Infra.BancoDados.Modulo_Taxa
{
    public class RepositorioTaxaEmBancoDados : RepositorioBase<Taxa, MapeadorTaxa, ValidadorTaxa>
    {
        protected override string Sql_insercao => @"INSERT INTO TBTAXA
                                                   (
                                                        [DESCRICAO],    
                                                        [TIPO],
                                                        [VALOR]
                                                   )
                                                   VALUES
                                                   (
                                                        @DESCRICAO,
                                                        @TIPO,
                                                        @VALOR

                                                   );SELECT SCOPE_IDENTITY();";

        protected override string Sql_edicao => @"UPDATE [TBTAXA] SET 

                                                    [DESCRICAO] = @DESCRICAO, 
                                                    [TIPO] = @TIPO,
                                                    [VALOR] = @VALOR

                                                WHERE
		                                             ID = @ID";

        protected override string Sql_exclusao => @"DELETE FROM TBTAXA WHERE ID = @ID;";

        protected override string Sql_selecao_por_id => @"SELECT * FROM TBTAXA WHERE ID = @ID";

        protected override string Sql_selecao_todos => @"SELECT * FROM TBTAXA";

        protected override bool VerificarDuplicidade(Taxa entidade)
        {
            var taxas = SelecionarTodos();

            foreach (Taxa t in taxas)
            {
                if (t.Descricao == entidade.Descricao)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
