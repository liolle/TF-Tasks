## Schemas 

#### Tasks Table
```sql
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending', 
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_Task_Status CHECK (Status IN ('Pending', 'In Progress', 'Completed')),
    CONSTRAINT FK_TASK_USER FOREIGN KEY(TaskId) REFERENCES Users(Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
```

#### Users Table
```sql
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
);
```

#### Update Tasks table
Add task id as foreign key
```sql
/*
ALTER TABLE only allows columns to be added that 
    - can contain nulls, 
    - have a DEFAULT definition specified, 
    - the column being added is an identity or timestamp column, 
    - table must be empty to allow addition of this column. 
*/
ALTER TABLE Tasks 
ADD TaskId INT NOT NULL;
/*Work around*/
ALTER TABLE Tasks
ADD TaskId INT NOT NULL 
CONSTRAINT DF_Id DEFAULT 0;

ALTER TABLE Tasks DROP CONSTRAINT DEF_Id;

/*Since already have element in the table that don't have a user*/
-- Add a default user and give him all those tasks
INSERT INTO Users (FirstName,LastName,Email,Password)
VALUES ('Jhon','Doe','Jhon@test.com','Defualt')

UPDATE Tasks
SET UserId = 1
WHERE UserId = 0 OR UserId is NULL

ALTER TABLE Tasks
ADD CONSTRAINT FK_TASK_USER 
FOREIGN KEY(UserId) REFERENCES Users(Id)
ON DELETE CASCADE
ON UPDATE CASCADE;

/* Unique constraint on email to be able to login */
ALTER TABLE Users
ADD CONSTRAINT UQ_Users_Email UNIQUE (Email)

```