IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [FoodItems] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Category] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_FoodItems] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [PaymentStatus] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OrderItems] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [FoodItemId] int NOT NULL,
    [Quantity] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_FoodItems_FoodItemId] FOREIGN KEY ([FoodItemId]) REFERENCES [FoodItems] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_OrderItems_FoodItemId] ON [OrderItems] ([FoodItemId]);

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250920074556_InitialCreate', N'9.0.9');

ALTER TABLE [OrderItems] DROP CONSTRAINT [FK_OrderItems_FoodItems_FoodItemId];

ALTER TABLE [OrderItems] DROP CONSTRAINT [FK_OrderItems_Orders_OrderId];

ALTER TABLE [OrderItems] DROP CONSTRAINT [PK_OrderItems];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FoodItems]') AND [c].[name] = N'ImageUrl');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [FoodItems] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [FoodItems] DROP COLUMN [ImageUrl];

EXEC sp_rename N'[OrderItems]', N'OrderItem', 'OBJECT';

EXEC sp_rename N'[OrderItem].[IX_OrderItems_OrderId]', N'IX_OrderItem_OrderId', 'INDEX';

EXEC sp_rename N'[OrderItem].[IX_OrderItems_FoodItemId]', N'IX_OrderItem_FoodItemId', 'INDEX';

ALTER TABLE [Users] ADD [Email] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]);

ALTER TABLE [OrderItem] ADD CONSTRAINT [FK_OrderItem_FoodItems_FoodItemId] FOREIGN KEY ([FoodItemId]) REFERENCES [FoodItems] ([Id]) ON DELETE CASCADE;

ALTER TABLE [OrderItem] ADD CONSTRAINT [FK_OrderItem_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250921034231_AddCategoryToFoodItem', N'9.0.9');

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Password', N'Role', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [Email], [Password], [Role], [Username])
VALUES (1, N'', N'admin123', N'Admin', N'admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Password', N'Role', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250921035133_SeedAdminUser', N'9.0.9');

UPDATE [Users] SET [Password] = N'$2a$12$DCFivOnraf6V7aeMTeey8e9v9nRbetjV1ebRQTNn4fAuBxv2qyajq'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250921035525_SeedAdminUserHashed', N'9.0.9');

UPDATE [Users] SET [Password] = N'$2a$12$DCFivOnraf6V7aeMTeey8e9v9nRbetjV1ebRQTNn4fAuBxv2qyajq'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250921044011_SeedAdminUserWithHashedPassword', N'9.0.9');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925155228_SeedFoodItems', N'9.0.9');

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Description', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[FoodItems]'))
    SET IDENTITY_INSERT [FoodItems] ON;
INSERT INTO [FoodItems] ([Id], [Category], [Description], [Name], [Price])
VALUES (1, N'Burgers', N'Juicy beef patty with melted cheese, lettuce, tomato, and our special sauce on a toasted bun.', N'Classic Cheeseburger', 12.99),
(2, N'Pizza', N'Traditional pizza with fresh mozzarella, tomatoes, basil, and olive oil on a crispy thin crust.', N'Margherita Pizza', 15.5),
(3, N'Salads', N'Crisp romaine lettuce with parmesan cheese, croutons, and our homemade Caesar dressing.', N'Caesar Salad', 9.99),
(4, N'Main Courses', N'Tender grilled chicken breast seasoned with herbs, served with roasted vegetables.', N'Grilled Chicken Breast', 18.75),
(5, N'Main Courses', N'Beer-battered fish fillets with golden fries and mushy peas, served with tartar sauce.', N'Fish & Chips', 16.25),
(6, N'Pizza', N'Classic pizza topped with spicy pepperoni and mozzarella cheese on our signature dough.', N'Pepperoni Pizza', 17.0),
(7, N'Desserts', N'Rich, fudgy chocolate brownie served warm with vanilla ice cream and chocolate sauce.', N'Chocolate Brownie', 7.5),
(8, N'Appetizers', N'Crispy chicken wings tossed in your choice of buffalo, BBQ, or honey mustard sauce.', N'Chicken Wings', 11.99),
(9, N'Burgers', N'House-made plant-based patty with avocado, sprouts, and chipotle mayo on a whole grain bun.', N'Veggie Burger', 13.5),
(10, N'Desserts', N'Classic Italian dessert with layers of coffee-soaked ladyfingers and mascarpone cream.', N'Tiramisu', 8.99),
(11, N'Salads', N'Fresh mixed greens with feta cheese, olives, tomatoes, and cucumber in olive oil dressing.', N'Greek Salad', 10.75),
(12, N'Appetizers', N'Crispy tortilla chips loaded with cheese, jalapeños, sour cream, and guacamole.', N'Loaded Nachos', 9.5);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Description', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[FoodItems]'))
    SET IDENTITY_INSERT [FoodItems] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925155316_AddSampleFoodItems', N'9.0.9');

ALTER TABLE [FoodItems] ADD [ImageUrl] nvarchar(max) NULL;

UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 3;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 4;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 5;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 6;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 7;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 8;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 9;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 10;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 11;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = NULL
WHERE [Id] = 12;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925163525_AddImageUrlToFoodItems', N'9.0.9');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925163629_UpdateFoodItemsWithRealImages', N'9.0.9');

UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1565299624946-b28f40a0ca4b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1604382355076-af4b0eb60143?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1546793665-c74683f339c1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 3;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1532550907401-a500c9a57435?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 4;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1544025162-d76694265947?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 5;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 6;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1606313564200-e75d5e30476c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 7;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1567620832903-9fc6debc209f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 8;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1520072959219-c595dc870360?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 9;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 10;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 11;
SELECT @@ROWCOUNT;


UPDATE [FoodItems] SET [ImageUrl] = N'https://images.unsplash.com/photo-1513456852971-30c0b8199d4d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
WHERE [Id] = 12;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925163756_UpdateFoodItemsSeedWithImages', N'9.0.9');

CREATE TABLE [CartItems] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [FoodItemId] int NOT NULL,
    [FoodName] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_FoodItems_FoodItemId] FOREIGN KEY ([FoodItemId]) REFERENCES [FoodItems] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_CartItems_FoodItemId] ON [CartItems] ([FoodItemId]);

CREATE INDEX [IX_CartItems_UserId] ON [CartItems] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925165918_AddUserSpecificCart', N'9.0.9');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250925165942_CreateUserCartItems', N'9.0.9');

COMMIT;
GO

