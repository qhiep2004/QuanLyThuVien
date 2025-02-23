create database QuanLyThuVien
use QuanLyThuVien

CREATE TABLE tblNguoiDung (
    sMaNguoiDung VARCHAR(20) PRIMARY KEY,
    sHoTen NVARCHAR(100) NOT NULL,
    sSDT NVARCHAR(15),
    sMatKhau NVARCHAR(100),
    sEmail NVARCHAR(100),
    sLoaiNguoiDung NVARCHAR(50) NOT NULL,
    CONSTRAINT chk_LoaiNguoiDung CHECK (sLoaiNguoiDung IN (N'Độc giả', N'Thủ thư', N'Quản trị viên'))
);
ALTER TABLE tblNguoiDung
DROP CONSTRAINT chk_LoaiNguoiDung;
ALTER TABLE tblNguoiDung
ADD CONSTRAINT chk_LoaiNguoiDung CHECK (sLoaiNguoiDung IN (N'Độc giả', N'Thủ thư', N'Quản trị viên'));


CREATE TABLE tblTaiKhoan (
    sMaTaiKhoan VARCHAR(20) PRIMARY KEY,
	sMaNguoiDung VARCHAR(20)  , 
    sTenDangNhap NVARCHAR(100) NOT NULL,
    sMatKhau NVARCHAR(100) NOT NULL,
   	 FOREIGN KEY (sMaNguoiDung) REFERENCES tblNguoiDung(sMaNguoiDung)
);

CREATE TABLE tblTaiLieu (
    sMaTaiLieu VARCHAR(20) PRIMARY KEY,
    sTenTaiLieu NVARCHAR(100) NOT NULL,
    sTenTacGia NVARCHAR(100),
    sTheLoai NVARCHAR(50),
    iNamXuatBan INT,
    sNXB NVARCHAR(100),
	iSoLuong int  ,
    sTinhTrang NVARCHAR(50),
	sViTri nvarchar(50)
);

ALTER TABLE tblTaiLieu
ALTER COLUMN sTheLoai NVARCHAR(50) NOT NULL;
SELECT * FROM tblTaiLieu
WHERE sTheLoai IS NULL;


CREATE TABLE tblMuonTra (
    sMaMuonTra VARCHAR(20) PRIMARY KEY,
    sMaNguoiDung VARCHAR(20) NOT NULL,
    dNgayMuon DATE NOT NULL,
    dNgayHenTra DATE,
	dNgayTraThucTe DATE,
    sTinhTrang NVARCHAR(50),
    FOREIGN KEY (sMaNguoiDung) REFERENCES tblNguoiDung(sMaNguoiDung),
	 CONSTRAINT chk_TinhTrang CHECK (sTinhTrang IN ('Đang mượn', 'Đã trả', 'Quá hạn'))
   
);

CREATE TABLE tblChiTietMuonTra (
    sMaChiTiet VARCHAR(20) PRIMARY KEY,
    sMaMuonTra VARCHAR(20) NOT NULL,
	sMaTaiLieu varchar(20) not null,
	iSoLuong int,	
     sTinhTrangSachMuon nvarchar(50),
    sTinhTrangSachTra NVARCHAR(50),
	fPhiPhat float
    FOREIGN KEY (sMaMuonTra) REFERENCES tblMuonTra(sMaMuonTra),
	 FOREIGN KEY (sMaTaiLieu) REFERENCES tblTaiLieu(sMaTaiLieu),
	 	 CONSTRAINT chk_TinhTrangMuon CHECK (sTinhTrangSachMuon IN ('Mới', 'Hư hỏng', 'Cũ')),
		 CONSTRAINT chk_TinhTrangTra CHECK (sTinhTrangSachTra IN ('Tốt', 'Hư hỏng', 'Mất'))
);
ALTER TABLE tblChiTietMuonTra
DROP CONSTRAINT chk_TinhTrangMuon;

ALTER TABLE tblChiTietMuonTra
DROP CONSTRAINT chk_TinhTrangTra;
 ALTER TABLE tblChiTietMuonTra
ADD CONSTRAINT chk_TinhTrangMuon CHECK (sTinhTrangSachMuon IN (N'Mới', N'Hư hỏng', N'Cũ'));

