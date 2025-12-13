
create database PP_Location_DATA

use PP_Location_DATA

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,   -- mật khẩu đã mã hóa
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Role NVARCHAR(20) NOT NULL,            -- 'Admin' hoặc 'Student'
    StudentCode NVARCHAR(20) NULL,         -- mã SV, chỉ dùng cho Role = 'Student'
    IsActive BIT NOT NULL DEFAULT 1,       -- khóa/mở tài khoản
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE ProviderCategories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,   -- Nhà trọ / Đồ ăn / Quần áo / ...
    Description NVARCHAR(255) NULL
);

CREATE TABLE Providers (
    ProviderId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    CategoryId INT NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20),
    Description NVARCHAR(500) NULL,
    AverageRating DECIMAL(3,2) NULL,       -- điểm trung bình 1-5
    IsVerified BIT NOT NULL DEFAULT 0,     -- đã kiểm chứng?
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active', -- 'Active', 'Blocked', 'Pending'
    CreatedBy INT NOT NULL,                -- ai tạo (admin hoặc sinh viên)
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Providers_Categories FOREIGN KEY (CategoryId) 
        REFERENCES ProviderCategories(CategoryId),
    CONSTRAINT FK_Providers_Users FOREIGN KEY (CreatedBy) 
        REFERENCES Users(UserId)
);

CREATE TABLE Feedback (
    FeedbackId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,                      -- sinh viên gửi
    ProviderId INT NULL,                      -- có thể null nếu đề xuất mới
    Type NVARCHAR(20) NOT NULL,               -- 'Recommendation' hoặc 'Complaint'
    Title NVARCHAR(150) NOT NULL,             -- tiêu đề ngắn
    Content NVARCHAR(MAX) NOT NULL,           -- mô tả chi tiết
    SuggestedProviderName NVARCHAR(150) NULL, -- dùng cho Recommend mới
    SuggestedCategoryId INT NULL,
    SuggestedAddress NVARCHAR(255) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', 
        -- 'Pending', 'Approved', 'Rejected', 'Resolved'
    AdminNote NVARCHAR(500) NULL,             -- ghi chú xử lý
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ProcessedBy INT NULL,                     -- admin xử lý
    CONSTRAINT FK_Feedback_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId),
    CONSTRAINT FK_Feedback_Providers FOREIGN KEY (ProviderId) 
        REFERENCES Providers(ProviderId),
    CONSTRAINT FK_Feedback_SuggestedCategory FOREIGN KEY (SuggestedCategoryId) 
        REFERENCES ProviderCategories(CategoryId),
    CONSTRAINT FK_Feedback_Admin FOREIGN KEY (ProcessedBy) 
        REFERENCES Users(UserId)
);

CREATE TABLE ProviderRatings (
    RatingId INT IDENTITY(1,1) PRIMARY KEY,
    ProviderId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL,               -- 1-5 sao
    Comment NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Ratings_Providers FOREIGN KEY (ProviderId) 
        REFERENCES Providers(ProviderId),
    CONSTRAINT FK_Ratings_Users FOREIGN KEY (UserId) 
        REFERENCES Users(UserId)
);


select * from Providers;
select * from ProviderRatings
select * from Feedback

--tạo bảng khu vực 
CREATE TABLE Areas (
    AreaId INT IDENTITY(1,1) PRIMARY KEY,
    AreaName NVARCHAR(100) NOT NULL,         -- Tên khu vực (Hòa Khánh, Liên Chiểu, Sơn Trà,...)
    Description NVARCHAR(255) NULL,          -- Mô tả thêm nếu cần
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- thêm cột AreaId vào bảng Providers
ALTER TABLE Providers
ADD AreaId INT NULL;  

-- khóa ngoại liên kết Providers → Areas

ALTER TABLE Providers
ADD CONSTRAINT FK_Providers_Areas
    FOREIGN KEY (AreaId) REFERENCES Areas(AreaId);

select * from Areas;
select * from Providers;


INSERT INTO Areas (AreaName, Description)
VALUES
(N'Bách Khoa – Hai Bà Trưng', N'Khu vực gần ĐH Bách Khoa, Kinh tế Quốc dân, Xây dựng'),
(N'Cầu Giấy – Xuân Thủy', N'Khu vực gần ĐH Quốc Gia, ĐH Sư Phạm, ĐH Thương Mại'),
(N'Hoàng Mai – Nghiêm Xuân Yêm', N'Khu vực gần ĐH Thăng Long'),
(N'Hòa Lạc – Thạch Thất', N'Khu vực gần ĐH FPT và khu công nghệ cao Hòa Lạc');

INSERT INTO ProviderCategories (CategoryName, Description)
VALUES
(N'Nhà trọ', N'Phòng trọ cho sinh viên'),
(N'Đồ ăn', N'Quán ăn sinh viên'),
(N'Quần áo – Đồ dùng', N'Cửa hàng thời trang, đồ sinh hoạt'),
(N'Dịch vụ', N'Giặt là, photo, sửa xe, tiện ích');

INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, StudentCode)
VALUES
(N'Bích Ngọc', N'123456', N'Nguyễn Thị Bích Ngọc ', N'ngoc@gmail.com', N'Admin', NULL),
(N'bichngoc_sv', N'123456', N'Nguyễn Thị Bích Ngọc', N'ngoc@gmail.com', N'Student', N'SV001');

