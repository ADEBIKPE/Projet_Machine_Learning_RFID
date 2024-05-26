CREATE TABLE [dbo].[ExecutionMethode] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [NomMethode]       NVARCHAR (MAX) NOT NULL,
    [Details]          NVARCHAR (MAX) NOT NULL,
    [Performance]      REAL           NOT NULL,
    [MatriceConfusion] NVARCHAR (MAX) NOT NULL,
    [Temps_Execution]  NVARCHAR (MAX) NOT NULL,
    [SimulationId]     INT            NULL,
    [UserId]           NVARCHAR (450) NOT NULL,
    [UserRoleId]       NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_ExecutionMethodeAdmin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExecutionMethode_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ExecutionMethode_AspNetRoles] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id])
);