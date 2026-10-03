-- 1. إنشاء جدول الأقسام
CREATE TABLE [dbo].[Department] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Location] NVARCHAR(100) NULL
);

-- 2. إضافة عمود المفتاح الخارجي لجدول الطلاب إذا لم يكن موجوداً
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Student]') AND name = N'DepartmentId'
)
BEGIN
    ALTER TABLE [dbo].[Student] ADD [DepartmentId] INT NULL;
    
    ALTER TABLE [dbo].[Student] 
    ADD CONSTRAINT FK_Student_Department 
    FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department]([Id]);
END

-- 3. إضافة قسم تجريبي لغرض الاختبار
INSERT INTO [dbo].[Department] ([Name], [Location]) 
VALUES (N'Computer Science', N'Building A');