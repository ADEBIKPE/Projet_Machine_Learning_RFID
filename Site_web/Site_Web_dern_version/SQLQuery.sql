CREATE TABLE [dbo].[UserViewModel] (
    [Id]          NVARCHAR (450) NOT NULL,
    [Email]       NVARCHAR (MAX) NOT NULL,
    [UserId]      NVARCHAR (MAX) NOT NULL,
    [PhoneNumber] NVARCHAR (MAX) NOT NULL,
    [Role]        NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_UserViewModel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[SimulationAdmin] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [DateSimulationAdmin]   DATETIME2 (7)  NOT NULL,
    [NomMethodeAdmin]       NVARCHAR (MAX) NOT NULL,
    [DetailsAdmin]          NVARCHAR (MAX) NOT NULL,
    [PerformanceAdmin]      REAL           NOT NULL,
    [MatriceConfusionAdmin] NVARCHAR (MAX) NOT NULL,
    [Temps_ExecutionAdmin]  NVARCHAR (MAX) NOT NULL,
    [SimulationIdAdmin]     INT            NULL,
    [UserId]                NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_SimulationAdmin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SimulationAdmin_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
 
 
GO
CREATE NONCLUSTERED INDEX [IX_SimulationAdmin_UserId]
    ON [dbo].[SimulationAdmin]([UserId] ASC);

CREATE TABLE [dbo].[Simulation] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DateSimulation] DATETIME2 (7)  NOT NULL,
    [UtilisateurId]  NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_Simulation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Simulation_AspNetUsers_UtilisateurId] FOREIGN KEY ([UtilisateurId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

CREATE TABLE [dbo].[ExecutionMethodesAdmin] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [NomMethode]       NVARCHAR (MAX) NOT NULL,
    [Details]          NVARCHAR (MAX) NOT NULL,
    [Performance]      REAL           NOT NULL,
    [MatriceConfusion] NVARCHAR (MAX) NOT NULL,
    [Temps_Execution]  NVARCHAR (MAX) NOT NULL,
    [SimulationId]     INT            NULL,
    [UserId]           NVARCHAR (MAX) NOT NULL,
    [UserRoleId]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ExecutionMethodesAdmin] PRIMARY KEY CLUSTERED ([Id] ASC)
);
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

CREATE TABLE [dbo].[ConnexionHistorique] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Email]         NVARCHAR (MAX) NOT NULL,
    [DateConnexion] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ConnexionHistorique] PRIMARY KEY CLUSTERED ([Id] ASC)
);
