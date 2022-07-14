﻿CREATE TABLE [dbo].[TBCONDUTOR] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [NOME]          VARCHAR (100)    NOT NULL,
    [CPF]           VARCHAR (20)     NOT NULL,
    [CNH]           VARCHAR (20)     NOT NULL,
    [VENCIMENTOCNH] DATE             NOT NULL,
    [EMAIL]         NCHAR (100)      NOT NULL,
    [ENDERECO]      VARCHAR (150)    NOT NULL,
    [TELEFONE]      VARCHAR (20)     NOT NULL,
    [CLIENTE_ID]    INT              NOT NULL
);
