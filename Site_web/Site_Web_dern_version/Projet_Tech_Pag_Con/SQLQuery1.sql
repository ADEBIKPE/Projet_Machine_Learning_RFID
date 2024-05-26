ALTER TABLE [dbo].[ExecutionMethode] ADD [UserId] NVARCHAR(450) NULL;
ALTER TABLE [dbo].[ExecutionMethode] ADD [UserRoleId] NVARCHAR(450) NULL;
-- Assuming you have a table `Simulations` with a primary key on `Id`
ALTER TABLE [dbo].[ExecutionMethode]
ADD CONSTRAINT [FK_ExecutionMethode_Simulation] FOREIGN KEY ([SimulationId]) REFERENCES [dbo].[Simulation] ([Id]);

-- Assuming you have a table `AspNetUsers` with a primary key on `Id`
ALTER TABLE [dbo].[ExecutionMethode]
ADD CONSTRAINT [FK_ExecutionMethode_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]);

-- Assuming you have a table `AspNetRoles` with a primary key on `Id`
ALTER TABLE [dbo].[ExecutionMethode]
ADD CONSTRAINT [FK_ExecutionMethode_AspNetRoles] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]);
