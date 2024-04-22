USE master
GO 
    IF NOT EXISTS(
        SELECT name 
        FROM sys.databases
        WHERE name = N'RPN'
    )
    CREATE DATABASE [RPN]
    GO
    CREATE TABLE [Stacks]
    (
        StackId uniqueidentifier default newid(),
        Item NVARCHAR(256)
    )
    GO