-- Tạo cơ sở dữ liệu
CREATE DATABASE schooldb;

\c schooldb

-- Tạo bảng students
CREATE TABLE students (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    age INT
);

-- Chèn 3 dòng dữ liệu mẫu tiếng Việt
INSERT INTO students (name, age) VALUES 
('Nguyễn Văn An', 20),
('Trần Thị Bích', 21),
('Lê Hoàng Cường', 19);
