-- Tạo bảng khóa học
CREATE TABLE courses (
    id SERIAL PRIMARY KEY,
    course_name VARCHAR(255) NOT NULL
);

-- Tạo bảng trung gian sinh viên - khóa học
CREATE TABLE student_courses (
    student_id INT REFERENCES students(id) ON DELETE CASCADE,
    course_id INT REFERENCES courses(id) ON DELETE CASCADE,
    PRIMARY KEY (student_id, course_id)
);

-- Chèn dữ liệu khóa học mẫu
INSERT INTO courses (course_name) VALUES 
('Toán cao cấp'),
('Lập trình C#'),
('Cơ sở dữ liệu');

-- Chèn dữ liệu đăng ký khóa học mẫu (Giả sử các Student có Id 1, 2, 3 đã được tạo)
INSERT INTO student_courses (student_id, course_id) VALUES 
(1, 1), (1, 2), -- Sinh viên 1 đăng ký Toán cao cấp và Lập trình C#
(2, 2), (2, 3), -- Sinh viên 2 đăng ký Lập trình C# và Cơ sở dữ liệu
(3, 1);         -- Sinh viên 3 đăng ký Toán cao cấp