ALTER TABLE tblChiTietMuonTra
ADD CONSTRAINT chk_TinhTrangTra CHECK (sTinhTrangSachTra IN (N'Tốt', N'Hư hỏng', N'Mất'));



CREATE TABLE tblBaoCaoThongKe (
    sMaBaoCao VARCHAR(20) PRIMARY KEY, -- Mã báo cáo
    dNgayBaoCao DATETIME NOT NULL,     -- Ngày tạo báo cáo
    iTongSachTrongKho INT NOT NULL,    -- Tổng số sách trong thư viện
    iTongSachDangMuon INT NOT NULL,    -- Số sách đang được mượn
    iTongSachBiMat INT NOT NULL,       -- Số sách bị mất
    iTongSachHuHong INT NOT NULL,      -- Số sách bị hư hỏng
    iSoLuotMuonHomNay INT NOT NULL,    -- Số lượt mượn trong ngày
    iSoLuongTraHomNay INT NOT NULL     -- Số lượt trả trong ngày
);


INSERT INTO tblNguoiDung (sMaNguoiDung, sHoTen, sSDT, sMatKhau, sEmail, sLoaiNguoiDung)
VALUES
('ND001', N'Nguyễn Văn A', '0987654321', '123456', 'nguyenvana@gmail.com', N'Độc giả'),
('ND002', N'Lê Thị B', '0976543210', 'abcdef', 'lethib@gmail.com', N'Thủ thư'),
('ND003', N'Trần Văn C', '0912345678', 'password', 'tranvanc@gmail.com', N'Quản trị viên');


INSERT INTO tblTaiKhoan (sMaTaiKhoan, sMaNguoiDung, sTenDangNhap, sMatKhau)
VALUES
('TK001', 'ND001', 'nguyenvana', '123456'),
('TK002', 'ND002', 'lethib', 'abcdef'),
('TK003', 'ND003', 'tranvanc', 'password');


EXEC sp_helpconstraint 'tblNguoiDung';
SELECT * FROM tblTaiKhoan;
SELECT * FROM tblNguoiDung;


CREATE PROCEDURE sp_Login
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem tài khoản có tồn tại không
    IF EXISTS (
        SELECT 1
        FROM tblTaiKhoan
        WHERE sTenDangNhap = @TenDangNhap AND sMatKhau = @MatKhau
    )
    BEGIN
        SELECT 'Success' AS Result;
    END
    ELSE
    BEGIN
        SELECT 'Fail' AS Result;
    END
END;


INSERT INTO tblTaiLieu (sMaTaiLieu, sTenTaiLieu, sTenTacGia, sTheLoai, iNamXuatBan, sNXB, iSoLuong, sTinhTrang, sViTri) 
VALUES
(N'TL001', N'Cấu trúc dữ liệu và giải thuật', N'Nguyễn Văn A', N'Khoa học máy tính & Lập trình', 2020, N'NXB Giáo Dục', 5, N'Còn sách', N'A1-101'),
(N'TL002', N'Lập trình Python cơ bản', N'Trần Văn B', N'Khoa học máy tính & Lập trình', 2021, N'NXB Trẻ', 3, N'Còn sách', N'A1-102'),
(N'TL003', N'Kiến trúc máy tính', N'Nguyễn Văn C', N'Hệ thống máy tính & Mạng', 2019, N'NXB Khoa Học', 2, N'Còn sách', N'B1-201'),
(N'TL004', N'Quản trị hệ thống Windows Server', N'Lê Thị D', N'Hệ thống máy tính & Mạng', 2022, N'NXB Thông Tin', 4, N'Còn sách', N'B1-202'),
(N'TL005', N'MySQL cơ bản và nâng cao', N'Phạm Văn E', N'Cơ sở dữ liệu & Khoa học dữ liệu', 2021, N'NXB Công Nghệ', 6, N'Còn sách', N'C1-301'),
(N'TL006', N'HTML, CSS và JavaScript cơ bản', N'Nguyễn Văn F', N'Phát triển phần mềm & Công nghệ Web', 2023, N'NXB Lập Trình', 7, N'Còn sách', N'C1-302'),
(N'TL007', N'Lập trình AI với TensorFlow', N'Trần Văn G', N'Trí tuệ nhân tạo & Máy học', 2022, N'NXB AI', 3, N'Còn sách', N'D1-401');


delete  from  tblTaiLieu

select * from tblTaiLieu