SELECT * FROM Users;
SELECT * FROM ProviderCategories;
SELECT * FROM Areas;

INSERT INTO Providers (Name, CategoryId, Address, Phone, Description, AreaId, CreatedBy)
VALUES
-- Bách Khoa – Hai Bà Trưng
(N'Nhà trọ Đồng Tâm', 1, N'Ngõ Đồng Tâm, Hai Bà Trưng, Hà Nội', N'0987654321',
 N'Nhà trọ giá rẻ cho SV Bách Khoa – Kinh Tế – Xây Dựng', 1, 1),

(N'Cơm Sinh Viên BK', 2, N'Trần Đại Nghĩa, Hai Bà Trưng, Hà Nội', N'0912345678',
 N'Quán cơm rẻ, suất 25k – 30k', 1, 1),

-- Cầu Giấy – Xuân Thủy
(N'Nhà trọ Dịch Vọng Hậu', 1, N'Dịch Vọng Hậu, Cầu Giấy, Hà Nội', N'0982221111',
 N'Trọ gần ĐH Quốc Gia – Sư Phạm – Thương Mại', 2, 1),

(N'Bún Chả Sinh Viên CG', 2, N'Xuân Thủy, Cầu Giấy, Hà Nội', N'0988112233',
 N'Bún chả ngon, giá sinh viên', 2, 1),

-- Hoàng Mai – Thăng Long
(N'KTX Thăng Long', 1, N'Nghiêm Xuân Yêm, Hoàng Mai, Hà Nội', N'0977665544',
 N'Ký túc xá giá rẻ cho sinh viên ĐH Thăng Long', 3, 1),

(N'Bánh Mì TL', 2, N'KĐT Kim Văn – Kim Lũ, Hoàng Mai, Hà Nội', N'0986445577',
 N'Bánh mì pate, giá 15k – 20k', 3, 1),

-- FPT – Hòa Lạc
(N'Ký túc xá FPT Hòa Lạc', 1, N'Khu Công Nghệ Cao Hòa Lạc, Thạch Thất, Hà Nội', N'0966778899',
 N'KTX chính thức của ĐH FPT', 4, 1),

(N'Quán Ăn FPT – HL', 2, N'Khu A – FPT University, Hòa Lạc', N'0909090909',
 N'Đồ ăn nhanh cho sinh viên FPT', 4, 1);


 INSERT INTO ProviderRatings (ProviderId, UserId, Rating, Comment)
VALUES
(3, 1, 5, N'Nhà trọ sạch, gần trường'),
(4, 1, 4, N'Cơm ngon, giá ổn'),
(5, 1, 5, N'Trọ đẹp, an ninh tốt'),
(6, 1, 4, N'Bún chả ngon'),
(7, 1, 5, N'KTX rộng rãi'),
(9, 1, 4, N'Ký túc xá FPT ổn');

INSERT INTO Feedback 
(UserId, ProviderId, Type, Title, Content, Status)
VALUES
(1, 3, N'Complaint', N'Nhà trọ hơi ồn', N'Tối có tiếng xe tải gần nhà.', N'Pending'),
(1, 4, N'Recommendation', N'Đề xuất thêm món ăn', N'Quán cơm nên thêm món cá kho.', N'Approved'),
(1, NULL, N'Recommendation', N'Đề xuất mở quán phở', N'Khu Cầu Giấy thiếu quán phở giá sinh viên.', N'Pending');

-- chưa chạy 
ALTER TABLE Student
ADD Category NVARCHAR(100),
    Address NVARCHAR(255),
    Rating INT,
    CreatedDate DATETIME;