CREATE TABLE [dbo].[Simulation] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DateSimulation] DATETIME2 (7)  NOT NULL,
    [UtilisateurId]  NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_Simulation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Simulation_AspNetUsers_UtilisateurId] FOREIGN KEY ([UtilisateurId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);